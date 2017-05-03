using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net.Configuration;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LSL;
namespace VESSELS
{
    class LSLcomm : GameComponent2
    {
        #region Fields      
         
        liblsl.StreamInfo[] streamInfo;
        liblsl.StreamInlet inlet;
        string[] command = new string[1];
        double ts;
        Game game;

        int OutputClass;
        KeyboardState oldKeyboardState;
        
        #endregion Fields

        // Constructor
        public LSLcomm(Game game)
        {
            this.game = game;
        }

        
        
        //Initialize communication
        public override void Initialize()
        {
            // resolve the current classification streams
            streamInfo = liblsl.resolve_stream("type", "Markers");

            // open an inlet to the classification stream
            inlet = new liblsl.StreamInlet(streamInfo[0]);

            // debug 
            System.Console.WriteLine(inlet.info().as_xml());

            base.Initialize();
        }

       //dispose resources
        protected override void Dispose(bool disposing)
        {
            //displose
            base.Dispose(disposing);
        }

        //Updates the main BCI2000 socket loop 
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            //Simulate BCI2000 input
            SimBCI();  // For debugging purposes

            // pull next sample in non-blocking mode
            ts = inlet.pull_sample(command, 0.0);
            if (ts != 0.0)
                sendCommand(Convert.ToInt32(command[0]));
            base.Update(elapsedTime, totalTime);
        }
        
        public void SimBCI()
        {
            //Simulate bci2000 input
            KeyboardState keyboardState = Keyboard.GetState();
            
                //OutputClass = 0;
                //int OutputClass = 67;

                if (keyboardState.IsKeyUp(Keys.Up) && oldKeyboardState.IsKeyDown(Keys.Up))
                {
                    OutputClass = 1;

                    //Update the new command
                    game.Services.RemoveService(typeof(int));
                    game.Services.AddService(typeof(int), OutputClass);

                    //Update the new command flag
                    game.Services.RemoveService(typeof(bool));
                    game.Services.AddService(typeof(bool), true);
                }
                else if (keyboardState.IsKeyUp(Keys.Down) && oldKeyboardState.IsKeyDown(Keys.Down))
                {
                    OutputClass = 2;
                    //Update the new command
                    game.Services.RemoveService(typeof(int));
                    game.Services.AddService(typeof(int), OutputClass);

                    //Update the new command flag
                    game.Services.RemoveService(typeof(bool));
                    game.Services.AddService(typeof(bool), true);
                }
                else if (keyboardState.IsKeyUp(Keys.Right) && oldKeyboardState.IsKeyDown(Keys.Right))
                {
                    OutputClass = 3;
                    //Update the new command
                    game.Services.RemoveService(typeof(int));
                    game.Services.AddService(typeof(int), OutputClass);

                    //Update the new command flag
                    game.Services.RemoveService(typeof(bool));
                    game.Services.AddService(typeof(bool), true);
                }
                else if (keyboardState.IsKeyUp(Keys.Left) && oldKeyboardState.IsKeyDown(Keys.Left))
                {
                    OutputClass = 4;
                    //Update the new command
                    game.Services.RemoveService(typeof(int));
                    game.Services.AddService(typeof(int), OutputClass);

                    //Update the new command flag
                    game.Services.RemoveService(typeof(bool));
                    game.Services.AddService(typeof(bool), true);
                }
                else if (keyboardState.IsKeyUp(Keys.Space) && oldKeyboardState.IsKeyDown(Keys.Space))
                {
                    OutputClass = 0;
                    //Update the new command
                    game.Services.RemoveService(typeof(int));
                    game.Services.AddService(typeof(int), OutputClass);

                    //Update the new command flag
                    game.Services.RemoveService(typeof(bool));
                    game.Services.AddService(typeof(bool), true);
                }

                oldKeyboardState = keyboardState;
            
                ////Update the new command
                //game.Services.RemoveService(typeof(int));
                //game.Services.AddService(typeof(int), OutputClass);

                ////Update the new command flag
                //game.Services.RemoveService(typeof(bool));
                //game.Services.AddService(typeof(bool), true);
            
            
        }


        void sendCommand(int command)
        {
            //Update the new command
            game.Services.RemoveService(typeof(int));
            game.Services.AddService(typeof(int), command);

            //Update the new command flag
            game.Services.RemoveService(typeof(bool));
            game.Services.AddService(typeof(bool), true);
        }

    }
}
