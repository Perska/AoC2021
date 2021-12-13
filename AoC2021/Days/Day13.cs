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
			var map = new Dictionary<(long, long), bool>();
			var newMap = new Dictionary<(long, long), bool>();

			int line = 0;
			while (input[line] != "")
			{
				var dot = input[line].SplitToLongArray(",");
				map[(dot[0], dot[1])] = true;
				line++;
			}
			line++;

			Regex regex = new Regex(@"^fold along ([xy])=(\d+)$");
			
			void Fold()
			{
				Match match = regex.Match(input[line]);
				if (match == null) return;
				long where = long.Parse(match.Groups[2].Value);
				newMap.Clear();
				if (match.Groups[1].Value == "x")
				{
					foreach (var dot in map)
					{
						long x = dot.Key.Item1;
						long y = dot.Key.Item2;
						if (x < where)
						{
							newMap[(x, y)] = true;
						}
						else if (x > where)
						{
							newMap[(where * 2 - x, y)] = true;
						}
					}
				}
				else
				{
					foreach (var dot in map)
					{
						long x = dot.Key.Item1;
						long y = dot.Key.Item2;
						if (y < where)
						{
							newMap[(x, y)] = true;
						}
						else if (y > where)
						{
							newMap[(x, where * 2 - y)] = true;
						}
					}
				}
				(map, newMap) = (newMap, map);
				line++;
			}

			Fold();

			Console.WriteLine($"Part 1: {map.Count}");

			long width = map.Max(s => s.Key.Item1);;
			long height = map.Max(s => s.Key.Item2);;
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


			bool Read(long x, long y)
			{
				if (map.ContainsKey((x, y))) return map[(x, y)];
				return false;
			}
		}
	}
}
