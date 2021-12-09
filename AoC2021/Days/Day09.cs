using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
	partial class Program
	{
		[NoTrailingNewLine]
		static void Day09(List<string> input)
		{
			int width = input[0].Length;
			int height = input.Count;
			int[] map = new int[width * height];
			
			List<(int height, int x, int y)> lowPoints = new List<(int, int, int)>();
			List<int> basins = new List<int>();

			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					map[i + j * width] = input[j][i] - '0';
				}
			}

			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					int current = map[i + j * width];
					if (current < ReadMap(i + 1, j) && current < ReadMap(i - 1, j) && current < ReadMap(i, j + 1) && current < ReadMap(i, j - 1)) lowPoints.Add((current, i, j));
				}
			}

			int danger = lowPoints.Sum(s => s.height + 1);
			Console.WriteLine(danger);

			foreach ((int height, int x, int y) point in lowPoints)
			{
				basins.Add(Flows(point.x, point.y, new bool[width * height]).Count(s => s));
			}
			basins.Sort();
			var biggest = basins.GetRange(basins.Count - 3, 3);

			Console.WriteLine(biggest[0] * biggest[1] * biggest[2]);

			bool[] Flows(int x, int y, bool[] visited)
			{
				int from = ReadMap(x, y);
				if (from >= 9) return visited;
				if (visited[x + y * width]) return visited;
				visited[x + y * width] = true;
				if ((ReadMap(x + 1, y) - from) > 0 && from < 8) { Flows(x + 1, y, visited); }
				if ((ReadMap(x, y + 1) - from) > 0 && from < 8) { Flows(x, y + 1, visited); }
				if ((ReadMap(x - 1, y) - from) > 0 && from < 8) { Flows(x - 1, y, visited); }
				if ((ReadMap(x, y - 1) - from) > 0 && from < 8) { Flows(x, y - 1, visited); }
				return visited;
			}

			int ReadMap(int x, int y)
			{
				if (x < 0) return 10;
				if (y < 0) return 10;
				if (x >= width) return 10;
				if (y >= height) return 10;
				return map[x + y * width];
			}
		}
	}
}
