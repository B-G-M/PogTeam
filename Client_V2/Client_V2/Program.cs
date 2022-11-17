using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_V2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Task<Socket> clientConn = CreateConn();
                Console.WriteLine("Hello World!");
                Socket socket = clientConn.Result;
                var send=Task.Run(() =>SendMessageAsync(socket)) ;
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
            catch (IOException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return client;
        }

        private static async Task SendMessageAsync(Socket socket)
        {
            await using var stream = new NetworkStream(socket);
            var message = "Hello METANIT.COM";
            var data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data);
            Console.WriteLine("Сообщение отправлено");
            
        }
        
    }
}