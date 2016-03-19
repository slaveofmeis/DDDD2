using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using MyContentPipeline.XMLContentShared;
using System.Collections.Generic;
using System;
using System.Xml;
using DDDD2.GameComponents;
using DDDD2.GameScreens;
using DDDD2.GameInformation;
using Steamworks;
namespace DDDD2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // I have no idea why these are static while game screens arent
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static AudioManager audioManager;
        public static GameInfo gameInfo;
        
        ScreenManager screenManager;
        public StartScreen startScreen;
        public GamePlayScreen gamePlayScreen;
        public NameHeroScreen nameHeroScreen;
        public EndingScreen endingScreen;
        public SaveLoadScreen saveLoadScreen;
        public CreditsScreen creditsScreen;
        public Texture2D escButton;

        private bool steamInit;
        //TODO:
        // ogg, console output, delete appid
        // BUGS:
        // If switching map while song is still fading out, volume issues
        // Common stuff here TODO: put in some separate class/library
        //private SpriteFont font;
        public static Texture2D pointerTexture;

        public Game1()
        {
            steamInit = SteamAPI.Init();
            //Console.WriteLine(SteamUser.GetSteamID().GetAccountID().m_AccountID);
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 576;
            Width = graphics.PreferredBackBufferWidth;
            Height = graphics.PreferredBackBufferHeight;
            
            screenManager = new ScreenManager(this);
            Components.Add(new InputManager(this));
            Components.Add(screenManager);
            startScreen = new StartScreen(this, screenManager);
            gamePlayScreen = new GamePlayScreen(this, screenManager);
            nameHeroScreen = new NameHeroScreen(this, screenManager);
            endingScreen = new EndingScreen(this, screenManager);
            saveLoadScreen = new SaveLoadScreen(this, screenManager);
            creditsScreen = new CreditsScreen(this, screenManager);
            audioManager = new AudioManager(this);
            gameInfo = new GameInfo();
            screenManager.ChangeScreens(startScreen);
            Window.Title = "Don't Die Dateless, Dummy!";
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

            //font = Content.Load<SpriteFont>("Fonts/RegularFont");
            pointerTexture = Content.Load<Texture2D>("Graphics/UI/pointer");
            gameInfo.parseScenes(Content.Load<XmlSource>("XMLContent/Scenes/XMLFile1"));
            escButton = Content.Load<Texture2D>("Graphics/UI/MenuInfo");
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
            // TODO: exit from menu, exit from startscreen
            if (startScreen.gotExit() == 1)
            {
                if(steamInit)
                    SteamAPI.Shutdown();
                Exit();
            }
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
            base.Draw(gameTime);
            //spriteBatch.Draw(safeTexture, safeArea, Color.White);
            spriteBatch.End();
        }
    }
}
