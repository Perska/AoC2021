using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2021
{
	partial class Program
	{
		static void Day14(List<string> input)
		{
			string template = input[0];
			Dictionary<string, string> pairRules = new Dictionary<string, string>();
			Dictionary<string, long> pairCount = new Dictionary<string, long>();
			Dictionary<char, long> howMany = new Dictionary<char, long>();
			int line = 2;
			Regex regex = new Regex(@"^(\w\w) -> (\w)$");
			while (input[line] != "")
			{
				Match match = regex.Match(input[line]);
				if (!match.Success) { line++; continue; }
				pairRules[match.Groups[1].Value] = match.Groups[2].Value;
				line++;
			}
			for (int i = 0; i < template.Length; i++)
			{
				howMany[template[i]] = howMany.Read(template[i]) + 1;
			}
			for (int i = 1; i < template.Length; i++)
			{
				string pair;
				pairCount[pair = $"{template[i - 1]}{template[i]}"] = pairCount.Read(pair) + 1;
			}
			for (int i = 0; i < 10; i++)
			{
				Mutate();
			}
			var counts = howMany.OrderBy(i => i.Value).ToList();
			Console.WriteLine(counts.Last().Value - counts.First().Value);

			for (int i = 10; i < 40; i++)
			{
				Mutate();
			}
			counts = howMany.OrderBy(i => i.Value).ToList();
			Console.WriteLine(counts.Last().Value - counts.First().Value);
			
			void Mutate()
			{
				var newPairs = new Dictionary<string, long>();
				foreach (var pair in pairCount)
				{
					if (pairRules.ContainsKey(pair.Key))
					{
						char result = pairRules[pair.Key][0];
						string pairA = $"{pair.Key[0]}{result}";
						string pairB = $"{result}{pair.Key[1]}";
						newPairs[pairA] = newPairs.Read(pairA) + pair.Value;
						newPairs[pairB] = newPairs.Read(pairB) + pair.Value;
						howMany[result] = howMany.Read(result) + pair.Value;
					}
					else
					{
						newPairs[pair.Key] = newPairs[pair.Key];
					}
				}
				pairCount = newPairs;
			}
		}
	}
}
