#region File Description
//-----------------------------------------------------------------------------
// Player.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VESSELS.MazeGameLogic
{
    /// <summary>
    /// Our fearless adventurer!
    /// </summary>
    class Player
    {
        
        //Constrained map
       //int[] path = new int[116] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3 };
        int pathLocal = 0;
        //Handle collisions
        bool colliding = false;
        int PreviousMove = 10;
        float playerOffset;
        //int[] path = new int 
        // Animations
        private Animation idleRight;
        private Animation idleLeft;
        private Animation idleUp;
        private Animation idleDown;
        private Animation runLeftAnimation;
        private Animation runRightAnimation;
        private Animation runUpAnimation;
        private Animation runDownAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private AnimationPlayer sprite;
        private int directionState = 1;
        // Sounds
        private SoundEffect killedSound;
        private SoundEffect jumpSound;
        private SoundEffect fallSound;

        public Level Level
        {
            get { return level; }
        }
        Level level;

        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;

        //game window offset
        private int offSetX,offSetY;

        // Physics state
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;
        Vector2 newPosition;
        private float previousBottom;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        // Constants for controling horizontal movement
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f; 

        //Variables for handleing the Automatic Moving by BCI2000
        private bool isMoving = false;
        private bool ifwasMoving = false;
        private float moveTime = 0;
        private float maxMoveTime = 0.5f;
        private int moveDirection = 0;
        int dir=0;

        // Input configuration
        private const float MoveStickScale = 1.0f;
        private const Buttons JumpButton = Buttons.A;

        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        /// <summary>
        /// Current user movement input.
        /// </summary>
        private float movementX;
        private float movementY;

        // Jumping state
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        //Game object
        Game game;
        int SCREENHEIGHT;
        int SCREENWIDTH;

        bool movementComplete = true;
        float deltaX, deltaY;

        private Rectangle localBounds;
        /// <summary>
        /// Gets a rectangle which bounds this player in world space.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - (localBounds.Width/2));
                int top = (int)Math.Round(Position.Y-localBounds.Height);

                //return new Rectangle(left, top-(int)((float)playerOffset), localBounds.Width, localBounds.Height);
                return new Rectangle(left, top, localBounds.Width, localBounds.Height);

            }
        }
        public int x;
        public int y;
        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Player(Level level, Vector2 position, int offSetX, int offSetY,Game game, int x, int y)
        {
            this.x = x;
            this.y = y;
            this.level = level;
            this.offSetX = offSetX;
            this.offSetY = offSetY;
            this.game = game;
            this.SCREENHEIGHT = game.GraphicsDevice.DisplayMode.Height;
            this.SCREENWIDTH = game.GraphicsDevice.DisplayMode.Width;
            LoadContent();

            Reset(position);
        }

        /// <summary>
        /// Loads the player sprite sheet and sounds.
        /// </summary>
        public void LoadContent()
        {
            // Load animated textures.
            idleLeft = new Animation(Level.Content.Load<Texture2D>(@"Textures/Sprites/Player/pacmanIdleLeft"), 0.1f, true,false);
            idleRight = new Animation(Level.Content.Load<Texture2D>(@"Textures/Sprites/Player/pacmanIdleRight"), 0.1f, true, false);
            idleUp = new Animation(Level.Content.Load<Texture2D>(@"Textures/Sprites/Player/pacmanIdleUp"), 0.1f, true, false);
            idleDown = new Animation(Level.Content.Load<Texture2D>(@"Textures/Sprites/Player/pacmanIdleDown"), 0.1f, true, false);
            runLeftAnimation = new Animation(Level.Content.Load<Texture2D>(@"Textures/Sprites/Player/pacmanLeft"), 0.1f, true, false);
            runRightAnimation = new Animation(Level.Content.Load<Texture2D>(@"Textures/Sprites/Player/pacmanRight"), 0.1f, true, false);
            runUpAnimation = new Animation(Level.Content.Load<Texture2D>(@"Textures/Sprites/Player/pacmanUp"), 0.1f, true, true);
            runDownAnimation = new Animation(Level.Content.Load<Texture2D>(@"Textures/Sprites/Player/pacmanDown"), 0.1f, true, true);

            // Calculate bounds within texture size.            
            int width = (int)Math.Round((double)(idleUp.FrameWidth)*1d);
            int left = (idleUp.FrameWidth - width);
            int height =  (int)(Math.Round((double)(idleUp.FrameWidth) * 1d) * ((float)game.Window.ClientBounds.Height / 1200.0f));
            int top = (int)((float)idleUp.FrameHeight*((float)SCREENHEIGHT/1200.0f)) - height;
            //top = 0;
            localBounds = new Rectangle(left, top, width, height);
            playerOffset = idleUp.FrameWidth;
            // Load sounds.            
            killedSound = Level.Content.Load<SoundEffect>("AudioLibrary/PlayerKilled");
            jumpSound = Level.Content.Load<SoundEffect>("AudioLibrary/PlayerJump");
            fallSound = Level.Content.Load<SoundEffect>("AudioLibrary/PlayerFall");
        }

        /// <summary>
        /// Resets the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            isAlive = true;
            sprite.PlayAnimation(idleRight, SCREENHEIGHT);
            //Update position service for stimuli
            game.Services.RemoveService(typeof(Vector2));
            game.Services.AddService(typeof(Vector2), position);
        }

        // very simple collision detection
        bool collisionDetect(int _x, int _y)
        {
            if (Level.GetCollision(_x, _y) == TileCollision.Impassable)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Handles input, performs physics, and animates the player sprite.
        /// </summary>
        /// <remarks>
        /// We pass in all of the input states so that our game is only polling the hardware
        /// once per frame. We also pass the game's orientation because when using the accelerometer,
        /// we need to reverse our motion when the orientation is in the LandscapeRight orientation.
        /// </remarks>
        public void Update(
            TimeSpan elapsedTime, 
            KeyboardState keyboardState, 
            GamePadState gamePadState)
        {

            //Get Update from BCI2000 
            try
            { UpdateFromBCI20002(elapsedTime); }
            catch (Exception e) { }

            //Animate pacman
            if (IsAlive)
            {
                if (Velocity.X > 0)
                {
                    sprite.PlayAnimation(runRightAnimation, SCREENHEIGHT);
                    directionState = 1;
                }
                else if (Velocity.X < 0)
                {
                    sprite.PlayAnimation(runLeftAnimation, SCREENHEIGHT);
                    directionState = 2;
                }
                else if (Velocity.Y > 0)
                {
                    sprite.PlayAnimation(runDownAnimation, SCREENHEIGHT);
                    directionState = 3;
                }
                else if (Velocity.Y < 0)
                {
                    sprite.PlayAnimation(runUpAnimation, SCREENHEIGHT);
                    directionState = 4;
                }
                else
                {
                    switch(directionState)
                    {
                        case 1:
                        sprite.PlayAnimation(idleRight, SCREENHEIGHT);
                        break;
                        case 2:
                        sprite.PlayAnimation(idleLeft, SCREENHEIGHT);
                        break;
                        case 3:
                        sprite.PlayAnimation(idleDown, SCREENHEIGHT);
                        break;
                        case 4:
                        sprite.PlayAnimation(idleUp, SCREENHEIGHT);
                        break;
                            
                    }
                }
            }

        }

        /// <summary>
        /// Update the position and direction based on input from BCI2000
        /// </summary>
        void UpdateFromBCI20002(TimeSpan elapsedTime)
        {
            AutoMovePlayer2(0, elapsedTime);
            //Check to see if a new command is available.
            if ((bool)game.Services.GetService(typeof(bool)) == true)
            {
                int Val = (int)game.Services.GetService(typeof(int));

                //update based on commnad
                switch (Val)
                {
                    case 0:     //Dont move
                        isMoving = false;
                        break;
                    case 1:     //move up
                        if (!collisionDetect(x, y - 1) && movementComplete)
                        {
                            isMoving = true;
                            y -= 1;
                            newPosition = RectangleExtensions.GetBottomCenter(Level.GetBounds(x, y));
                            AutoMovePlayer2(1, elapsedTime);
                        }
                        break;
                    case 2:     //move down
                        if (!collisionDetect(x, y + 1) && movementComplete)
                        {
                            isMoving = true;
                            y += 1;
                            newPosition = RectangleExtensions.GetBottomCenter(Level.GetBounds(x, y));
                            AutoMovePlayer2(2, elapsedTime);
                        }
                        break;
                    case 3:     //move right
                        if (!collisionDetect(x + 1, y) && movementComplete)
                        {
                            isMoving = true;
                            x += 1;
                            newPosition = RectangleExtensions.GetBottomCenter(Level.GetBounds(x, y));
                            AutoMovePlayer2(3, elapsedTime);
                        }
                        break;
                    case 4:     //move left
                        if (!collisionDetect(x - 1, y) && movementComplete)
                        {
                            isMoving = true;
                            x -= 1;
                            newPosition = RectangleExtensions.GetBottomCenter(Level.GetBounds(x, y));
                            AutoMovePlayer2(4, elapsedTime);
                        }
                        break;
                    case 99:
                        isMoving = false;
                        break;
                    default:
                        AutoMovePlayer(0, elapsedTime);
                        break;
                }

                //set the new command status to false
                game.Services.RemoveService(typeof(bool));
                game.Services.AddService(typeof(bool), false);
            }
        }

        /// <summary>
        /// Update the position and direction based on input from BCI2000
        /// </summary>
        void UpdateFromBCI2000(TimeSpan elapsedTime)
        {
            //TODO: Add some movement timers to move a lil bit after a command
            AutoMovePlayer(0, elapsedTime);
            //Check to see if a new command is available.
            if ((bool)game.Services.GetService(typeof(bool)) == true)
            {
                int Val = (int)game.Services.GetService(typeof(int));
                
                //update based on commnad
                switch (Val)
                {
                    case 0:     //Dont move
                        isMoving = false;
                        break;
                    case 1:     //move up
                       // if(PreviousMove !=2)
                       // {
                            isMoving = true;
                            AutoMovePlayer(1,elapsedTime);
                            //Store Previous Move
                            PreviousMove = Val;
                            //pathLocal++;
                       // }
                        break;
                    case 2:     //move down
                       // if (PreviousMove != 1)
                       // {
                            isMoving = true;
                            AutoMovePlayer(2, elapsedTime);
                            //Store Previous Move
                            PreviousMove = Val;
                            //pathLocal++;
                        //}
                        break;
                    case 3:     //move right
                       // if (PreviousMove != 4)
                        //{
                            isMoving = true;
                            AutoMovePlayer(3, elapsedTime);
                            //Store Previous Move
                            PreviousMove = Val;
                            //pathLocal++;
                        //}
                        break;

                    case 4:     //move left
                        //if (PreviousMove != 3)
                       // {
                            isMoving = true;
                            AutoMovePlayer(4, elapsedTime);
                            //Store Previous Move
                            PreviousMove = Val;
                           //pathLocal++;
                       // }
                        break;
                    case 99:
                        isMoving = false;
                        break;
                    default:
                        AutoMovePlayer(0, elapsedTime);
                        break;
                }

                

                //set the new command status to false
                game.Services.RemoveService(typeof(bool));
                game.Services.AddService(typeof(bool), false);
            }
        }

        //Auto move the player based on BCI2000 command
        public void AutoMovePlayer2(int direction, TimeSpan elapsedTime)
        {
            if (isMoving)
            {
                movementComplete = false;
                if (!ifwasMoving || moveTime > 0.0f)
                {
                    if (moveTime == 0)
                    {
                        dir = direction;
                    }
                    moveTime += (float)elapsedTime.TotalSeconds;

                    switch (dir)
                    {
                        case 1:
                            movementY = -2;
                            velocity.Y = -1.0f;
                            position.Y += movementY;
                            break;
                        case 2:
                            movementY = 2;
                            velocity.Y = 1.0f;
                            position.Y += movementY;
                            break;
                        case 3:
                            movementX = 2;
                            velocity.X = 1.0f;
                            position.X += movementX;
                            break;
                        case 4:
                            movementX = -2;
                            velocity.X = -1.0f;
                            position.X += movementX;
                            break;
                    }

                    deltaX = Math.Abs(newPosition.X - position.X);
                    deltaY = Math.Abs(newPosition.Y - position.Y);
                    if(deltaX<=2 && deltaY<=2)
                    {
                        isMoving = false;
                        ifwasMoving = false;
                        moveTime = 0;
                        velocity.X = 0;
                        velocity.Y = 0;
                        movementX = 0;
                        movementY = 0;
                        position = newPosition;
                        movementComplete = true;
                    }
                    ifwasMoving = isMoving;

                    //Update position service for stimuli
                    game.Services.RemoveService(typeof(Vector2));
                    game.Services.AddService(typeof(Vector2), position);
                }
            }
        }
        
        //Auto move the player based on BCI2000 command
        public void AutoMovePlayer(int direction,TimeSpan elapsedTime)
        {
            if (isMoving)
            {
                if (!ifwasMoving || moveTime > 0.0f)
                {
                    if (moveTime == 0)
                    {
                        dir = direction;
                    }
                    moveTime += (float)elapsedTime.TotalSeconds;


                    switch (dir)
                    {
                        case 1:
                            movementY = -2.0f;
                            break;
                        case 2:
                            movementY = 2.0f;
                            break;
                        case 3:
                            movementX = 2.0f;
                            break;
                        case 4:
                            movementX = -2.0f;
                            break;
                    }
                    if (moveTime > maxMoveTime)
                    {
                        isMoving = false;
                        ifwasMoving = false;
                        moveTime = 0;
                       // pathLocal++;
                    }
                    ifwasMoving = isMoving;
                }
            }
        }

        /// <summary>
        /// Gets player horizontal movement and jump commands from input.
        /// </summary>
        private void GetInput(
            KeyboardState keyboardState, 
            GamePadState gamePadState)
        {
            // Get analog horizontal movement.
            movementX = gamePadState.ThumbSticks.Left.X * MoveStickScale;

            // If any digital horizontal movement input is found, override the analog movement.
            if (gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A))
            {
               // movementX = -1.0f;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadRight) ||
                     keyboardState.IsKeyDown(Keys.Right) ||
                     keyboardState.IsKeyDown(Keys.D))
            {
               // movementX = 1.0f;
            }

            // If any digital horizontal movement input is found, override the analog movement.
            if (gamePadState.IsButtonDown(Buttons.DPadUp) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W))
            {
              //  movementY = -1.0f;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadDown) ||
                     keyboardState.IsKeyDown(Keys.Down) ||
                     keyboardState.IsKeyDown(Keys.S))
            {
              //  movementY = 1.0f;
            }
        }

        /// <summary>
        /// Updates the player's velocity and position based on input, gravity, etc.
        /// </summary>
        public void ApplyPhysics(TimeSpan elapsedTime)
        {
            float elapsed = (float)elapsedTime.TotalSeconds;

            Vector2 previousPosition = Position;

            // Base velocity
            velocity.X += movementX * MoveAcceleration * elapsed;
            velocity.Y += movementY * MoveAcceleration * elapsed;

            // Apply pseudo-drag 
                velocity.X *= GroundDragFactor;
                velocity.Y *= GroundDragFactor;
                
            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);
            velocity.Y = MathHelper.Clamp(velocity.Y, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity.
           // Position += velocity * elapsed;
           // Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
            previousPosition = position;
            
            
            //position.X += movementX; position.Y += movementY;
            //position.Y
            
            
            
            if (colliding)
            {
                //position = previousPosition;
            }
            // If the player is now colliding with the level, separate them.
            colliding = false;
            //HandleCollisions();

            if (colliding)
            {
                //position = previousPosition;
            }

            // If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == previousPosition.X)
                velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;

            //Update Position service
            game.Services.RemoveService(typeof(Vector2));
            game.Services.AddService(typeof(Vector2), position);
        }

        #region ObsoleteJump
        /// <summary>
        /// Calculates the Y velocity accounting for jumping and
        /// animates accordingly.
        /// </summary>
        /// <remarks>
        /// During the accent of a jump, the Y velocity is completely
        /// overridden by a power curve. During the decent, gravity takes
        /// over. The jump velocity is controlled by the jumpTime field
        /// which measures time into the accent of the current jump.
        /// </remarks>
        /// <param name="velocityY">
        /// The player's current velocity along the Y axis.
        /// </param>
        /// <returns>
        /// A new Y velocity if beginning or continuing a jump.
        /// Otherwise, the existing Y velocity.
        /// </returns>
        private float DoJump(float velocityY, TimeSpan elapsedTime)
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && IsOnGround) || jumpTime > 0.0f)
                {
                    if (jumpTime == 0.0f)
                        jumpSound.Play();

                    jumpTime += (float)elapsedTime.TotalSeconds;
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }
        #endregion ObsoleteJump

        /// <summary>
        /// Detects and resolves all collisions between the player and his neighboring
        /// tiles. When a collision is detected, the player is pushed away along one
        /// axis to prevent overlapping. There is some special logic for the Y axis to
        /// handle platforms which behave differently depending on direction of movement.
        /// </summary>
        private void HandleCollisions()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle bounds = BoundingRectangle;
            bounds.X -= offSetX;
            bounds.Y -= offSetY;
            int leftTile = (int)Math.Floor((float)(bounds.Left) / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)(bounds.Right) / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)(bounds.Top) / ((float)Tile.Height * ((float)SCREENHEIGHT / 1200f)));
            int bottomTile = (int)Math.Ceiling(((float)(bounds.Bottom) / ((float)Tile.Height * ((float)SCREENHEIGHT / 1200f)))) - 1;
            bounds = BoundingRectangle;
            // Reset flag to search for ground collision.
            isOnGround = true;

            // For each potentially colliding tile,
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    TileCollision collision = Level.GetCollision(x, y);
                    if (collision != TileCollision.Passable)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = Level.GetBounds(x, y);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            colliding = true;
                        }
                        else
                        {
                            colliding = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when the player has been killed.
        /// </summary>
        /// <param name="killedBy">
        /// The enemy who killed the player. This parameter is null if the player was
        /// not killed by an enemy (fell into a hole).
        /// </param>
        public void OnKilled(Enemy killedBy)
        {
            isAlive = false;

            if (killedBy != null)
                killedSound.Play();
            else
                fallSound.Play();
        }

        /// <summary>
        /// Called when this player reaches the level's exit.
        /// </summary>
        public void OnReachedExit()
        {
        }

        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void Draw(TimeSpan elapsedTime, SpriteBatch spriteBatch)
        {
            // Draw that sprite.
            sprite.Draw(elapsedTime, spriteBatch, Position, flip);
        }
    }
}
