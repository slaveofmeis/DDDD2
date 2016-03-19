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
using DDDD2.GameScreens;
namespace DDDD2.GameComponents
{
    public class ScreenFader
    {
        public static int DEFAULT_SPEED = 10;
        public static int SLOW_SPEED = 2;
        bool isFadeOut, isFadeIn;
        Texture2D transitionTexture;
        int transitionValue = 0;
        int transitionSpeed = DEFAULT_SPEED;
        Rectangle fadeRectangle;
        Game myGame;

        public ScreenFader(Game game)
        {
            isFadeOut = false;
            isFadeIn = false;
            myGame = game;
            SwitchOK = false;
        }

        public void LoadContent()
        {
            transitionTexture = new Texture2D(myGame.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            transitionTexture.SetData<Color>(new Color[] { new Color(255, 255, 255, 255) });
            fadeRectangle = new Rectangle(0, 0, Game1.Width, Game1.Height);
        }

        public int TransitionSpeed
        {
            get { return transitionSpeed; }
            set { transitionSpeed = value; }
        }

        public int TransitionValue
        {
            get { return transitionValue; }
            set { transitionValue = value; }
        }

        public bool IsFadeOut
        {
            get { return isFadeOut; }
            set { isFadeOut = value; }
        }

        public bool IsFadeIn
        {
            get { return isFadeIn; }
            set
            {
                //transitionValue = 255;
                isFadeIn = value;
            }
        }

        public bool IsFadeSpecial
        {
            get;
            set;
        }

        public bool IsFadeScreen
        {
            get;
            set;
        }

        public bool Wait
        {
            get;
            set;
        }

        public bool SwitchOK
        {
            get;
            set;
        }

        public bool SwitchScreenHelper
        {
            get;
            set;
        }
        //fasdfds
        public void Update()
        {
            if (isFadeOut)
            {
                if (transitionValue < 255)
                {
                    transitionValue += TransitionSpeed;
                    if (transitionValue >= 255)
                    {
                        transitionValue = 255;
                        SwitchOK = true;
                    }
                    //transitionTexture.SetData<Color>(new Color[] { new Color(180, 255, 0, transitionValue) });
                }
                else
                {
                    isFadeOut = false;
                    isFadeIn = true;
                }
            }
            else if (isFadeIn)
            {
                SwitchOK = false;
                if (transitionValue > 0)
                {
                    transitionValue -= TransitionSpeed;
                    if (transitionValue <= 0)
                        transitionValue = 0;
                    //transitionTexture.SetData<Color>(new Color[] { new Color(180, 255, 0, transitionValue) });
                }
                else
                {
                    isFadeIn = false;
                }
            }
            else if (IsFadeSpecial)
            {
                if (transitionValue < Game1.Width)
                {
                    transitionValue += TransitionSpeed;
                    transitionValue = (int)MathHelper.Clamp(transitionValue, 0, Game1.Width);
                    //transitionTexture.SetData<Color>(new Color[] { new Color(180, 255, 0, transitionValue) });
                }
                else
                {
                    IsFadeSpecial = false;
                }
            }
            else if (IsFadeScreen)
            {
                if (transitionValue < 255)
                {
                    transitionValue += TransitionSpeed;
                    if (transitionValue >= 255)
                    {
                        transitionValue = 255;
                    }
                }
                else
                {
                    IsFadeScreen = false;
                    //transitionValue = 0;
                }
            }

        }

        public void FadeMeIn()
        {
            TransitionValue = 255;
            IsFadeIn = true;
            SwitchOK = false;
        }

        public void Draw()
        {
            if (isFadeOut || isFadeIn || IsFadeSpecial || Wait || IsFadeScreen || SwitchScreenHelper)
            {
                Game1.spriteBatch.Draw(transitionTexture, fadeRectangle,
                         new Color(0, 0, 0, (byte)transitionValue));
                /*if (IsFadeSpecial || Wait)
                {
                    Game1.spriteBatch.Draw(transitionTexture, slideRectangle,
                         Color.Black);
                }
                else
                {
                    Game1.spriteBatch.Draw(transitionTexture, fadeRectangle,
                         new Color(0, 0, 0, (byte)transitionValue));
                }*/
                //Game1.Game1.spriteBatch.DrawString(myGame.Content.Load<SpriteFont>(@"Fonts\dialogueFont"),transitionValue.ToString(),Vector2.Zero,Color.White);
            }
        }
    }
}
