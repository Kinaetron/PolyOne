using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace PolyOne.LevelProcessor
{
    public struct TileSet
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("image")]
        private string image;

        public string Image
        {
            get { return "Tiles/" + Path.GetFileNameWithoutExtension(image); }
        }

        [JsonProperty("firstgid")]
        public int TileId;

        [JsonProperty("margin")]
        public int Margin;

        [JsonProperty("spacing")]
        public int Spacing;

        [JsonProperty("imageheight")]
        public int Height;

        [JsonProperty("imagewidth")]
        public int Width;
    }

    public class Object
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("height")]
        public int Height;

        [JsonProperty("width")]
        public int Width;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("x")]
        public int X;

        [JsonProperty("y")]
        public int Y;

        [JsonProperty("properties")]
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
    }

    public class Layer
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("image")]
        private string image;

        public string Image
        {
            get { return "Images/" + Path.GetFileNameWithoutExtension(image); }
        }

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("height")]
        public int Height;

        [JsonProperty("width")]
        public int Width;

        [JsonProperty("x")]
        public int X;

        [JsonProperty("y")]
        public int Y;

        public int[,] MapData
        {
            get
            {
                mapData = new int[Width, Height];
                if (data != null)
                {
                    mapData = LevelData.Make2DArray(data, Width, Height);
                }
                return mapData;
            }
        }
        private int[,] mapData;

        [JsonProperty("data")]
        private int[] data;

        [JsonProperty("objects")]
        public List<Object> Objects = new List<Object>();
    }

    public class LevelData
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int TileHeight { get; set; }
        public int Tilewidth { get; set; }

        public List<Layer> Layers = new List<Layer>();
        public List<TileSet> TileSets = new List<TileSet>();

        private List<Texture2D> images = new List<Texture2D>();


        public static LevelData Load(string path)
        {
            LevelData levelFile;
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                levelFile =  (LevelData)serializer.Deserialize(file, typeof(LevelData));
            }

            return levelFile;
        }

        public static T[,] Make2DArray<T>(T[] input, int width, int height)
        {
            T[,] output = new T[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    output[x, y] = input[x + width * y];
                }
            }
            return output;
        }
    }
}
