using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace VESSELS.MazeGameLogic
{
    class GameCore:DrawableGameComponent2
    {

        #region Fields
        //Copy of the screenmanager
        ScreenManager screenManager;

        //Screen parameters
        int ScreenWidth, ScreenHeight, OffsetX, OffsetY, gameWidth, gameHeight;

        // Resources for drawing.
        private SpriteBatch spriteBatch;
        ContentManager content;
        // Global content.
        private SpriteFont hudFont;

        private Texture2D winOverlay;
        private Texture2D loseOverlay;
        private Texture2D diedOverlay;

        // Meta-level game state.
        private int levelIndex = -1;
        private Level level;
        private bool wasContinuePressed;

        // When the time remaining is less than the warning time, it blinks on the hud
        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(10);

        // We store our input states so that we only poll once per frame, 
        // then we use the same input state wherever needed
        private GamePadState gamePadState;
        private KeyboardState keyboardState;

        // The number of levels in the Levels directory of our content. We assume that
        // levels in our content are 0-based and that all numbers under this constant
        // have a level file present. This allows us to not need to check for the file
        // or handle exceptions, both of which can add unnecessary time to level loading.
        private const int numberOfLevels = 3;
        #endregion Fields

        //Levels currentLevel = Levels.Medium;

        //Constructor
        public GameCore(ScreenManager screenManager, int levelIndex)
        {
            this.screenManager = screenManager;
            this.levelIndex = levelIndex;
        }
        float temp;
        //Load Content
        public override void Initialize()
        {
            //content manager
            if (content == null)
                content = new ContentManager(screenManager.Game.Services, "Content");
            

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(screenManager.GraphicsDevice);

            // Load fonts
            hudFont = content.Load<SpriteFont>("Fonts/Hud");

            // Load overlay textures
            winOverlay = content.Load<Texture2D>(@"Textures/Overlays/you_win");
            loseOverlay = content.Load<Texture2D>(@"Textures/Overlays/you_lose");
            diedOverlay = content.Load<Texture2D>(@"Textures/Overlays/you_died");

            ScreenHeight = screenManager.GraphicsDevice.DisplayMode.Height;
            ScreenWidth = screenManager.GraphicsDevice.DisplayMode.Width;
            temp=(14.0f * 64f * 1) - 128f;
            //OffsetY = 128 + (int)(((48f * ((float)ScreenHeight / (float)1200))) / 2f);
            OffsetY = 115 + (int)(((48f * ((float)ScreenHeight / (float)1200))) / 2f);
            OffsetX = 129;
            gameWidth = ScreenWidth - 2 * OffsetX;
            gameHeight = ScreenHeight - 2 * OffsetY;
            
            //Load Initial Level
            LoadNextLevel();
            base.Initialize();
        }


        //Update Game
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {

            //get current input for game
            HandleInput();

            // update our level, passing down the GameTime along with all of our input states
            level.Update(elapsedTime,totalTime, keyboardState, gamePadState);
            
            base.Update(elapsedTime, totalTime);
        }

        void HandleInput()
        {
            //Poll again for the keyboard state to go to the game locally
            keyboardState = Keyboard.GetState();

            bool continuePressed =
                   keyboardState.IsKeyDown(Keys.Space) ||
                   gamePadState.IsButtonDown(Buttons.A);

            //Perform the appropriate action to advance the game and
            //to get the player back to playing.
            if (!wasContinuePressed && continuePressed)
            {
                if (!level.Player.IsAlive)
                {
                    level.StartNewLife();
                }
                else if (level.TimeRemaining == TimeSpan.Zero)
                {
                    if (level.ReachedExit)
                        LoadNextLevel();
                    else
                        ReloadCurrentLevel();
                }
            }

            wasContinuePressed = continuePressed;
        }

        private void LoadNextLevel()
        {
            // move to the next level
            //levelIndex = (levelIndex + 1) % numberOfLevels;
            // Unloads the content for the current level before loading the next one.
            if (level != null)
                level.Dispose();
            //levelIndex = 1;

            //switch (currentLevel)
            //{
            //    case Levels.Easy:
            //        levelIndex = 0;
            //        break;
            //    case Levels.Medium:
            //        levelIndex = 1;
            //        break;
            //    case Levels.Hard:
            //        levelIndex = 2;
            //        break;
            //}

            // Load the level.
            string levelPath = string.Format("Content/Levels/{0}.txt", levelIndex);
            //string levelMap = string.Format("Content/Levels/{0}map.txt", levelIndex);
           // Stream fileStreamMap = TitleContainer.OpenStream(levelMap);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(screenManager.Game.Services, fileStream, levelIndex,gameWidth,gameHeight,OffsetX,OffsetY,screenManager.Game);
        }

        private void ReloadCurrentLevel()
        {
            --levelIndex;
            LoadNextLevel();
        }


        //Draw Game
        public override void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            spriteBatch.Begin();

            level.Draw(elapsedTime, spriteBatch);

            DrawHud();

            spriteBatch.End();

            base.Draw(elapsedTime, totalTime);
        }

        private void DrawHud()
        {
            Rectangle titleSafeArea = screenManager.GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f);

            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            string timeString = "TIME: " + level.TimeRemaining.Minutes.ToString("00") + ":" + level.TimeRemaining.Seconds.ToString("00");
            Color timeColor;
            if (level.TimeRemaining > WarningTime ||
                level.ReachedExit ||
                (int)level.TimeRemaining.TotalSeconds % 2 == 0)
            {
                timeColor = Color.Yellow;
            }
            else
            {
                timeColor = Color.Red;
            }
            DrawShadowedString(hudFont, timeString, hudLocation, timeColor);

            // Draw score
            float timeHeight = hudFont.MeasureString(timeString).Y;
            DrawShadowedString(hudFont, "SCORE: " + level.Score.ToString(), hudLocation + new Vector2(0.0f, timeHeight * 1.2f), Color.Yellow);

            // Determine the status overlay message to show.
            Texture2D status = null;
            if (level.TimeRemaining == TimeSpan.Zero)
            {
                if (level.ReachedExit)
                {
                    status = winOverlay;
                }
                else
                {
                    status = loseOverlay;
                }
            }
            else if (!level.Player.IsAlive)
            {
                status = diedOverlay;
            }

            if (status != null)
            {
                // Draw status message.
                Vector2 statusSize = new Vector2(status.Width, status.Height);
                spriteBatch.Draw(status, center - statusSize / 2, Color.White);
            }
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            //spriteBatch.DrawString(font, value, position, color);
            spriteBatch.DrawString(font, value, position, color, 0,Vector2.Zero, 1.5f, SpriteEffects.None, 0);
        }

    }
}
