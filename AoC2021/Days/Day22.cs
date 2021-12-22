using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace AoC2021
{
	partial class Program
	{
		struct Cuboid
		{
			public int X1;
			public int X2;
			public int Y1;
			public int Y2;
			public int Z1;
			public int Z2;

			public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2)
			{
				X1 = x1;
				X2 = x2;
				Y1 = y1;
				Y2 = y2;
				Z1 = z1;
				Z2 = z2;
			}

			public bool Valid
			{
				get
				{
					return (X1 <= X2 && Y1 <= Y2 && Z1 <= Z2);
				}
			}

			public long Width
			{
				get
				{
					return X2 - X1 + 1;
				}
			}

			public long Height
			{
				get
				{
					return Y2 - Y1 + 1;
				}
			}

			public long Depth
			{
				get
				{
					return Z2 - Z1 + 1;
				}
			}

			public long Volume
			{
				get
				{
					return Width * Height * Depth;
				}
			}

			public override string ToString()
			{
				return $"{X1}..{X2}, {Y1}..{Y2}, {Z1}..{Z2}";
			}

			public static Cuboid Intersection(Cuboid a, Cuboid b)
			{
				return new Cuboid(Math.Max(a.X1, b.X1), Math.Min(a.X2, b.X2), Math.Max(a.Y1, b.Y1), Math.Min(a.Y2, b.Y2), Math.Max(a.Z1, b.Z1), Math.Min(a.Z2, b.Z2));
			}

			public static Cuboid[] Abjunction(Cuboid a, Cuboid b)
			{
				Cuboid intersection = Intersection(a, b);
				if (intersection.Valid)
				{
					List<Cuboid> cuboids = new List<Cuboid>();
					if (intersection.X1 > a.X1)
					{
						cuboids.Add(new Cuboid(a.X1, intersection.X1 - 1, a.Y1, a.Y2, a.Z1, a.Z2));
					}
					if (intersection.X2 < a.X2)
					{
						cuboids.Add(new Cuboid(intersection.X2 + 1, a.X2, a.Y1, a.Y2, a.Z1, a.Z2));
					}
					if (intersection.Y1 > a.Y1)
					{
						cuboids.Add(new Cuboid(intersection.X1, intersection.X2, a.Y1, intersection.Y1 - 1, a.Z1, a.Z2));
					}
					if (intersection.Y2 < a.Y2)
					{
						cuboids.Add(new Cuboid(intersection.X1, intersection.X2, intersection.Y2 + 1, a.Y2, a.Z1, a.Z2));
					}
					if (intersection.Z1 > a.Z1)
					{
						cuboids.Add(new Cuboid(intersection.X1, intersection.X2, intersection.Y1, intersection.Y2, a.Z1, intersection.Z1 - 1));
					}
					if (intersection.Z2 < a.Z2)
					{
						cuboids.Add(new Cuboid(intersection.X1, intersection.X2, intersection.Y1, intersection.Y2, intersection.Z2 + 1, a.Z2));
					}
					return cuboids.ToArray();
				}
				else
				{
					return new Cuboid[] { a };
				}
			}
		}
		
		[NoTrailingNewLine]
		static void Day22(List<string> input)
		{
			Regex regex = new Regex(@"^(on|off) x=(-?\d+|-)\.\.(-?\d+|-),y=(-?\d+|-)\.\.(-?\d+|-),z=(-?\d+|-)\.\.(-?\d+|-)$");

			Console.WriteLine($"Part 1: {DoTheThing(false)}");
			Console.WriteLine($"Part 2: {DoTheThing(true)}");

			long DoTheThing(bool part2)
			{
				List<Cuboid> cuboids = new List<Cuboid>();
				List<Cuboid> cuboids2 = new List<Cuboid>();
				foreach (string line in input)
				{
					Match match = regex.Match(line);
					if (match.Success)
					{
						bool setOn = match.Groups[1].Value == "on";
						(int x1, int x2, int y1, int y2, int z1, int z2) = (
							int.Parse(match.Groups[2].Value),
							int.Parse(match.Groups[3].Value),
							int.Parse(match.Groups[4].Value),
							int.Parse(match.Groups[5].Value),
							int.Parse(match.Groups[6].Value),
							int.Parse(match.Groups[7].Value)
						);
						Cuboid newCuboid = new Cuboid(x1, x2, y1, y2, z1, z2);
						if (!part2 && (x1 < -50 || x2 > 50 || y1 < -50 || y2 > 50 || z1 < -50 || z2 > 50)) continue;
						for (int i = 0; i < cuboids.Count; i++)
						{
							cuboids2.AddRange(Cuboid.Abjunction(cuboids[i], newCuboid));
						}
						if (setOn)
							cuboids2.Add(newCuboid);
						(cuboids, cuboids2) = (cuboids2, cuboids);
						cuboids2.Clear();
					}
				}

				return cuboids.Sum(s => s.Volume);
			}
		}
	}
}
