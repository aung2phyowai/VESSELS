using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using System.Device.Location;
using System.Net;
using System.Xml.Linq;
using System.Xml;


namespace VESSELS.BingsMapLogic
{
    class MapsCore:DrawableGameComponent2
    {

        #region Fields
        //Copy of the screenmanager
        ScreenManager screenManager;

        //Screen parameters
        int ScreenWidth, ScreenHeight, OffsetX, OffsetY, gameWidth, gameHeight;

        // Resources for drawing.
        private SpriteBatch spriteBatch;
        ContentManager content;
        // Global content.
        private SpriteFont hudFont;

        // We store our input states so that we only poll once per frame, 
        // then we use the same input state wherever needed
        private GamePadState gamePadState;
        private KeyboardState keyboardState;

        //******************************************************
        // Bing Maps
        GeoCoordinate startingCoordinate;
        BingMapsViewer bingMapsViewer;
        //const string BingAppKey = "ArsjcSFYlpjHcNbMo2eB2JU2r0nsoUhFbpwqjThzjg7CkkZg9nzVp3T3Hxwwx-3w";
        const string BingAppKey = "ArsjcSFYlpjHcNbMo2eB2JU2r0nsoUhFbpwqjThzjg7CkkZg9nzVp3T3Hxwwx-3w";
        //const string BingAppKey = "Am1lcUXGLhn2Or4ipz9LIpy9XiHa0xlXypowuRJQGfn9MtfwVay6Xyx28NKjfMmZ";
        // Web client used to retrieve the coordinates of locations request by the user
        WebClient locationWebClient;
        public int ZoomLevel { get; private set; }
        Texture2D blank, noImage;
        int viewType = 1;
        //******************************************************

        private bool isMoving = false;
        private bool ifwasMoving = false;
        private float moveTime = 0;
        private float maxMoveTime = 1.5f;
        private int moveDirection = 0;
        int dir = 0;

        #endregion Fields

        
        //PngBitmapDecoder decoder;

