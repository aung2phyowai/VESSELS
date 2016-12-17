#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VESSELS.MazeGameLogic
{
    /// <summary>
    /// Controls playback of an Animation.
    /// </summary>
    struct AnimationPlayer
    {
        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        public Animation Animation
        {
            get { return animation; }
        }
        Animation animation;

        int SCREENHEIGHT;
        int SCREENWIDTH;

        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;

        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        private float time;

        /// <summary>
        /// Gets a texture origin at the bottom center of each frame.
        /// </summary>
        public Vector2 Origin
        {
            get {
                if (Animation.IsVert)
                {
                    return new Vector2(Animation.FrameWidthVert / 2.0f, Animation.FrameHeightVert);
                }
                else
                {
                    return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight);
                }
                }
        }

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation animation, int screenHeight)
        {
            // If this animation is already running, do not restart it.
            if (Animation == animation)
                return;
            this.SCREENHEIGHT = screenHeight;
            // Start the new animation.
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;

        }

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        public void Draw(TimeSpan elapsedTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            // Process passing time.
            time += (float)elapsedTime.TotalSeconds;            
            while (time > Animation.FrameTime)
            {
                time -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    if (Animation.IsVert)
                    {
                        frameIndex = (frameIndex + 1) % Animation.FrameCountVert;
                    }
                    else
                    {
                        frameIndex = (frameIndex + 1) % Animation.FrameCount;
                    }
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
                }
            }

            // Calculate the source rectangle of the current frame.
            Rectangle source;
            if (Animation.IsVert)
            {
                source = new Rectangle(0, FrameIndex * Animation.Texture.Width, Animation.Texture.Width, Animation.Texture.Width);
            }
            else
            {
                source = new Rectangle(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);
            }

            // Draw the current frame.
            spriteBatch.Draw(Animation.Texture, position, source, Color.White, 0.0f, Origin, new Vector2(1.0f, 1.0f*((float)SCREENHEIGHT/1200.0f)), spriteEffects, 0.0f);
        }
    }
}
