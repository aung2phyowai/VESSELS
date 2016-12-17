using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VESSELS.SOCI.DirectX_Overlay
{
    //Shapes
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
        public Texture2D texture;
        public Type type;
        public double stimScale;
    }

    class DirectX_Shape_Helper
    {
        //Fields
        static Texture2D[] textureShapes = new Texture2D[28];
        ContentManager content;

        //Constructor
        public DirectX_Shape_Helper(ContentManager content)
        {
            this.content = content;
            //Load textures
            loadImages();
        }

        //Load Texture images
        private void loadImages()
        {
            textureShapes[0] = content.Load<Texture2D>(@"Images\rectangles\recUw");
            textureShapes[1] = content.Load<Texture2D>(@"Images\triangles\triUw");
            textureShapes[2] = content.Load<Texture2D>(@"Images\arrows\arUw");
            textureShapes[3] = content.Load<Texture2D>(@"Images\arrowRect\arrUw");
            textureShapes[4] = content.Load<Texture2D>(@"Images\angles\anUw");
            textureShapes[5] = content.Load<Texture2D>(@"Images\frames\frUw");
            textureShapes[6] = content.Load<Texture2D>(@"Images\lightning\liUw");
            textureShapes[7] = content.Load<Texture2D>(@"Images\rectangles\recRw");
            textureShapes[8] = content.Load<Texture2D>(@"Images\triangles\triRw");
            textureShapes[9] = content.Load<Texture2D>(@"Images\arrows\arRw");
            textureShapes[10] = content.Load<Texture2D>(@"Images\arrowRect\arrRw");
            textureShapes[11] = content.Load<Texture2D>(@"Images\angles\anRw");
            textureShapes[12] = content.Load<Texture2D>(@"Images\frames\frUw");
            textureShapes[13] = content.Load<Texture2D>(@"Images\lightning\liUw");

            textureShapes[14] = content.Load<Texture2D>(@"Images\rectangles\recDw");
            textureShapes[15] = content.Load<Texture2D>(@"Images\triangles\triDw");
            textureShapes[16] = content.Load<Texture2D>(@"Images\arrows\arDw");
            textureShapes[17] = content.Load<Texture2D>(@"Images\arrowRect\arrDw");
            textureShapes[18] = content.Load<Texture2D>(@"Images\angles\anDw");
            textureShapes[19] = content.Load<Texture2D>(@"Images\frames\frUw");
            textureShapes[20] = content.Load<Texture2D>(@"Images\lightning\liUw");
            textureShapes[21] = content.Load<Texture2D>(@"Images\rectangles\recLw");
            textureShapes[22] = content.Load<Texture2D>(@"Images\triangles\triLw");
            textureShapes[23] = content.Load<Texture2D>(@"Images\arrows\arLw");
            textureShapes[24] = content.Load<Texture2D>(@"Images\arrowRect\arrLw");
            textureShapes[25] = content.Load<Texture2D>(@"Images\angles\anLw");
            textureShapes[26] = content.Load<Texture2D>(@"Images\frames\frUw");
            textureShapes[27] = content.Load<Texture2D>(@"Images\lightning\liUw");
        }

        //Get the stimulus shape
        public StimStruct GetShape(StimStruct stimStruct)
        {
            Rectangle tempRect = stimStruct.stimRect;
            Rectangle tempSize = tempRect;
            
            switch (stimStruct.shape)
            {
                case Shape.Angle:
                    if (stimStruct.type == Type.Up)
                        stimStruct.texture = textureShapes[(int)Shape.Angle];
                    else if(stimStruct.type == Type.Right)
                        stimStruct.texture = textureShapes[(int)Shape.Angle + 7];
                    else if(stimStruct.type == Type.Down)
                        stimStruct.texture = textureShapes[(int)Shape.Angle + 14];
                    else
                        stimStruct.texture = textureShapes[(int)Shape.Angle + 21];
                    break;
                case Shape.Arrow:
                    if (stimStruct.type == Type.Up)
                        stimStruct.texture = textureShapes[(int)Shape.Arrow];
                    else if(stimStruct.type == Type.Right)
                        stimStruct.texture = textureShapes[(int)Shape.Arrow + 7];
                    else if(stimStruct.type == Type.Down)
                        stimStruct.texture = textureShapes[(int)Shape.Arrow + 14];
                    else
                        stimStruct.texture = textureShapes[(int)Shape.Arrow + 21];
                    break;
                case Shape.ArrowRectangle:
                    if (stimStruct.type == Type.Up)
                        stimStruct.texture = textureShapes[(int)Shape.ArrowRectangle];
                    else if(stimStruct.type == Type.Right)
                        stimStruct.texture = textureShapes[(int)Shape.ArrowRectangle + 7];
                    else if(stimStruct.type == Type.Down)
                        stimStruct.texture = textureShapes[(int)Shape.ArrowRectangle + 14];
                    else
                        stimStruct.texture = textureShapes[(int)Shape.ArrowRectangle + 21];
                    break;
                case Shape.Frame:
                    stimStruct.texture = textureShapes[(int)Shape.Frame];
                    break;
                case Shape.Lightning:
                    stimStruct.texture = textureShapes[(int)Shape.Lightning];
                    break;
                case Shape.Rectangle:
                    if (stimStruct.type == Type.Up)
                        stimStruct.texture = textureShapes[(int)Shape.Rectangle];
                    else if(stimStruct.type == Type.Right)
                        stimStruct.texture = textureShapes[(int)Shape.Rectangle + 7];
                    else if(stimStruct.type == Type.Down)
                        stimStruct.texture = textureShapes[(int)Shape.Rectangle + 14];
                    else
                        stimStruct.texture = textureShapes[(int)Shape.Rectangle + 21];
                    break;
                case Shape.Triangle:
                    if (stimStruct.type == Type.Up)
                        stimStruct.texture = textureShapes[(int)Shape.Triangle];
                    else if(stimStruct.type == Type.Right)
                        stimStruct.texture = textureShapes[(int)Shape.Triangle + 7];
                    else if(stimStruct.type == Type.Down)
                        stimStruct.texture = textureShapes[(int)Shape.Triangle + 14];
                    else
                        stimStruct.texture = textureShapes[(int)Shape.Triangle + 21];
                    break;
            }

            tempSize.Width = (int)((double)stimStruct.texture.Width * stimStruct.stimScale);
            tempSize.Height = (int)((double)stimStruct.texture.Height * stimStruct.stimScale);
            stimStruct.stimRect.Width = tempSize.Width;
            stimStruct.stimRect.Height = tempSize.Height;

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
