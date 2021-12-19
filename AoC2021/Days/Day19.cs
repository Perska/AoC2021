using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
	partial class Program
	{
		class SparseMatrix3D<T>
		{
			public Dictionary<(int X, int Y, int Z), T> Map { get; private set; }

			public SparseMatrix3D()
			{
				Map = new Dictionary<(int, int, int), T>();
			}

			public T this[int x, int y, int z]
			{
				get
				{
					return Map.ContainsKey((x, y, z)) ? Map[(x, y, z)] : default(T);
				}
				set
				{
					Map[(x, y, z)] = value;
				}
			}

			public void Join(SparseMatrix3D<T> with, (int X, int Y, int Z) off)
			{
				foreach (var item in with.Map)
				{
					this[item.Key.X - off.X, item.Key.Y - off.Y, item.Key.Z - off.Z] = item.Value;
				}
			}
		}

		// [UseSRL] // Uncomment if you wanna use SuperReadLine
		// [NoTrailingNewLine] // Uncomment to not include an extra blank line in the input at the end
		static void Day19(List<string> input)
		{
			List<SparseMatrix3D<bool>[]> maps = new List<SparseMatrix3D<bool>[]>();
			SparseMatrix3D<bool> map = null;
			for (int i = 0; i < input.Count; i++)
			{
				if (input[i].StartsWith("---"))
				{
					maps.Add(new SparseMatrix3D<bool>[24] { map = new SparseMatrix3D<bool>(), null, null, null, null, null,
																					null, null, null, null, null, null,
																					null, null, null, null, null, null,
																					null, null, null, null, null, null});
				}
				else if (input[i] != "")
				{
					var coords = input[i].SplitToIntArray(",");
					(int x, int y, int z) = (coords[0], coords[1], coords[2]);
					map[x, y, z] = true;
				}
			}
			for (int i = 0; i < maps.Count; i++)
			{
				map = maps[i][0];
				for (int j = 0; j < 6; j++)
				{
					for (int k = 0; k < 4; k++)
					{
						if (j == 0 && k == 0) continue;
						SparseMatrix3D<bool> rot = maps[i][j + k * 6] = new SparseMatrix3D<bool>();
						foreach (var item in map.Map)
						{
							(int x, int y, int z) = item.Key;
							switch (j)
							{
								case 1:
									(x, y, z) = (x, z, -y);
									break;
								case 2:
									(x, y, z) = (x, -y, -z);
									break;
								case 3:
									(x, y, z) = (x, -z, y);
									break;
								case 4:
									(x, y, z) = (-y, x, z);
									break;
								case 5:
									(x, y, z) = (y, -x, z);
									break;
							}
							switch (k)
							{
								case 1:
									(x, y, z) = (z, y, -x);
									break;
								case 2:
									(x, y, z) = (-x, y, -z);
									break;
								case 3:
									(x, y, z) = (-z, y, x);
									break;
							}
							rot[x, y, z] = true;
						}
					}
				}
			}

			List<((int A, int B, int C), (int X, int Y, int Z))> connections = new List<((int A, int B, int C), (int X, int Y, int Z))>();

			for (int k = 0; k < maps.Count; k++)
			{
				var baseMap = maps[k][0];
				for (int i = 1; i < maps.Count; i++)
				{
					if (k == i) continue;
					for (int j = 0; j < maps[i].Length; j++)
					{
						map = maps[i][j];
						foreach (var a in map.Map)
						{
							foreach (var b in baseMap.Map)
							{
								(int x, int y, int z) = (a.Key.X - b.Key.X, a.Key.Y - b.Key.Y, a.Key.Z - b.Key.Z);
								int counted = 0;
								int inspected = 0;
								foreach (var item in map.Map)
								{
									(int sx, int sy, int sz) = item.Key;
									if (baseMap[sx - x, sy - y, sz - z])
									{
										counted++;
									}
									inspected++;
									if (counted >= 12)
									{
										connections.Add(((k, i, j), (x, y, z)));
										goto foundMatch;
									}
									if (map.Map.Count - inspected + counted < 12)
									{
										break;
									}
								}
							}
						}
					foundMatch:;
					}
				}
			}
			;
			

			SparseMatrix3D<bool> deducedMap = new SparseMatrix3D<bool>();
			bool[] onMap = new bool[maps.Count];
			deducedMap.Join(maps[0][0], (0, 0, 0));
			onMap[0] = true;
			((int X, int Y, int Z) pos, int rot)[] scannerSpots = new ((int X, int Y, int Z), int rot)[maps.Count];
			scannerSpots[0] = ((0, 0, 0), 0);

			//List<(SparseMatrix3D<bool>, int)[]> left = new List<SparseMatrix3D<bool>[]>(maps.Where(s => (s, 0)));
			//left.RemoveAt(0);
			while (onMap.Contains(true))
			{
				for (int i = 0; i < maps.Count; i++)
				{
					if (i == 2)
						;
					if (onMap[i]) continue;
					var where = connections.FirstOrDefault(s => s.Item1.B == i && onMap[s.Item1.A]);
					if (where.Item1.A == where.Item1.B) continue;
					// = where.Item2;
					var offset = scannerSpots[where.Item1.A];
					(int x, int y, int z) = UnRotateVector(where.Item2, offset.rot);
					(x, y, z) = (x + offset.pos.X, y + offset.pos.Y, z + offset.pos.Z);
					for (int j = 0; j < maps[i].Length; j++)
					{
						map = maps[i][j];
						//foreach (var a in map.Map)
						//{
						//foreach (var b in deducedMap.Map)
						//{
							//(int x, int y, int z) = (a.Key.X - b.Key.X, a.Key.Y - b.Key.Y, a.Key.Z - b.Key.Z);
							int counted = 0;
							int inspected = 0;
							foreach (var item in map.Map)
							{
								(int sx, int sy, int sz) = item.Key;
								if (deducedMap[sx - x, sy - y, sz - z])
								{
									counted++;
								}
								inspected++;
								if (counted >= 12)
								{
								scannerSpots[i] = ((x, y, z), j);
									deducedMap.Join(map, (x, y, z));
									onMap[i] = true;
									//left.Remove(left[i]);
									goto foundMatch;
								}
								if (map.Map.Count - inspected + counted < 12)
								{
									break;
								}
								//	}
							//}
						}
					}
				}
			foundMatch:;
			}
			Console.WriteLine(deducedMap.Map.Count);
			int highest = 0;
			foreach (var a in scannerSpots)
			{
				foreach (var b in scannerSpots)
				{
					highest = Math.Max(highest, Manhattan(a.pos, b.pos));
				}
			}
			Console.WriteLine(highest);

			int Manhattan((int X, int Y, int Z) a, (int X, int Y, int Z) b)
			{
				return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
			}

			(int X, int Y, int Z) RotateVector((int X, int Y, int Z) vector, int rotation)
			{
				(int x, int y, int z) = vector;
				switch (rotation % 6)
				{
					case 1:
						(x, y, z) = (x, z, -y);
						break;
					case 2:
						(x, y, z) = (x, -y, -z);
						break;
					case 3:
						(x, y, z) = (x, -z, y);
						break;
					case 4:
						(x, y, z) = (-y, x, z);
						break;
					case 5:
						(x, y, z) = (y, -x, z);
						break;
				}
				switch (rotation / 6 % 3)
				{
					case 1:
						(x, y, z) = (z, y, -x);
						break;
					case 2:
						(x, y, z) = (-x, y, -z);
						break;
					case 3:
						(x, y, z) = (-z, y, x);
						break;
				}
				return (x, y, z);
			}

			(int X, int Y, int Z) UnRotateVector((int X, int Y, int Z) vector, int rotation)
			{
				(int x, int y, int z) = vector;
				switch (rotation / 6 % 3)
				{
					case 1:
						(x, y, z) = (-z, y, x);
						break;
					case 2:
						(x, y, z) = (-x, y, -z);
						break;
					case 3:
						(x, y, z) = (z, y, -x);
						break;
				}
				switch (rotation % 6)
				{
					case 1:
						(x, y, z) = (x, -z, y);
						break;
					case 2:
						(x, y, z) = (x, -y, -z);
						break;
					case 3:
						(x, y, z) = (x, z, -y);
						break;
					case 4:
						(x, y, z) = (y, -x, z);
						break;
					case 5:
						(x, y, z) = (-y, x, z);
						break;
				}
				return (x, y, z);
			}


		}
	}
}
