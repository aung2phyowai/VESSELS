using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net.Configuration;
using System.Net;
using MjpegProcessor;

namespace VESSELS.AndroidAppLogic
{
    class AndroidAppCoreSquare : DrawableGameComponent2
    {
        #region Fields
        //Local copy of the screenmanager
        ScreenManager screenManager;
        SpriteBatch spriteBatch;
        int SCREENWIDTH, SCREENHEIGHT;
        ContentManager content;
        Texture2D Cross;
        Texture2D[] Arrow = new Texture2D[17];
        Texture2D fbArrow;

        KeyboardState oldKeyboardState;
        
        //Cross Positions
        Vector2[] Radial = new Vector2[17];
        Vector2 FB;
        //MJPEG decoder
        MjpegDecoder videoDecoder;
        //Video size
        float videoScale = 2.0f; 

        // camera uri
       // String camUri = "http://10.252.31.166:1234/videofeed";  //Android Primary phone
        //String camUri = "http://192.168.0.8:1234/videofeed";  //Home network
        String camUri = "http://10.252.28.247:1234/videofeed";  //Android debug phone

        //Android UDP Communications
        Socket sending_socket;
        IPAddress send_to_address;
        IPEndPoint sending_end_point;
        byte[] forward;
        byte[] backward;
        byte[] left;
        byte[] right;
        byte[] stop;

        // cam image
        Texture2D camImage;
        Texture2D myImage;
        Texture2D mask;

        #endregion Fields

        //Constructor
        public AndroidAppCoreSquare(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            this.content = screenManager.Game.Content;
            spriteBatch = screenManager.SpriteBatch;
            this.SCREENHEIGHT = screenManager.GraphicsDevice.Viewport.Height;
            this.SCREENWIDTH = screenManager.GraphicsDevice.Viewport.Width;
        }

         //Load Content
        public override void Initialize()
        {
            //Load mask
            myImage = content.Load<Texture2D>(@"Textures/SSVEP_Textures/solid1");
            mask = content.Load<Texture2D>(@"textures/Ring Textures/maskS3");

            // Load Textures
            Arrow[9] = content.Load<Texture2D>(@"Textures/Ring Textures/upArrow");
            Arrow[11] = content.Load<Texture2D>(@"Textures/Ring Textures/rightArrow");
            Arrow[13] = content.Load<Texture2D>(@"Textures/Ring Textures/downArrow");
            Arrow[15] = content.Load<Texture2D>(@"Textures/Ring Textures/leftArrow");
            
            // Initialize targets
            InitializePointsForArrow();

            //Initiate android comm
            sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //send_to_address = IPAddress.Parse("10.252.28.29");          //Android ip (debug phone)
            send_to_address = IPAddress.Parse("10.252.31.166");  
            sending_end_point = new IPEndPoint(send_to_address, 1235);  //Android port
            
            //Initialize android command buffers
            forward = Encoding.ASCII.GetBytes("a");
            backward = Encoding.ASCII.GetBytes("b");
            left = Encoding.ASCII.GetBytes("c");
            right = Encoding.ASCII.GetBytes("d");
            stop = Encoding.ASCII.GetBytes("e");

            //Initialize the video decoder
            videoDecoder = new MjpegDecoder();
            videoDecoder.ParseStream(new Uri(camUri));

            base.Initialize();
        }

         //Update Game
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            try
            { UpdateFromBCI2000(elapsedTime); }
            catch (Exception) { }

            KeyboardOverride();

            base.Update(elapsedTime, totalTime);
        }

        public void KeyboardOverride()
        {
            //Simulate bci2000 input
            KeyboardState keyboardState = Keyboard.GetState();

            //OutputClass = 0;
            //int OutputClass = 0;

            if (keyboardState.IsKeyUp(Keys.Up) && oldKeyboardState.IsKeyDown(Keys.Up))
            {
                sending_socket.SendTo(forward, sending_end_point);
                FB = Radial[9];
                fbArrow = Arrow[9];
            }
            else if (keyboardState.IsKeyUp(Keys.Down) && oldKeyboardState.IsKeyDown(Keys.Down))
            {
                sending_socket.SendTo(stop, sending_end_point);
                FB = Radial[13];
                fbArrow = Arrow[13];
            }
            else if (keyboardState.IsKeyUp(Keys.Right) && oldKeyboardState.IsKeyDown(Keys.Right))
            {
                sending_socket.SendTo(right, sending_end_point);
                FB = Radial[11];
                fbArrow = Arrow[11];
            }
            else if (keyboardState.IsKeyUp(Keys.Left) && oldKeyboardState.IsKeyDown(Keys.Left))
            {
                sending_socket.SendTo(left, sending_end_point);
                FB = Radial[15];
                fbArrow = Arrow[15];
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && oldKeyboardState.IsKeyDown(Keys.Space))
            {
                sending_socket.SendTo(stop, sending_end_point);
                FB = Vector2.Zero;
            }

            oldKeyboardState = keyboardState;
        }

