using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
	partial class Program
	{
		// [UseSRL] // Uncomment if you wanna use SuperReadLine
		[NoTrailingNewLine] // Uncomment to not include an extra blank line in the input at the end
		static void Day23(List<string> input)
		{
			char[,] map = new char[13, 5];
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 13; j++)
				{
					if (input[i].Length <= j) break;
					map[j,i] = input[i][j];
				}
			}

			int[] cost = new[] { 1, 10, 100, 1000 };

			/*char[,] burrow = new char[4, 2];
			for (int i = 0; i < 4; i++)
			{
				burrow[i, 0] = map[3 + i * 2, 2];
				burrow[i, 1] = map[3 + i * 2, 3];
			}*/

			(int X, int Y)[,] ams = new (int, int)[4, 2];
			bool[] has = new bool[4];
			//bool fa = false, fb = false, fc = false, fd = false;

			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Console.Write(Burrow(j, i));
					int id = ID(Burrow(j, i));

					if (!has[id])
					{
						ams[id, 0] = (3 + j * 2, 2 + i);
						has[id] = true;
					}
					else
					{
						ams[id, 1] = (3 + j * 2, 2 + i);
					}
				}
				Console.WriteLine();
			}

			for (int i = 0; i < 4; i++)
			{
				char who = Burrow(i, 0);
				int id = ID(who);
				for (int j = 0; j < 4; j++)
				{
					if (i == j) continue;
					if (j == id)
					{

					}
				}
			}

/*
#############
#...........#
###B#A#C#D###
  #A#B#C#D#
  #########

*/

			bool RoomComplete(int room)
			{
				return false;
			}

			int ID(char chr)
			{
				return chr - 'A';
			}

			char Burrow(int col, int row)
			{
				return map[3 + col * 2, 2 + row];
			}

			Queue<((int x, int y) target, int who, (int x, int y)[] positions, int cost)> pathfind;
			Queue<((int x, int y) target, int who, (int x, int y)[] positions, int cost)> pathfind2;

			/*void Paths(((int x, int y) target, int cost) route)
			{
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
							pathfind.Enqueue((target, tileCost));
					}
					target = (x, y + 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							pathfind.Enqueue((target, tileCost));
					}
					target = (x - 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							pathfind.Enqueue((target, tileCost));
					}
					target = (x, y - 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							pathfind.Enqueue((target, tileCost));
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
							pathfind.Enqueue((target, tileCost));
					}
					target = (x + 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							pathfind.Enqueue((target, tileCost));
					}
					target = (x, y - 1);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							pathfind.Enqueue((target, tileCost));
					}
					target = (x - 1, y);
					if ((tileCost = ReadMap(target.x, target.y)) != 10)
					{
						tileCost += route.cost;
						if (tileCost < worst && tileCost < shortestTo[target.x + target.y * width])
							pathfind.Enqueue((target, tileCost));
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
			}*/
		}
	}
}
