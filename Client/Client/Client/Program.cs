using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            bool OK;
            try
            {
                socket.Connect("26.122.69.5", 1457);
                OK = true;
            }
            catch (SocketException)
            {
                Console.WriteLine($"Не удалось установить подключение с {socket.RemoteEndPoint}");
                OK = false;
            }

            if (OK)
            {
                Console.WriteLine($"Подключение к {socket.RemoteEndPoint} установлено");
                string msg = "ку";
                byte[] array = Encoding.UTF8.GetBytes(msg);
                socket.Send(array);
				socket.Send(array);

                
				while (msg != "stop")
                {
					byte[] ans = new byte[1024];
                    socket.Receive(ans);
                    msg = Encoding.UTF8.GetString(ans);
                    Console.WriteLine(msg);
                }

            }
        }
    }
}

