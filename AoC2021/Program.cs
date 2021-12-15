using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
	partial class Program
	{
		public delegate void DayProgram(List<string> input);

		public static List<DayProgram> days = new List<DayProgram>
		{
			Day01, Day02, Day03, Day04, Day05, Day06, Day07, Day08, Day09,
			Day10, Day11, Day12, Day13, Day14, Day15, 
		};


		public class UseSRLAttribute : Attribute
		{

		}

		public class NoTrailingNewLineAttribute : Attribute
		{

		}

		[STAThread]
		static void Main()
		{
			bool keepGoing = true;
			
			//Console.WriteLine(SuperReadLine());
			while (keepGoing)
			{
				Console.WriteLine("Enter the day you want to run the program for (enter 0 to stop)");
				if (int.TryParse(Console.ReadLine(), out int day))
				{
					DayProgram program = null;
					if (day == 0)
					{
						keepGoing = false;
					}
					else if (1 <= day && day <= days.Count)
					{
						program = days[day - 1];
					}
					else
					{
						Console.WriteLine($"Program for Day {day} is not implemented.");
					}

					if (program != null)
					{
						bool useSRL = program.Method.GetCustomAttributes(typeof(UseSRLAttribute), false).Any();
						bool trailingNewLine = !program.Method.GetCustomAttributes(typeof(NoTrailingNewLineAttribute), false).Any();
						List<string> input = new List<string>();
						Console.WriteLine("Please enter the program input. Once done, enter \"end\"\n(To instantly read the clipboard input, type \"paste\")");
						while (true)
						{
							string line = useSRL ? SuperReadLine() : Console.ReadLine();
							if (line.Length == 254) Console.WriteLine("Line was 254 characters long... Coincidence or is SuperReadLine required?");
							if (line.ToLowerInvariant() == "end") break;
							if (line.ToLowerInvariant() == "paste")
							{
								if (System.Windows.Forms.Clipboard.ContainsText())
								{
									string clipboard = System.Windows.Forms.Clipboard.GetText();
									input.AddRange(clipboard.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None));
									Console.WriteLine($"Successfully read {clipboard.Length} characters of text.");
									break;
								}
								else
								{
									Console.WriteLine("The clipboard didn't contain any text.");
									continue;
								}
							}
							input.Add(line);
						}
						//input.RemoveAll(item => item.Length == 0);
						if (input.LastOrDefault() != "" && trailingNewLine)
						{
							input.Add("");
						}
						if (input.Last() == "" && !trailingNewLine)
						{
							input.RemoveAt(input.Count - 1);
						}
						program(input);
					}
				}
				else
				{
					Console.WriteLine("Not a valid number");
				}
				Console.WriteLine();
			}
		}

		static string SuperReadLine()
		{
			char key;
			StringBuilder input = new StringBuilder();
			int x = Console.CursorLeft, y = Console.CursorTop;
			while ((key = Console.ReadKey().KeyChar) != 13)
			{
				if (key == 8 && input.Length > 0)
				{
					input.Remove(input.Length - 1, 1);
					Console.SetCursorPosition((x + input.Length) % Console.BufferWidth, y + (x + input.Length) / Console.BufferWidth);
					Console.Write('\0');
					Console.SetCursorPosition((x + input.Length) % Console.BufferWidth, y + (x + input.Length) / Console.BufferWidth);
				}
				else if (key != 8)
				{
					input.Append(key);
				}
			}
			Console.SetCursorPosition(0, y + (x + input.Length) / Console.BufferWidth + 1);
			return input.ToString();
		}
	}
}
