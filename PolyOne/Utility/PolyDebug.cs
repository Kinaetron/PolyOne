using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using PolyOne.Collision;

namespace PolyOne.Utility
{
    public static class PolyDebug
    {
        private static Texture2D blankTexture;

        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            blankTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blankTexture.SetData(new[] { Color.White });
        }

        public static void LoadContent(Texture2D blanTexture)
        {
            blankTexture = blanTexture;
        }

        static public void Log()
        {
            Debug.WriteLine("Log");
        }

        static public void Log(params object[] obj)
        {
            foreach (var o in obj)
            {
                if (o == null)
                {
                    Debug.WriteLine("null");
                }
                else
                {
                    Debug.WriteLine(o.ToString());
                }
            }
        }

        static public void LogEach<T>(IEnumerable<T> collection)
        {
            foreach (var o in collection)
            {
                Debug.WriteLine(o.ToString());
            }
        }

        static public void Dissect(Object obj)
        {
            Debug.Write(obj.GetType().Name + " { ");
            foreach (var v in obj.GetType().GetFields())
            {
                Debug.Write(v.Name + ": " + v.GetValue(obj) + ", ");
            }
            Debug.Write(" }");
        }

        static private Stopwatch stopwatch;

        static public void StartTimer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        static public void EndTimer()
        {
            if (stopwatch != null)
            {
                string message = "Timer: " + stopwatch.ElapsedTicks + " ticks, or " + TimeSpan.FromTicks(stopwatch.ElapsedTicks).TotalSeconds.ToString("00.0000000") + " seconds";
                Debug.WriteLine(message);
                stopwatch = null;
            }
        }

        static public Delegate GetMethod<T>(Object obj, string method) where T : class
        {
            var info = obj.GetType().GetMethod(method, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (info == null)
            {
                return null;
            }
            else
            {
                return Delegate.CreateDelegate(typeof(T), obj, method);
            }
        }

        public static void DrawLineSegment(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, int lineWidth)
        {

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blankTexture, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, lineWidth),
                       SpriteEffects.None, 0);
        }

        public static void DrawPolygon(this SpriteBatch spriteBatch, Vector2[] vertex, int count, Color color, int lineWidth)
        {
            if (count > 0)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    DrawLineSegment(spriteBatch, vertex[i], vertex[i + 1], color, lineWidth);
                }
                DrawLineSegment(spriteBatch, vertex[count - 1], vertex[0], color, lineWidth);
            }
        }

        public static void DrawCircle(this SpriteBatch spritbatch, Circle circle, Color color, int lineWidth, int segments = 32)
        {

            Vector2[] vertex = new Vector2[segments];

            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            for (int i = 0; i < segments; i++)
            {
                vertex[i] = circle.Centre + circle.Radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += increment;
            }

            DrawPolygon(spritbatch, vertex, segments, color, lineWidth);
        }
    }
}
