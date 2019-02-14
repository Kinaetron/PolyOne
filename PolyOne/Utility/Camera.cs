using System;
using Microsoft.Xna.Framework;

namespace PolyOne.Utility
{
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }
        private float zoom;

        public Matrix TransformMatrix
        {
            get
            {
                if (Resolution.GetScaleMatrix == null)
                {
                    return Matrix.CreateTranslation(new Vector3((float)Math.Round(-Position.X), (float)Math.Round(-Position.Y), 0f)) *
                           Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
                }
                else
                {
                    return Matrix.CreateTranslation(new Vector3((float)Math.Round(-Position.X), (float)Math.Round(-Position.Y), 0f)) *
                                                   (Resolution.GetScaleMatrix);
                }
            }
        }

        public void ClampToArea(int width, int height)
        {
            if (Position.X > width)
                Position.X = width;
            if (Position.Y > height)
                Position.Y = height;

            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;
        }
    }
}