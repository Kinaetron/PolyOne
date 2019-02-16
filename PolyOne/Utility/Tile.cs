using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PolyOne.Utility
{
    public enum TileCollision
    {
        Passable = 0,
        Impassable = 1,
        NormalTile = 2,
        StartCameraNode = 3,
        EndCameraNode = 4,
        CameraNode = 5,
        RightSlope = 6,
        LeftSlope = 7
    };

    public struct Tile
    {
        public Texture2D Texture;
        public TileCollision Collision;

        public const int Width = 32;
        public const int Height = 32;
        public const int Centre = Width / 2;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(Texture2D texture, TileCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }
}
