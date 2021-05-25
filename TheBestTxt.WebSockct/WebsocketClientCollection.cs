using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;

namespace TheBestTxt.SocketAbout.Common
{
    public static class WebsocketClientCollection
    {
        private static List<WebsocketClient> websockets { get; set; } = new List<WebsocketClient>();


        public static void Add(WebsocketClient webSocket)
        {
            websockets.Add(webSocket);
        }

        public static void Remove(WebsocketClient webSocket)
        {
            websockets.Remove(webSocket);
        }

        public static WebsocketClient Get(string id)
        {
            return websockets.FirstOrDefault(websocket => websocket.Id == id);
        }

        public static List<WebsocketClient> Gets(string RoomNo)
        {
            return websockets.Where(websocket => websocket.RoomNo == RoomNo).ToList();
        }

        public static List<WebsocketClient> GetAll()
        {
            return websockets;
        }
    }
}
