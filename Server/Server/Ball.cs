using System;
namespace Server.Server
{

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
			const float rightScape = (float)11.2;
			const float leftScape = (float)-11.2;
			const float topScape = (float)4.62;
			const float downScape = (float)-4.564;
			const float height = topScape - downScape;
			const float platformRadius = 1.25F;
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

				if (route.side == 1)
				{

					if ((route.point.x - widthDelta) <= leftScape) // летим влево и ударяемся о границу
					{
						widthDelta = Math.Abs(route.point.x - leftScape);
						return new Route(new Point(leftScape, (float)(topScape - widthDelta / Math.Abs(Math.Tan((double)(route.angle / 2 / 90 * Math.PI / 2))))), route.angle, 0);
					}

					return new Route(new Point((float)(route.point.x - widthDelta), downScape), route.angle, 1);
				}

				if (route.point.x + widthDelta >= rightScape) // летим вправо и ударяемся о границу
				{
					widthDelta = Math.Abs(route.point.x - rightScape);
					return new Route(new Point(rightScape, (float)(topScape - widthDelta / Math.Abs(Math.Tan((double)(route.angle / 2 / 90 * Math.PI / 2))))), route.angle, 1);
				}
				return new Route(new Point((float)(route.point.x + widthDelta), downScape), route.angle, 0);

			}



			if (route.point.y == downScape) //Нижняя граница
			{
				double widthDelta = Math.Abs(Math.Tan(route.angle / 2 / 90 * Math.PI / 2) * height);
				if (route.side == 1)
				{
					if (route.point.x - widthDelta <= leftScape) // летим влево и ударяемся о границу
					{
						widthDelta = Math.Abs(route.point.x - leftScape);
						return new Route(new Point(leftScape, (float)(downScape + widthDelta / Math.Abs(Math.Tan((double)(route.angle / 2 / 90 * Math.PI / 2))))), route.angle, 0);
					}
					return new Route(new Point((float)(route.point.x - widthDelta), topScape), route.angle, 1);
				}

				if (route.point.x + widthDelta >= rightScape) // летим вправо и ударяемся о границу
				{
					widthDelta = Math.Abs(route.point.x - rightScape);
					return new Route(new Point(rightScape, (float)(downScape + widthDelta / Math.Tan((double)(route.angle / 2 / 90 * Math.PI / 2)))), route.angle, 1);
				}
				return new Route(new Point((float)(route.point.x + widthDelta), topScape), route.angle, 0);

			}




			if (route.point.x == rightScape) // летим в правую границу 
			{
				// тест пройден!!!
				if (route.point.y >= platformCenter.y && route.point.y <= platformCenter.y + platformRadius) // врезались в верхнюю часть палки
				{

					float alpha = (float)(Math.Abs(Math.Atan((topScape - route.point.y) / (rightScape * 2))) / (Math.PI / 2) * 90); // угол до левого верхнего угла
					float betta = (route.point.y - platformCenter.y) * 80 + 10;// угол отлета


					if (betta <= alpha)
					{
						float heightDelta = (float)Math.Abs(Math.Tan((double)(betta / 90 * (Math.PI / 2)))) * 2 * rightScape;
						Console.WriteLine("Я тут");

						return (new Route(new Point(leftScape, (float)(topScape - heightDelta)), betta, 1));
					}

					float weightDelta = (float)((topScape - route.point.y) / Math.Abs(Math.Tan((double)(betta / 90 * (Math.PI / 2)))));
					return new Route(new Point((float)(rightScape - weightDelta), topScape), betta, 1);
				}

				// тестирую // хз как но оно работало
				if (route.point.y <= platformCenter.y && route.point.y >= platformCenter.y - platformRadius) // врезались в нижнюю часть палки
				{
					//Вот тут еботня 
					float alpha = (float)Math.Abs(Math.Atan((double)(rightScape * 2 / Math.Abs(downScape - route.point.y))) * 90 / (Math.PI / 2)); // угол до левого верхнего угла
					float betta = (float)(90 - Math.Abs(route.point.y - platformCenter.y) * 80 + 10); // угол отлетаа

					if (betta < alpha)
					{
						float weightDelta = (float)(Math.Abs(Math.Tan(betta / 90 * (Math.PI / 2))) * Math.Abs(downScape - route.point.y));

						return (new Route(new Point(rightScape - weightDelta, downScape), betta, 1));
					}

					Console.WriteLine("Я тут");
					float heightDelta = (float)(2 * rightScape / (Math.Abs(Math.Tan((betta / 90 * (Math.PI / 2))))));

					Console.WriteLine(heightDelta);
					return (new Route(new Point(leftScape, (float)(topScape - heightDelta)), betta, 1));

				}

				return new Route(new Point(0, 0), 0, -1); // гол! возвращаемся в центр координат
			}

			if (route.point.x == leftScape) // летим в левую границу 
			{
				// тест пройден!!!
				if (route.point.y >= platformCenter.y && route.point.y <= platformCenter.y + platformRadius) // врезались в верхнюю часть палки
				{

					float alpha = (float)Math.Abs(Math.Atan((double)((topScape - route.point.y) / (rightScape * 2))) / (Math.PI / 2) * 90); // угол до левого верхнего угла
					float betta = (route.point.y - platformCenter.y) * 80 + 10;// угол отлета
					if (betta <= alpha)
					{
						float heightDelta = (float)(Math.Abs(Math.Tan(betta / 90 * (Math.PI / 2))) * 2 * rightScape); //?

						return (new Route(new Point(rightScape, topScape - heightDelta), betta, 0));
					}

					float weightDelta = (float)((topScape - route.point.y) / Math.Abs(Math.Tan(betta / 90 * (Math.PI / 2))));
					return (new Route(new Point(leftScape + weightDelta, topScape), betta, 0));
				}

				// тестирую // хз как но оно работало
				if (route.point.y <= platformCenter.y && route.point.y >= platformCenter.y - platformRadius) // врезались в нижнюю часть палки
				{

					float alpha = (float)(Math.Abs(Math.Atan(rightScape * 2 / Math.Abs(downScape - route.point.y)) / (Math.PI / 2) * 90)); // угол до левого верхнего угла
					float betta = 90 - Math.Abs(route.point.y - platformCenter.y) * 80 + 10; // угол отлетаа

					//Вроде работает
					if (betta < alpha) // летим вниз
					{
						float weightDelta = (float)(Math.Abs(Math.Tan(betta / 90 * (Math.PI / 2))) * Math.Abs(downScape - route.point.y));
						Console.WriteLine(weightDelta);
						return (new Route(new Point((float)(leftScape + weightDelta), downScape), betta, 0));
					}


					float heightDelta = (float)(2 * rightScape / Math.Abs(Math.Tan((betta / 90 * (Math.PI / 2)))));


					return (new Route(new Point(rightScape, (float)(topScape - heightDelta)), betta, 0));

				}

				return new Route(new Point(0, 0), 0, -1); // гол! возвращаемся в центр координат
			}

			return new Route(new Point(-100, -100), -10, 2);
		}

	}

}
