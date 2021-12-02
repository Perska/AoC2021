using System;
using System.Collections.Generic;

namespace AoC2021
{
	partial class Program
	{
		static void Day02(List<string> input)
		{
			long x = 0;
			long y = 0;
			long y2 = 0;
			long aim = 0;
			foreach (string line in input)
			{
				var cmd = line.Split(' ');
				switch (cmd[0])
				{
					case "forward":
						x += int.Parse(cmd[1]);
						y2 += aim * int.Parse(cmd[1]);
						break;
					case "down":
						y += int.Parse(cmd[1]);
						aim += int.Parse(cmd[1]);
						break;
					case "up":
						y -= int.Parse(cmd[1]);
						aim -= int.Parse(cmd[1]);
						break;
					default:
						break;
				}
			}
			Console.WriteLine($"Part 1: {x * y}\nPart 2: {x * y2}");
		}
	}
}
