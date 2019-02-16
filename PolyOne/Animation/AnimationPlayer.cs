using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PolyOne.Animation
{
    public class AnimationPlayer
    {

        protected Rectangle source;

        public AnimationData Animation { get; private set; }

        public int FrameIndex { get; private set; }

        protected int previousFrame;

        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight / 2.0f); }
        }

        public bool AnimationFinished { get; private set; }

        public bool AnimationStarted { get; set; }

        public int MsUntilNextCel { get; set; }

        public bool SameFrame { get; private set; }

        public bool Stop { get; set; }

        private float pauseTime;

        public void PlayAnimation(AnimationData animation)
        {
            if (Animation == animation) {
                return;
            }

            Animation = animation;
            FrameIndex = 0;

            animation.AnimationFinished = false;
            AnimationFinished = false;
            AnimationStarted = false;
            MsUntilNextCel = animation.MsPerCel;
        }

        public void ResetAnimation()
        {
            FrameIndex = 0;

            Animation.AnimationFinished = false;
            AnimationFinished = false;
            AnimationStarted = false;
            MsUntilNextCel = Animation.MsPerCel;
        }


        public void PauseAnimation(float pauseTime)
        {
            if (this.pauseTime <= 0)
            {
                this.pauseTime = pauseTime;
            }
        }

        public void Update()
        {
            SameFrame = false;

            if (Animation == null) {
                throw new NotSupportedException("No animation is currently playing.");
            }

            if (pauseTime > 0)
            {
                pauseTime -= Engine.Engine.DeltaTime;
            }
            else
            {
                MsUntilNextCel -= (int)Engine.Engine.DeltaTime;
            }

            if (FrameIndex == Animation.NextMoveFrame && Animation.NextMoveFrame > 0) {
                Animation.NextMove = true;
            }

            if (MsUntilNextCel <= 0)
            {
                FrameIndex++;
                MsUntilNextCel = Animation.MsPerCel;
            }
            else
            {
                SameFrame = true;
            }

            if (Stop == true)
            {
                FrameIndex = 0;
            }
            else if (Animation.IsLooping == true && FrameIndex >= Animation.FrameCount)
            {
                FrameIndex = 0;
                Animation.AnimationFinished = true;
            }
            else if (FrameIndex > Animation.FrameCount - 1)
            {
                FrameIndex = Animation.FrameCount - 1;
                Animation.AnimationFinished = true;
            }

            if (pauseTime <= 0)
            {
                pauseTime = 0;
            }
        }

        public void Update(float velocityX, float maxSpeed)
        {
            SameFrame = false;

            if (Animation == null) {
                throw new NotSupportedException("No animation is currently playing.");
            }

            if (pauseTime > 0) {
                pauseTime -= Engine.Engine.DeltaTime;
            }
            else {
                MsUntilNextCel -= (int)Engine.Engine.DeltaTime;
            }

            float relativeVelocity = Math.Abs(velocityX / maxSpeed);

            if (FrameIndex == Animation.NextMoveFrame && Animation.NextMoveFrame > 0) {
                Animation.NextMove = true;
            }

            if (MsUntilNextCel <= 0)
            {
                FrameIndex++;
                MsUntilNextCel = (int)(Animation.MsPerCel * (2.0f - relativeVelocity));
            }
            else {
                SameFrame = true;
            }

            if (Stop == true) {
                FrameIndex = 0;
            }
            else if (Animation.IsLooping == true && FrameIndex >= Animation.FrameCount)
            {
                FrameIndex = 0;
                Animation.AnimationFinished = true;
            }
            else if (FrameIndex > Animation.FrameCount - 1)
            {
                FrameIndex = Animation.FrameCount - 1;
                Animation.AnimationFinished = true;
            }

            if (pauseTime <= 0) {
                pauseTime = 0;
            }
        }

        public void Draw(Vector2 position, float rotation,
                         SpriteEffects spriteEffects, Color color = default(Color))
        {
            if (Animation == null) {
                throw new NotSupportedException("No animation is currently playing.");
            }

            if (object.Equals(color, default(Color)))
                color = Color.White;

            source = new Rectangle(FrameIndex * (int)Animation.FrameWidth, 0, Animation.FrameWidth,
                                                                         Animation.FrameHeight);

            Engine.Engine.SpriteBatch.Draw(Animation.Texture, new Vector2((int)position.X, (int)position.Y), source, color, rotation,
                                                            new Vector2((int)Origin.X, (int)Origin.Y), 1.0f, spriteEffects, 0.0f);
        }
    }
}