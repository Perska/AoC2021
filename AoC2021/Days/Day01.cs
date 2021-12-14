using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AoC2021
{
	partial class Program
	{
		[NoTrailingNewLine]
		[HasVisual]
		static void Day01(List<string> input)
		{
			int current = int.Parse(input[0]);
			int differing = 0;
			int visMin = current, visMax = current;
			for (int i = 1; i < input.Count; i++)
			{
				int next = int.Parse(input[i]);
				if (current < next) differing++;
				current = next;
				visMin = Math.Min(visMin, current);
				visMax = Math.Max(visMax, current);
			}
			Console.WriteLine($"Part 1: {differing} increased measurements!");

			


			current = int.Parse(input[0]) + int.Parse(input[1]) + int.Parse(input[2]);
			differing = 0;
			int visMin2 = current, visMax2 = current;
			for (int i = 1; i < input.Count - 2; i++)
			{
				int next = int.Parse(input[i]) + int.Parse(input[i + 1]) + int.Parse(input[i + 2]);
				if (current < next) differing++;
				current = next;
				visMin2 = Math.Min(visMin2, current);
				visMax2 = Math.Max(visMax2, current);
			}

			Console.WriteLine($"Part 2: {differing} increased measurements!");
			
			StartDraw(false);
			if (input.Count < 1000)
			{
				for (int i = 0; i < input.Count - 1; i++)
				{
					visual.DrawLine(new Vector2(i, 0), new Vector2(i, 1), new Color(16, 16, 16), 0, 0, input.Count - 1, 1);
				}
			}

			visual.Font.Draw(visual.SpriteBatch, visMin.ToString(), new Vector2(Visual.WindowWidth / 2, 4) + visual.Font.CenteredOffsetW(visMin.ToString()), Color.White);
			visual.Font.Draw(visual.SpriteBatch, visMax.ToString(), new Vector2(Visual.WindowWidth / 2, Visual.WindowHeight - 18) + visual.Font.CenteredOffsetW(visMax.ToString()), Color.White);

			visual.DrawBox(new Rectangle(1104, 16, 160, 86), Color.White);
			visual.DrawBox(new Rectangle(1105, 17, 158, 84), Color.Black);
			if (input.Count < 500)
			{
				visual.Font.Draw(visual.SpriteBatch, "Definitions:\nNo increase P1\nNo increase P2\nIncrease in part 1\nIncrease in part 2", new Vector2(1112, 24), Color.White);
				visual.DrawLine(new Vector2(1236, 45), new Vector2(1252, 45), Color.White);
				visual.DrawLine(new Vector2(1236, 59), new Vector2(1252, 59), Color.Gray);
				visual.DrawLine(new Vector2(1236, 73), new Vector2(1252, 73), Color.Yellow);
				visual.DrawLine(new Vector2(1236, 87), new Vector2(1252, 87), Color.Green);
			}
			else
			{
				visual.Font.Draw(visual.SpriteBatch, "Definitions:\nNo increase\nIncrease in part 1\nIncrease in part 2\nIncrease in both", new Vector2(1112, 24), Color.White);
				visual.DrawLine(new Vector2(1236, 45), new Vector2(1252, 45), Color.White);
				visual.DrawLine(new Vector2(1236, 59), new Vector2(1252, 59), Color.Yellow);
				visual.DrawLine(new Vector2(1236, 73), new Vector2(1252, 73), Color.Blue);
				visual.DrawLine(new Vector2(1236, 87), new Vector2(1252, 87), Color.Green);
			}
			StopDraw();

			StartDraw(false);
			current = int.Parse(input[0]);
			int current2 = int.Parse(input[0]) + int.Parse(input[1]) + int.Parse(input[2]);
			for (int i = 1; i < input.Count; i++)
			{
				int next = int.Parse(input[i]);
				int next2 = current2;
				if (i < input.Count - 2)
					next2 = int.Parse(input[i]) + int.Parse(input[i + 1]) + int.Parse(input[i + 2]);

				if (input.Count < 500)
				{
					visual.DrawLine(new Vector2(i - 1, current), new Vector2(i, next), current < next ? Color.Yellow : Color.White, 0, visMin, input.Count - 1, visMax);
					visual.DrawLine(new Vector2(i - 1, current2), new Vector2(i, next2), current2 < next2 ? Color.Green : Color.Gray, 0, visMin2, input.Count - 1, visMax2);
				}
				else
				{
					DrawLineX(new Vector2(i - 1, current), new Vector2(i, next), current < next, current2 < next2, Color.Yellow, Color.White, Color.Green, Color.Blue, 0, visMin, input.Count - 1, visMax);
				}
				
				current = next;
				current2 = next2;
			}
			StopDraw();
			

			void DrawLineX(Vector2 from, Vector2 to, bool increased, bool increased2, Color colorA, Color colorB, Color colorC, Color colorD, int minX, int minY, int maxX, int maxY)
			{
				visual.DrawLine(from, to, increased ? (increased2 ? colorC : colorA) : (increased2 ? colorD : colorB), minX, minY, maxX, maxY);
			}
		}
	}
}
