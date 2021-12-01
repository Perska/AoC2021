using System;
using System.Collections.Generic;

namespace AoC2021
{
	partial class Program
	{
		static void Day01(List<string> input)
		{
			int current = int.Parse(input[0]);
			int differing = 0;
			for (int i = 1; i < input.Count - 1; i++)
			{
				int next = int.Parse(input[i]);
				if (current < next) differing++;
				current = next;
			}
			Console.WriteLine($"Part 1: {differing} increased measurements!");

			current = int.Parse(input[0]) + int.Parse(input[1]) + int.Parse(input[1]);
			differing = 0;
			for (int i = 1; i < input.Count - 3; i++)
			{
				int next = int.Parse(input[i]) + int.Parse(input[i + 1]) + int.Parse(input[i + 2]);
				if (current < next) differing++;
				current = next;
			}

			Console.WriteLine($"Part 2: {differing} increased measurements!");
		}
	}
}
