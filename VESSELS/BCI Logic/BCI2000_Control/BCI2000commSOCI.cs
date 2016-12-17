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

//*****************************
//Recieve data from BCI2000 and translate into device commands. 
//2608 is the offset 2608 = 0 or 99 - Null condition - dont move
//2609 = 1 - class 1 - move forward
//2610 = 2 - class 2 - move Back
//2611 = 3 - class 3 - move right
//2612 = 4 - class 4 - move left
//*****************************

namespace VESSELS.BCI_Logic.BCI2000_Control
{
    class BCI2000commSOCI : GameComponent
    {
        #region Fields

        // UDP communication fields
        //Receive
        private const int listenPort = 20321;
        UdpClient receiver;
        Socket receive_socket;
        IPAddress receive_from_address;
        IPEndPoint receive_end_point;
        UdpState s;
        byte[] receive_buffer;
        KeyboardState oldKeyboardState;

        //byte offset
        int offset = 2608;

        #endregion Fields

        // Constructor
        public BCI2000commSOCI(Game game)
            :base(game)
        {
        }

        // UDP object State for Async callback
        struct UdpState
        {
            public IPEndPoint e;
            public UdpClient u;
        }

        //Initialize communication
        public override void Initialize()
        {
            //Initialize UDP communications
            //Receive
            receive_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            receive_from_address = IPAddress.Parse("127.0.0.1");
            receive_end_point = new IPEndPoint(receive_from_address, 20321);
            receiver = new UdpClient(receive_end_point);

            s = new UdpState();
            s.e = receive_end_point;
            s.u = receiver;

            //register async callback for UDP communication
            receiver.BeginReceive(new AsyncCallback(ParseBCIdata), s);

            base.Initialize();
        }

        //dispose resources
        protected override void Dispose(bool disposing)
        {
            //Close udp client
            receiver.Close();

            //dispose
            base.Dispose(disposing);
        }

        //Updates the main BCI2000 socket loop 
        public override void Update(GameTime gameTime)
        {
            //SimBCI();  // For debugging purposes
            //AutoBCI();
            base.Update(gameTime);
        }
        
        // Auto BCI
        int[] simArray = new int[4] {1, 3, 2, 4};
        int loopCntr = 0;
        int arrayCntr = 0;

        //Auto generate bci2000 commands
        public void AutoBCI()
        {

            loopCntr++;

            if (loopCntr >= 30)
            {
                sendCommand(simArray[arrayCntr]);
                arrayCntr++;
                loopCntr = 0;
            }

            if (arrayCntr >= 4)
            {
                arrayCntr = 0;
            }

        }

        //Simulate bci2000 with keypresses
        public void SimBCI()
        {
            //Simulate bci2000 input
            KeyboardState keyboardState = Keyboard.GetState();

            //OutputClass = 0;
            int OutputClass = 67;

            if (keyboardState.IsKeyUp(Keys.B) && oldKeyboardState.IsKeyDown(Keys.B))
            {
                OutputClass = 1;
                sendCommand(OutputClass);
            }
            if (keyboardState.IsKeyUp(Keys.V) && oldKeyboardState.IsKeyDown(Keys.V))
            {
                OutputClass = 2;
                sendCommand(OutputClass);
            }
            if (keyboardState.IsKeyUp(Keys.C) && oldKeyboardState.IsKeyDown(Keys.C))
            {
                OutputClass = 3;
                sendCommand(OutputClass);
            }
            if (keyboardState.IsKeyUp(Keys.X) && oldKeyboardState.IsKeyDown(Keys.X))
            {
                OutputClass = 4;
                sendCommand(OutputClass);
            }
            //Device Control
            

            oldKeyboardState = keyboardState;
        }

        ///<summary>
        // Parse BCI data and translate into application commands
        ///</summary>
        void ParseBCIdata(IAsyncResult BciData)
        {
            //Parse out the client info from the state object
            UdpClient u = (UdpClient)((UdpState)(BciData.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(BciData.AsyncState)).e;
            int OutputClass = 0;
            try
            {
                //get the actual data
                receive_buffer = u.EndReceive(BciData, ref receive_end_point);
                OutputClass = BitConverter.ToInt16(receive_buffer, 0) - offset;

                //Device Control
                sendCommand(OutputClass);

                //register the async callback again
                receiver.BeginReceive(new AsyncCallback(ParseBCIdata), s);
            }
            catch (Exception) { }
        }

        //Translate output into device command
        protected virtual void sendCommand(int output)
        {
        }

    }
}
