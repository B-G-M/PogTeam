using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_V2
{
    class Program
    {
        static void  Main(string[] args)
        {
            try
            {
                Task<Socket> clientConn = CreateConn();
                Console.WriteLine("Hello World!");
                Socket socket = clientConn.Result;
                String result=RecieveMessage(socket);
                Console.WriteLine(result);
                SendMessage(socket);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task<Socket> CreateConn()
        {
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                await client.ConnectAsync("10.180.94.127", 8080);
                Console.WriteLine("Connected");
                
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return client;
        }

        private static void SendMessage(Socket socket)
        {
            var message = "check server";
            byte[] requestData = Encoding.UTF8.GetBytes(message);
            socket.Send(requestData);
            Console.WriteLine("Send the message");
            
        }

        private static String RecieveMessage(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            return res;
        }
        
    }
}