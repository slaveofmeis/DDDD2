using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DDDD2.EventArguments;
namespace DDDD2.GameComponents
{
    public class ScreenManager : GameComponent
    {
        #region Fields and Properties Region
        public static Stack<GameScreen> gameScreens = new Stack<GameScreen>();
        public event EventHandler<ScreenEventArgs> OnScreenChange;
        const int startDrawOrder = 5000;
        const int drawOrderInc = 100;
        int drawOrder;
        public GameScreen CurrentScreen
        {
            get { return gameScreens.Peek(); }
        }
        #endregion
        #region
        public ScreenManager(Game game)
            : base(game)
        {
            drawOrder = startDrawOrder;
        }
        #endregion
        #region Methods Region
        public void PopScreen()
        {
            GameScreen oldScreen = RemoveScreen();
            drawOrder -= drawOrderInc;
            if (OnScreenChange != null)
                OnScreenChange(this, new ScreenEventArgs(oldScreen));
        }
        private GameScreen RemoveScreen()
        {
            GameScreen screen = (GameScreen)gameScreens.Peek();
            OnScreenChange -= screen.ScreenChange;
            Game.Components.Remove(screen);
            return gameScreens.Pop();
        }
        public void PushScreen(GameScreen newScreen)
        {
            drawOrder += drawOrderInc;
            newScreen.DrawOrder = drawOrder;
            AddScreen(newScreen);
            if (OnScreenChange != null)
                OnScreenChange(this, new ScreenEventArgs(newScreen));
        }
        private void AddScreen(GameScreen newScreen)
        {
            gameScreens.Push(newScreen);
            Game.Components.Add(newScreen);
            OnScreenChange += newScreen.ScreenChange;
        }
        public void ChangeScreens(GameScreen newScreen)
        {
            while (gameScreens.Count > 0)
                RemoveScreen();
            newScreen.DrawOrder = startDrawOrder;
            drawOrder = startDrawOrder;
            AddScreen(newScreen);
            if (OnScreenChange != null)
                OnScreenChange(this, new ScreenEventArgs(newScreen));
        }
        #endregion
    }
}
