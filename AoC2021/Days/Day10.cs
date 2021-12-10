using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
	partial class Program
	{
		static void Day10(List<string> input)
		{
			Stack<char> nest = new Stack<char>();

			Dictionary<char, char> pairs = new Dictionary<char, char>
			{
				{ ')', '(' },
				{ ']', '[' },
				{ '}', '{' },
				{ '>', '<' },
			};

			Dictionary<char,int> badScore = new Dictionary<char, int>
			{
				{ ')', 3 },
				{ ']', 57 },
				{ '}', 1197 },
				{ '>', 25137 },
			};

			Dictionary<char, int> incScore = new Dictionary<char, int>
			{
				{ '(', 1 },
				{ '[', 2 },
				{ '{', 3 },
				{ '<', 4 },
			};

			List<string> incomplete = new List<string>();

			long score = 0;

			foreach (string line in input)
			{
				bool corrupt = false;
				nest.Clear();
				for (int i = 0; i < line.Length; i++)
				{
					if (!pairs.ContainsKey(line[i]))
					{
						nest.Push(line[i]);
					}
					else if (nest.Peek() != pairs[line[i]])
					{
						score += badScore[line[i]];
						corrupt = true;
						break;
					}
					else
					{
						nest.Pop();
					}
				}
				if (!corrupt) incomplete.Add(line);

			}

			List<long> scores = new List<long>();

			foreach (string line in incomplete)
			{
				long score2 = 0;
				nest.Clear();
				for (int i = 0; i < line.Length; i++)
				{
					if (!pairs.ContainsKey(line[i]))
					{
						nest.Push(line[i]);
					}
					else
					{
						nest.Pop();
					}
				}
				while (nest.Count > 0)
				{
					score2 = score2 * 5 + incScore[nest.Pop()];
				}
				scores.Add(score2);
			}

			scores.Sort();

			Console.WriteLine($"Part 1: {score}\nPart 2{scores[scores.Count / 2]}");

		}
	}
}
