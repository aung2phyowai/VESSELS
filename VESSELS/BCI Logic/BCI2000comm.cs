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

namespace VESSELS
{
    class BCI2000comm : GameComponent2
    {
        #region Fields        
        // UDP communication fields
        //Receive
        private const int listenPort=20321;
        UdpClient receiver;
        Socket receive_socket;
        IPAddress receive_from_address;
        IPEndPoint receive_end_point;
        UdpState s;
        byte[] receive_buffer;

        KeyboardState oldKeyboardState;

        //send
        int Cntr = 0;
        Socket sending_socket;
        IPAddress send_to_address;
        IPEndPoint sending_end_point;
        byte[] send_buffer;
        //string send_string;
        
        //byte offset
        int offset = 2608;

        //game
        Game game;

        #endregion Fields

        // Constructor
        public BCI2000comm(Game game)
        {
            this.game = game;
        }

        // UDP object State for Async callback
        struct UdpState {
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
            //receiver.Client.Bind(new IPEndPoint(receive_from_address,20321));

            s = new UdpState();
            s.e = receive_end_point;
            s.u = receiver;
            
            //register async callback for UDP communication
            receiver.BeginReceive(new AsyncCallback(ParseBCIdata), s);

            //Send
            //sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //send_to_address = IPAddress.Parse("192.168.0.6");               //Arduino ip
            //sending_end_point = new IPEndPoint(send_to_address, 8888);      //Arduino port
            //send_to_address = IPAddress.Parse("10.252.31.166");             //Android ip (primary phone)
            
            base.Initialize();
        }

       //dispose resources
        protected override void Dispose(bool disposing)
        {
            try
            {
                //Close udp client
                receiver.Close();
            }
            catch (Exception) { }
            //dispose
            base.Dispose(disposing);
        }

        //Updates the main BCI2000 socket loop 
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            //Simulate BCI2000 input
            SimBCI();  // For debugging purposes
            //Cntr++;
            //if (Cntr > 63)
            //{
            //    send_buffer = Encoding.ASCII.GetBytes("a");
            //    sending_socket.SendTo(send_buffer, sending_end_point);
            //    Console.WriteLine("a");
            //    Cntr = 0;
            //}


            base.Update(elapsedTime, totalTime);
        }
        int OutputClass;
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

        ///<summary>
        // Parse BCI data and translate into application commands
        ///</summary>
        void ParseBCIdata(IAsyncResult BciData)
        {
            //BciDataGlobal = BciData;
            //Parse out the client info from the state object
            UdpClient u = (UdpClient)((UdpState)(BciData.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(BciData.AsyncState)).e;
            //int OutputClass = 0;
            try
            {
                //*****************************
                //Translate into command. 2608 is the offset. 
                //2608 = 0 or 99 - Null condition - dont move
                //2609 = 1 - class 1 - move forward
                //2610 = 2 - class 2 - move Back
                //2611 = 3 - class 3 - move right
                //2612 = 4 - class 4 - move left
                //*****************************

                //get the actual data
                receive_buffer = u.EndReceive(BciData, ref receive_end_point);
                OutputClass = BitConverter.ToInt16(receive_buffer, 0) - offset;

                //Update the new command
                game.Services.RemoveService(typeof(int));
                game.Services.AddService(typeof(int), OutputClass);

                //Update the new command flag
                game.Services.RemoveService(typeof(bool));
                game.Services.AddService(typeof(bool), true);

                //register the async callback again
                receiver.BeginReceive(new AsyncCallback(ParseBCIdata), s);
            } catch (Exception){}
        }
    }
}
