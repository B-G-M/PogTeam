using System;
using System.Net.Sockets;
using System.Text;

namespace Client_V2
{
    public class ConnToServer
    {
        private readonly Socket socket;

        public ConnToServer(Socket socket)
        {
            this.socket = socket;
        }

        public  void SendMessage(String outMessage)
        {
            var message = outMessage;
            byte[] requestData = Encoding.UTF8.GetBytes(message);
            socket.Send(requestData);
            Console.WriteLine("Send the message");
            
        }

        public void RecieveMessage()
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            String[] param = res.Split(" ");

            switch (param[0])
            {
                case  "GetId":
                    Console.WriteLine("GetId");
                    break;
                case "GetConnect":
                    Console.WriteLine("GetConnect");
                    break;
                case "BothConn":
                    break;
                case "Ready":
                    break;
                    
            }
        }
    }
}