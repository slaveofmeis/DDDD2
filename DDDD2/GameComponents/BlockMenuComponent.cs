using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class BlockMenuComponent : MenuComponent
    {
        private int maxColumns;
        private bool isCaps;
        public BlockMenuComponent(SpriteFont spriteFont, int menuStatus, int maxColumns) : base(spriteFont, menuStatus)
        {
            this.spriteFont = spriteFont;
            this.menuStatus = menuStatus;
            this.maxColumns = maxColumns;
            isCaps = true;
        }

        public bool IsCaps
        {
            get { return isCaps; }
            set { isCaps = value; }
        }

        public override void Update()
        {
            if (InputManager.KeyDirectionPressed(Keys.Left))
            {
                selectedIndex--;
                if (selectedIndex < 0 || selectedIndex % maxColumns == maxColumns-1)
                    selectedIndex++;
                else
                    Game1.audioManager.PlayNavSound();
            }
            else if (InputManager.KeyDirectionPressed(Keys.Right))
            {
                selectedIndex++;
                if (selectedIndex >= menuItems.Count || selectedIndex % maxColumns == 0)
                {
                    selectedIndex--;
                }
                else
                    Game1.audioManager.PlayNavSound();
            }
            else if (InputManager.KeyDirectionPressed(Keys.Up))
            {
                selectedIndex -= maxColumns;

                if (selectedIndex < 0)
                {
                    selectedIndex += maxColumns;
                }
                else
                    Game1.audioManager.PlayNavSound();
            }
            else if (InputManager.KeyDirectionPressed(Keys.Down))
            {
                selectedIndex += maxColumns;
                if (selectedIndex >= menuItems.Count)
                {
                    //selectedIndex = 0 + selectedIndex % maxColumns;
                    selectedIndex -= maxColumns;
                }
                else
                    Game1.audioManager.PlayNavSound();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, int lineSpacing, bool alwaysHighlight, bool drawPointer = true)
        {
            Vector2 menuPosition = position;
            highlight = alwaysHighlight;

            for (int i = 0; i < menuItems.Count; i++)
            {
                // TODO: revisit hero name
                //string shownString = menuItems[i];
                string shownString = menuItems[i].Replace(GamePlayScreen.MC_IDENTIFIER, GamePlayScreen.HERO_NAME);
                if(isCaps)
                {
                    shownString = shownString.ToUpper();
                }
                else
                {
                    if (shownString.Length == 1)
                        shownString = shownString.ToLower();
                }
                if (i == selectedIndex && (hasFocus == true || highlight == true))
                    spriteBatch.DrawString(spriteFont, shownString, menuPosition, HiliteColor);
                else
                    spriteBatch.DrawString(spriteFont, shownString, menuPosition, NormalColor);
                if ((i + 1) % maxColumns == 0) // TODO: should just be linespacing here, the calculations should be done before
                {
                    menuPosition.Y += 3*lineSpacing/2;
                    menuPosition.X = position.X;
                }
                else
                    menuPosition.X += 3*lineSpacing/2;
            }
            if (HasFocus && drawPointer)
                spriteBatch.Draw(Game1.pointerTexture, new Vector2(position.X - (Game1.Width / 41), (position.Y + lineSpacing / 2) + selectedIndex * lineSpacing), Color.White);
        }
    }
}
