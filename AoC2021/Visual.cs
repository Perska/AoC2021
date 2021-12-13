using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AoC2021
{
	partial class Program
	{
		class Visual
		{
			public Form Window;
			public GraphicsDevice GraphicsDevice;

			public Texture2D Pixel;

			public SpriteBatch SpriteBatch;

			public RenderTarget2D RenderTarget;

			public Font Font;
			//private Thread thread;

			public const int WindowWidth = 1280;
			public const int WindowHeight = 720;
			public const int GraphicsWidth = WindowWidth;
			public const int GraphicsHeight = WindowHeight;

			public Visual()
			{
				Window = new Form();
				Window.ClientSize = new System.Drawing.Size(WindowWidth, WindowHeight);
				Window.MaximumSize = Window.Size;
				Window.MinimumSize = Window.Size;
				Window.MaximizeBox = false;

				PresentationParameters param = new PresentationParameters();

				param.DeviceWindowHandle = Window.Handle;
				param.BackBufferFormat = SurfaceFormat.Color;
				param.BackBufferWidth = WindowWidth;
				param.BackBufferHeight = WindowHeight;

				param.RenderTargetUsage = RenderTargetUsage.DiscardContents;
				param.IsFullScreen = false;

				param.MultiSampleCount = 0;

				param.DepthStencilFormat = DepthFormat.None;
				param.PresentationInterval = PresentInterval.Immediate;

				GraphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, param);

				SpriteBatch = new SpriteBatch(GraphicsDevice);

				Pixel = new Texture2D(GraphicsDevice, 1, 1);
				Pixel.SetData(new int[] { -1 });

				Font = new Font("gohu14", GraphicsDevice);

				RenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsWidth, GraphicsHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

				Window.Show();

				//Application.DoEvents();
				//Window
				//Application.Idle += Application_Idle;

				//Application.Run();
				//Application.Run(Window);
			}
			
			public void DrawBox(Rectangle rectangle, Color color)
			{
				SpriteBatch.Draw(Pixel, rectangle, color);
			}
			
			public void DrawLine(Vector2 from, Vector2 to, Color color)
			{
				DrawLine(from, to, color, 0, 0, WindowWidth - 1, WindowHeight - 1);
			}

			public void DrawLine(Vector2 from, Vector2 to, Color color, int minX, int minY, int maxX, int maxY)
			{
				from = (from - new Vector2(minX, minY)) / new Vector2(maxX - minX, maxY - minY) * new Vector2(WindowWidth - 1, WindowHeight - 1);
				to = (to - new Vector2(minX, minY)) / new Vector2(maxX - minX, maxY - minY) * new Vector2(WindowWidth - 1, WindowHeight - 1);
				SpriteBatch.Draw(Pixel, new Vector2(from.X, from.Y),
					null, color,
					(float)Math.Atan2(to.Y - from.Y, to.X - from.X) + (float)Math.PI * 2,
					Vector2.Zero,
					new Vector2((float)Math.Ceiling(Vector2.Distance(from, to)), 1),
					0, 0);
			}

		}
	}
}
