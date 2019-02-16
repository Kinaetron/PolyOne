using Microsoft.Xna.Framework;

namespace PolyOne.Utility
{
    public static class TileInformation
    {
        public static int TileWidth { get; private set; }
        public static int TileHeight { get; private set; }

        public static void TileDiemensions(int tileWidth, int tileHeight)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public static Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)TileWidth),
                (int)(position.Y / (float)TileHeight));
        }

        public static Point GetTile(float x, float y)
        {
            return new Point((int)(x) / Tile.Width, (int)(y) / Tile.Height);
        }

        public static Vector2 GetPosition(float x, float y)
        {
            return new Vector2(x * TileWidth, y * TileHeight);
        }

        public static Rectangle CreateRectForCell(Point cell)
        {
            return new Rectangle(
                cell.X * TileWidth,
                cell.Y * TileHeight,
                TileWidth,
                TileHeight);
        }
    }
}
