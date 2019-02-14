using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PolyOne.Utility
{
    public static class Resolution
    {
        static private int internalWidth;
        static private int internalHeight;
        static private int virtualWidth;
        static private int virtualHeight;

        static private Matrix scaleMatrix;
        static private bool internalFullScreen;
        static private bool dirtyMatrix = true;
        static private float internalScale;
        static private float fullscreenScale;

        static public Matrix GetScaleMatrix
        {
            get
            {
                if (dirtyMatrix == true)
                {
                    RecreateScaleMatrix();
                }
                return scaleMatrix;
            }
        }

        static public bool IsFullScreen
        {
            get { return internalFullScreen; }

            set
            {
                internalFullScreen = value;
                ApplyResolutionSettings();
            }
        }

        static public void SetResolution(int width, int height, bool fullScreen, float scale = 1.0f)
        {
            if (scale < 1.0f)
            {
                internalScale = 1.0f;
            }
            else
            {
                internalScale = scale;
            }

            internalWidth = width * (int)internalScale;
            internalHeight = height * (int)internalScale;

            virtualWidth = width;
            virtualHeight = height;

            internalFullScreen = fullScreen;

            ApplyResolutionSettings();
        }

        static public void SetResolution(int virtualWidthParam, int virtualHeightParam, int width, int height, bool fullScreen)
        {
            internalWidth = width;
            internalHeight = height;

            virtualWidth = virtualWidthParam;
            virtualHeight = virtualHeightParam;

            internalFullScreen = fullScreen;

            ApplyResolutionSettings();
        }

        static private void ApplyResolutionSettings()
        {
            if (internalFullScreen == false)
            {
                if ((internalWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) &&
                    (internalHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    internalWidth = Engine.Engine.VirtualWidth * (int)internalScale;
                    internalHeight = Engine.Engine.VirtualHeight * (int)internalScale;

                    Engine.Engine.Graphics.PreferredBackBufferWidth = internalWidth;
                    Engine.Engine.Graphics.PreferredBackBufferHeight = internalHeight;
                    Engine.Engine.Graphics.IsFullScreen = internalFullScreen;
                    Engine.Engine.Graphics.ApplyChanges();
                }
            }
            else
            {
                fullscreenScale = (float)Math.Floor((double)Math.Min((float)Engine.Engine.Instance.GraphicsDevice.DisplayMode.Width / (float)Engine.Engine.VirtualWidth,
                                                                     (float)Engine.Engine.Instance.GraphicsDevice.DisplayMode.Height / (float)Engine.Engine.VirtualHeight));

                internalWidth = Engine.Engine.VirtualWidth * (int)fullscreenScale;
                internalHeight = Engine.Engine.VirtualHeight * (int)fullscreenScale;

                Engine.Engine.Graphics.PreferredBackBufferWidth = Engine.Engine.Instance.GraphicsDevice.DisplayMode.Width;
                Engine.Engine.Graphics.PreferredBackBufferHeight = Engine.Engine.Instance.GraphicsDevice.DisplayMode.Height;
                Engine.Engine.Graphics.IsFullScreen = internalFullScreen;
                Engine.Engine.Graphics.ApplyChanges();

                ResetViewport();
            }

            dirtyMatrix = true;
            internalWidth = Engine.Engine.Graphics.PreferredBackBufferWidth;
            internalHeight = Engine.Engine.Graphics.PreferredBackBufferHeight;
        }

        static public void BeginDraw(Color colour)
        {
            FullViewport();
            Engine.Engine.Graphics.GraphicsDevice.Clear(Color.Black);
            ResetViewport();
            Engine.Engine.Graphics.GraphicsDevice.Clear(colour);
        }

        static private void RecreateScaleMatrix()
        {
            dirtyMatrix = true;
            scaleMatrix = Matrix.CreateScale(
                          (float)Engine.Engine.Graphics.GraphicsDevice.Viewport.Width / virtualWidth,
                          (float)Engine.Engine.Graphics.GraphicsDevice.Viewport.Height / virtualHeight,
                          1.0f);
        }

        static public void FullViewport()
        {
            Viewport viewport = new Viewport();
            viewport.X = viewport.Y = 0;
            viewport.Width = internalWidth;
            viewport.Height = internalHeight;
            Engine.Engine.Graphics.GraphicsDevice.Viewport = viewport;
        }

        static public float GetVirtualAspectRatio()
        {
            return (float)virtualWidth / (float)virtualHeight;
        }

        static public void ResetViewport()
        {
            float targetAspectRatio = GetVirtualAspectRatio();

            int width = Engine.Engine.Graphics.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRatio + 0.5f);

            bool changed = false;

            if (height > Engine.Engine.Graphics.PreferredBackBufferHeight)
            {
                height = Engine.Engine.Graphics.PreferredBackBufferHeight;
                width = (int)(height * targetAspectRatio + 0.5f);
                changed = true;
            }

            Viewport viewport = new Viewport();

            viewport.X = (Engine.Engine.Graphics.PreferredBackBufferWidth / 2) - (width / 2);
            viewport.Y = (Engine.Engine.Graphics.PreferredBackBufferHeight / 2) - (height / 2);
            viewport.Width = width;
            viewport.Height = height;

            if (changed == true)
            {
                dirtyMatrix = true;
            }

            Engine.Engine.Graphics.GraphicsDevice.Viewport = viewport;
        }
    }
}