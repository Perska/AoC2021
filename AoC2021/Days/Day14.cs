using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AoC2021
{
	partial class Program
	{
		[HasVisual]
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

			int round;
			int draw;
			for (int i = 0; i < 10; i++)
			{
				round = i + 1;
				Mutate();
			}
			var counts = howMany.OrderBy(i => i.Value).ToList();
			Console.WriteLine(counts.Last().Value - counts.First().Value);

			

			for (int i = 10; i < 40; i++)
			{
				round = i + 1;
				Mutate();
			}
			counts = howMany.OrderBy(i => i.Value).ToList();
			Console.WriteLine(counts.Last().Value - counts.First().Value);
			frameSpeed = 2000;
			Vsync();
			Vsync();

			StartDraw(true);
			draw = 0;
			foreach (var chr in counts)
			{
				visual.Font.Draw(visual.SpriteBatch, $"{chr.Key}\n{chr.Value}", new Vector2(draw * 128, 16), Color.White);
				draw++;
			}
			string solve = $"{counts.Last().Value} - {counts.First().Value} = {counts.Last().Value - counts.First().Value}";
			visual.Font.Draw(visual.SpriteBatch, solve, new Vector2(640, 48) + visual.Font.CenteredOffsetW(solve), Color.White);
			StopDraw();

			frameSpeed = 100;

			void Mutate()
			{
				frameSpeed = 1000 / round / round;
				var newPairs = new Dictionary<string, long>();
				Vsync();
				foreach (var pair in pairCount.OrderBy(s => s.Key))
				{
					StartDraw(true);
					visual.Font.Draw(visual.SpriteBatch, $"Round {round}", Vector2.Zero, Color.White);
					draw = 0;
					foreach (var chr in howMany)
					{
						visual.Font.Draw(visual.SpriteBatch, $"{chr.Key}\n{chr.Value}", new Vector2(draw * 128, 16), Color.White);
						draw++;
					}
					if (pairRules.ContainsKey(pair.Key))
					{
						char result = pairRules[pair.Key][0];
						string pairA = $"{pair.Key[0]}{result}";
						string pairB = $"{result}{pair.Key[1]}";
						newPairs[pairA] = newPairs.Read(pairA) + pair.Value;
						newPairs[pairB] = newPairs.Read(pairB) + pair.Value;
						howMany[result] = howMany.Read(result) + pair.Value;

						string disp = $"{pairA} and {pairB} increase by {pair.Key}'s {pair.Value}. Current amount of polymers: {howMany.Sum(s => s.Value)}";
						visual.Font.Draw(visual.SpriteBatch, disp, new Vector2(640, 48) + visual.Font.CenteredOffsetW(disp), Color.White);
					}
					else
					{
						newPairs[pair.Key] = newPairs[pair.Key];
					}

					draw = 0;
					foreach (var pair2 in pairCount.OrderBy(s => s.Key))
					{
						visual.Font.Draw(visual.SpriteBatch, $"{pair2.Key}{(pair.Key == pair2.Key ? " (Current)" : "")}\n{pair2.Value}", new Vector2((draw % 5) * 120, 64 + draw / 5 * 32), Color.White);
						visual.Font.Draw(visual.SpriteBatch, $"{pair2.Key}{(pair.Key == pair2.Key ? " (Current)" : "")}\n{(newPairs.ContainsKey(pair2.Key) ? newPairs[pair2.Key] : pair2.Value)}", new Vector2((draw % 5) * 120 + 640, 64 + draw / 5 * 32), Color.White);
						draw++;
					}
					foreach (var pair2 in newPairs.Where(s => !pairCount.ContainsKey(s.Key)))
					{
						visual.Font.Draw(visual.SpriteBatch, $"{pair2.Key} (New)\n{pair2.Value}", new Vector2((draw % 5) * 120 + 640, 64 + draw / 5 * 32), Color.White);
						draw++;
					}

					StopDraw();
					Vsync();
				}
				pairCount = newPairs;
			}
		}
	}
}
