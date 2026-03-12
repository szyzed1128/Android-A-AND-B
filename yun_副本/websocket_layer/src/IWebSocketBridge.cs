using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// WebSocket桥接统一接口（客户端/服务器）
    /// </summary>
    public interface IWebSocketBridge : IDisposable
    {
        event EventHandler<WSMessage> MessageReceived;
        event EventHandler<string> RawMessageReceived;
        event EventHandler<WebSocketState> StateChanged;
        event EventHandler<string> ConnectionClosed;

        WebSocketState State { get; }
        bool IsConnected { get; }

        /// <summary>
        /// 客户端: 连接到服务器
        /// 服务器: 启动监听（serverUrl 用于解析端口/路径）
        /// </summary>
        Task ConnectAsync(string serverUrl);

        Task SendAsync(WSMessage message);
        Task SendRawAsync(string json);
        Task<WSMessage> SendAndWaitAsync(WSMessage request);
        Task CloseAsync();
    }
}
