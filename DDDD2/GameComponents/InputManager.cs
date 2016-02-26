using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.Diagnostics;
using System.Reflection;
namespace DDDD2.GameComponents
{
    public class InputManager : Microsoft.Xna.Framework.GameComponent
    {
        #region Field Region
        static KeyboardState keyboardState;
        static KeyboardState lastKeyboardState;
        private static Stopwatch myStopWatch;
        static GamePadState[] gamePadStates;
        static GamePadState[] lastGamePadStates;
        private const int SCROLL_TIME = 200;
        private static bool allowScroll;
        #endregion

        #region Constructor Region
        public InputManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            gamePadStates = new GamePadState[4];
            lastGamePadStates = new GamePadState[4];
            foreach (PlayerIndex index in GetEnumValues(typeof(PlayerIndex)))
                gamePadStates[(int)index] = GamePad.GetState(index);
            keyboardState = Keyboard.GetState();
            myStopWatch = new Stopwatch();
            allowScroll = true;
        }
        public static Enum[] GetEnumValues(Type enumType)
        {
            if (enumType.BaseType == typeof(Enum))
            {
                FieldInfo[] info = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
                Enum[] values = new Enum[info.Length];
                for (int i = 0; i < values.Length; ++i)
                {
                    values[i] = (Enum)info[i].GetValue(null);
                }
                return values;
            }
            else
            {
                throw new Exception("Given type is not an Enum type");
            }
        }
        #endregion


        #region Game Component Method Region
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            lastGamePadStates = (GamePadState[])gamePadStates.Clone();
            foreach (PlayerIndex index in GetEnumValues(typeof(PlayerIndex)))
                gamePadStates[(int)index] = GamePad.GetState(index);

            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (myStopWatch.ElapsedMilliseconds > SCROLL_TIME)
            {
                allowScroll = true;
                myStopWatch.Stop();
                myStopWatch.Reset();
            }
            else
            {
                allowScroll = false;
            }

            base.Update(gameTime);
        }
        #endregion

        #region Keyboard Region
        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }
        public static KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }
        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
            lastKeyboardState.IsKeyDown(key);
        }
        public static bool KeyDirectionPressed(Keys key)
        {
            if (KeyPressed(key))
            {
                myStopWatch.Reset();
                myStopWatch.Start();
                return KeyPressed(key);
            }
            else
            {
                if (!myStopWatch.IsRunning)
                    myStopWatch.Start();
                return (keyboardState.IsKeyDown(key) && allowScroll);
            }
        }
        public static bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
            lastKeyboardState.IsKeyUp(key);
        }
        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
        #endregion

        #region Game Pad Region
        public static GamePadState[] GamePadStates
        {
            get { return gamePadStates; }
        }
        public static GamePadState[] LastGamePadStates
        {
            get { return lastGamePadStates; }
        }
        public static bool ButtonReleased(PlayerIndex index, Buttons button)
        {
            return gamePadStates[(int)index].IsButtonUp(button) &&
            lastGamePadStates[(int)index].IsButtonDown(button);
        }
        public static bool KeyDirectionPressed(PlayerIndex index, Buttons button)
        {
            if (ButtonPressed(index, button))
            {
                myStopWatch.Reset();
                myStopWatch.Start();
                return ButtonPressed(index, button);
            }
            else
            {
                if (!myStopWatch.IsRunning)
                    myStopWatch.Start();
                return (gamePadStates[(int)index].IsButtonDown(button) && allowScroll);
            }
        }
        public static bool ButtonPressed(PlayerIndex index, Buttons button)
        {
            return gamePadStates[(int)index].IsButtonDown(button) &&
            lastGamePadStates[(int)index].IsButtonUp(button);
        }
        public static bool ButtonDown(PlayerIndex index, Buttons button)
        {
            return gamePadStates[(int)index].IsButtonDown(button);
        }
        #endregion

    }
}
