using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace VESSELS
{
    class RingStimulator : DrawableGameComponent2
    {
        #region Fields

        // stimulus tracker variables 
        int State = 1; int Cntr1 = 0;
        int State2 = 1; int Cntr2 = 0;
        int State3 = 1; int Cntr3 = 0;
        int State4 = 1; int Cntr4 = 0;
        int color1 = 1; int color2 = 1;
        int color3 = 1; int color4 = 1;

        int SCREENHEIGHT;
        int SCREENWIDTH;
        //Vertex buffers
        VertexBuffer vertexBuffer;
        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        BasicEffect basicEffect;
        VertexDeclaration vertexDeclaration;
        VertexPositionColor[] pointList;
        VertexPositionColor[] pointList2;
        VertexPositionColor[] MasterPointList;
        //vertex positionColors for the individual triangle strips
        VertexPositionColor[] Strip1, Strip2, Strip3, Strip4, Strip5, Strip6, Strip7, Strip8;
        short[] triangleStripIndices;

        int subpoints = 18;
        int points = 65;
        int MasterPoints = 130;
        #endregion Fields

        // graphics
        RasterizerState rasterizerState;
        GraphicsDevice graphics;

        //Constructor
        public RingStimulator(ScreenManager screenManager)
        {
            graphics = screenManager.GraphicsDevice;
            SCREENHEIGHT = graphics.Viewport.Height;
            SCREENWIDTH = graphics.Viewport.Width;
        }

        //Initialize
        public override void Initialize()
        {
            // Initialize Ring construction
            InitializeTransform();
            InitializeEffect();
            InitializePoints();
            InitializeTriangleStrips();
            rasterizerState = new RasterizerState();
            rasterizerState.FillMode = FillMode.Solid;
            rasterizerState.CullMode = CullMode.None;



            base.Initialize();
        }

        #region RingConstruction
        /// <summary>
        /// Initializes the transforms used by the game.
        /// </summary>
        private void InitializeTransform()
        {
            viewMatrix = Matrix.CreateLookAt(
                new Vector3(0.0f, 0.0f, 1.0f),
                Vector3.Zero,
                Vector3.Up
                );

            projectionMatrix = Matrix.CreateOrthographic(
                  (float)graphics.Viewport.Width,
                  (float)graphics.Viewport.Height,
                  1.0f, 10.0f);
        }

        /// <summary>
        /// Initializes the effect (loading, parameter setting, and technique selection)
        /// used by the game.
        /// </summary>
        private void InitializeEffect()
        {
            vertexDeclaration = new VertexDeclaration(new VertexElement[]
                { 
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                }
            );

            basicEffect = new BasicEffect(graphics);
            basicEffect.VertexColorEnabled = true;
            worldMatrix = Matrix.CreateTranslation(Vector3.Zero);
            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;
        }

        /// <summary>
        /// Initializes the point list. // Ring
        /// </summary>
        private void InitializePoints()
        {
            pointList = new VertexPositionColor[points];
            pointList2 = new VertexPositionColor[points];
            MasterPointList = new VertexPositionColor[2 * points];
            Strip1 = new VertexPositionColor[subpoints]; Strip2 = new VertexPositionColor[subpoints]; Strip3 = new VertexPositionColor[subpoints]; Strip4 = new VertexPositionColor[subpoints];
            Strip5 = new VertexPositionColor[subpoints]; Strip6 = new VertexPositionColor[subpoints]; Strip7 = new VertexPositionColor[subpoints]; Strip8 = new VertexPositionColor[subpoints];

            // Draws a ring
            int cx = 0; int cy = 0; float r = (float)(SCREENHEIGHT / 2);
            float r2 = r - 120f;
            float theta = (float)(2 * Math.PI) / (points - 1);
            float tangential_factor = (float)Math.Tan(theta);
            float radial_factor = (float)Math.Cos(theta);
            float x = r; float y = 0;
            float x2 = r2; float y2 = 0;

            //tangential vectors
            float tx = 0; float ty = 0; float tx2 = 0; float ty2 = 0;

            // initilize ring points
            for (int i = 0; i < points - 1; i++)
            {
                // save circle indicies
                pointList[i] = new VertexPositionColor(new Vector3(x + cx, y + cy, 0), Color.White);
                pointList2[i] = new VertexPositionColor(new Vector3(x2 + cx, y2 + cy, 0), Color.White);
                //calculate tangential vector
                tx = -y; ty = x;
                tx2 = -y2; ty2 = x2;

                //add tangential vector
                x += tx * tangential_factor;
                y += ty * tangential_factor;
                x2 += tx2 * tangential_factor;
                y2 += ty2 * tangential_factor;

                //correct using radial vector
                x *= radial_factor;
                y *= radial_factor;
                x2 *= radial_factor;
                y2 *= radial_factor;
            }



            //add the last point
            pointList[points - 1] = new VertexPositionColor(new Vector3(r, 0, 0), Color.White);
            pointList2[points - 1] = new VertexPositionColor(new Vector3(r2, 0, 0), Color.White);


            //combine into master point list
            int cnt = 0;
            for (int i = 0; i < points; i++)
            {
                MasterPointList[cnt] = pointList[i];
                cnt++;
                MasterPointList[cnt] = pointList2[i];
                cnt++;
            }
            points = MasterPoints;

            //create the 8 subsets for the triangle strips
            for (int i = 0; i < subpoints; i++)
            {
                Strip1[i] = MasterPointList[i];
                Strip2[i] = MasterPointList[i + (subpoints - 2)];
                Strip3[i] = MasterPointList[i + (subpoints - 2) * 2];
                Strip4[i] = MasterPointList[i + (subpoints - 2) * 3];
                Strip5[i] = MasterPointList[i + (subpoints - 2) * 4];
                Strip6[i] = MasterPointList[i + (subpoints - 2) * 5];
                Strip7[i] = MasterPointList[i + (subpoints - 2) * 6];
                Strip8[i] = MasterPointList[i + (subpoints - 2) * 7];
            }

            // Initialize the vertex buffer, allocating memory for each vertex.
            vertexBuffer = new VertexBuffer(graphics, vertexDeclaration,
                points, BufferUsage.None);

            // Set the vertex buffer data to the array of vertices.
            vertexBuffer.SetData<VertexPositionColor>(MasterPointList);

        }

        /// <summary>
        /// Initializes the triangle strip.
        /// </summary>
        private void InitializeTriangleStrips()
        {
            // Initialize an array of indices of type short.
            triangleStripIndices = new short[subpoints];

            // Populate the array with references to indices in the vertex buffer.
            for (int i = 0; i < subpoints; i++)
            {
                triangleStripIndices[i] = (short)i;
            }
        }

        #endregion RingConstruction

        /// <summary>
        /// Updates the stimuli states
        /// </summary>
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            // Update the stimuli
            UpdateStimulus();
        }

        /// <summary>
        /// Draws the stimuli States
        /// </summary>
        public override void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            //Draw Triangle Strip
            DrawTriangleStrip();
        }

        /// <summary>
        /// Draws the triangle strip.
        /// </summary>
        private void DrawTriangleStrip()
        {
            //color3 = 0; color1 = 1; color2 = 1; color4 = 0;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.RasterizerState = rasterizerState;
                //Strip 1  right top
                for (int i = 0; i < Strip1.Length; i++)
                    Strip1[i].Color = Color.FromNonPremultiplied(new Vector4(color3, color3, color3, 1));

                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    Strip1,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    subpoints,  // number of vertices to draw
                    triangleStripIndices,
                    0,  // first index element to read
                    subpoints - 2   // number of primitives to draw
                );

                // Strip 2 vert
                for (int i = 0; i < Strip2.Length; i++)
                    Strip2[i].Color = Color.FromNonPremultiplied(new Vector4(color1, color1, color1, 1));

                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    Strip2,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    subpoints,  // number of vertices to draw
                    triangleStripIndices,
                    0,  // first index element to read
                    subpoints - 2   // number of primitives to draw
                );

                // Strip 3 vert 
                for (int i = 0; i < Strip3.Length; i++)
                    Strip3[i].Color = Color.FromNonPremultiplied(new Vector4(color1, color1, color1, 1));
                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    Strip3,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    subpoints,  // number of vertices to draw
                    triangleStripIndices,
                    0,  // first index element to read
                    subpoints - 2   // number of primitives to draw
                );

                //// Strip 4 left top
                for (int i = 0; i < Strip4.Length; i++)
                    Strip4[i].Color = Color.FromNonPremultiplied(new Vector4(color4, color4, color4, 1));

                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    Strip4,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    subpoints,  // number of vertices to draw
                    triangleStripIndices,
                    0,  // first index element to read
                    subpoints - 2   // number of primitives to draw
                );

                // Strip 5 left bottom
                for (int i = 0; i < Strip5.Length; i++)
                    Strip5[i].Color = Color.FromNonPremultiplied(new Vector4(color4, color4, color4, 1));

                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    Strip5,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    subpoints,  // number of vertices to draw
                    triangleStripIndices,
                    0,  // first index element to read
                    subpoints - 2   // number of primitives to draw
                );

                // Strip 6
                for (int i = 0; i < Strip6.Length; i++)
                    Strip6[i].Color = Color.FromNonPremultiplied(new Vector4(color2, color2, color2, 1));

                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    Strip6,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    subpoints,  // number of vertices to draw
                    triangleStripIndices,
                    0,  // first index element to read
                    subpoints - 2   // number of primitives to draw
                );

                // Strip 7
                for (int i = 0; i < Strip7.Length; i++)
                    Strip7[i].Color = Color.FromNonPremultiplied(new Vector4(color2, color2, color2, 1));

                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    Strip7,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    subpoints,  // number of vertices to draw
                    triangleStripIndices,
                    0,  // first index element to read
                    subpoints - 2   // number of primitives to draw
                );

                // Strip 8 right bottom
                for (int i = 0; i < Strip8.Length; i++)
                    Strip8[i].Color = Color.FromNonPremultiplied(new Vector4(color3, color3, color3, 1));

                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    Strip8,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    subpoints,  // number of vertices to draw
                    triangleStripIndices,
                    0,  // first index element to read
                    subpoints - 2   // number of primitives to draw
                );


            }
        
        }

        /// <summary>
        /// Updates the states of the stimuli
        /// </summary>
        void UpdateStimulus()
        {
            //update the counters
            Cntr1++; Cntr2++;
            Cntr3++; Cntr4++;

            //Update the 8.5hz stimulus 
            if (Cntr1 == 3 && State == 1)
            {
                color1 = 1;
                Cntr1 = 0;
                State = 0;
            }
            else if (Cntr1 == 4 && State == 0)
            {
                color1 = 0;
                Cntr1 = 0;
                State = 1;
            }

            //Update the 6hz stimulus 
            if (Cntr2 == 5 && State2 == 1)
            {
                color2 = 1;
                Cntr2 = 0;
                State2 = 0;
            }
            else if (Cntr2 == 5 && State2 == 0)
            {
                color2 = 0;
                Cntr2 = 0;
                State2 = 1;
            }

            //Update the 7.5hz stimulus 
            if (Cntr3 == 4 && State3 == 1)
            {
                color3 = 1;
                Cntr3 = 0;
                State3 = 0;
            }
            else if (Cntr3 == 4 && State3 == 0)
            {
                color3 = 0; ;
                Cntr3 = 0;
                State3 = 1;
            }

            //Update the 6.66hz stimulus 
            if (Cntr4 == 4 && State4 == 1)
            {
                color4 = 1;
                Cntr4 = 0;
                State4 = 0;
            }
            else if (Cntr4 == 5 && State4 == 0)
            {
                color4 = 0;
                Cntr4 = 0;
                State4 = 1;
            }
        }

    }
}
