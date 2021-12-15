using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
	partial class Program
	{
		[NoTrailingNewLine]
		static void Day15(List<string> input)
		{
			int width = input[0].Length;
			int height = input.Count;
			int[] map = new int[width * height];
			long[] shortestTo = new long[width * height];
			List<long> paths = new List<long>();

			for (int i = 0; i < shortestTo.Length; i++)
			{
				shortestTo[i] = long.MaxValue;
			}

			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					map[i + j * width] = input[j][i] - '0';
				}
			}

			long worst = 9 * 200;

			Queue<((int x, int y) target, long cost)> upLeft     = new Queue<((int x, int y) target, long cost)>();
			Queue<((int x, int y) target, long cost)> rightDown  = new Queue<((int x, int y) target, long cost)>();
			Queue<((int x, int y) target, long cost)> upLeft2    = new Queue<((int x, int y) target, long cost)>();
			Queue<((int x, int y) target, long cost)> rightDown2 = new Queue<((int x, int y) target, long cost)>();
			

			Paths(((0, 0), 0));
			
			while (rightDown.Count > 0 || upLeft.Count > 0)
			{
				(rightDown, rightDown2) = (rightDown2, rightDown);
				while (rightDown2.Count > 0)
				{
					Paths(rightDown2.Dequeue());
				}
				(upLeft, upLeft2) = (upLeft2, upLeft);
				while (upLeft2.Count > 0)
				{
					Paths(upLeft2.Dequeue());
				}
			}
			Console.WriteLine(paths.Min());


			int[] mapOld;
			(map, mapOld) = (new int[width * height * 25], map);
			shortestTo = new long[width * height * 25];
			for (int i = 0; i < shortestTo.Length; i++)
			{
				shortestTo[i] = long.MaxValue;
			}
			paths.Clear();
			worst = 9 * width + 8 + width + 7 * width + 6 * width + 5 * width + 9 * height + 8 + height + 7 * height + 6 * height + 5 * height;

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
			while (rightDown.Count > 0 || upLeft.Count > 0)
			{
				(rightDown, rightDown2) = (rightDown2, rightDown);
				while (rightDown2.Count > 0)
				{
					Paths(rightDown2.Dequeue());
				}
				(upLeft, upLeft2) = (upLeft2, upLeft);
				while (upLeft2.Count > 0)
				{
					Paths(upLeft2.Dequeue());
				}
			}
			Console.WriteLine(paths.Min());
			PlotPath();

			void Paths(((int x, int y) target, long cost) route)
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
						rightDown.Enqueue((target, route.cost + tileCost));
					}
					target = (x, y + 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						rightDown.Enqueue((target, route.cost + tileCost));
					}
					target = (x - 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						upLeft.Enqueue((target, route.cost + tileCost));
					}
					target = (x, y - 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						upLeft.Enqueue((target, route.cost + tileCost));
					}
				}
				else
				{
					int tileCost;
					(int x, int y) target;
					target = (x, y + 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						rightDown.Enqueue((target, route.cost + tileCost));
					}
					target = (x + 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						rightDown.Enqueue((target, route.cost + tileCost));
					}
					target = (x, y - 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						upLeft.Enqueue((target, route.cost + tileCost));
					}
					target = (x - 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						upLeft.Enqueue((target, route.cost + tileCost));
					}
				}
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
				Console.Clear();
				int sx = width - 1, sy = height - 1;
				while (!(sx == 0 && sy == 0))
				{
					Console.SetCursorPosition(sx, sy);
					Console.Write("#");
					List<(long cost, (int x, int y) pos)> where = new List<(long cost, (int x, int y) pos)>
					{
						(GetCost(sx + 1, sy), (sx + 1, sy)),
						(GetCost(sx, sy + 1), (sx, sy + 1)),
						(GetCost(sx - 1, sy), (sx - 1, sy)),
						(GetCost(sx, sy - 1), (sx, sy - 1))
					};
					where = where.OrderBy(s => s.cost).ToList();
					(sx, sy) = where.First().pos;
					;
				}
				Console.SetCursorPosition(sx, sy);
				Console.Write("#");

				Console.SetCursorPosition(0, height);
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
