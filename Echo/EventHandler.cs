using System;

namespace Echo
{
    public class EventHandler
    {
        public static void OnDisconnect
            (ClientState c)
        {
            //Console.WriteLine("OnDisconnect");
            string desc = c.socket.RemoteEndPoint.ToString();
            string sendStr = "Leave|" + desc + ",";
            foreach (var cs in Program.clients.Values)
            {
                Program.Send(cs,sendStr);
            }
        }
    }
}