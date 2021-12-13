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

			
		}
	}
}
