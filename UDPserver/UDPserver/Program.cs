using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPserver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
 
            var localIP = new IPEndPoint(IPAddress.Parse("192.168.99.2"), 1487);
            udpSocket.Bind(localIP);
            Console.WriteLine("UDP-сервер запущен...");
            byte[] data = new byte[256]; 
            EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
            Console.Write("Введите свое имя: ");
            string? username = Console.ReadLine();
            
            await SendMessageAsync();
        }

        static async Task SendMessageAsync()
        {
            using Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");
            // отправляем сообщения
            while (true)
            {
                var message = Console.ReadLine(); // сообщение для отправки
                // если введена пустая строка, выходим из цикла и завершаем ввод сообщений
                if (string.IsNullOrWhiteSpace(message)) break;
                // иначе добавляем к сообщению имя пользователя
                message = $"{username}: {message}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                // и отправляем на 127.0.0.1:remotePort
                await sender.SendToAsync(data, new IPEndPoint(localAddress, remotePort));
            }
        }
    }
}