        //Constructor
        public MapsCore(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        //Load Content
        public override void Initialize()
        {
            //content manager
            if (content == null)
                content = new ContentManager(screenManager.Game.Services, "Content");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(screenManager.GraphicsDevice);

            //Load Content
            blank = content.Load<Texture2D>(@"Textures/blank");
            Texture2D defaultImage = content.Load<Texture2D>(@"Textures/noImage");
            Texture2D unavailableImage = content.Load<Texture2D>(@"Textures/noImage");

            //Bing Maps initilization
            locationWebClient = new WebClient();
            locationWebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(ReceivedLocationCoordinates);
            
            ZoomLevel = 14;

            // The coordinate that the app will focus
            startingCoordinate = new GeoCoordinate(36.88333, -76.3);
            //Initialize the Bing map viewer
            bingMapsViewer = new BingMapsViewer(BingAppKey, defaultImage, unavailableImage,
                startingCoordinate, 13, ZoomLevel, spriteBatch);
            bingMapsViewer.CenterOnLocation(startingCoordinate);
            //Screen offsets for SSVEP stimuli
            ScreenHeight = screenManager.GraphicsDevice.Viewport.Height;
            ScreenWidth = screenManager.GraphicsDevice.Viewport.Width;
            OffsetY = 130;//(int)Math.Round(ScreenHeight * 0.12);
            OffsetX = 130;
            gameWidth = ScreenWidth - 2 * OffsetX;
            gameHeight = ScreenHeight - 2 * OffsetY;
            //1680 x 976 

          

            //screenManager.GraphicsDevice.Viewport.
            base.Initialize();
        }

        /// <summary>
        /// Handler called when the request for a location's coordinates returns.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceivedLocationCoordinates(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            if (e.Error != null)
            {
                Guide.BeginShowMessageBox("Error focusing on location", e.Error.Message, new string[] { "OK" },
                    0, MessageBoxIcon.Error, null, null);
                return;
            }

            GeoCoordinate receivedCoordinate;

            try
            {
                // Parse the response XML to get the location's geo-coordinate
                XDocument locationResponseDoc = XDocument.Load(e.Result);
                XNamespace docNamespace = locationResponseDoc.Root.GetDefaultNamespace();
                var locationNodes = locationResponseDoc.Descendants(XName.Get("Location", docNamespace.NamespaceName));

                if (locationNodes.Count() == 0)
                {
                    Guide.BeginShowMessageBox("Invalid location",
                        "The requested location was not recognized by the system.", new string[] { "OK" },
                        0, MessageBoxIcon.Error, null, null);
                    return;
                }

                XElement pointNode = locationNodes.First().Descendants(
                    XName.Get("Point", docNamespace.NamespaceName)).FirstOrDefault();

                if (pointNode == null)
                {
                    Guide.BeginShowMessageBox("Invalid location result", "The location result is missing data.",
                        new string[] { "OK" }, 0, MessageBoxIcon.Error, null, null);
                    return;
                }

                XElement longitudeNode = pointNode.Element(XName.Get("Longitude", docNamespace.NamespaceName));
                XElement latitudeNode = pointNode.Element(XName.Get("Latitude", docNamespace.NamespaceName));

                if (longitudeNode == null || latitudeNode == null)
                {
                    Guide.BeginShowMessageBox("Invalid location result", "The location result is missing data.",
                        new string[] { "OK" }, 0, MessageBoxIcon.Error, null, null);
                    return;
                }

                receivedCoordinate = new GeoCoordinate(double.Parse(latitudeNode.Value),
                    double.Parse(longitudeNode.Value));
                //tank.Move(receivedCoordinate);
            }
            catch (Exception err)
            {
                Guide.BeginShowMessageBox("Error getting location coordinates", err.Message, new string[] { "OK" },
                    0, MessageBoxIcon.Error, null, null);
                return;
            }
            Console.WriteLine("I get called");
            bingMapsViewer.CenterOnLocation(receivedCoordinate);
        }

        //Update Game
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {

            //get current input for game
            HandleInput(elapsedTime);
            
            base.Update(elapsedTime, totalTime);
        }

        void HandleInput(TimeSpan elapsedTime)
        {
            //Poll again for the keyboard state to go to the game locally
            keyboardState = Keyboard.GetState();
            // MouseState mouseState = Mouse.GetState();
            // Console.WriteLine("{0} , {1}", mouseState.X, mouseState.Y);

            //Move Map
            //if (keyboardState.IsKeyDown(Keys.Left))
            //{
            //    bingMapsViewer.MoveByOffset(new Vector2(4, 0));
            //}
            //if (keyboardState.IsKeyDown(Keys.Right))
            //{
            //    bingMapsViewer.MoveByOffset(new Vector2(-4, 0));
            //}
            //if (keyboardState.IsKeyDown(Keys.Up))
            //{
            //    bingMapsViewer.MoveByOffset(new Vector2(0, 4));
            //}
            //if (keyboardState.IsKeyDown(Keys.Down))
            //{
            //    bingMapsViewer.MoveByOffset(new Vector2(0, -4));
            //}
         
            //Change View Type
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (viewType == 1)
                {
                    bingMapsViewer.RefreshImages(BingMapsViewType.Road);
                    viewType = 2;
                }
                else if (viewType == 2)
                {
                    bingMapsViewer.RefreshImages(BingMapsViewType.AerialWithLabels);
                    viewType = 3;
                }
                else
                {
                    bingMapsViewer.RefreshImages(BingMapsViewType.Aerial);
                    viewType = 1;
                }
            }

           //Update Player tank based on BCI2000 command
           try
           { UpdateFromBCI2000(elapsedTime); }
           catch (Exception e) { }

