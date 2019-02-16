using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using PolyOne.Input;
using PolyOne.Utility;

namespace PolyOne.Engine
{
    public class Engine : Game
    {
        static public Engine Instance { get; private set; }
        static public GraphicsDeviceManager Graphics { get; private set; }
        static public int VirtualWidth { get; private set; }
        static public int VirtualHeight { get; private set; }
        static public int ActualWidth { get; private set; }
        static public int ActualHeight { get; private set; }
        static public float DeltaTime { get; private set; }
        static public float DeltaTimeMilli { get; private set; }
        static public TimeSpan TotalTime { get; private set; }
        static public SpriteBatch SpriteBatch { get; private set; }
        static public Color ClearColor;
        static public bool ExitOnEscapeKeyPress;
        public static ContentManager MenuContentManager;
        public static float FreezeTimer;

        private static float scaleStat;
        private string windowTitle;

#if DEBUG
        private bool debugInformation = false;
        //private SpriteFont debugFont;

        private TimeSpan counterElapsed = TimeSpan.Zero;
        private int frameCount = 0;
        private int FPS = 0;
#endif

        public Engine(int width, int height, string windowTitle, float scale = 1.0f, bool isFullScreen = false)
        {
            Instance = this;

            scaleStat = (int)scale;
            VirtualWidth = width;
            VirtualHeight = height;
            ActualWidth = width * (int)scaleStat;
            ActualHeight = height * (int)scaleStat;

            Window.Title = this.windowTitle = windowTitle;
            ClearColor = Color.Black;

            Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            MenuContentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            Resolution.SetResolution(VirtualWidth, VirtualHeight, isFullScreen, scaleStat);

            IsMouseVisible = false;
            ExitOnEscapeKeyPress = true;

            base.IsFixedTimeStep = true;
        }
        protected override void Initialize()
        {
            base.Initialize();

            Tracker.Initialize();
            PolyInput.Initialize();
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            //debugFont = Content.Load<SpriteFont>("MenuAssets/menufont");
        }

        protected override void Update(GameTime gameTime)
        {
            DeltaTimeMilli = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TotalTime = gameTime.TotalGameTime;

            counterElapsed += gameTime.ElapsedGameTime;

            PolyInput.Update(gameTime);

            if (PolyInput.Keyboard.Pressed(Microsoft.Xna.Framework.Input.Keys.P)) {
                debugInformation = !debugInformation;
            }

            if (ExitOnEscapeKeyPress == true &&
               PolyInput.Keyboard.Pressed(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                Exit();
                return;
            }

            base.Update(gameTime);

            if (counterElapsed > TimeSpan.FromSeconds(1))
            {
                counterElapsed -= TimeSpan.FromSeconds(1);
                FPS = frameCount;
                frameCount = 0;
            }
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            PolyInput.ShutDown();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearColor);

            base.Draw(gameTime);
#if DEBUG
            //if (debugInformation == true)
            //{
            //    frameCount++;
            //    Begin();
            //    SpriteBatch.DrawString(debugFont, "FPS: " + FPS.ToString(), new Vector2(20, 40), Color.White);
            //    SpriteBatch.DrawString(debugFont, "Frame Time: " + DeltaTime.ToString(), new Vector2(20, 70), Color.White);
            //    SpriteBatch.DrawString(debugFont, "Total Memory: " + (GC.GetTotalMemory(true) / 1048576f).ToString("F") + " MB ", new Vector2(20, 100), Color.White);

            //    End();
            //}
#endif
        }

        public static void Begin()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                                   SamplerState.PointClamp, DepthStencilState.None,
                                   RasterizerState.CullNone);
        }

        public static void Begin(Matrix Transform)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                              SamplerState.PointClamp,
                              DepthStencilState.None,
                              RasterizerState.CullNone, null,
                              Transform);
        }

        public static void BeginParallax()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                                 SamplerState.PointWrap, DepthStencilState.None,
                                 RasterizerState.CullNone);
        }

        public static void BeginParallax(Matrix Transform)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                             BlendState.AlphaBlend,
                             SamplerState.PointWrap,
                             DepthStencilState.Default,
                             RasterizerState.CullNone, null,
                             Transform);
        }

        public static void End()
        {
            SpriteBatch.End();
        }
    }
}