using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
	partial class Program
	{
		[UseSRL]
		static void Day16(List<string> input)
		{
			string preData = string.Join("", input.ToArray());
			string data;
			StringBuilder builder = new StringBuilder(preData.Length * 4);
			for (int i = 0; i < preData.Length; i++)
			{
				builder.Append($"{Convert.ToString(Convert.ToInt32(preData[i].ToString(), 16), 2).PadLeft(4, '0')}");
			}
			data = builder.ToString();
			int index = 0;
			//Console.WriteLine(data);

			long versionSum = 0;
			long result = ParsePacket();

			Console.WriteLine($"Part 1: {versionSum}\nPart 2: {result}");

			long ParsePacket()
			{
				int version = CharEater(3);
				int type = CharEater(3);
				versionSum += version;
				StringBuilder output = new StringBuilder();
				if (type == 4)
				{
					bool cont = CharEater(1) == 1;
					while (true)
					{
						output.Append(BitEater(4));
						if (!cont) break;
						cont = CharEater(1) == 1;
					}
					return Convert.ToInt64(output.ToString(), 2);
				}
				else
				{
					bool subtype = CharEater(1) == 1;
					int subLength = CharEater(subtype ? 11 : 15);
					int last = index;
					int packets = 0;
					List<long> values = new List<long>();
					while (true)
					{
						values.Add(ParsePacket());
						packets++;
						if (subtype && (packets == subLength)) break;
						else if (!subtype && (last + subLength) <= index) break;
					}
					switch (type)
					{
						case 0:
							return values.Sum();
						case 1:
							return values.Aggregate((p, n) => p * n);
						case 2:
							return values.Min();
						case 3:
							return values.Max();
						case 5:
							return values[0] > values[1] ? 1 : 0;
						case 6:
							return values[0] < values[1] ? 1 : 0;
						case 7:
							return values[0] == values[1] ? 1 : 0;
						default:
							return -1;
					}
				}
			}


			string BitEater(int howMany)
			{
				string res = data.Substring(index, howMany);
				//Console.WriteLine($"Read {data.Substring(index, howMany)}");
				index += howMany;
				return res;
			}

			int CharEater(int howMany)
			{
				int res = Convert.ToInt32(data.Substring(index, howMany), 2);
				//Console.WriteLine($"Read {data.Substring(index, howMany)} for {res}");
				index += howMany;
				return res;
			}
		}
	}
}
