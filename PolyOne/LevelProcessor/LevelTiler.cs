using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace PolyOne.LevelProcessor
{

    public class Entity
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public Vector2 Position { get; private set; }
        public Dictionary<string, string> Properties = new Dictionary<string, string>();

        public Entity(string name, string type, Vector2 position)
        {
            Name = name;
            Type = type;
            Position = position;
        }

        public Entity(string name, string type, Vector2 position, Dictionary<string, string> properties)
        {
            Name = name;
            Type = type;
            Position = position;
            Properties = properties;
        }
    }

    public class LevelTiler
    {
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }

        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        public float MapWidthInPixels
        {
            get { return MapWidth * TileWidth; }
        }

        public float MapHeightInPixels
        {
            get { return MapHeight * TileHeight; }
        }

        public Vector2 MapSizeInPixels;

        public List<Vector2> PlayerPosition { get; private set; }

        public List<Entity> Entites = new List<Entity>();

        public int[,] CollisionLayer { get; private set; }

        public string BackgroundSong { get; private set; }

        private Texture2D foregroundImage;
        private Texture2D midgroundImage;
        private Texture2D backgroundImage;

        private Vector2 foregroundPosition;
        private Vector2 midgroundPosition;
        private Vector2 backgroundPosition;

        private Texture2D tileSet;
        private int tileSetWidth;
        private int tileSetHeight;
        private int xOffSet = 100;

        int[,] foregroundLayer;
        int[,] backgroundLayer;

        public void LoadContent(LevelData data)
        {
            MapWidth = data.Width;
            MapHeight = data.Height;

            TileWidth = data.Tilewidth;
            TileHeight = data.TileHeight;

            PlayerPosition = new List<Vector2>();

            MapSizeInPixels = new Vector2(MapWidthInPixels, MapHeightInPixels);

            foreach (Layer layer in data.Layers)
            {
                if (layer.Name == "ForegroundImage")
                {
                    foregroundImage = Engine.Engine.Instance.Content.Load<Texture2D>(layer.Image);
                    foregroundPosition = new Vector2(layer.X, layer.Y);
                }

                if (layer.Name == "MidgroundImage")
                {
                    midgroundImage = Engine.Engine.Instance.Content.Load<Texture2D>(layer.Image);
                    midgroundPosition = new Vector2(layer.X, layer.Y);
                }

                if (layer.Name == "BackgroundImage")
                {
                    backgroundImage = Engine.Engine.Instance.Content.Load<Texture2D>(layer.Image);
                    backgroundPosition = new Vector2(layer.X, layer.Y);
                }

                if (layer.Name == "CollisionLayer")
                {
                    CollisionLayer = layer.MapData;
                }

                if (layer.Name == "ForegroundLayer")
                {
                    foregroundLayer = layer.MapData;
                }

                if (layer.Name == "BackgroundLayer")
                {
                    backgroundLayer = layer.MapData;
                }

                if (layer.Name == "Entites")
                {
                    foreach (var obj in layer.Objects)
                    {
                        if (obj.Type == "Player")
                        {
                            PlayerPosition.Add(new Vector2(obj.X, obj.Y));
                        }
                        else if (obj.Type == "Song")
                        {
                            BackgroundSong = obj.Name;
                        }
                        else if (obj.Properties.Count <= 0)
                        {
                            Entites.Add(new Entity(obj.Name, obj.Type, new Vector2(obj.X, obj.Y)));
                        }
                        else
                        {
                            Entites.Add(new Entity(obj.Name, obj.Type, new Vector2(obj.X, obj.Y), obj.Properties));
                        }
                    }
                }
            }

            tileSet = Engine.Engine.Instance.Content.Load<Texture2D>(data.TileSets[0].Image);
            tileSetWidth = data.TileSets[0].Width;
            tileSetHeight = data.TileSets[0].Height;
        }

        private void DrawForegroundTiles()
        {
            for (int x = 0; x < foregroundLayer.GetLongLength(0); x++)
            {
                for (int y = 0; y < foregroundLayer.GetLongLength(1); y++)
                {
                    int index = foregroundLayer[x, y];

                    if (index == 0)
                    {
                        continue;
                    }

                    int indexX = index % (tileSetWidth / TileWidth) - 1;
                    int indexY = index / (tileSetWidth / TileWidth);

                    Engine.Engine.SpriteBatch.Draw(tileSet, new Vector2(x * TileWidth, y * TileHeight),
                                                            new Rectangle(indexX * TileWidth,
                                                                          indexY * TileHeight,
                                                                          TileWidth, TileHeight),
                                                                          Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
            }
        }

        private void DrawForegroundTiles(Point min, Point max)
        {
            min.X = (int)Math.Max(min.X, 0);
            min.Y = (int)Math.Max(min.Y, 0);

            max.X = (int)Math.Min(max.X, foregroundLayer.GetLongLength(0));
            max.Y = (int)Math.Min(max.Y, foregroundLayer.GetLongLength(1));

            for (int x = min.X; x < max.X; x++)
            {
                for (int y = min.Y; y < max.Y; y++)
                {
                    int index = foregroundLayer[x, y];

                    if (index == 0)
                    {
                        continue;
                    }

                    int indexX = index % (tileSetWidth / TileWidth) - 1;
                    int indexY = index / (tileSetWidth / TileWidth);

                    Engine.Engine.SpriteBatch.Draw(tileSet, new Vector2(x * TileWidth, y * TileHeight),
                                                            new Rectangle(indexX * TileWidth,
                                                                          indexY * TileHeight,
                                                                          TileWidth, TileHeight),
                                                                          Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
            }
        }

        private void DrawBackgroundTiles()
        {
            for (int x = 0; x < backgroundLayer.GetLongLength(0); x++)
            {
                for (int y = 0; y < backgroundLayer.GetLongLength(1); y++)
                {
                    int index = backgroundLayer[x, y];

                    if (index == 0)
                    {
                        continue;
                    }

                    int indexX = index % (tileSetWidth / TileWidth) - 1;
                    int indexY = index / (tileSetWidth / TileWidth);

                    Engine.Engine.SpriteBatch.Draw(tileSet, new Vector2(x * TileWidth, y * TileHeight),
                                                            new Rectangle(indexX * TileWidth,
                                                                          indexY * TileHeight,
                                                                          TileWidth, TileHeight),
                                                                          Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
            }
        }

        private void DrawBackgroundTiles(Point min, Point max)
        {
            min.X = (int)Math.Max(min.X, 0);
            min.Y = (int)Math.Max(min.Y, 0);

            max.X = (int)Math.Min(max.X, backgroundLayer.GetLongLength(0));
            max.Y = (int)Math.Min(max.Y, backgroundLayer.GetLongLength(1));

            for (int x = min.X; x < max.X; x++)
            {
                for (int y = min.Y; y < max.Y; y++)
                {
                    int index = backgroundLayer[x, y];

                    if (index == 0)
                    {
                        continue;
                    }

                    int indexX = index % (tileSetWidth / TileWidth) - 1;
                    int indexY = index / (tileSetWidth / TileWidth);

                    Engine.Engine.SpriteBatch.Draw(tileSet, new Vector2(x * TileWidth, y * TileHeight),
                                                            new Rectangle(indexX * TileWidth,
                                                                          indexY * TileHeight,
                                                                          TileWidth, TileHeight),
                                                                          Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
            }
        }

        public void DrawImageBackground()
        {
            if (backgroundImage != null)
            {
                Engine.Engine.SpriteBatch.Draw(backgroundImage, new Rectangle((int)backgroundPosition.X, (int)backgroundPosition.Y,
                                                                              (int)backgroundImage.Width, (int)backgroundImage.Height), Color.White);
            }
        }

        public void DrawImageMidground()
        {
            if (midgroundImage != null)
            {
                Engine.Engine.SpriteBatch.Draw(midgroundImage, new Rectangle((int)midgroundPosition.X, (int)midgroundPosition.Y,
                                                                             (int)midgroundImage.Width, (int)midgroundImage.Height), Color.White);
            }
        }

        public void DrawImageForeground()
        {
            if (foregroundImage != null)
            {
                Engine.Engine.SpriteBatch.Draw(foregroundImage, new Rectangle((int)foregroundPosition.X, (int)foregroundPosition.Y,
                                                                             (int)foregroundImage.Width, (int)foregroundImage.Height), Color.White);
            }
        }


        public void DrawImageBackground(Vector2 cameraPosition, float scrollingSpeedX = 1.0f, float scrollingSpeedY = 1.0f)
        {
            backgroundPosition.X = cameraPosition.X * scrollingSpeedX;
            backgroundPosition.Y = cameraPosition.Y * scrollingSpeedY;

            if (backgroundImage != null)
            {
                Engine.Engine.SpriteBatch.Draw(backgroundImage, cameraPosition, new Rectangle((int)backgroundPosition.X, (int)backgroundPosition.Y,
                                                                              (int)backgroundImage.Width, (int)backgroundImage.Height), Color.White);
            }
        }

        public void DrawImageMidground(Vector2 cameraPosition, float scrollingSpeedX = 1.0f, float scrollingSpeedY = 1.0f)
        {
            midgroundPosition.X = cameraPosition.X * scrollingSpeedX;
            midgroundPosition.Y = cameraPosition.Y * scrollingSpeedY;

            if (midgroundImage != null)
            {
                Engine.Engine.SpriteBatch.Draw(midgroundImage, cameraPosition, new Rectangle((int)midgroundPosition.X, (int)midgroundPosition.Y,
                                                                             (int)midgroundImage.Width, (int)midgroundImage.Height), Color.White);
            }
        }

        public void DrawImageForeground(Vector2 cameraPosition, float scrollingSpeedX = 1.0f, float scrollingSpeedY = 1.0f)
        {
            foregroundPosition.X = cameraPosition.X * scrollingSpeedX;
            foregroundPosition.Y = cameraPosition.Y * scrollingSpeedY;


            if (foregroundImage != null)
            {
                Engine.Engine.SpriteBatch.Draw(foregroundImage, cameraPosition, new Rectangle((int)foregroundPosition.X, (int)foregroundPosition.Y,
                                                                             (int)foregroundImage.Width, (int)foregroundImage.Height), Color.White);
            }
        }

        public void DrawForeground()
        {
            DrawForegroundTiles();
        }

        public void DrawBackground()
        {
            DrawBackgroundTiles();
        }

        public void DrawForeground(Point min, Point max)
        {
            DrawForegroundTiles(min, max);
        }

        public void DrawBackground(Point min, Point max)
        {
            DrawBackgroundTiles(min, max);
        }

        public static bool[,] TileConverison(int[,] data, int tileNo)
        {
            bool[,] tempArray = new bool[data.GetLength(0), data.GetLength(1)];

            for (int x = 0; x < data.GetLength(0); x++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    int index = data[x, y];
                    if (index == tileNo)
                    {
                        tempArray[x, y] = true;
                    }
                    else
                    {
                        tempArray[x, y] = false;
                    }
                }
            }
            return tempArray;
        }
    }
}