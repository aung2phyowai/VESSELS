using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace VESSELS.SOCI.DirectX_Overlay
{
    class DXGDI_Interface
    {
        #region Fields

        //Forms drawing
        SolidBrush blackBrush;

        //GPU graphics back buffers
        protected System.Drawing.BufferedGraphics backBuffer;
        protected System.Drawing.BufferedGraphicsContext currenContext;

        //local copy of game form
        Form form;

        //GDI+ graphics drawing surface
        Graphics formGraphics;

        //Memory stream for texture2d to bmp conversion
        System.IO.MemoryStream mem;

        #endregion Fields

        //Constructor
        public DXGDI_Interface(Form form)
        {
            // local copy of game form
            this.form = form;

            //Create drawing surface from game form
            formGraphics= form.CreateGraphics();
  
            //Initialize back-buffer graphics
            currenContext = BufferedGraphicsManager.Current;
            backBuffer = currenContext.Allocate(formGraphics, form.ClientRectangle);

            //Initialize drawing brush
            blackBrush = new SolidBrush(Color.White);

        }

        //Update and draw the new rect positionss
        public void UpdateFormRect(StimStruct[] stimStruct)
        {
            //Clear back buffer
            backBuffer.Graphics.Clear(form.BackColor);
            //Update Rectangle pos and/or size and draw to the backbuffer
            for (int i = 0; i < 4; i++)
            {
                backBuffer.Graphics.FillRectangle(blackBrush, rectConvert(stimStruct[i].stimRect));
            }
            
            //Draw whenever updated
            drawForm();
        }


        //Update and draw the new rect positionss
        public void UpdateFormImages(StimStruct[] stimStruct)
        {
            //Clear back buffer
            backBuffer.Graphics.Clear(form.BackColor);
            //Update Rectangle pos and/or size and draw to the backbuffer
            for (int i = 0; i < 4; i++)
            {
                mem = new System.IO.MemoryStream();
                stimStruct[i].texture.SaveAsPng(mem, stimStruct[i].texture.Width, stimStruct[i].texture.Height);
                backBuffer.Graphics.DrawImage(new Bitmap(mem),rectConvert(stimStruct[i].stimRect));
            }
            //Draw whenever updated
            drawForm();
        }

        //draw
        public void drawForm()
        {
            //Render to the GCI+ surface
            backBuffer.Render();
        }

        // convert direct X (xna) rectangles to system.drawing rect
        private Rectangle rectConvert(Microsoft.Xna.Framework.Rectangle xnaRect)
        {
            return new Rectangle(xnaRect.X, xnaRect.Y, xnaRect.Width, xnaRect.Height);
        }
    }
}
