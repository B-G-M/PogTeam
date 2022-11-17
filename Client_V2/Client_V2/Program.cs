using System;
using System.Dynamic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_V2
{
    class Program
    {
        private static Socket socket;
        public String Id;
        static void  Main(string[] args)
        {
            try
            {
                Task<Socket> clientConn = CreateConn();
                Console.WriteLine("Hello World!");
                socket = clientConn.Result;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            ConnToServer connector = new ConnToServer(socket);
            
            connector.RecieveMessage();
            Command command = new Command(connector);
            
            command.GetAuthentification("gg","wp");
        }

        private static async Task<Socket> CreateConn()
        {
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                await client.ConnectAsync("10.162.250.246", 8080);
                Console.WriteLine("Connected");
                
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return client;
        }

    }
    
}