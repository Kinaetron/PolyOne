using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PolyOne.Animation;

namespace PolyOne.ParticleSystem
{
    public class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<AnimationData> sprites;
        private List<AnimationPlayer> spritePlayers;

        public ParticleEngine(List<AnimationData> sprites, Vector2 location)
        {
            EmitterLocation = location;
            this.sprites = sprites;
            this.particles = new List<Particle>();
            this.spritePlayers = new List<AnimationPlayer>();
            random = new Random();
        }

        public void Update()
        {
            int total = 10;

            for (int i = 0; i < total; i++)
            {
                spritePlayers.Add(new AnimationPlayer());
                particles.Add(GenerateNewParticle());
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            AnimationData sprite = sprites[0];
            AnimationPlayer player = spritePlayers[0];
            Vector2 position = EmitterLocation;
            Vector2 velocity = Vector2.Zero;
            SpriteEffects spriteEffect = SpriteEffects.None;
            float angle = 0;
            float angularVelocity = 0f;
            Color color = Color.White;
            float size = 1.0f;
            int ttl = 7;

            return new Particle(sprite, player, position, velocity, angle, angularVelocity, color, spriteEffect, size, ttl);
        }

        public void Draw()
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw();
            }
        }
    }
}