using Server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
	class Game
	{
		public Game()
		{
			Random rnd = new Random();
			if (rnd.Next(0, 1) == 0)
			{
				player1Side = 0;
				player2Side = 1;
			}
			else
			{
				player1Side = 1;
				player2Side = 0;
			}
		}

		public Player player1;
		public Player player2;
		int player1Side = 0;
		int player2Side = 1;
		bool player1Connect = false;
		bool player2Connect = false;

		public void Start()
		{
			while (!GameRdy())
			{

			}
			string? msg = "";
			player1.Enemy = player2;
			player2.Enemy = player1;
			Thread recP1 = new Thread(player1.ReciveMsg);
			Thread recP2 = new Thread(player2.ReciveMsg);
			recP1.Start();
			recP2.Start();

			while (true)
			{

			}
		}
		public bool GameRdy()
		{
			if (player1Connect && player2Connect)
				return true;
			else return false;
		}

		public async void NewPlayer(Socket socket)
		{
			await Task.Run(() =>
			{
				Player player = new Player(socket);
				player.ReciveMsg();

				if (player1Connect == false)
				{
					player1Connect = true;
					player1 = player;
					player1.side = player1Side;
				}
				else
				{
					player2Connect = true;
					player2 = player;
					player2.side = player2Side;
				}

			});
		}
	}
}
