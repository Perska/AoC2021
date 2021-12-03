using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
	partial class Program
	{
		[NoTrailingNewLine]
		static void Day03(List<string> input)
		{
			int bits = input[0].Length;
			int[,] bitsets = new int[bits,2];
			List<int> nums = new List<int>();

			foreach (string line in input)
			{
				if (line == "") continue;
				int num = Convert.ToInt32(line, 2);
				for (int i = 0; i < bits; i++)
				{
					if ((num & (1 << i)) != 0) bitsets[i, 0]++; else bitsets[i, 1]++;
				}
				nums.Add(num);
			}

			int gamma = 0;
			int epsilon = 0;
			for (int i = 0; i < bits; i++)
			{
				gamma |= (bitsets[i, 0] >= bitsets[i, 1]) ? 1 << i : 0;
				epsilon |= (bitsets[i, 0] < bitsets[i, 1]) ? 1 << i : 0;
			}

			Console.WriteLine($"Part 1: {epsilon * gamma}");

			List<int> current = nums;
			var oxygen = 0;
			for (int i = bits - 1; i >= 0; i--)
			{
				var bitCount = CountBits(current, i);
				if (bitCount.one >= bitCount.zero)
				{
					current = current.Where(num => (num & (1 << i)) != 0).ToList();
				}
				else
				{
					current = current.Where(num => (num & (1 << i)) == 0).ToList();
				}
				if (current.Count == 1)
				{
					oxygen = current.First();
					break;
				}
			}

			current = nums;
			var co2 = 0;
			for (int i = bits - 1; i >= 0; i--)
			{
				var bitCount = CountBits(current, i);
				if (bitCount.one < bitCount.zero)
				{
					current = current.Where(num => (num & (1 << i)) != 0).ToList();
				}
				else
				{
					current = current.Where(num => (num & (1 << i)) == 0).ToList();
				}
				if (current.Count == 1)
				{
					co2 = current.First();
					break;
				}
			}


			Console.WriteLine($"Part 2: {oxygen * co2}");

			(int one, int zero) CountBits(List<int> set, int bit)
			{
				int one = 0, zero = 0;
				foreach (int num in set)
				{
					if ((num & (1 << bit)) != 0) one++; else zero++;
				}
				return (one, zero);
			}
		}
	}
}