           if (keyboardState.IsKeyDown(Keys.OemPlus))
           {
               ZoomLevel += 2;
               if (ZoomLevel >= 23)
               {
                   ZoomLevel = 23;
               }
               bingMapsViewer.ChangeZoom(ZoomLevel);
           }
           if (keyboardState.IsKeyDown(Keys.OemMinus))
           {
               ZoomLevel -= 2;
               if (ZoomLevel <= 1)
               {
                   ZoomLevel = 1;
               }
               bingMapsViewer.ChangeZoom(ZoomLevel);
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
            if ((bool)screenManager.Game.Services.GetService(typeof(bool)) == true)
            {
                int Val = (int)screenManager.Game.Services.GetService(typeof(int));
                //update based on commnad
                switch (Val)
                {
                    case 0:     //Dont move
                        //isMoving = false;
                        break;
                    case 1:     //move up
                        
                            isMoving = true;
                            AutoMovePlayer(1, elapsedTime);
                            //pathLocal++;
                        
                        break;
                    case 2:     //move down
                        
                            isMoving = true;
                            AutoMovePlayer(2, elapsedTime);
                            //pathLocal++;
                        
                        break;
                    case 3:     //move right
                        
                            isMoving = true;
                            AutoMovePlayer(3, elapsedTime);
                            //pathLocal++;
                        
                        break;

                    case 4:     //move left
                        
                            isMoving = true;
                            AutoMovePlayer(4, elapsedTime);
                            //pathLocal++;
                        
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
                            bingMapsViewer.MoveByOffset(new Vector2(0, 1));
                            break;
                        case 2:
                            bingMapsViewer.MoveByOffset(new Vector2(0, -1));
                            break;
                        case 3:
                            bingMapsViewer.MoveByOffset(new Vector2(-1, 0));
                            break;
                        case 4:
                            bingMapsViewer.MoveByOffset(new Vector2(1, 0));
                            break;
                    }
                    if (moveTime > maxMoveTime)
                    {
                        isMoving = false;
                        ifwasMoving = false;
                        moveTime = 0;
                        
                    }
                    ifwasMoving = isMoving;
                }
            }
        }

        ///// <summary>
        ///// Update the position and direction based on input from BCI2000
        ///// </summary>
        //void UpdateFromBCI2000()
        //{
        //    //Check to see if a new command is available.
        //    if ((bool)screenManager.Game.Services.GetService(typeof(bool)) == true)
        //    {
        //        //update based on commnad
        //        switch ((int)screenManager.Game.Services.GetService(typeof(int)))
        //        {
        //            case 0:     //Dont move
        //                break;
        //            case 1:     //move up
        //                bingMapsViewer.MoveByOffset(new Vector2(0, 4));
        //                break;
        //            case 2:     //move left
        //                bingMapsViewer.MoveByOffset(new Vector2(4, 0));
        //                break;
        //            case 3:     //move right
        //                bingMapsViewer.MoveByOffset(new Vector2(-4, 0));
        //                break;
        //            case 4:     //move down
        //                bingMapsViewer.MoveByOffset(new Vector2(0, 4));
        //                break;
        //            default:
        //                break;
        //        }

        //        //set the new command status to false
        //        screenManager.Game.Services.RemoveService(typeof(bool));
        //        screenManager.Game.Services.AddService(typeof(bool), false);
        //    }
        //}

        //Draw Game
        public override void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            try
            {
                //Draw small center viewport to make room for SSVEP stimuli. 
                screenManager.GraphicsDevice.Viewport = new Viewport(OffsetX, OffsetY, gameWidth, gameHeight);
                spriteBatch.Begin();

                // Draw the map 
                bingMapsViewer.Draw();

                spriteBatch.End();

                //Return to normal Viewport
                screenManager.GraphicsDevice.Viewport = new Viewport(0, 0, ScreenWidth, ScreenHeight);
            }
            catch (Exception) { }
            base.Draw(elapsedTime, totalTime);
        }


    }
}
