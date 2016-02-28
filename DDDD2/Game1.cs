using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using MyContentPipeline.XMLContentShared;
using System.Collections.Generic;
using System;
using System.Xml;
using DDDD2.GameComponents;
using DDDD2.GameScreens;
namespace DDDD2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static AudioManager audioManager;
        
        XmlDocument xd;
        ScreenManager screenManager;
        public StartScreen startScreen;
        public GamePlayScreen gamePlayScreen;

        // Common stuff here TODO: put in some separate class/library
        private SpriteFont font;
        public static Texture2D pointerTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 848;
            graphics.PreferredBackBufferHeight = 480;
            Width = graphics.PreferredBackBufferWidth;
            Height = graphics.PreferredBackBufferHeight;

            screenManager = new ScreenManager(this);
            Components.Add(new InputManager(this));
            Components.Add(screenManager);
            startScreen = new StartScreen(this, screenManager);
            gamePlayScreen = new GamePlayScreen(this, screenManager);
            audioManager = new AudioManager(this);
            screenManager.ChangeScreens(startScreen);
            
        }

        public static int Height
        {
            get;
            set;
        }

        public static int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            /*screenWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);*/

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //texture = Content.Load<Texture2D>("Graphics/Backgrounds/nagi-no-asukara-miuna-mother");
            //Content.Load<SceneData>("XMLContent/Scenes/XMLFile1");
            XmlSource xs = Content.Load<XmlSource>("XMLContent/Scenes/XMLFile1");
            font = Content.Load<SpriteFont>("Fonts/RegularFont");
            pointerTexture = Content.Load<Texture2D>("Graphics/UI/pointer");
            xd = new XmlDocument();
            xd.LoadXml(xs.XmlCode);
            XmlNodeList elemList = xd.GetElementsByTagName("Item");
            for (int i=0; i < elemList.Count; i++)
            {
                //xd.LoadXml("<Item>"+elemList[i].InnerXml+"</Item>");
                //XmlNodeList subList = xd.GetElementsByTagName("SceneNumber");
                Console.WriteLine("SCENENUMBER:" + elemList[i].SelectSingleNode("SceneNumber").InnerText);
                Console.WriteLine("MAINDIALOGUE:" + elemList[i].SelectSingleNode("MainDialogue").InnerText);
            }
            audioManager.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            audioManager.Update();
            //TODO gamepad GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || startScreen.gotExit() == 1)
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //spriteBatch.Draw(texture, screenRectangle, Color.White);
            spriteBatch.DrawString(font, "Game1.cs", Vector2.Zero, Color.Yellow);
            base.Draw(gameTime);
            //spriteBatch.Draw(safeTexture, safeArea, Color.White);
            spriteBatch.End();
        }
    }
}
