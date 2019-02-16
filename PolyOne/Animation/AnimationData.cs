using Microsoft.Xna.Framework.Graphics;

namespace PolyOne.Animation
{
    public class AnimationData
    {
        public Texture2D Texture { get; }

        public bool IsLooping { get; }

        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        public int FrameWidth { get; private set; }

        public int FrameHeight
        {
            get { return Texture.Height; }
        }


        public bool AnimationFinished { get; set; }

        public int MsPerCel { get; }

        public bool NextMove { get; set; }

        public int NextMoveFrame { get; set; }

        public AnimationData(Texture2D texture, int msPerCel, int frameWidth, bool isLooping)
        {
            Texture = texture;
            MsPerCel = msPerCel;
            FrameWidth = frameWidth;
            IsLooping = isLooping;
            AnimationFinished = false;
        }
    }
}
