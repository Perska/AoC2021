using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2021
{
	partial class Program
	{
		static void Day13(List<string> input)
		{
			var map = new Dictionary<(int, int), bool>();
			var newMap = new Dictionary<(int, int), bool>();

			int width = 0, height = 0;
			int line = 0;
			while (input[line] != "")
			{
				var dot = input[line].SplitToIntArray(",");
				map[(dot[0], dot[1])] = true;
				width = Math.Max(width, dot[0]);
				height = Math.Max(height, dot[1]);
				line++;
			}
			line++;

			Regex regex = new Regex(@"^fold along ([xy])=(\d+)$");
			
			void Fold()
			{
				Match match = regex.Match(input[line]);
				if (match == null) return;
				int where = int.Parse(match.Groups[2].Value);
				newMap.Clear();
				if (match.Groups[1].Value == "x")
				{
					Console.WriteLine($"{where}/{width}");
					for (int i = 0; i < where; i++)
					{
						for (int j = 0; j <= height; j++)
						{
							if (Read(i, j) || Read(where * 2 - i, j))
							{
								newMap[(i, j)] = true;
							}
						}
					}
				}
				else
				{
					Console.WriteLine($"{where}/{height}");
					for (int i = 0; i < where; i++)
					{
						for (int j = 0; j <= width; j++)
						{
							if (Read(j, i) || Read(j, where * 2 - i))
							{
								newMap[(j, i)] = true;
							}
						}
					}
				}
				(map, newMap) = (newMap, map);
				width = map.Max(s => s.Key.Item1);
				height = map.Max(s => s.Key.Item2);
				line++;
			}

			Fold();

			Console.WriteLine(map.Count);
			
			while (input[line] != "")
			{
				Fold();
			}
			for (int j = 0; j <= map.Max(s => s.Key.Item2); j++)
			{
				for (int i = 0; i <= map.Max(s => s.Key.Item1); i++)
				{
					Console.Write(Read(i, j) ? "#" : " ");
				}
				Console.WriteLine();
			}


			bool Read(int x, int y)
			{
				if (map.ContainsKey((x, y))) return map[(x, y)];
				return false;
			}
		}
	}
}
