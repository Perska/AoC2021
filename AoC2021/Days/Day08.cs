using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
	partial class Program
	{
		static void Day08(List<string> input)
		{
			List<string[]> inputs = new List<string[]>();
			List<string[]> outputs = new List<string[]>();

			List<int> deducedOutput = new List<int>();
			List<string[]> pinToNum = new List<string[]>();

			foreach (string line in input)
			{
				var split = line.Split('|');
				if (split.Length == 2)
				{
					inputs.Add(split[0].SplitToStringArray(' ', true));
					outputs.Add(split[1].SplitToStringArray(' ', true));
					pinToNum.Add(new string[10]);
				}
			}

			for (int i = 0; i < inputs.Count; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					char[] str = inputs[i][j].ToArray();
					Array.Sort(str);
					inputs[i][j] = new string(str);
				}
				for (int j = 0; j < 4; j++)
				{
					char[] str = outputs[i][j].ToArray();
					Array.Sort(str);
					outputs[i][j] = new string(str);
				}
			}

			Console.WriteLine($"Part 1: {outputs.Sum(line => line.Count(disp => disp.Length == 2 || disp.Length == 4 || disp.Length == 3 || disp.Length == 7))}");

			for (int i = 0; i < inputs.Count; i++)
			{
				var pin = pinToNum[i];
				// The digits 1, 4, 7 and 8 all have distinct segment counts, and thus can be instantly deduced.
				pin[1] = inputs[i].First(disp => disp.Length == 2);
				pin[4] = inputs[i].First(disp => disp.Length == 4);
				pin[7] = inputs[i].First(disp => disp.Length == 3);
				pin[8] = inputs[i].First(disp => disp.Length == 7);

				// We'll make two groups. One for digits that use 5 segments, and one for digits that use 6.
				List<string> group235 = inputs[i].Where(disp => disp.Length == 5).ToList();
				List<string> group069 = inputs[i].Where(disp => disp.Length == 6).ToList();

				// We'll take the segments that's used for one...
				string one = pin[1];
				// And find a set of segments that uses both the segments in one, from the 235 group. That's 3.
				pin[3] = group235.First(disp => disp.Contains(one[0]) && disp.Contains(one[1]));
				group235.Remove(pin[3]); // Remove 3 from the group, so it won't interfere with future deductions.
				
				// This time, we'll find a set of segments from the 069 group that include one of the segments from one, but *not* both. That's 6.
				pin[6] = group069.First(disp => disp.Contains(one[0]) ^ disp.Contains(one[1]));
				group069.Remove(pin[6]); // Remove 6 from the group.

				// Take the segments used in four.
				string four = pin[4];
				
				// Find the segments that use all segments from four in the 09 group. That's 9.
				pin[9] = group069.First(disp => disp.Contains(four[0]) && disp.Contains(four[1]) && disp.Contains(four[2]) && disp.Contains(four[3]));
				group069.Remove(pin[9]); // With 9 removed from the group...

				pin[0] = group069[0]; // The remaining segment set in the group must be 0!

				// Get the single intersecting segment between one and six
				string oneSix = one.Intersect(pin[6]).First().ToString();

				// Find a set of segments that include the one segment from the intersection of 0 and 6. Oh, wouldn't that happen to be 5?
				pin[5] = group235.First(disp => disp.Contains(oneSix));
				group235.Remove(pin[5]); // So... We remove 5 from the group, and all we have left is the...

				pin[2] = group235[0]; // Segments for 2!

				int digit1 = Array.IndexOf(pin, outputs[i][0]);
				int digit2 = Array.IndexOf(pin, outputs[i][1]);
				int digit3 = Array.IndexOf(pin, outputs[i][2]);
				int digit4 = Array.IndexOf(pin, outputs[i][3]);

				deducedOutput.Add(digit1 * 1000 + digit2 * 100 + digit3 * 10 + digit4);

				pinToNum[i] = pin;
			}

			Console.WriteLine($"Part 2: {deducedOutput.Sum()}");
		}
	}
}
