using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Forms = System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace AoC2021
{
	public class Font
	{
		public Texture2D Texture { get; private set; }
		Rectangle[] boxes;
		
		public Font(string fontname, GraphicsDevice device)
		{
			Texture = embeddedTexture($"AoC2021.{fontname}.bmp", device);
			
			string[] box = null;
			Assembly assembly = Assembly.GetExecutingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream($"AoC2021.{fontname}.txt"))
			{
				using (StreamReader text = new StreamReader(stream))
				{
					box = ReadText(text).ToArray();
				}
			}

			IEnumerable<string> ReadText(TextReader reader)
			{
				string line;

				while ((line = reader.ReadLine()) != null)
				{
					yield return line;
				}
			}
			
			boxes = new Rectangle[65536];
			foreach (string line in box)
			{
				string[] split = line.Split(' ');
				if (split[0].Trim().StartsWith("#")) continue;
				if (split.Length == 5)
					boxes[int.Parse(split[0])] = new Rectangle(int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]), int.Parse(split[4]));
			}
		}

		public void Draw(SpriteBatch sp, string text, Vector2 position, Color color, int scale = 1)
		{
			int offsetX = 0;
			int perLineY = 0;
			int totalY = 0;
			bool mono = false;

			position = new Vector2((float)Math.Floor(position.X), (float)Math.Floor(position.Y));

			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == 10)
				{
					offsetX = 0;
					totalY += perLineY;
					perLineY = 0;
					perLineY = Math.Max(GetChar(text[i]).Height, perLineY);
				}
				else if (text[i] == '¤')
				{
					if (text[i + 1] == 'm' && (text.Length - i > 2))
					{
						mono = text[i + 2] == '1';
						i += 2;
					}
					else if (text[i + 1] == 'c' && (text.Length - i > 4))
					{

						color = new Color((text[i + 2] - 48) * 29, (text[i + 3] - 48) * 29, (text[i + 4] - 48) * 29);
						i += 4;
					}
				}
				else
				{
					sp.Draw(Texture, position + new Vector2(offsetX, totalY), GetChar(text[i]), color, 0, Vector2.Zero, scale, 0, 0);
					offsetX += (mono ? GetChar('#').Width : GetChar(text[i]).Width) * scale;
					perLineY = Math.Max(GetChar(text[i]).Height * scale, perLineY);
				}
			}
		}

		public Vector2 Measure(string text)
		{
			int offsetX = 0;
			int perLineY = 0;
			int totalY = 0;
			int width = 0;
			bool mono = false;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == 10)
				{
					width = Math.Max(width, offsetX);
					offsetX = 0;
					totalY += perLineY;
					perLineY = 0;
					perLineY = Math.Max(GetChar(text[i]).Height, perLineY);
				}
				else if (text[i] == '¤')
				{
					if (text[i + 1] == 'm' && (text.Length - i > 2))
					{
						mono = text[i + 2] == '1';
						i += 2;
					}
					else if (text[i + 1] == 'c' && (text.Length - i > 4))
					{
						i += 4;
					}
				}
				else
				{
					offsetX += mono ? GetChar('#').Width : GetChar(text[i]).Width;
					perLineY = Math.Max(GetChar(text[i]).Height, perLineY);
				}
			}
			width = Math.Max(width, offsetX);
			totalY += perLineY;
			return new Vector2(width, totalY);
		}

		public Vector2 CenteredOffset(string text)
		{
			Vector2 vec = Measure(text);
			return vec / 2 - vec;
		}
		public Vector2 CenteredOffsetW(string text)
		{
			Vector2 vec = Measure(text) * Vector2.UnitX;
			return vec / 2 - vec;
		}
		public Vector2 CenteredOffsetH(string text)
		{
			Vector2 vec = Measure(text) * Vector2.UnitY;
			return vec / 2 - vec;
		}


		public Rectangle GetChar(ushort index)
		{
			return boxes[index];
		}

		private Texture2D embeddedTexture(string filename, GraphicsDevice device)
		{
			Texture2D texture = null;
			try
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				Stream stream = assembly.GetManifestResourceStream(filename);
				texture = Texture2D.FromStream(device, stream);
				stream.Dispose();
			}
			catch (Exception e)
			{
				Forms.MessageBox.Show($"Could not read file ({filename}):\n" + e.ToString());
			}
			return texture;
		}
	}
}
