using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using PolyOne.Animation;

namespace PolyOne.ParticleSystem
{
    public class Particle
    {
        public AnimationData Sprite { get; set; }
        public AnimationPlayer SpritePlayer { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public int TTL { get; set; }
        public SpriteEffects SpriteEffect { get; set; }

        public Particle(AnimationData sprite, AnimationPlayer spritePlayer, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, SpriteEffects spriteEffect, float size, int ttl)
        {
            Sprite = sprite;
            SpritePlayer = spritePlayer;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            SpriteEffect = spriteEffect;
            Size = size;
            TTL = ttl;
        }

        public void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;

            SpritePlayer.PlayAnimation(Sprite);
        }

        public void Draw()
        {
            SpritePlayer.PlayAnimation(Sprite);

            if (TTL > 0)
            {
                SpritePlayer.Draw(Position, Angle, SpriteEffect);
            }
        }
    }
}
