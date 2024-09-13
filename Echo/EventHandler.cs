using System;

namespace Echo
{
    public class EventHandler
    {
        public static void OnDisconnect
            (ClientState c)
        {
            Console.WriteLine("OnDisconnect");
        }
    }
}