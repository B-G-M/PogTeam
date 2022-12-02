﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.Server;

namespace Server.Server
{
	class Game
	{
		public Game()
		{
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
		int player1Score = 0;
		int player2Score = 0;
		bool player1Connect = false;
		bool player2Connect = false;

		float ballSpeed = 20f;
		Random rnd = new Random();

		public void Start()
		{
			while (!GameRdy()){}


			string? msg = "";
			player1.Enemy = player2;
			player2.Enemy = player1;
			Thread recP1 = new Thread(player1.ReciveMsg);
			Thread recP2 = new Thread(player2.ReciveMsg);
			recP1.Start();
			recP2.Start();

			while (player1Score < 5 || player2Score < 5)
			{
				int goal = Round();

				if (goal == 0)
					player1Score += 1;
				else
					player2Score += 1;
			}
			// Логика завершения игры, быстро !!
			if (player1Score == 5)
			{
				player1.SendMsg("EndGame_win;");
				player2.SendMsg("EndGame_lost;");
			}
			else if(player2Score == 5)
			{
				player2.SendMsg("EndGame_win;");
				player1.SendMsg("EndGame_lost;");
			}
			
			//Докрутить логику голов
		}

		public int Round()
		{
			Point ball = new Point(0f, 0f);
			Route ballPos = new Route(ball, 0, rnd.Next(0, 1));
			Route ballNextPos;
			float platform;

			int goalSide = -1;
			do
			{
				platform = (ballPos.side == 0) ? -11.2f : 11.2f;

				ballNextPos = (ballPos.side == player1.side) ?
					Route.BallRoute(ballPos, new Point(platform, player1.stickY)) :
					Route.BallRoute(ballPos, new Point(platform, player2.stickY));

				while (!player2.IsReady || !player1.IsReady) { };
				string mes = "ballDir_" + ballPos.point.x + "_" + ballPos.point.y + "_" + ballNextPos.point.x + "_" + ballPos.point.x + ";";

                player1.SendMsg(mes);
				player2.SendMsg(mes);

				if (ballPos.side == player1Side && ballNextPos.side == -1)
				{
					goalSide = 1;
					player1.stickY = 0.0f;
					player2.stickY = 0.0f;
					player2.SendMsg("scored_"+ballNextPos.point.x+"_"+ballNextPos.point.y+"_"+player2.stickX+"_"+player2.stickY+";");
                    player1.SendMsg("gotScored_" + ballNextPos.point.x + "_" + ballNextPos.point.y
						+ "_" + player1.stickX + "_" + player1.stickY + ";");
                }

				else if (ballPos.side == player2Side && ballNextPos.side == -1)
				{
                    goalSide = 0;
                    player1.SendMsg("scored_" + ballNextPos.point.x + "_" + ballNextPos.point.y + "_" + player1.stickX + "_" + player1.stickY + ";");
                    player2.SendMsg("gotScored_" + ballNextPos.point.x + "_" + ballNextPos.point.y
                        + "_" + player2.stickX + "_" + player2.stickY + ";");
                }

				while (!player1.pointAchieved && !player2.pointAchieved) { };
				player1.pointAchieved = false;
				player2.pointAchieved = false;
				ballPos = ballNextPos;

			} while (ballPos.side == -1);

			return goalSide;
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
