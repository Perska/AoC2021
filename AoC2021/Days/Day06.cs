using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
	partial class Program
	{
		[UseSRL]
		static void Day06(List<string> input)
		{
			long[] fish = new long[9];

			string[] init = input[0].Split(',');

			foreach (string item in init)
			{
				int slot = int.Parse(item);
				fish[slot]++;
			}

			int lastCycle = 0;

			long SimulateFish(int untilCycles)
			{
				int i;
				for (i = lastCycle; i < untilCycles; i++)
				{
					long addFish = 0;
					for (int j = 0; j < 9; j++)
					{
						if (j == 0)
						{
							addFish = fish[0];
							fish[0] = 0;
						}
						else
						{
							fish[j - 1] += fish[j];
							fish[j] = 0;
						}
					}
					fish[6] += addFish;
					fish[8] = addFish;
					//Console.WriteLine($"Day {i + 1}: {string.Join(",", Array.ConvertAll(fish, s => s.ToString()))}");
				}
				lastCycle = i;

				return fish.Sum();
			}

			Console.WriteLine($"Part 1: {SimulateFish(80)}");
			Console.WriteLine($"Part 2: {SimulateFish(256)}");
		}
	}
}
