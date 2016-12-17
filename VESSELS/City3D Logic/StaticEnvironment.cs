using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VESSELS.City3D_Logic
{
    class StaticEnvironment : DrawableGameComponent2
    {
        
        //SkyBox fields
        Effect skyEffect;
        TextureCube skyTex;
        VertexBuffer vertices;
        IndexBuffer indices;
        const int number_of_vertices = 8;
        const int number_of_indices = 36;
        Vector3 pos;
        Matrix world;

        //Environment Models
        Model building;
        Model tunnel;
        Model box;
        Model boulder;
        Model cityBlock;
        Model newModel;

        Model France;

        //Environment Textures
        Texture2D groundTexture;
        Texture2D roadTexture;
        Texture2D grassTexture;

        //View and projection
        Matrix view;
        Matrix proj;
        static float viewAngle = MathHelper.PiOver4;
        static float nearClip = 0.1f;
        static float farClip = 1000000.0f;

        //Ground structure
        Ground ground;
        //Ground Effects
        BasicEffect groundEffect;

        //ScreenManager
        ScreenManager screenManager;
        ContentManager content;
        GraphicsDevice graphicsDevice;
        Game game;
        //Constructor
        public StaticEnvironment(ScreenManager screenManager, Game game, GraphicsDevice graphicsDevice)
        {
            this.screenManager = screenManager;
            content = screenManager.Game.Content;
            this.game = game;
            this.graphicsDevice = graphicsDevice;
        }


        //Initilize 
        public override void Initialize()
        {
            //Set up Projection Matrix (do once)
            float aspectRatio = (float)screenManager.GraphicsDevice.Viewport.Width / (float)screenManager.GraphicsDevice.Viewport.Width;
            proj = Matrix.CreatePerspectiveFieldOfView(viewAngle, aspectRatio, nearClip, farClip);

            //Initialize Ground
            InitializeGround();

            //LoadContent
            LoadContent();
            
            //Initialize Skybox 
            InitSkyBox();

            base.Initialize();
        }

        //Load all of the content for the static environment
        protected override void LoadContent()
        {
            //Load content
            cityBlock = content.Load<Model>(@"Models/OldTown/old town block");
            //cityBlock = content.Load<Model>(@"Models/turboSquid/street");
        }

        // Initilize the ground 
        public void InitializeGround()
        {
            //create new ground structure
            ground = new Ground(Vector3.Zero, Vector3.Backward, Vector3.Up, 1, 1);
            //create new ground basic effect
            groundEffect = new BasicEffect(screenManager.GraphicsDevice);
            groundEffect.EnableDefaultLighting();
            groundEffect.SpecularColor = new Vector3(0.55f, 0.55f, 0.55f);
            groundEffect.DiffuseColor = new Vector3(0.9f, 0.9f, 0.9f);
            //Define the world structure for the ground
            groundEffect.World = Matrix.CreateScale(500) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) *
                Matrix.Identity;
            groundEffect.View = view;
            groundEffect.Projection = proj;
            groundEffect.TextureEnabled = true;
            groundEffect.Texture = groundTexture;
        }

        // Initialize The SkyBox
        public void InitSkyBox()
        {
            skyEffect = content.Load<Effect>(@"Shaders/SkyEffect");
            skyTex = content.Load<TextureCube>(@"Textures/City3D/SkyBox");
            //skyTex = content.Load<TextureCube>(@"Models/Stage1/FranceCubeMap");
            skyEffect.Parameters["tex"].SetValue(skyTex);
            world = Matrix.Identity;
            //Create the verticies and indexes to hold the skybox (cube)
            CreateCubeVertexIndexBuffer();
            
        }



        //Update Environment Logic
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            try
            {
                //Get Current View Matrix
                view = (Matrix)game.Services.GetService(typeof(Matrix));
                //Get Camera Position
                pos = (Vector3)game.Services.GetService(typeof(Vector3));
            }
            catch (Exception e) { }
            
            //Move skybox over camera position
            world = Matrix.CreateScale(1f)* Matrix.CreateTranslation(pos);

                //Update ground view
                groundEffect.View = view;

            base.Update(elapsedTime,totalTime);
        }

        //Draw Environment
        public override void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            screenManager.GraphicsDevice.SetVertexBuffer(vertices);
            screenManager.GraphicsDevice.Indices = indices;

            ////Draw The SkyBox
            skyEffect.Parameters["WVP"].SetValue(world * view * proj);
            skyEffect.CurrentTechnique.Passes[0].Apply();
            screenManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, number_of_vertices, 0, number_of_indices / 3);

            //Draw ground
            DrawGround2();

            //Draw City
            DepthStencilState dsState = new DepthStencilState();
            dsState.DepthBufferEnable = true;
            dsState.DepthBufferWriteEnable = true;

            graphicsDevice.DepthStencilState = dsState;
            

            DrawModel(cityBlock, Matrix.CreateTranslation(new Vector3(0,-45,0))*Matrix.CreateScale(1f));// Matrix.CreateScale(8)*Matrix.CreateRotationX(MathHelper.ToRadians(-90)));
            //DrawModel(cityBlock, Matrix.CreateTranslation(new Vector3(0, -100, 0)) * Matrix.CreateScale(00.1f));// Matrix.CreateScale(8)*Matrix.CreateRotationX(MathHelper.ToRadians(-90)));
            base.Draw(elapsedTime,totalTime);
        }

        /// <summary>
        /// Draws The environment ground
        /// </summary>
        void DrawGround2()
        {
                    //Define the world structure for the ground
                    groundEffect.World = Matrix.CreateScale(10000) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) *
                       Matrix.CreateTranslation(new Vector3(0f, -1f, 0f));
                    //groundEffect.Texture = groundTexture;
                    groundEffect.AmbientLightColor= new Vector3(0.2f,0.1f,0.0f);
                    foreach (EffectPass pass in groundEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        screenManager.GraphicsDevice.DrawUserIndexedPrimitives
                            <VertexPositionNormalTexture>(
                            PrimitiveType.TriangleList,
                            ground.Vertices, 0, 4,
                            ground.Indexes, 0, 2);
                    }
        }

        /// <summary>
        /// Draws The environment ground
        /// </summary>
        void DrawGround()
        {

            for (int z = -7; z < 7; z++)
            {
                for (int x = -7; x < 7; x++)
                {
                    
                    //Define the world structure for the ground
                    groundEffect.World = Matrix.CreateScale(100) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) *
                       Matrix.CreateTranslation(new Vector3(x * 100f, 0f, z * 100f));
                    if (x == -5 || x == 0 || x == 5)
                        groundEffect.Texture = roadTexture;
                    else if (x == -6 || x == 0 || x == 6)
                        groundEffect.Texture = grassTexture;
                    else if (z == -5 || z == 0 || z == 5)
                    {
                        groundEffect.Texture = roadTexture;
                        groundEffect.World = Matrix.CreateScale(100) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(90))*Matrix.CreateTranslation(new Vector3(x * 100f, 0f, z * 100f));
                    }
                    else if (z == -6 || z == 0 || z == 6)
                        groundEffect.Texture = grassTexture;
                    else
                        groundEffect.Texture = groundTexture;

                    foreach (EffectPass pass in groundEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        screenManager.GraphicsDevice.DrawUserIndexedPrimitives
                            <VertexPositionNormalTexture>(
                            PrimitiveType.TriangleList,
                            ground.Vertices, 0, 4,
                            ground.Indexes, 0, 2);
                    }
                }
            }
        }

        /// <summary>
        /// Draws The environment buildings
        /// </summary>
        void DrawPerimeterBuildings()
        {
            for (int z = 0; z < 7; z++)
            {
                //Draw Surrounding buildings
                    DrawModel(building, Matrix.CreateScale(0.5f) *Matrix.CreateRotationY(MathHelper.ToRadians(90))* Matrix.CreateTranslation(-642.5f, 0, -552.5f+(185f*z)));
                    DrawModel(building, Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(642.5f, 0, -552.5f + (185f * z)));
                    DrawModel(building, Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-560f + (185f * z), 0, -650f));
                    DrawModel(building, Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(MathHelper.ToRadians(180)) * Matrix.CreateTranslation(-550.5f + (185f * z), 0, 650.0f));  
            }
        }

        /// <summary>
        /// Draws The tunnel
        /// </summary>
        void DrawTunnel()
        {
            //Middle Tunnel
            DrawModel(tunnel, Matrix.CreateScale(new Vector3(1f, 0.5f, 0.5f)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(0, 0, 750));
            DrawModel(box, Matrix.CreateScale(new Vector3(5,4,4)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(-10, 0, 630));

            DrawModel(tunnel, Matrix.CreateScale(new Vector3(1f, 0.5f, 0.5f)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(0, 0, -575));
            DrawModel(box, Matrix.CreateScale(new Vector3(5, 4, 4)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(-10, 0, -630));

            //Left Tunnel
            DrawModel(tunnel, Matrix.CreateScale(new Vector3(1f, 0.5f, 0.5f)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(500, 0, 750));
            DrawModel(box, Matrix.CreateScale(new Vector3(5, 4, 4)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(490, 0, 610));

            DrawModel(tunnel, Matrix.CreateScale(new Vector3(1f, 0.5f, 0.5f)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(500, 0, -575));
            DrawModel(box, Matrix.CreateScale(new Vector3(5, 4, 4)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(490, 0, -630));

            //Right Tunnel
            DrawModel(tunnel, Matrix.CreateScale(new Vector3(1f,0.5f,0.5f)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(-500, 0, 750));
            DrawModel(box, Matrix.CreateScale(new Vector3(5, 4, 4)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(-510, 0, 630));

            DrawModel(tunnel, Matrix.CreateScale(new Vector3(1f, 0.5f, 0.5f)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(-500, 0, -575));
            DrawModel(box, Matrix.CreateScale(new Vector3(5, 4, 4)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(-510, 0, -630));
        }


        /// <summary>
        /// Draws the 3D specified model.
        /// </summary>
        /// <param name="model">The 3D model being drawn.</param>
        /// <param name="world">Transformation matrix for world coords.</param>
        /// <param name="texture">Texture used for the drawn 3D model.</param>
        void DrawModel(Model model, Matrix world, Texture2D texture)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.Projection = proj;
                    be.View = view;
                    be.World = world;
                    be.Texture = texture;
                    be.TextureEnabled = true;
                    be.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// Draws the 3D specified model.
        /// </summary>
        /// <param name="model">The 3D model being drawn.</param>
        /// <param name="world">Transformation matrix for world coords.</param>
        /// <param name="texture">Texture used for the drawn 3D model.</param>
        void DrawModel(Model model, Matrix world)
        {
            //EffectParameter blah = new EffectParameter();
           // DirectionalLight light1 = new DirectionalLight(blah, new Vector3(0.5f,0.5f,0.5f), new Vector3(0.5f,0.5f,0.5f));

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.Projection = proj;
                    be.View = view;
                    be.World = world;
                    be.TextureEnabled = true;
                    be.LightingEnabled = true;
                    be.AmbientLightColor = new Vector3(0.6f,0.6f,0.6f);
                    be.DiffuseColor = new Vector3(0.6f, 0.6f, 0.6f);
                    be.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);
                    be.SpecularPower = 0.5f;
                    //be.DirectionalLight0=new Microsoft.Xna.Framework.Graphics.DirectionalLight(new Microsoft.Xna.Framework.Graphics.EffectParameter(
                    //be.EnableDefaultLighting();
                    
                }
                mesh.Draw();

            }
        }


        /// <summary>
        /// Get the bounding sphere of the model
        /// </summary>
        public BoundingBox GetBoundingBox(Model model, Matrix location, float scale)
        {
            BoundingSphere sphere = new BoundingSphere();
            //Get the bounding Sphere of the tank
            foreach (ModelMesh mesh in model.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }
            // world.
            Vector3 Position = new Vector3(location.M41, location.M42, location.M43);
            sphere.Center = Position;
            sphere.Radius = scale;
            BoundingBox box = BoundingBox.CreateFromSphere(sphere);
            return box;
                
            
        }

        #region VertexBuffer

        // Create the vertex and index buffer arrays to hold the box for the skybox
        public void CreateCubeVertexIndexBuffer()
        {
            //Create the vertex buffer
            Vector3[] cubeVertices = new Vector3[number_of_vertices];

            cubeVertices[0] = new Vector3(-1, -1, -1);
            cubeVertices[1] = new Vector3(-1, -1, 1);
            cubeVertices[2] = new Vector3(1, -1, 1);
            cubeVertices[3] = new Vector3(1, -1, -1);
            cubeVertices[4] = new Vector3(-1, 1, -1);
            cubeVertices[5] = new Vector3(-1, 1, 1);
            cubeVertices[6] = new Vector3(1, 1, 1);
            cubeVertices[7] = new Vector3(1, 1, -1);

            VertexDeclaration VertexPositionDeclaration = new VertexDeclaration
                (
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0)
                );

            vertices = new VertexBuffer(screenManager.GraphicsDevice, VertexPositionDeclaration, number_of_vertices, BufferUsage.WriteOnly);
            vertices.SetData<Vector3>(cubeVertices);

            //Create the index buffer
            UInt16[] cubeIndices = new UInt16[number_of_indices];

            //bottom face
            cubeIndices[0] = 0;
            cubeIndices[1] = 2;
            cubeIndices[2] = 3;
            cubeIndices[3] = 0;
            cubeIndices[4] = 1;
            cubeIndices[5] = 2;

            //top face
            cubeIndices[6] = 4;
            cubeIndices[7] = 6;
            cubeIndices[8] = 5;
            cubeIndices[9] = 4;
            cubeIndices[10] = 7;
            cubeIndices[11] = 6;

            //front face
            cubeIndices[12] = 5;
            cubeIndices[13] = 2;
            cubeIndices[14] = 1;
            cubeIndices[15] = 5;
            cubeIndices[16] = 6;
            cubeIndices[17] = 2;

            //back face
            cubeIndices[18] = 0;
            cubeIndices[19] = 7;
            cubeIndices[20] = 4;
            cubeIndices[21] = 0;
            cubeIndices[22] = 3;
            cubeIndices[23] = 7;

            //left face
            cubeIndices[24] = 0;
            cubeIndices[25] = 4;
            cubeIndices[26] = 1;
            cubeIndices[27] = 1;
            cubeIndices[28] = 4;
            cubeIndices[29] = 5;

            //right face
            cubeIndices[30] = 2;
            cubeIndices[31] = 6;
            cubeIndices[32] = 3;
            cubeIndices[33] = 3;
            cubeIndices[34] = 6;
            cubeIndices[35] = 7;

            indices = new IndexBuffer(screenManager.GraphicsDevice, IndexElementSize.SixteenBits, number_of_indices, BufferUsage.WriteOnly);
            indices.SetData<UInt16>(cubeIndices);

            //Set vertex and index buffers
            screenManager.GraphicsDevice.SetVertexBuffer(vertices);
            screenManager.GraphicsDevice.Indices = indices;
            
        }
        #endregion VertexBuffer

    }
}
