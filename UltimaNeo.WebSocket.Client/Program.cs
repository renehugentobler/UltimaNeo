using System;
using WebSocketSharp;

namespace UltimaNeo.WebSocket.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ws = new WebSocketSharp.WebSocket("ws://localhost:8081/ultimaneo"))
            {
                ws.OnMessage += (sender, e) =>
                  Console.WriteLine("Ultimaneo says: " + e.Data);

                ws.Connect();
                ws.Send("PING");
                ws.Send("PONG");
                Console.ReadKey(true);
            }
        }
    }
}
