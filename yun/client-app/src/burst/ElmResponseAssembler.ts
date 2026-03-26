/**
 * ElmResponseAssembler - ELM327 响应组包层
 *
 * 从蓝牙分片中累积文本，识别 '>' 终结符，处理超时，输出完整响应。
 * 采用 beginWait → feed → complete 模式，消除快响应竞态。
 */

import { Buffer } from 'buffer';

export interface AssembledResponse {
  rawElmText: string;
  promptSeen: boolean;
  timeout: boolean;
  completedBy: 'prompt' | 'timeout' | 'abort';
  chunkCount: number;
}

export class ElmResponseAssembler {
  private buffer = '';
  private chunkCount = 0;
  private resolveWait: ((resp: AssembledResponse) => void) | null = null;
  private timeoutHandle: ReturnType<typeof setTimeout> | null = null;
  private waiting = false;
  private aborted = false;

  /**
   * 进入等待状态：清空缓冲区并准备接收。
   * 必须在发送 AT 命令之前调用，确保快响应不会丢失。
   */
  beginWait(timeoutMs: number): Promise<AssembledResponse> {
    // 清空上一轮状态
    this.buffer = '';
    this.chunkCount = 0;
    this.waiting = true;
    this.aborted = false;

    if (this.timeoutHandle) {
      clearTimeout(this.timeoutHandle);
      this.timeoutHandle = null;
    }

    return new Promise<AssembledResponse>((resolve) => {
      this.resolveWait = resolve;

      this.timeoutHandle = setTimeout(() => {
        if (this.waiting) {
          this.complete('timeout');
        }
      }, timeoutMs);
    });
  }

  /**
   * 喂入一个蓝牙数据片段（base64 编码）。
   * 由外部蓝牙监听器调用，可能在 beginWait 之后、命令发送之前就到达（快响应）。
   */
  feed(base64Data: string): void {
    if (this.aborted || !this.waiting) return;

    let text: string;
    try {
      text = Buffer.from(base64Data, 'base64').toString('ascii');
    } catch {
      return;
    }

    this.buffer += text;
    this.chunkCount++;

    if (this.buffer.includes('>') && this.resolveWait) {
      this.complete('prompt');
    }
  }

  /**
   * 中止当前等待
   */
  abort(): void {
    this.aborted = true;
    if (this.waiting) {
      this.complete('abort');
    }
  }

  destroy(): void {
    this.abort();
    if (this.timeoutHandle) {
      clearTimeout(this.timeoutHandle);
      this.timeoutHandle = null;
    }
  }

  private complete(completedBy: 'prompt' | 'timeout' | 'abort'): void {
    this.waiting = false;
    if (this.timeoutHandle) {
      clearTimeout(this.timeoutHandle);
      this.timeoutHandle = null;
    }
    const resolve = this.resolveWait;
    this.resolveWait = null;
    resolve?.({
      rawElmText: this.buffer,
      promptSeen: this.buffer.includes('>'),
      timeout: completedBy === 'timeout',
      completedBy,
      chunkCount: this.chunkCount,
    });
  }
}
