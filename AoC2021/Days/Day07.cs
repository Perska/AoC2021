using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
	partial class Program
	{
		[UseSRL]
		static void Day07(List<string> input)
		{
			int[] positions = Array.ConvertAll(input[0].Split(','), s => int.Parse(s));
			int[] positionsEx = new int[positions.Length];
			positions.CopyTo(positionsEx, 0);

			Dictionary<int, long> positionFuel = new Dictionary<int, long>();
			Dictionary<int, long> positionFuelEx = new Dictionary<int, long>();
			Dictionary<int, long> exponentialFuel = new Dictionary<int, long>();

			int min = positions.Min();
			int max = positions.Max();

			for (int i = min; i <= max; i++)
			{
				long currentFuel = 0;
				long currentFuelEx = 0;
				for (int j = 0; j < positions.Length; j++)
				{
					currentFuel += Math.Abs(positions[j] - i);
					currentFuelEx += GetFuel(positionsEx[j], i);
				}
				positionFuel[i] = currentFuel;
				positionFuelEx[i] = currentFuelEx;
			}

			long GetFuel(int a, int b)
			{
				if (exponentialFuel.ContainsKey(Math.Abs(a - b))) return exponentialFuel[Math.Abs(a - b)];
				long x = 0;
				for (int i = 0; i < Math.Abs(a - b); i++)
				{
					x += i + 1;
				}
				exponentialFuel[Math.Abs(a - b)] = x;
				return x;
			}

			KeyValuePair<int, long> smallestFuel = positionFuel.OrderBy(kvp => kvp.Value).First();
			KeyValuePair<int, long> smallestFuelEx = positionFuelEx.OrderBy(kvp => kvp.Value).First();
			Console.WriteLine($"Part 1: {smallestFuel.Value}\nPart 2: {smallestFuelEx.Value}");
		}
	}
}
