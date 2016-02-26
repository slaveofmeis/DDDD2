using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDD2.GameComponents;
namespace DDDD2.EventArguments
{
    public class ScreenEventArgs : EventArgs
    {
        #region Fields and Properties
        GameScreen gameScreen;
        public GameScreen GameScreen
        {
            get { return gameScreen; }
            private set { gameScreen = value; }
        }
        #endregion
        #region Constrcutor
        public ScreenEventArgs(GameScreen gameScreen)
        {
            GameScreen = gameScreen;
        }
        #endregion
    }
}