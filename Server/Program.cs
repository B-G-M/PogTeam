using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
	internal class Program
	{
		class Server
		{
			IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 1457);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			public void Start() 
			{
				socket.Bind(ipPoint);
				socket.Listen(2);

				Game game = new(socket.Accept(), socket.Accept());
				game.Start();

			}
		}

		class Game
		{
			public Game(Socket p1,Socket p2) {
				#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
				player1.socket = p1;
				player2.socket = p2;

				//убрать этот костыль и сделать конструктор игрока
				player1.side = "left";
				player2.side = "right";
            }
			public Player? player1 = new();
			public Player? player2 = new();

			

			public void Start()
			{
				string? msg = "";
				while(msg != "stop")
				{
					msg = Console.ReadLine();
					var msgS = msg.Split(" ");
					if (msgS[0] == "p1")
                        player1.SendMsg(msg);
					else if (msgS[0] == "p2")
						player2.SendMsg(msg);
					else if (msgS[0] == "pp")
                    {
                        player1.SendMsg(msg);
                        player2.SendMsg(msg);
                    }
                    if (msg == "stop")
					{
						player1.socket.Close();
						player2.socket.Close();
					}
					msg = "";
				}
				
			}
			
		}


		class Player
		{
			public string nickName = "";
			public string password = "";
			public int id;
			public float stickY;
			public string side = "";

			public bool IsReady = false;

			public Socket? socket;

			public bool SendMsg(string msg) 
			{
				byte[] ansArr = Encoding.UTF8.GetBytes(msg);
				socket.Send(ansArr);
				return true;//прикрути проверку на отправку
			}
			public string ReciveMsg()
			{
				byte[] msgArr = new byte[1024];
				socket.Receive(msgArr);
				string msg = Encoding.UTF8.GetString(msgArr);
				
				return msg;
			}
			private bool AuthPlayer(string login, string pass)
            {
                if (nickName == login && pass == password)
                {
					return true;
                }
				return false;
            }
			private string CheckSide()
            {
				return side;
            }
			private bool ChangeReady(string Readyness)
            {
				if (Readyness == "true" && !IsReady) IsReady = true;
				else if (Readyness == "false" && IsReady) IsReady = false;
				return IsReady;
            }
            private void CommandProcessing(string Request)
            {
                string[] requestPart = Request.Split(" ");
                switch (requestPart[0])
                {
                    case "auth":
						if (AuthPlayer(requestPart[1], requestPart[2])) SendMsg("OK");
						else SendMsg("ERROR");
                        break;
                    case "ch_side":
						SendMsg(CheckSide());
                        break;
                    case "rdy":
						if (ChangeReady(requestPart[2])) SendMsg("Ready");
						else SendMsg("Not Ready");
                        break;
                    case "   ":
                        Console.WriteLine();
                        break;
                    default:
                        break;
                }
            }
        }

		static void Main(string[] args)
		{
			Server server = new Server();
			server.Start();

		}
	}
}