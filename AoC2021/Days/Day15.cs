using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace AoC2021
{
	partial class Program
	{
		[NoTrailingNewLine]
		[HasVisual]
		static void Day15(List<string> input)
		{
			int width = input[0].Length;
			int height = input.Count;
			int[] map = new int[width * height];
			int[] shortestTo = new int[width * height];
			int[] shortestToLast = new int[width * height];
			List<long> paths = new List<long>();

			for (int i = 0; i < shortestTo.Length; i++)
			{
				shortestTo[i] = int.MaxValue;
				shortestToLast[i] = int.MaxValue;
			}

			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					map[i + j * width] = input[j][i] - '0';
				}
			}

			long worst = 9 * 200;
			long max = 9 * 200;

			Queue<((int x, int y) target, int cost)> upLeft     = new Queue<((int x, int y) target, int cost)>();
			Queue<((int x, int y) target, int cost)> rightDown  = new Queue<((int x, int y) target, int cost)>();
			Queue<((int x, int y) target, int cost)> upLeft2    = new Queue<((int x, int y) target, int cost)>();
			Queue<((int x, int y) target, int cost)> rightDown2 = new Queue<((int x, int y) target, int cost)>();
			

			Paths(((0, 0), 0));
			
			while (rightDown.Count > 0 || upLeft.Count > 0)
			{
				(rightDown, rightDown2) = (rightDown2, rightDown);
				(upLeft, upLeft2) = (upLeft2, upLeft);
				while (rightDown2.Count > 0)
				{
					Paths(rightDown2.Dequeue());
				}
				while (upLeft2.Count > 0)
				{
					Paths(upLeft2.Dequeue());
				}
				
				DrawMap(398);
			}
			Console.WriteLine(paths.Min());
			DrawMap(398);
			PlotPath();

			for (int i = 0; i < 60; i++)
			{
				Vsync();
			}

			//return;

			int[] mapOld;
			(map, mapOld) = (new int[width * height * 25], map);
			shortestTo = new int[width * height * 25];
			shortestToLast = new int[width * height * 25];
			for (int i = 0; i < shortestTo.Length; i++)
			{
				shortestTo[i] = int.MaxValue;
				shortestToLast[i] = int.MaxValue;
			}
			paths.Clear();

			worst = 9 * width + 8 + width + 7 * width + 6 * width + 5 * width + 9 * height + 8 + height + 7 * height + 6 * height + 5 * height;
			max = 9 * width + 8 + width + 7 * width + 6 * width + 5 * width + 9 * height + 8 + height + 7 * height + 6 * height + 5 * height;

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					for (int x = 0; x < width; x++)
					{
						for (int y = 0; y < height; y++)
						{
							int newTile = mapOld[x + y * width] + i + j;
							if (newTile >= 10)
								newTile -= 9;
							map[(x + i * width) + (y + j * height) * width * 5] = newTile;
						}
					}
				}
			}

			width *= 5;
			height *= 5;
			
			Paths(((0, 0), 0));
			frameSpeed = 0;
			while (rightDown.Count > 0 || upLeft.Count > 0)
			{
				(rightDown, rightDown2) = (rightDown2, rightDown);
				(upLeft, upLeft2) = (upLeft2, upLeft);
				while (rightDown2.Count > 0)
				{
					Paths(rightDown2.Dequeue());
				}
				while (upLeft2.Count > 0)
				{
					Paths(upLeft2.Dequeue());
				}

				DrawMap(2817);
			}
			DrawMap(2817);
			PlotPath();
			Console.WriteLine(paths.Min());

			void Paths(((int x, int y) target, int cost) route)
			{
				if (route.cost >= worst) return;
				int x, y;
				(x, y) = route.target;
				if (route.cost >= shortestTo[x + y * width]) return;
				shortestTo[x + y * width] = route.cost;
				if (x == width - 1 && y == height - 1)
				{
					paths.Add(route.cost);
					worst = route.cost;
					return;
				}
				
				if (ReadMap(x + 1, y) < ReadMap(x, y + 1))
				{
					int tileCost;
					(int x, int y) target;
					target = (x + 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							rightDown.Enqueue((target, tileCost));
					}
					target = (x, y + 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							rightDown.Enqueue((target, tileCost));
					}
					target = (x - 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							upLeft.Enqueue((target, tileCost));
					}
					target = (x, y - 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							upLeft.Enqueue((target, tileCost));
					}
				}
				else
				{
					int tileCost;
					(int x, int y) target;
					target = (x, y + 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							rightDown.Enqueue((target, tileCost));
					}
					target = (x + 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							rightDown.Enqueue((target, tileCost));
					}
					target = (x, y - 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							upLeft.Enqueue((target, tileCost));
					}
					target = (x - 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							upLeft.Enqueue((target, tileCost));
					}
				}
				//frameSpeed = 16;
				//int size = Visual.WindowHeight / height;
				//
				//StartDraw(false);
				//for (int i = 0; i < height; i++)
				//{
				//	for (int j = 0; j < width; j++)
				//	{
				//		int tile = map[x + y * width];
				//		int visit = (int)(shortestTo[x + y * width] * 1.0 / 398 * 255);
				//		visual.SpriteBatch.Draw(visual.Pixel, new Rectangle(x * size, y * size, size, size), new Color(visit,visit,visit));
				//	}
				//}
				//StopDraw();
				//Vsync();
			}

			int ReadMap(int x, int y)
			{
				if (x < 0) return 10;
				if (y < 0) return 10;
				if (x >= width) return 10;
				if (y >= height) return 10;
				return map[x + y * width];
			}

			void PlotPath()
			{
				frameSpeed = 5;
				Vsync();
				int sx = width - 1, sy = height - 1;
				int size = Math.Min(Visual.WindowWidth / width, Visual.WindowHeight / height);
				while (!(sx == 0 && sy == 0))
				{
					StartDraw(false);
					visual.SpriteBatch.Draw(visual.Pixel, new Rectangle(sx * size, sy * size, size, size), Color.Red);
					List<(long cost, (int x, int y) pos)> where = new List<(long cost, (int x, int y) pos)>
					{
						(GetCost(sx + 1, sy), (sx + 1, sy)),
						(GetCost(sx, sy + 1), (sx, sy + 1)),
						(GetCost(sx - 1, sy), (sx - 1, sy)),
						(GetCost(sx, sy - 1), (sx, sy - 1))
					};
					where = where.OrderBy(s => s.cost).ToList();
					(sx, sy) = where.First().pos;
					StopDraw();
					Vsync();
				}
				StartDraw(false);
				visual.SpriteBatch.Draw(visual.Pixel, new Rectangle(sx * size, sy * size, size, size), Color.Red);
				StopDraw();
				Vsync();

			}

			void DrawMap(int cmax)
			{
				int size = Math.Min(Visual.WindowWidth / width, Visual.WindowHeight / height);
				StartDraw(false);
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						int tile = map[j + i * width] - 1;
						float visit = shortestTo[j + i * width] * 1.0f / cmax;
						//visit = (float)Math.Pow(visit, 2.2);
						if (visit < 0) visit = 0;
						if (shortestTo[j + i * width] != shortestToLast[j + i * width])
						{
							visual.SpriteBatch.Draw(visual.Pixel, new Rectangle(j * size, i * size, size, size), new Color(255, 0, 0));
							//visual.SpriteBatch.Draw(visual.Pixel, new Rectangle(j * size + width * size, i * size, size, size), new Color(0, 0, 255));
						}
						else
						{
							if (shortestTo[j + i * width] == long.MaxValue)
								visual.SpriteBatch.Draw(visual.Pixel, new Rectangle(j * size, i * size, size, size), new Color(tile * 32, tile * 32, tile * 32));
							else
								visual.SpriteBatch.Draw(visual.Pixel, new Rectangle(j * size, i * size, size, size), new Color(visit, visit, visit));
							//visual.SpriteBatch.Draw(visual.Pixel, new Rectangle(j * size + width * size, i * size, size, size), new Color(visit, visit, visit));
						}
					}
				}
				StopDraw();
				Vsync();
				for (int i = 0; i < shortestTo.Length; i++)
				{
					shortestToLast[i] = shortestTo[i];
				}
			}

			long GetCost(int x, int y)
			{
				if (x < 0) return long.MaxValue;
				if (y < 0) return long.MaxValue;
				if (x >= width) return long.MaxValue;
				if (y >= height) return long.MaxValue;
				return shortestTo[x + y * width];
			}
		}
	}
}
