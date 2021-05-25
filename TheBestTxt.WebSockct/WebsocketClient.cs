using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheBestTxt.SocketAbout.Common
{
    public class WebsocketClient
    {
        public string Id { get; set; }
        public WebSocket WebSocket { get; set; }
        public string RoomNo { get; set; }
        public async Task SendMessageAsync(string msg)
        {
            var buffer = Encoding.UTF8.GetBytes(msg);
            var segment = new ArraySegment<byte>(buffer);
            await WebSocket.SendAsync(segment, WebSocketMessageType.Text, true, default(CancellationToken));
        }
    }
}
