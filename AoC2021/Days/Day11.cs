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
		static void Day11(List<string> input)
		{
			int width = input[0].Length;
			int height = input.Count;
			int[] octopi = new int[width * height];
			bool[] flashed = new bool[width * height];

			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					octopi[i + j * width] = input[j][i] - '0';
				}
			}

			int flashCount = 0;
			int step = 0;

			void SimulateUntil(int steps)
			{
				for (; step < steps; step++)
				{
					Simulate();
				}
			}

			int Simulate()
			{
				for (int j = 0; j < height; j++)
				{
					for (int i = 0; i < width; i++)
					{
						octopi[i + j * width]++;
					}
				}
				bool flashes = false;

				flashed = new bool[width * height];
				do
				{
					flashes = false;
					for (int j = 0; j < height; j++)
					{
						for (int i = 0; i < width; i++)
						{
							if (octopi[i + j * width] > 9 && !flashed[i + j * width])
							{
								flashes = true;
								octopi[i + j * width]++;
								flashed[i + j * width] = true;
								flashCount++;
								for (int x = i - 1; x <= i + 1; x++)
								{
									for (int y = j - 1; y <= j + 1; y++)
									{
										if (x < 0) continue;
										if (y < 0) continue;
										if (x >= width) continue;
										if (y >= height) continue;
										if (x == i && y == j) continue;

										octopi[x + y * width]++;
									}
								}
							}
						}
					}
				} while (flashes);
				for (int j = 0; j < height; j++)
				{
					for (int i = 0; i < width; i++)
					{
						if (octopi[i + j * width] > 9) octopi[i + j * width] = 0;
						//if (step < 10) Console.Write(octopi[i + j * width]);
					}
					//if (step < 10) Console.WriteLine();
				}
				//if (step < 10) Console.WriteLine(flashCount);
				return flashed.Count(s => s);
			}

			SimulateUntil(100);

			Console.WriteLine($"Part 1: {flashCount}");
			
			do
			{
				step++;
			}
			while (Simulate() != 100);
			Console.WriteLine($"Part 2: {step}");
		}
	}
}
