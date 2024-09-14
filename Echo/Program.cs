using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Echo
{
    public class ClientState
    {
        public Socket socket;
        public byte[] readBUff = new byte[1024];
        public int hp = -100;
        public float x = 0;
        public float y = 0;
        public float z = 0;
        public float eulerY = 0;
    }
    internal class Program
    {
        private static Socket listenfd;
        public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();
        
        public static void Main(string[] args)
        {
            Socket listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            listenfd.Bind(new IPEndPoint(IPAddress.Parse("127.0.01"),8888 ));
            listenfd.Listen(0);
            Console.WriteLine("服务器启动成功");

            //checkRead
            List<Socket> checkRead = new List<Socket>();
            while (true)
            {
                checkRead.Clear();
                checkRead.Add(listenfd);
                foreach (var s in clients.Values)
                {
                    checkRead.Add(s.socket);
                }
                //select
                Socket.Select(checkRead,null,null,1000);
                foreach (var s in checkRead)
                {
                    if (s == listenfd)
                    {
                        ReadListenfd(s);
                    }
                    else
                    {
                        ReadClientfd(s);
                    }
                }
            }
        }

        public static void ReadListenfd(Socket listenfd)
        {
            Console.WriteLine("Accept");
            Socket clientFd = listenfd.Accept();
            ClientState state = new ClientState();
            state.socket = clientFd;
            clients.Add(clientFd,state);
        }

        public static bool ReadClientfd(Socket clientfd)
        {
            ClientState state = clients[clientfd];
            int count = 0;
            try
            {
                count = clientfd.Receive(state.readBUff);
            }
            catch (Exception e)
            {
                
                MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
                object[] ob = { state };
                mei.Invoke(null, ob);
                
                clientfd.Close();
                clients.Remove(clientfd);
                Console.WriteLine("Receive SocketException "+e.ToString());
                return false;
            }

            if (count <= 0)
            {
                MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
                object[] ob = { state };
                mei.Invoke(null, ob);

                clientfd.Close();
                clients.Remove(clientfd);
                Console.WriteLine("Socket Close");
                return false;
            }

            
            string recvStr = System.Text.Encoding.Default.GetString(state.readBUff, 0, count);
            string[] split = recvStr.Split('|');
            Console.WriteLine("Receive"+recvStr);

            string msgName = split[0];
            string msgArgs = split[1];
            string funName = "Msg" + msgName;
            MethodInfo mi = typeof(MsgHandler).GetMethod(funName);
            object[] o = { state, msgArgs };
            mi.Invoke(null, o);
            
            return true;
        }
        
        public static void Send(ClientState cs,string sendStr)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(sendStr);
            cs.socket.Send(bytes);
            Console.WriteLine("发送："+sendStr);
        }
    }
    
    
}