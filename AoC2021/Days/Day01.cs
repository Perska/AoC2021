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
			for (int i = 1; i < input.Count - 2; i++)
			{
				int next = int.Parse(input[i]) + int.Parse(input[i + 1]) + int.Parse(input[i + 2]);
				if (current < next) differing++;
				current = next;
			}

			Console.WriteLine($"Part 2: {differing} increased measurements!");
			
			StartDraw();
			visual.SpriteBatch.Begin();
			for (int i = 0; i < input.Count - 1; i++)
			{
				DrawLine(new Vector2(i, 0), new Vector2(i, 1), new Color(16, 16, 16), 0, 0, input.Count - 1, 1);
			}

			visual.Font.Draw(visual.SpriteBatch, visMin.ToString(), new Vector2(Visual.WindowWidth / 2, 4) + visual.Font.CenteredOffsetW(visMin.ToString()), Color.White);
			visual.Font.Draw(visual.SpriteBatch, visMax.ToString(), new Vector2(Visual.WindowWidth / 2, Visual.WindowHeight - 18) + visual.Font.CenteredOffsetW(visMax.ToString()), Color.White);
			visual.SpriteBatch.End();
			StopDraw();

			current = int.Parse(input[0]);
			int current2 = int.Parse(input[0]) + int.Parse(input[1]) + int.Parse(input[2]);
			for (int i = 1; i < input.Count; i++)
			{
				int next = int.Parse(input[i]);
				int next2 = current2;
				if (i < input.Count - 2)
					next2 = int.Parse(input[i]) + int.Parse(input[i + 1]) + int.Parse(input[i + 2]);

				StartDraw();
				visual.SpriteBatch.Begin();

				DrawLineX(new Vector2(i - 1, current), new Vector2(i, next), current < next, current2 < next2, Color.Yellow, Color.White, Color.Green, Color.Blue, 0, visMin, input.Count - 1, visMax);
				//if (current < next) DrawLine()
				visual.SpriteBatch.End();
				StopDraw();
				current = next;
				current2 = next2;
			}

			/*for (int i = 1; i < input.Count - 2; i++)
			{
				current = next;
			}*/

			//Draw(Draw1, new { from = new Vector2(0, 0), to = new Vector2(640, 480) });

			
			void DrawLineX(Vector2 from, Vector2 to, bool increased, bool increased2, Color colorA, Color colorB, Color colorC, Color colorD, int minX, int minY, int maxX, int maxY)
			{
				DrawLine(from, to, increased ? (increased2 ? colorC : colorA) : (increased2 ? colorD : colorB), minX, minY, maxX, maxY);
			}
		}

		static void DrawLine(Vector2 from, Vector2 to, Color color, int minX, int minY, int maxX, int maxY)
		{
			from = (from - new Vector2(minX, minY)) / new Vector2(maxX - minX, maxY - minY) * new Vector2(Visual.GraphicsWidth, Visual.WindowHeight);
			to = (to - new Vector2(minX, minY)) / new Vector2(maxX - minX, maxY - minY) * new Vector2(Visual.GraphicsWidth, Visual.WindowHeight);
			visual.SpriteBatch.Draw(visual.Pixel, new Vector2(from.X, from.Y),
				null, color,
				(float)Math.Atan2(to.Y - from.Y, to.X - from.X) + (float)Math.PI * 2, Vector2.Zero, new Vector2(Vector2.Distance(from, to), 1), 0, 0);
			//Console.WriteLine(Math.Atan2(to.Y - from.Y, to.X - from.X) / Math.PI * 180);
		}

	}
}
