using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using TheBestTxt.SocketAbout.Common;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace TheBestTxt.SocketAbout.Web.Test
{
    public class WebSocketMiddleWare
    {
        private readonly RequestDelegate _next;

        public WebSocketMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    System.Net.WebSockets.WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    string clientId = Guid.NewGuid().ToString(); ;
                    var wsClient = new WebsocketClient
                    {
                        Id = clientId,
                        WebSocket = webSocket
                    };
                    try
                    {
                        await Handle(wsClient);
                    }
                    catch (Exception ex)
                    {
                        await context.Response.WriteAsync("closed");
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
            else
            {
                await _next(context);
            }
        }


        private async Task Handle(WebsocketClient webSocket)
        {
            WebsocketClientCollection.Add(webSocket); ;

            WebSocketReceiveResult result = null;
            do
            {
                var buffer = new byte[1024 * 1];
                result = await webSocket.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text && !result.CloseStatus.HasValue)
                {
                    var msgString = Encoding.UTF8.GetString(buffer);
                    //var message = System.Text.Json.JsonSerializer.Deserialize<Message>(msgString);
                    var message = JsonConvert.DeserializeObject<Message>(msgString);
                    message.SendClientId = webSocket.Id;
                    await MessageRoute(message);
                }
            }
            while (!result.CloseStatus.HasValue);
            WebsocketClientCollection.Remove(webSocket);
        }

        private async Task MessageRoute(Message message)
        {
            var client = WebsocketClientCollection.Get(message.SendClientId);

            switch (message.Action)
            {
                case "join":
                    client.RoomNo = message.Msg;
                    var roomClients = WebsocketClientCollection.Gets(client.RoomNo);
                    roomClients.ForEach(c =>
                    {
                        c.SendMessageAsync($"{message.Nick} join room {client.RoomNo} success .");
                    });
                    break;
                case "send_to_room":
                    if (string.IsNullOrEmpty(client.RoomNo))
                    {
                        break;
                    }
                    var clients = WebsocketClientCollection.Gets(client.RoomNo);
                    clients.ForEach(c =>
                    {
                        c.SendMessageAsync(message.Nick + " : " + message.Msg).Wait();
                    });

                    break;
                case "leave":
                    var roomNo = client.RoomNo;
                    client.RoomNo = "";
                    await client.SendMessageAsync($"{message.Nick} leave room {roomNo} success .");
                    break;
                default:
                    break;
            }
        }
    }
}
