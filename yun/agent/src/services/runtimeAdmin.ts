import { execFile } from 'child_process';
import { promisify } from 'util';
import config, { InstanceConfig } from '../config';

const execFileAsync = promisify(execFile);

export interface InstanceRuntimeLogPayload {
  instanceId: string;
  slotIndex: number;
  adbTarget: string;
  probePort: number;
  wsPort: number;
  scriptsDir: string;
  summary: string[];
  sections: Array<{
    title: string;
    content: string;
  }>;
}

export async function restartInstanceRuntime(instance: InstanceConfig): Promise<void> {
  const slotIndex = getSlotIndex(instance);

  if (slotIndex === 1) {
    await execFileAsync('systemctl', ['restart', 'cardemo.service'], {
      timeout: 300000,
      maxBuffer: 1024 * 1024 * 4,
    });
    return;
  }

  const stopScript = `${config.deploymentScriptsDir}/stop_instance.sh`;
  const startScript = `${config.deploymentScriptsDir}/start_instance.sh`;
  await runBash(`test -x ${shellEscape(stopScript)} && ${shellEscape(stopScript)} ${slotIndex} || true`);
  await runBash(`${shellEscape(startScript)} ${slotIndex}`);
}

export async function collectInstanceRuntimeLogs(
  instance: InstanceConfig
): Promise<InstanceRuntimeLogPayload> {
  const slotIndex = getSlotIndex(instance);
  const lxcName = slotIndex === 1 ? 'waydroid' : `waydroid${slotIndex}`;
  const lxcPath = slotIndex === 1 ? '/var/lib/waydroid/lxc' : `/var/lib/${lxcName}/lxc`;
  const adbPort = parseInt(instance.adbTarget.split(':').pop() || '0', 10);
  const grepPattern = `${instance.id}|${instance.adbTarget}|${instance.probePort}|${instance.wsPort}`;

  return {
    instanceId: instance.id,
    slotIndex,
    adbTarget: instance.adbTarget,
    probePort: instance.probePort,
    wsPort: instance.wsPort,
    scriptsDir: config.deploymentScriptsDir,
    summary: [
      `instance=${instance.id}`,
      `slot=${slotIndex}`,
      `adb=${instance.adbTarget}`,
      `probe=${instance.probePort}`,
      `ws=${instance.wsPort}`,
    ],
    sections: [
      {
        title: 'LXC',
        content: await runCommand('lxc-info', ['-P', lxcPath, '-n', lxcName], 10000),
      },
      {
        title: 'ADB',
        content: await runBash(`adb devices | grep '${instance.adbTarget}' || true`),
      },
      {
        title: 'Socat',
        content: await runBash(`ps -ef | grep 'socat' | grep '${adbPort}' | grep -v grep || true`),
      },
      {
        title: 'Probe',
        content: await runHandshakeProbe(instance.probePort),
      },
      {
        title: 'Ingress',
        content: await runHandshakeProbe(instance.wsPort),
      },
      {
        title: 'AgentLog',
        content: await runBash(
          `journalctl -u obd-agent -n 200 --no-pager | grep -E '${grepPattern}' | tail -80 || true`,
          20000
        ),
      },
      {
        title: 'Runtime',
        content: slotIndex === 1
          ? await runBash(`systemctl status cardemo.service --no-pager -l | sed -n '1,80p' || true`, 20000)
          : await runBash(
              `find ${shellEscape(config.deploymentScriptsDir)} -maxdepth 1 -type f \\( -name 'start_instance.sh' -o -name 'stop_instance.sh' \\) -print`,
              10000
            ),
      },
    ],
  };
}

function getSlotIndex(instance: InstanceConfig): number {
  return Math.max(1, instance.wsPort - 8080);
}

async function runHandshakeProbe(port: number): Promise<string> {
  return runBash(
    `curl -s -o /dev/null -w '%{http_code}\n' --max-time 3 ` +
      `-H 'Upgrade: websocket' -H 'Connection: Upgrade' ` +
      `-H 'Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==' ` +
      `-H 'Sec-WebSocket-Version: 13' http://127.0.0.1:${port}/ws || true`
  );
}

async function runBash(command: string, timeoutMs = 300000): Promise<string> {
  return runCommand('/bin/bash', ['-lc', command], timeoutMs);
}

async function runCommand(
  command: string,
  args: string[],
  timeoutMs: number
): Promise<string> {
  try {
    const { stdout, stderr } = await execFileAsync(command, args, {
      timeout: timeoutMs,
      maxBuffer: 1024 * 1024 * 4,
    });
    return [stdout, stderr].filter(Boolean).join('').trim();
  } catch (err: any) {
    return [
      err?.stdout ? String(err.stdout) : '',
      err?.stderr ? String(err.stderr) : '',
      err?.message ? String(err.message) : '',
    ].filter(Boolean).join('\n').trim();
  }
}

function shellEscape(value: string): string {
  return `'${value.replace(/'/g, `'\"'\"'`)}'`;
}
