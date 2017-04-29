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

//*****************************
//Recieve data from LSL and translate into device commands. 
//0 or 99 - Null condition - dont move
//1 - class 1 - move forward
//2 - class 2 - move Back
//3 - class 3 - move right
//4 - class 4 - move left
//*****************************

namespace VESSELS.BCI_Logic.BCI2000_Control
{
    class LSLcommSOCI : GameComponent
    {
        #region Fields

        liblsl.StreamInfo[] streamInfo;
        liblsl.StreamInlet inlet;
        string[] command = new string[1];
        double ts;
        Game game;

        #endregion Fields

        // Constructor
        public LSLcommSOCI(Game game)
            :base(game)
        {
            //reference to the main game instance
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
            

            //dispose
            base.Dispose(disposing);
        }

        //Updates the main BCI2000 socket loop 
        public override void Update(GameTime gameTime)
        {
            // pull next sample in non-blocking mode

            ts = inlet.pull_sample(command, 0.0);
            Console.WriteLine(command[0].ToString());
            if (ts != 0.0)
                sendCommand(Convert.ToInt32(command[0]));
          
            base.Update(gameTime);
        }
        
       
        //Translate output into device command
        protected virtual void sendCommand(int output)
        {
        }

    }
}
