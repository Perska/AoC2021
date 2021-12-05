using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2021
{
	partial class Program
	{
		static void Day05(List<string> input)
		{
			Regex regex = new Regex(@"^(\d+),(\d+) -> (\d+),(\d+)$");

			bool part2 = false;
		start:
			Dictionary<(int, int), int> map = new Dictionary<(int, int), int>();

			int maxX = 0, maxY = 0;

			foreach (string line in input)
			{
				if (line == "") continue;
				if (regex.IsMatch(line))
				{
					Match match = regex.Match(line);
					int x1, y1, x2, y2;
					x1 = int.Parse(match.Groups[1].Value);
					y1 = int.Parse(match.Groups[2].Value);
					x2 = int.Parse(match.Groups[3].Value);
					y2 = int.Parse(match.Groups[4].Value);

					maxX = Math.Max(Math.Max(x1, x2), maxX);
					maxY = Math.Max(Math.Max(y1, y2), maxY);

					
					if (x1 == x2 && y1 == y2)
					{
						map[(x1, y1)] = gmap(x1, y1) + 1;
					}
					else if (x1==x2 && y1 != y2)
					{
						if (y1 > y2) (y1, y2) = (y2, y1);
						for (int i = y1; i <= y2; i++)
						{
							map[(x1, i)] = gmap(x1, i) + 1;
						}
					}
					else if (x1 != x2 && y1 == y2)
					{
						if (x1 > x2) (x1, x2) = (x2, x1);
						for (int i = x1; i <= x2; i++)
						{
							map[(i, y1)] = gmap(i, y1) + 1;
						}
					}
					else if (x1 != x2 && y1 != y2 && part2)
					{
						int dist = Math.Abs(x2 - x1);

						for (int i = 0; i <= dist; i++)
						{
							if (x1 < x2 && y1 < y2)
							{
								map[(x1 + i, y1 + i)] = gmap(x1 + i, y1 + i) + 1;
							}
							else if (x1 < x2 && y1 > y2)
							{
								map[(x1 + i, y1 - i)] = gmap(x1 + i, y1 - i) + 1;
							}
							else if (x1 > x2 && y1 < y2)
							{
								map[(x1 - i, y1 + i)] = gmap(x1 - i, y1 + i) + 1;
							}
							else if (x1 > x2 && y1 > y2)
							{
								map[(x1 - i, y1 - i)] = gmap(x1 - i, y1 - i) + 1;
							}
						}
					}

				}
			}

			if (maxX <= 20 && maxY <= 20)
			{
				for (int i = 0; i <= maxY; i++)
				{
					for (int j = 0; j <= maxX; j++)
					{
						Console.Write(gmap(j, i));
					}
					Console.WriteLine();
				}
			}

			Console.WriteLine(map.Count(kv => kv.Value > 1));

			if (!part2)
			{
				part2 = true;
				goto start;
			}

			int gmap(int x, int y)
			{
				if (map.ContainsKey((x, y)))
				{
					return map[(x, y)];
				}
				else
				{
					return 0;
				}
			} 
		}
	}
}
