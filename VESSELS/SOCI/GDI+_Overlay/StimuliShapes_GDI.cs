using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace VESSELS.SOCI.GDI_Overlay
{   //Shapes
    public enum Shape
    {
        Rectangle,
        Triangle,
        Arrow,
        ArrowRectangle,
        Angle,
        Frame,
        Lightning
    }

    //Stim Type
    public enum Type
    {
        Up, Down, Right, Left
    }

    //Stimuli Structure
    public struct StimStruct
    {
        public Shape shape;
        public Rectangle stimRect;
        public Bitmap bmpOn;
        public Bitmap bmpOff;
        public Type type;
        public double sizeOffset;
    }

    class StimuliShapes_GDI
    {
        #region Fields

        //Global Path to images
        //protected string path = @"D:\Dropbox\SSVEP_Project\SSVEP_Overlay\SSVEP_Overlay\SSVEP_OverlayContent\Images\";
        protected string path = @"C:\Users\Nick Waytowich\Dropbox\SSVEP_Project\SSVEP_Overlay\SSVEP_Overlay\SSVEP_OverlayContent\Images\";

        //Rectangles
        public Bitmap[] rectangle = new Bitmap[8];

        //Triangles
        public Bitmap[] triangle = new Bitmap[8];

        //Arrows
        public Bitmap[] arrow = new Bitmap[8];

        //Arrow Rectangles
        public Bitmap[] arrowRect = new Bitmap[8];

        //Angles
        public Bitmap[] angle = new Bitmap[8];

        //Frames
        public Bitmap[] frame = new Bitmap[8];

        //Lightning
        public Bitmap[] lightning = new Bitmap[8];

        //Bounding Rectangles
        public Rectangle[] rect = new Rectangle[4];

        #endregion Fields

        //Constructor
        public StimuliShapes_GDI(Form form)
        {
            loadImages();
        }

        //Load Bitmap Images
        private void loadImages()
        {
            //Rectangles
            rectangle[0] = new Bitmap(Path.Combine(path + @"rectangles\recUb.png"));  //Ub
            rectangle[1] = new Bitmap(Path.Combine(path + @"rectangles\recDb.png"));  //Db
            rectangle[2] = new Bitmap(Path.Combine(path + @"rectangles\recRb.png"));  //Rb
            rectangle[3] = new Bitmap(Path.Combine(path + @"rectangles\recLb.png"));  //Lb
            rectangle[4] = new Bitmap(Path.Combine(path + @"rectangles\recUw.png"));  //Uw
            rectangle[5] = new Bitmap(Path.Combine(path + @"rectangles\recDw.png"));  //Dw
            rectangle[6] = new Bitmap(Path.Combine(path + @"rectangles\recRw.png"));  //Rw
            rectangle[7] = new Bitmap(Path.Combine(path + @"rectangles\recLw.png"));  //Lw

            //Triangles
            triangle[0] = new Bitmap(Path.Combine(path + @"triangles\triUb.png"));
            triangle[1] = new Bitmap(Path.Combine(path + @"triangles\triDb.png"));
            triangle[2] = new Bitmap(Path.Combine(path + @"triangles\triRb.png"));
            triangle[3] = new Bitmap(Path.Combine(path + @"triangles\triLb.png"));
            triangle[4] = new Bitmap(Path.Combine(path + @"triangles\triUw.png"));
            triangle[5] = new Bitmap(Path.Combine(path + @"triangles\triDw.png"));
            triangle[6] = new Bitmap(Path.Combine(path + @"triangles\triRw.png"));
            triangle[7] = new Bitmap(Path.Combine(path + @"triangles\triLw.png"));

            //Arrows
            arrow[0] = new Bitmap(Path.Combine(path + @"arrows\arUb.png"));
            arrow[1] = new Bitmap(Path.Combine(path + @"arrows\arDb.png"));
            arrow[2] = new Bitmap(Path.Combine(path + @"arrows\arRb.png"));
            arrow[3] = new Bitmap(Path.Combine(path + @"arrows\arLb.png"));
            arrow[4] = new Bitmap(Path.Combine(path + @"arrows\arUw.png"));
            arrow[5] = new Bitmap(Path.Combine(path + @"arrows\arDw.png"));
            arrow[6] = new Bitmap(Path.Combine(path + @"arrows\arRw.png"));
            arrow[7] = new Bitmap(Path.Combine(path + @"arrows\arLw.png"));

            //Arrow Rectangles
            arrowRect[0] = new Bitmap(Path.Combine(path + @"arrowRect\arrUb.png"));
            arrowRect[1] = new Bitmap(Path.Combine(path + @"arrowRect\arrDb.png"));
            arrowRect[2] = new Bitmap(Path.Combine(path + @"arrowRect\arrRb.png"));
            arrowRect[3] = new Bitmap(Path.Combine(path + @"arrowRect\arrLb.png"));
            arrowRect[4] = new Bitmap(Path.Combine(path + @"arrowRect\arrUw.png"));
            arrowRect[5] = new Bitmap(Path.Combine(path + @"arrowRect\arrDw.png"));
            arrowRect[6] = new Bitmap(Path.Combine(path + @"arrowRect\arrRw.png"));
            arrowRect[7] = new Bitmap(Path.Combine(path + @"arrowRect\arrLw.png"));

            //Angles
            angle[0] = new Bitmap(Path.Combine(path + @"angles\anUb.png"));
            angle[1] = new Bitmap(Path.Combine(path + @"angles\anDb.png"));
            angle[2] = new Bitmap(Path.Combine(path + @"angles\anRb.png"));
            angle[3] = new Bitmap(Path.Combine(path + @"angles\anLb.png"));
            angle[4] = new Bitmap(Path.Combine(path + @"angles\anUw.png"));
            angle[5] = new Bitmap(Path.Combine(path + @"angles\anDw.png"));
            angle[6] = new Bitmap(Path.Combine(path + @"angles\anRw.png"));
            angle[7] = new Bitmap(Path.Combine(path + @"angles\anLw.png"));

            //Frames
            frame[0] = new Bitmap(Path.Combine(path + @"frames\frUb.png"));
            frame[1] = new Bitmap(Path.Combine(path + @"frames\frUb.png"));
            frame[2] = new Bitmap(Path.Combine(path + @"frames\frUb.png"));
            frame[3] = new Bitmap(Path.Combine(path + @"frames\frUb.png"));
            frame[4] = new Bitmap(Path.Combine(path + @"frames\frUw.png"));
            frame[5] = new Bitmap(Path.Combine(path + @"frames\frUw.png"));
            frame[6] = new Bitmap(Path.Combine(path + @"frames\frUw.png"));
            frame[7] = new Bitmap(Path.Combine(path + @"frames\frUw.png"));

            //Lightning
            lightning[0] = new Bitmap(Path.Combine(path + @"lightning\liUb.png"));
            lightning[1] = new Bitmap(Path.Combine(path + @"lightning\liUb.png"));
            lightning[2] = new Bitmap(Path.Combine(path + @"lightning\liUb.png"));
            lightning[3] = new Bitmap(Path.Combine(path + @"lightning\liUb.png"));
            lightning[4] = new Bitmap(Path.Combine(path + @"lightning\liUw.png"));
            lightning[5] = new Bitmap(Path.Combine(path + @"lightning\liUw.png"));
            lightning[6] = new Bitmap(Path.Combine(path + @"lightning\liUw.png"));
            lightning[7] = new Bitmap(Path.Combine(path + @"lightning\liUw.png"));
        }

        //Get the bounds of the images
        public StimStruct GetBoundingRects(StimStruct stimStruct)
        { 
            //Helper vars for switching shapes
            Size tempRect = stimStruct.stimRect.Size;
            Size tempSize = tempRect;
                switch (stimStruct.shape)
                {
                    case Shape.Angle:
                            tempSize = new Size((int)((double)angle[(int)stimStruct.type].Size.Width * stimStruct.sizeOffset), (int)((double)angle[(int)stimStruct.type].Size.Height * stimStruct.sizeOffset));
                            stimStruct.stimRect.Size=tempSize;
                            stimStruct.bmpOn = new Bitmap(angle[(int)stimStruct.type], tempSize);
                            stimStruct.bmpOff = new Bitmap(angle[(int)stimStruct.type + 4], tempSize);
                        break;
                    case Shape.Arrow:
                        tempSize = new Size((int)((double)arrow[(int)stimStruct.type].Size.Width * stimStruct.sizeOffset), (int)((double)arrow[(int)stimStruct.type].Size.Height * stimStruct.sizeOffset));
                            stimStruct.stimRect.Size=tempSize;
                            stimStruct.bmpOn = new Bitmap(arrow[(int)stimStruct.type], tempSize);
                            stimStruct.bmpOff = new Bitmap(arrow[(int)stimStruct.type + 4], tempSize);
                        break;
                    case Shape.ArrowRectangle:
                        tempSize = new Size((int)((double)arrowRect[(int)stimStruct.type].Size.Width * stimStruct.sizeOffset), (int)((double)arrowRect[(int)stimStruct.type].Size.Height * stimStruct.sizeOffset));
                            stimStruct.stimRect.Size=tempSize;
                            stimStruct.bmpOn = new Bitmap(arrowRect[(int)stimStruct.type], tempSize);
                            stimStruct.bmpOff = new Bitmap(arrowRect[(int)stimStruct.type + 4], tempSize);
                        break;
                    case Shape.Frame:
                        tempSize = new Size((int)((double)frame[(int)stimStruct.type].Size.Width * stimStruct.sizeOffset), (int)((double)frame[(int)stimStruct.type].Size.Height * stimStruct.sizeOffset));
                            stimStruct.stimRect.Size=tempSize;
                            stimStruct.bmpOn = new Bitmap(frame[(int)stimStruct.type], tempSize);
                            stimStruct.bmpOff = new Bitmap(frame[(int)stimStruct.type + 4], tempSize);
                        break;
                    case Shape.Lightning:
                        tempSize = new Size((int)((double)lightning[(int)stimStruct.type].Size.Width * stimStruct.sizeOffset), (int)((double)lightning[(int)stimStruct.type].Size.Height * stimStruct.sizeOffset));
                            stimStruct.stimRect.Size=tempSize;
                            stimStruct.bmpOn = new Bitmap(lightning[(int)stimStruct.type], tempSize);
                            stimStruct.bmpOff = new Bitmap(lightning[(int)stimStruct.type + 4], tempSize);
                        break;
                    case Shape.Rectangle:
                            tempSize = new Size((int)((double)rectangle[(int)stimStruct.type].Size.Width * stimStruct.sizeOffset), (int)((double)rectangle[(int)stimStruct.type].Size.Height * stimStruct.sizeOffset));
                            stimStruct.stimRect.Size=tempSize;
                            stimStruct.bmpOn = new Bitmap(rectangle[(int)stimStruct.type], tempSize);
                            stimStruct.bmpOff = new Bitmap(rectangle[(int)stimStruct.type + 4], tempSize);
                        break;
                    case Shape.Triangle:
                        tempSize = new Size((int)((double)triangle[(int)stimStruct.type].Size.Width * stimStruct.sizeOffset), (int)((double)triangle[(int)stimStruct.type].Size.Height * stimStruct.sizeOffset));
                            stimStruct.stimRect.Size=tempSize;
                            stimStruct.bmpOn = new Bitmap(triangle[(int)stimStruct.type], tempSize);
                            stimStruct.bmpOff = new Bitmap(triangle[(int)stimStruct.type + 4], tempSize);
                        break;
                }
                if ((int)stimStruct.type == 0)
                    stimStruct.stimRect.Offset((tempRect.Width - tempSize.Width) / 2, 0);
                else if ((int)stimStruct.type == 1)
                    stimStruct.stimRect.Offset((tempRect.Width - tempSize.Width) / 2, (tempRect.Height - tempSize.Height));
                else if ((int)stimStruct.type == 2)
                    stimStruct.stimRect.Offset((tempRect.Width - tempSize.Width), (tempRect.Height - tempSize.Height) / 2);
                else if ((int)stimStruct.type == 3)
                    stimStruct.stimRect.Offset(0, (tempRect.Height - tempSize.Height) / 2);
            return stimStruct;
        }
    }
}