        //Update the android udp com based on input from BCI2000
        public void UpdateFromBCI2000(TimeSpan elapsedTime)
        {
            if ((bool)screenManager.Game.Services.GetService(typeof(bool)))
            {
                int command = (int)screenManager.Game.Services.GetService(typeof(int));

                switch (command)
                {
                    case 0:
                        sending_socket.SendTo(stop, sending_end_point);
                        FB = Vector2.Zero;
                        //Console.WriteLine("e");
                        break;
                    case 1:
                        sending_socket.SendTo(forward, sending_end_point);
                        FB = Radial[9];
                        fbArrow = Arrow[9];
                        //Console.WriteLine("a");
                        break;
                    case 2: //This normally means go backwards, but for the robot, we want it to stop instead
                        sending_socket.SendTo(stop, sending_end_point);
                        FB = Radial[13];
                        fbArrow = Arrow[13];
                        //Console.WriteLine("e");
                        break;
                    case 3:
                        sending_socket.SendTo(right, sending_end_point);
                        FB = Radial[11];
                        fbArrow = Arrow[11];
                        //Console.WriteLine("c");
                        break;
                    case 4:
                        sending_socket.SendTo(left, sending_end_point);
                        FB = Radial[15];
                        fbArrow = Arrow[15];
                        //Console.WriteLine("d");
                        break;
                    case 99:
                        break;
                    default:
                        break;
                }
                //set the new command status to false
                screenManager.Game.Services.RemoveService(typeof(bool));
                screenManager.Game.Services.AddService(typeof(bool), false);
            }
        }

         //Draw Camera Feed as feedback
        public override void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            //screenManager.GraphicsDevice.Clear(myColor);
            spriteBatch.Begin();
            try
            {
                //Grab Current Image
                camImage = videoDecoder.GetMjpegFrame(screenManager.GraphicsDevice);
                

                //Draw the image (video stream)
                if (camImage != null)
                {
                    spriteBatch.Draw(camImage, new Vector2(SCREENWIDTH / 2.0f - ((camImage.Height * videoScale) / 2.0f), SCREENHEIGHT / 2.0f + ((camImage.Width * videoScale) / 2.0f)), null, Color.White, -(float)Math.PI/2, Vector2.Zero, videoScale, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(myImage, new Vector2(SCREENWIDTH / 2.0f - 320, SCREENHEIGHT / 2.0f - 320), Color.White);   
                }
            }
            catch (Exception) { }

            //Draw Mask
          //  spriteBatch.Draw(mask, Vector2.Zero, Color.DarkGray);

            ////Draw All Fixation Crosses
            //for (int i = 9; i < 16; i = i + 2)
            //{
            //    spriteBatch.Draw(Arrow[i], Radial[i], null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 1);
            //}

            ////Draw feedback target
            //if (FB.X != 0 && FB.Y != 0)
            //{
            //    spriteBatch.Draw(fbArrow, FB, null, Color.FromNonPremultiplied(new Vector4(0, 0.8f, 0, 1)), 0, Vector2.Zero, 0.1f, SpriteEffects.None, 1);
            //}

            spriteBatch.End();

            base.Draw(elapsedTime, totalTime);
        }

        // Initialize Arrow Positions
        private void InitializePointsForArrow(){
            int cnt = 0;
            // Draws a ring
            int cx = 0; int cy = 0; float r = (float)(SCREENHEIGHT / 2);
            float r2 = r - 120f;
            #region CrossPositions
            float max = r2 - 50;
            float interval = max / 5;
            float x = r; float y = 0;
            float x2 = r2; float y2 = 0;

            //Radial
            cnt = 0; float[] radius = { r2 + 60, r2 - 85 };  //middle ring(ring closest to stimuli): r2+60. inner ring (ring furthest from stimuli): r2-85
            for (int j = 0; j < 2; j++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    x = ((SCREENWIDTH / 2) - ((Arrow[11].Bounds.Width * 0.1f) / 2)) - ((float)((radius[j]) * Math.Cos(MathHelper.ToRadians((45f * i)))));
                    y = ((SCREENHEIGHT / 2) - ((Arrow[11].Bounds.Height * 0.1f) / 2)) - ((float)((radius[j]) * Math.Sin(MathHelper.ToRadians((45f * i)))));
                    Radial[cnt] = new Vector2(x, y);
                    cnt++;
                }
            }
            x = ((SCREENWIDTH / 2) - ((Arrow[11].Bounds.Width * 0.1f) / 2));
            y = ((SCREENHEIGHT / 2) - ((Arrow[11].Bounds.Height * 0.1f) / 2));
            Radial[cnt] = new Vector2(x, y);
            #endregion CrossPositions
        }

        // Initialize Cross positions
        private void InitializePointsForCross()
        {
            int cnt = 0;
            // Draws a ring
            int cx = 0; int cy = 0; float r = (float)(SCREENHEIGHT / 2);
            float r2 = r - 120f;
            #region CrossPositions
            float max = r2 - 50;
            float interval = max / 5;
            float x = r; float y = 0;
            float x2 = r2; float y2 = 0;

            //Radial
            cnt = 0; float[] radius = { r2 + 60, r2 - 85 };
            for (int j = 0; j < 2; j++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    x = ((SCREENWIDTH / 2) - ((Cross.Bounds.Width * 0.1f) / 2)) - ((float)((radius[j]) * Math.Cos(MathHelper.ToRadians((45f * i)))));
                    y = ((SCREENHEIGHT / 2) - ((Cross.Bounds.Height * 0.1f) / 2)) - ((float)((radius[j]) * Math.Sin(MathHelper.ToRadians((45f * i)))));
                    Radial[cnt] = new Vector2(x, y);
                    cnt++;
                }
            }
            x = ((SCREENWIDTH / 2) - ((Cross.Bounds.Width * 0.1f) / 2));
            y = ((SCREENHEIGHT / 2) - ((Cross.Bounds.Height * 0.1f) / 2));
            Radial[cnt] = new Vector2(x, y);
            #endregion CrossPositions
        }
        
    }
}
