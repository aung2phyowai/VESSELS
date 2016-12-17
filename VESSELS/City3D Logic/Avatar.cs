using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;

namespace VESSELS.City3D_Logic
{
    class Avatar: GameComponent2
    {
        #region Fields
        
        // Position and Orientation of avatar
        public Matrix proj;
        public float avatarYaw = -147.66f;
        public Vector3 avatarPosition = new Vector3(10, 29, 280);
        //public Vector3 avatarPosition = new Vector3(0, 50, 0);
        Matrix forwardMovement = Matrix.Identity;
        Vector3 v = Vector3.Zero;

        //Variables for handleing the Automatic Moving by BCI2000
        private bool isMoving = false;
        private bool ifwasMoving = false;
        private float moveTime = 0;
        private float maxMoveTime = 0.75f;
        private int moveDirection = 0;
        int dir = 0;

        // Camera Properties
        static float viewAngle = MathHelper.PiOver4;
        static float nearClip = 1.0f;
        static float farClip = 2000.0f;
        Vector3 cameraPosition = Vector3.Zero;
        // Set the direction the camera points without rotation.
        Vector3 cameraReference = new Vector3(0, 0, 1);
        public Matrix view;

        // Previous Input
        GamePadState oldGamepadstate;
        KeyboardState oldKeyState;

        // Set rates in world units per 1/60th second (the default fixed-step interval).
        public float rotationSpeed = 0.19f / 60f;
        public float forwardSpeed = 40f / 60f;

        // Game and content
        public ScreenManager screenManager;
        public ContentManager content;

        // The XNA framework Model object that we are going to display.
        //public Model AvatarModel;

        #endregion Fields

        //Constructor
        public Avatar(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        //Initialize Avatar settings
        public override void Initialize()
        {
            //Set up Projection Matrix 
            float aspectRatio = (float)screenManager.GraphicsDevice.Viewport.Width / (float)screenManager.GraphicsDevice.Viewport.Height;
            proj = Matrix.CreatePerspectiveFieldOfView(viewAngle, aspectRatio, nearClip, farClip);
   
            
            base.Initialize();
        }


        /// <summary>
        /// Updates the position of the Avatar
        /// </summary>
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            //Update Avatar Position (from keyboard)
            //UpdateAvatarPosition();

            try
            { UpdateFromBCI2000(elapsedTime); }
            catch (Exception) { }

            //Update First person camera
            UpdateCamera();

            //Update the View Matrix Service
            screenManager.Game.Services.RemoveService(typeof(Matrix));
            screenManager.Game.Services.AddService(typeof(Matrix), view);

            //update position service
            screenManager.Game.Services.RemoveService(typeof(Vector3));
            screenManager.Game.Services.AddService(typeof(Vector3),cameraPosition);

            base.Update(elapsedTime, totalTime);
        }

        //Update the avatar based on input from BCI2000
        public void UpdateFromBCI2000(TimeSpan elapsedTime)
        {
            AutoMovePlayer(0, elapsedTime);
            if ((bool)screenManager.Game.Services.GetService(typeof(bool)))
            {
                int command = (int)screenManager.Game.Services.GetService(typeof(int));

                switch (command)
                {
                    case 0:
                        break;
                    case 1:
                        isMoving = true;
                        AutoMovePlayer(1,elapsedTime);
                        break;
                    case 2:
                        isMoving = true;
                        AutoMovePlayer(2,elapsedTime);
                        break;
                    case 3:
                        isMoving = true;
                        AutoMovePlayer(3,elapsedTime);
                        break;
                    case 4:
                        isMoving = true;
                        AutoMovePlayer(4,elapsedTime);
                        break;
                    case 99:
                        isMoving = false;
                        break;
                    default:
                        AutoMovePlayer(0, elapsedTime);
                        break;
                }
                //set the new command status to false
                screenManager.Game.Services.RemoveService(typeof(bool));
                screenManager.Game.Services.AddService(typeof(bool), false);
            }
        }

        //Auto move the player based on BCI2000 command
        public void AutoMovePlayer(int direction, TimeSpan elapsedTime)
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
                            forwardMovement = Matrix.CreateRotationY(avatarYaw);
                            v = new Vector3(0, 0, forwardSpeed);
                            v = Vector3.Transform(v, forwardMovement);
                            avatarPosition.Z += v.Z;
                            avatarPosition.X += v.X;
                            break;
                        case 2:
                            forwardMovement = Matrix.CreateRotationY(avatarYaw);
                            v = new Vector3(0, 0, -forwardSpeed);
                            v = Vector3.Transform(v, forwardMovement);
                            avatarPosition.Z += v.Z;
                            avatarPosition.X += v.X;
                            break;
                        case 3:
                            // Rotate right.
                            avatarYaw -= rotationSpeed;
                            break;
                        case 4:
                            // Rotate left.
                            avatarYaw += rotationSpeed;
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

        //Update the position of the Avatar
        public void UpdateAvatarPosition()
        {
            //Get current keyboard State
            KeyboardState keyboardState = Keyboard.GetState();

            //Update position
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                // Rotate left.
                avatarYaw += rotationSpeed*2;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                // Rotate right.
                avatarYaw -= rotationSpeed*2;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(avatarYaw);
                Vector3 v = new Vector3(0, 0, forwardSpeed +0.5f);
                v = Vector3.Transform(v, forwardMovement);
                avatarPosition.Z += v.Z;
                avatarPosition.X += v.X;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(avatarYaw);
                Vector3 v = new Vector3(0, 0, -forwardSpeed+0.5f);
                v = Vector3.Transform(v, forwardMovement);
                avatarPosition.Z += v.Z;
                avatarPosition.X += v.X;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                avatarPosition.Y +=3.5f;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                avatarPosition.Y -= 3.5f;
            }

        }

        //Update the Camera
        public void UpdateCamera()
        {

            //Calculate Camera's Current Position
            cameraPosition = avatarPosition;

            //Compute Rotation Matrix
            Matrix rotationMatrix = Matrix.CreateRotationY(avatarYaw);
            // Create a vector pointing the direction the camera is facing.
            Vector3 transformedReference = Vector3.Transform(cameraReference, rotationMatrix);

            // Calculate the position the camera is looking at.
            Vector3 cameraLookat = cameraPosition + transformedReference;

            // Set up the view matrix and projection matrix.
            view = Matrix.CreateLookAt(cameraPosition, cameraLookat, new Vector3(0.0f, 1.0f, 0.0f));

        }

    }
}
