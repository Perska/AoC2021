using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AoC2021
{
	partial class Program
	{
		static void Day17(List<string> input)
		{
			Regex regex = new Regex(@"^target area: x=(-?\d+|-)\.\.(-?\d+|-), y=(-?\d+|-)\.\.(-?\d+|-)$");
			Match match = regex.Match(input[0]);
			if (match.Success)
			{
				int x1 = int.Parse(match.Groups[1].Value), y1 = int.Parse(match.Groups[3].Value);
				int x2 = int.Parse(match.Groups[2].Value), y2 = int.Parse(match.Groups[4].Value);
				int sx = 0, sy = 0;
				List<int> potentialX = new List<int>();
				(int x, int y, List<int> visitsX, int? highest) data = (0, 0, null, null);
				while (sx <= x2)
				{
					if (ScanShoot(sx, sy, x1, y1, x2, y2)) potentialX.Add(sx);
					sx++;
				}

				int bestScore = 0;
				List<(int x, int y)> goodAims = new List<(int x, int y)>();
				for (int i = 0; i < potentialX.Count; i++)
				{
					sx = potentialX[i];
					for (int j = -1000; j < 1000; j++)
					{
						sy = j;
						if (j == 0) ;
						data = Shoot(sx, sy, x1, y1, x2, y2);
						if (data.highest.HasValue)
						{
							bestScore = Math.Max(bestScore, data.highest.Value);
							goodAims.Add((sx, sy));
						}
					}
				}
				Console.WriteLine($"Part 1: {bestScore}\nPart 2: {goodAims.Count}");
			}
			else
			{
				Console.WriteLine("Error with regex.");
			}

			bool ScanShoot(int vx, int vy, int x1, int y1, int x2, int y2)
			{
				int mx = 0, my = 0;
				int prevX = 0;
				while (mx <= x2)
				{
					mx += vx;
					my += vy;
					vx -= Math.Sign(vx);
					vy--;
					if (mx == prevX) break;
					prevX = mx;
					if (x1 <= mx && mx <= x2)
					{
						return true;
					}
				}
				return false;
			}

			(int x, int y, List<int> visitsX, int? highest) Shoot(int vx, int vy, int x1, int y1, int x2, int y2)
			{
				int mx = 0, my = 0;
				int highest = 0;
				while (mx <= x2 && my >= y1)
				{
					mx += vx;
					my += vy;
					vx -= Math.Sign(vx);
					vy--;
					highest = Math.Max(highest, my);
					if (x1 <= mx && y1 <= my && mx <= x2 && my <= y2)
					{
						return (mx, my, null, highest);
					}
				}
				return (mx, my, null, null);
			}
		}
	}
}
