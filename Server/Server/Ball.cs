using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
	class MainClass
	{
		/*public static void Main(string[] args)
		{
			Point point = new Point(2, (float)-4.564);
			Route route = new Route(point, 30, 0);

			Point platform = new Point((float)-11.2, 0);

			Route res = Route.BallRoute(route, platform);
			//Console.WriteLine(res.point.x);
			//Console.WriteLine(res.point.y);
			Route res2 = Route.BallRoute(res, platform);
			//Console.WriteLine(res2.point.x);
			//Console.WriteLine(res2.point.y);

			Point ball = new Point((float)-11.2, (float)-0.5);
			Route route2 = new Route(ball, 45, 1);
			Route res3 = Route.BallRoute(route2, platform);
			Route res4 = Route.BallRoute(res3, platform);
			Route res5 = Route.BallRoute(res4, platform);
			Route res6 = Route.BallRoute(res5, platform);

			//Console.WriteLine(res3.point.x);
			//Console.WriteLine(res3.point.y);
			//Console.WriteLine(res4.point.x);
			//Console.WriteLine(res4.point.y);
			//Console.WriteLine(res5.point.x);
			//Console.WriteLine(res5.point.y);
			//Console.WriteLine(res6.point.x);
			//Console.WriteLine(res6.point.y);
		}*/
		
	}

	public class Point
	{
		public float x, y;
		public Point(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public class Route
	{

		public Point point;
		public float angle; // угол полета
		public int side; // в какую сторону летим
		public Route(Point point, float angle, int side)
		{
			this.point = point;
			this.angle = angle;
			this.side = side; // 1 - летим вправо, 0 летим влево
		}



		public static Route BallRoute(Route route, Point platformCenter)
		{
			const float rightScape = 11.2f;
			const float leftScape = (float)-11.2;
			const float topScape = (float)4.62;
			const float downScape = (float)-4.564;
			const float height = topScape - downScape;
			const float platformRadius = 2.0f;
			const float platformWidth = (float)0.4;
			const int rightAngle = 90;

			if (route.point.x == 0 && route.point.y == 0) // Вылетаем из центра
			{
				if (route.side == 0) return new Route(new Point(leftScape, 0), 0, 1);
				return new Route(new Point(rightScape, 0), 0, 0);
			}
			//Тест прошел хорошо! (вроде бы)
			if (route.point.y == topScape) // Верхняя граница
			{

				double widthDelta = Math.Abs(Math.Tan(route.angle / 2 / 90 * Math.PI / 2) * height);

				if (route.side == 0)
				{


					if ((route.point.x - widthDelta) <= leftScape) // летим влево и ударяемся о границу
					{
						widthDelta = Math.Abs(route.point.x - leftScape);
						return new Route(new Point(leftScape, (float)(topScape - widthDelta / Math.Abs(Math.Tan((double)(route.angle / 2 / 90 * Math.PI / 2))))), route.angle, 0);
					}

					return new Route(new Point((float)(route.point.x - widthDelta), downScape), route.angle, 0);
				}

				if (route.point.x + widthDelta >= rightScape) // летим вправо и ударяемся о границу
				{
					widthDelta = Math.Abs(route.point.x - rightScape);
					return new Route(new Point(rightScape, (float)(topScape - widthDelta / Math.Abs(Math.Tan((double)(route.angle / 2 / 90 * Math.PI / 2))))), route.angle, 1);
				}
				return new Route(new Point((float)(route.point.x + widthDelta), downScape), route.angle, 1);

			}



			if (route.point.y == downScape) //Нижняя граница
			{
				double widthDelta = Math.Abs(Math.Tan(route.angle / 2 / 90 * Math.PI / 2) * height);
				if (route.side == 0)
				{
					if (route.point.x - widthDelta <= leftScape) // летим влево и ударяемся о границу
					{
						widthDelta = Math.Abs(route.point.x - leftScape);
						return new Route(new Point(leftScape, (float)(downScape + widthDelta / Math.Abs(Math.Tan((double)(route.angle / 2 / 90 * Math.PI / 2))))), route.angle, 0);
					}
					return new Route(new Point((float)(route.point.x - widthDelta), topScape), route.angle, 0);
				}

				if (route.point.x + widthDelta >= rightScape) // летим вправо и ударяемся о границу
				{
					widthDelta = Math.Abs(route.point.x - rightScape);
					return new Route(new Point(rightScape, (float)(downScape + widthDelta / Math.Tan((double)(route.angle / 2 / 90 * Math.PI / 2)))), route.angle, 1);
				}
				return new Route(new Point((float)(route.point.x + widthDelta), topScape), route.angle, 1);

			}




			if (route.point.x == rightScape) // летим в правую границу 
			{
				// тест пройден!!!
				if (route.point.y >= platformCenter.y && route.point.y <= platformCenter.y + platformRadius) // врезались в верхнюю часть палки
				{

					float alpha = (float)(Math.Abs(Math.Atan((topScape - route.point.y) / (rightScape * 2))) / (Math.PI / 2) * 90); // угол до левого верхнего угла
					float betta = (route.point.y - platformCenter.y) / platformRadius * 80 + 10;// угол отлета
					if (betta <= alpha)
					{
						float heightDelta = (float)Math.Abs(Math.Tan((double)(betta / 90 * (Math.PI / 2)))) * 2 * rightScape;

						return (new Route(new Point(leftScape, (float)(topScape - heightDelta)), betta, 0));
					}

					float weightDelta = (float)((topScape - route.point.y) / Math.Abs(Math.Tan((double)(betta / 90 * (Math.PI / 2)))));
					return new Route(new Point((float)(rightScape - weightDelta), topScape), betta, 0);
				}

				// тестирую // хз как но оно работало
				if (route.point.y <= platformCenter.y && route.point.y >= platformCenter.y - platformRadius) // врезались в нижнюю часть палки
				{

					float alpha = (float)Math.Abs(Math.Atan((double)(rightScape * 2 / Math.Abs(downScape - route.point.y)) / (Math.PI / 2) * 90)); // угол до левого верхнего угла
					float betta = (float)(90 - Math.Abs(route.point.y - platformCenter.y) / platformRadius * 80 + 10); // угол отлетаа

					//Вроде работает
					if (betta < alpha)
					{
						float weightDelta = (float)Math.Abs(Math.Tan(betta / 90 * (Math.PI / 2))) * Math.Abs(downScape - route.point.y);
						Console.WriteLine(weightDelta);
						return (new Route(new Point(rightScape - weightDelta, downScape), betta, 0));
					}


					float heightDelta = (float)(2 * rightScape / Math.Abs(Math.Tan((betta / 90 * (Math.PI / 2)))));
					return (new Route(new Point(leftScape, (float)(topScape - heightDelta)), betta, 0));

				}

				return new Route(new Point(0, 0), 0, -1); // гол! возвращаемся в центр координат
			}

			if (route.point.x == leftScape) // летим в левую границу 
			{
				// тест пройден!!!
				if (route.point.y >= platformCenter.y && route.point.y <= platformCenter.y + platformRadius) // врезались в верхнюю часть палки
				{

					float alpha = (float)Math.Abs(Math.Atan((double)((topScape - route.point.y) / (rightScape * 2))) / (Math.PI / 2) * 90); // угол до левого верхнего угла
					float betta = (route.point.y - platformCenter.y) / platformRadius * 80 + 10;// угол отлета
					if (betta <= alpha)
					{
						float heightDelta = (float)(Math.Abs(Math.Tan(betta / 90 * (Math.PI / 2))) * 2 * rightScape); //?

						return (new Route(new Point(rightScape, topScape - heightDelta), betta, 1));
					}

					float weightDelta = (float)((topScape - route.point.y) / Math.Abs(Math.Tan(betta / 90 * (Math.PI / 2))));
					return (new Route(new Point(leftScape + weightDelta, topScape), betta, 1));
				}

				// тестирую // хз как но оно работало
				if (route.point.y <= platformCenter.y && route.point.y >= platformCenter.y - platformRadius) // врезались в нижнюю часть палки
				{

					float alpha = (float)(Math.Abs(Math.Atan(rightScape * 2 / Math.Abs(downScape - route.point.y)) / (Math.PI / 2) * 90)); // угол до левого верхнего угла
					float betta = 90 - Math.Abs(route.point.y - platformCenter.y) / platformRadius * 80 + 10; // угол отлетаа

					//Вроде работает
					if (betta < alpha) // летим вниз
					{
						float weightDelta = (float)(Math.Abs(Math.Tan(betta / 90 * (Math.PI / 2))) * Math.Abs(downScape - route.point.y));
						Console.WriteLine(weightDelta);
						return (new Route(new Point((float)(leftScape + weightDelta), downScape), betta, 1));
					}


					float heightDelta = (float)(2 * rightScape / Math.Abs(Math.Tan((betta / 90 * (Math.PI / 2)))));


					return (new Route(new Point(rightScape, (float)(topScape - heightDelta)), betta, 1));

				}

				return new Route(new Point(0, 0), 0, -1); // гол! возвращаемся в центр координат
			}

			return new Route(new Point(-100, -100), -10, 2);
		}

	}

}

