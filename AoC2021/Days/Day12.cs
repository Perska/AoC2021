using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
	partial class Program
	{
		[NoTrailingNewLine]
		static void Day12(List<string> input)
		{
			Dictionary<string, List<string>> caveSystem = new Dictionary<string, List<string>>();
			List<List<string>> paths = new List<List<string>>();

			foreach (string path in input)
			{
				string[] pa = path.Split('-');
				if (!caveSystem.ContainsKey(pa[0]))
				{
					caveSystem[pa[0]] = new List<string>();
				}
				if (!caveSystem.ContainsKey(pa[1]))
				{
					caveSystem[pa[1]] = new List<string>();
				}
				caveSystem[pa[0]].Add(pa[1]);
				caveSystem[pa[1]].Add(pa[0]);
			}

			Paths(new List<string> { "start" }, true);
			Console.WriteLine($"Part 1: {paths.Count}");
			paths.Clear();

			Paths(new List<string> { "start" }, false);
			Console.WriteLine($"Part 2: {paths.Count}");

			void Paths(List<string> route, bool usedRevisit)
			{
				if (route.Last() == "end")
				{
					paths.Add(route);
					return;
				}
				foreach (var direction in caveSystem[route.Last()])
				{
					if (!(route.Contains(direction) && direction == direction.ToLowerInvariant()))
					{
						var newRoute = new List<string>(route) { direction };
						Paths(newRoute, usedRevisit);
					}
					else if (route.Contains(direction) && direction == direction.ToLowerInvariant() && !usedRevisit && direction != "start")
					{
						var newRoute = new List<string>(route) { direction };
						Paths(newRoute, true);
					}
				}
			}

		}
	}
}
