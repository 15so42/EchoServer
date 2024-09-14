using System;
using System.Runtime.CompilerServices;

namespace Echo
{
    public class MsgHandler
    {
        public static void MsgEnter(ClientState c, string msgArgs)
        {
            Console.WriteLine("MsgEnter|" +
                              ""+msgArgs);

            string[] split = msgArgs.Split(',');
            string desc = split[0];
            float x = float.Parse(split[1]);
            float y = float.Parse(split[2]);
            float z = float.Parse(split[3]);
            float eulerY = float.Parse(split[4]);

            c.hp = 100;
            c.x = x;
            c.y = y;
            c.z = z;
            c.eulerY = eulerY;

            string sendStr = "Enter|" + msgArgs;
            foreach (var cs in Program.clients.Values)
            {
                Program.Send(cs,sendStr);
            }
        }

        public static void MsgList(ClientState c, string msgArgs)
        {
            Console.WriteLine("MsgList|"+msgArgs);

            string sendStr = "List|";
            foreach (var cs in Program.clients.Values)
            {
                sendStr += cs.socket.RemoteEndPoint.ToString() + ",";
                sendStr += cs.x.ToString() + ",";
                sendStr += cs.y.ToString() + ",";
                sendStr += cs.z.ToString() + ",";
                sendStr += cs.eulerY.ToString() + ",";
                sendStr += cs.hp.ToString() + ",";
            }
            Program.Send(c,sendStr);
        }
        
        public static void MsgMove(ClientState c, string msgArgs)
        {
            Console.WriteLine("MsgMove|"+msgArgs);

            string[] split = msgArgs.Split(',');
            string desc = split[0];
            float x = float.Parse(split[1]);
            float y = float.Parse(split[2]);
            float z = float.Parse(split[3]);
           

           
            c.x = x;
            c.y = y;
            c.z = z;
            

            string sendStr = "Move|" + msgArgs;
            foreach (var cs in Program.clients.Values)
            {
                Program.Send(cs,sendStr);
            }
           
        }
        
        public static void MsgAttack(ClientState c, string msgArgs)
        {
            Console.WriteLine("MsgAttack|"+msgArgs);

            string sendStr = "Attack|"+msgArgs;
            foreach (var cs in Program.clients.Values)
            {
                Program.Send(cs,sendStr);
            }
            
        }
    }
}