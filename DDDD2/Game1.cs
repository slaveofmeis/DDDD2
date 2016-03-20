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

        public bool steamInit;
        public bool statsRequested = false;
        public bool statsReceived = false;
        public int VeraWin, EllieWin, LiaWin, Lose_1, Lose_2, Lose_3, Lose_4, Lose_5, Lose_6, Lose_7, Lose_8;
        public Callback<UserStatsReceived_t> m_UserStatsReceived;
        protected Callback<UserStatsStored_t> m_UserStatsStored;
        //TODO:
        // console output, delete appid
        // BUGS:
        // If switching map while song is still fading out, volume issues
        // Common stuff here TODO: put in some separate class/library
        //private SpriteFont font;
        public static Texture2D pointerTexture;

        public Game1()
        {
            steamInit = SteamAPI.Init();
            //Console.WriteLine(SteamUser.GetSteamID().GetAccountID().m_AccountID);
            if (steamInit)
            {
                m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
                m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
                statsRequested = SteamUserStats.RequestCurrentStats();
            }
            VeraWin = 0; EllieWin = 0; LiaWin = 0; Lose_1 = 0; Lose_2 = 0; Lose_3 = 0; Lose_4 = 0; Lose_5 = 0; Lose_6 = 0; Lose_7 = 0; Lose_8 = 0;
            
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
            LoadSteamStats();
        }
        
        private void OnUserStatsReceived(UserStatsReceived_t pCallback)
        {
            //Console.WriteLine("[" + UserStatsReceived_t.k_iCallback + " - UserStatsReceived] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult + " -- " + pCallback.m_steamIDUser);
        }
        private void OnUserStatsStored(UserStatsStored_t pCallback)
        {
            //Console.WriteLine("[" + UserStatsStored_t.k_iCallback + " - UserStatsStored] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
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

        public void LoadSteamStats()
        {
            if (statsRequested)
            {

                //SteamUserStats.SetStat("VeraVictory", 1);
                SteamUserStats.GetStat("VeraVictory", out VeraWin);
                SteamUserStats.GetStat("LiaVictory", out LiaWin);
                SteamUserStats.GetStat("EllieVictory", out EllieWin);

                SteamUserStats.GetStat("Wizard1", out Lose_1);
                SteamUserStats.GetStat("Wizard2", out Lose_2);
                SteamUserStats.GetStat("Wizard3", out Lose_3);
                SteamUserStats.GetStat("Wizard4", out Lose_4);
                SteamUserStats.GetStat("Wizard5", out Lose_5);
                SteamUserStats.GetStat("Wizard6", out Lose_6);
                SteamUserStats.GetStat("Wizard7", out Lose_7);
                SteamUserStats.GetStat("Wizard8", out Lose_8);
            }
        }

        public void CheckAchievements()
        {
            if(statsRequested)
            {
                if(VeraWin == 1 || LiaWin == 1 || EllieWin == 1)
                {
                    SteamUserStats.SetAchievement("ACH_CHAD");
                }
                if(VeraWin == 1 && LiaWin == 1 && EllieWin == 1)
                {
                    SteamUserStats.SetAchievement("ACH_KING_CHAD");
                }
                if(Lose_1 == 1 || Lose_2 == 1 || Lose_3 == 1 || Lose_4 == 1 || Lose_5 == 1 || Lose_6 == 1 || Lose_7 == 1 || Lose_8 == 1)
                {
                    SteamUserStats.SetAchievement("ACH_WIZARD");
                }
                if (Lose_1 == 1 && Lose_2 == 1 && Lose_3 == 1 && Lose_4 == 1 && Lose_5 == 1 && Lose_6 == 1 && Lose_7 == 1 && Lose_8 == 1)
                {
                    SteamUserStats.SetAchievement("ACH_ARCHMAGE");
                }
                SteamUserStats.StoreStats();
            }
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
            if (steamInit)
            {
                SteamAPI.RunCallbacks();
            }
            
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
