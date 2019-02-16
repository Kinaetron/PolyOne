using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PolyOne.Utility
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }

        public Vector2 Origin
        {
            get { return origin; }
        }
        private Vector2 origin;

        public Vector2 Position { get; set; }

        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
        }
        private Rectangle sourceRectangle;

        public float Rotation { get; set; }

        public float Scale { get; set; }

        public float Depth { get; private set; }

        public Color Color { get; set; }

        public Sprite()
        {
        }

        public void Initialize(Texture2D texture)
        {
            this.Texture = texture;
            sourceRectangle.X = 0;
            sourceRectangle.Y = 0;
            sourceRectangle.Height = texture.Width;
            sourceRectangle.Width = texture.Height;

            origin.X = SourceRectangle.Width / 2;
            origin.Y = SourceRectangle.Height / 2;

            Scale = 1.0f;
            Rotation = 0.0f;
            Depth = 1.0f;
            Color = Color.White;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color, Rotation, origin, Scale, SpriteEffects.None, Depth);
        }
    }
}
