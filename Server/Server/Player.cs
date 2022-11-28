using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
	class Player
	{
		public Player(Socket socket)
		{
			Random rnd = new Random();

			this.socket = socket;
			id = rnd.Next(0, 99);//поменять генерацию 
			Console.WriteLine("Игрок {0} подключился", id);
			stickY = 0.0f;
		}

		Player _enemy;
		public Player Enemy { set { _enemy = value; } }
		public string nickName;
		public string password;
		public int id;
		public bool IsReady = false;

		public float stickY;
		private float StepSize = 0.2f;
		public int side;
		private float speed = 20.0f;
		private float UpperRange = 4.0f;
		private float LowerRange = -3.9f;

		public Socket socket;

		public bool SendMsg(string msg)
		{
			byte[] ansArr = Encoding.UTF8.GetBytes(msg);
			socket.Send(ansArr);
			Console.WriteLine("Отправлено игроку {0}: {1}", id, msg);
			return true;//прикрути проверку на отправку
		}

		public void ReciveMsg()
		{
			bool haveRest = false;
			bool stop = true;
			string rest = "";
			while (true)
			{
				byte[] msgArr = new byte[1024];
				socket.Receive(msgArr);

				string msg = Encoding.UTF8.GetString(msgArr);
				msg = rest + msg;
				rest = "";

				Console.WriteLine("Получено от игрока {0}: {1}", id, msg);
				msg = msg.Replace("\0", string.Empty);

				var msgSplit = msg.Split(";");
				if (msg[msg.Length - 1] != ';')
				{
					rest = msgSplit[msgSplit.Length - 1];
					msgSplit = msgSplit.SkipLast(1).ToArray();
				}

				foreach (var req in msgSplit)
				{
					if (req == "")
						break;	

					stop = CommandProcessing(req);
					if (!stop)
						return;
				}
			}

		}

		private bool AuthPlayer(string login, string password)
		{
			nickName = login;
			this.password = password;

			//return AuntificationMethods.Login(login,password);
			return true;
		}

		private string CheckSide()
		{
			return side.ToString();
		}

		private bool ChangeReady(string readyness)
		{
			if (readyness == "True" && !IsReady) IsReady = true;
			else if (readyness == "False" && IsReady) IsReady = false;
			return IsReady;
		}

		private string MoveUpCalculating()
		{
			string comand = "";

			comand += stickY.ToString() + "_";

			if (stickY + StepSize < UpperRange)
				stickY += StepSize;
			else
				stickY = UpperRange;

			comand += stickY.ToString();
			comand += "_" + speed.ToString();

			return comand;
		}
		private string MoveDownCalculating()
		{
			string comand = "";
			comand += stickY.ToString() + "_";

			if (stickY - StepSize > LowerRange)
				stickY -= StepSize;
			else
				stickY = LowerRange;

			comand += stickY.ToString();
			comand += "_" + speed.ToString();

			return comand;
		}
		private bool CommandProcessing(string request)
		{
			string[] requestPart = request.Split("_");
			string ansver = "";
			bool flag = true;

			if (requestPart.Length > 1 && requestPart[1] == "ERROR")
			{
				_enemy.socket.Shutdown(0);
				_enemy.socket.Close();
				socket.Shutdown(0);
				socket.Close();
			}
			if (requestPart.Length > 1 && requestPart[1] == "OK")
				return true;

			switch (requestPart[0])
			{
				case "auth":

					if (AuthPlayer(requestPart[1], requestPart[2]))
					{
						//ansver = "OK";
						ansver = $"id_{id}";

					}
					else SendMsg("ERROR_AUTH;");
					flag = false;
					break;

				case "chSide":
					var temp = CheckSide();
					ansver = "side_" + temp + "_" + _enemy.nickName;
					break;

				case "rdy":

					if (ChangeReady(requestPart[1]))
					{
						ansver = "Ready";
						_enemy.SendMsg("changeRdy_Ready;");
					}
					else
					{
						ansver = "NotReady";
						_enemy.SendMsg("changeRdy_NotReady;");
					}
					break;

				case "moveUp":
					ansver = MoveUpCalculating();
					_enemy.SendMsg("sMoveUp_" + ansver);
					ansver = "moveUp_" + ansver;
					break;

				case "moveDown":
					ansver = MoveDownCalculating();
					_enemy.SendMsg("sMoveDown_" + ansver);
					ansver = "moveDown_" + ansver;
					break;

				default:

					ansver = "ERROR";

					_enemy.socket.Shutdown(0);
					_enemy.socket.Close();
					socket.Shutdown(0);
					socket.Close();
					break;
			}
			SendMsg(ansver + ";");
			return flag;
		}
	}
}
