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
    public class MenuComponent
    {
        #region Field and Property Region
        protected List<String> menuItems;
        protected int selectedIndex;
        protected int menuStatus;
        protected int exit;
        protected float width;
        protected float height;
        protected bool hasFocus;
        protected bool highlight;
        protected Vector2 position;
        protected SpriteFont spriteFont;
        protected float fontCharHeight;
        public Color NormalColor
        {
            get;
            set;
        }
        public Color UnusableColor
        {
            get;
            set;
        }
        public List<String> MenuItems
        {
            get { return menuItems; }
        }
        public Color HiliteColor
        {
            get;
            set;
        }
        public Vector2 Position
        {
            get { return position; }
        }
        public bool Highlight
        {
            get { return highlight; }
            set { highlight = value; }
        }
        public float Width
        {
            get { return width; }
        }
        public float Height
        {
            get { return height; }
        }
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }
        public bool HasFocus
        {
            get { return hasFocus; }
            set { hasFocus = value; }
        }
        public SpriteFont Font
        {
            get { return spriteFont; }
        }
        public int MenuStatus
        {
            get { return menuStatus; }
            set { menuStatus = value; }
        }
        public int getExitValue
        {
            get { return exit; }
        }

        public bool IsVisible
        {
            get; set;
        }
        #endregion
        #region Constructor Region
        /*public MenuComponent(SpriteFont spriteFont, string[] items)
        {
            this.spriteFont = spriteFont;
            SetMenuItems(items);
            NormalColor = Color.White;
            HiliteColor = Color.Red;
            exit = 0;
        }*/
        public MenuComponent(SpriteFont spriteFont, int menuStatus)
        {
            this.spriteFont = spriteFont;
            fontCharHeight = spriteFont.MeasureString("W").Y;
            NormalColor = Color.White;
            HiliteColor = Color.Yellow;
            UnusableColor = Color.DarkGray;
            exit = 0;
            menuItems = new List<String>();
            hasFocus = true;
            highlight = false;
            IsVisible = true;
            this.menuStatus = menuStatus;

        }
        #endregion
        #region Method Region
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }
        public void SetMenuItems(List<String> items)
        {
            menuItems = items;
            MeasureMenu();
        }
        protected void MeasureMenu()
        {
            width = 0;
            height = 0;
            foreach (string s in menuItems)
            {
                if (width < spriteFont.MeasureString(s).X)
                    width = spriteFont.MeasureString(s).X;
                height += spriteFont.LineSpacing;
            }
        }
        public virtual void Update()
        {
            HandleMenuSounds();
            // TODO: Gamepad?
            if (InputManager.KeyDirectionPressed(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Count - 1;
            }
            if (InputManager.KeyDirectionPressed(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex >= menuItems.Count)
                    selectedIndex = 0;
            }
            /*if (InputManager.KeyReleased(Keys.Escape))
            {
                exit = 1;
            }*/
        }
        public virtual void HandleMenuSounds()
        {
                if (menuItems.Count != 0 && (InputManager.KeyDirectionPressed(Keys.Up) || InputManager.KeyDirectionPressed(Keys.Down))) //||
                    //InputManager.KeyDirectionPressed(Keys.Left) || InputManager.KeyDirectionPressed(Keys.Right)))
                {/*||
                InputManager.KeyDirectionPressed(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One), Buttons.LeftThumbstickUp) ||
                InputManager.KeyDirectionPressed(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One), Buttons.DPadUp) ||
                InputManager.KeyDirectionPressed(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One), Buttons.LeftThumbstickLeft) ||
                InputManager.KeyDirectionPressed(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One), Buttons.DPadLeft) ||
                InputManager.KeyDirectionPressed(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One), Buttons.LeftThumbstickRight) ||
                InputManager.KeyDirectionPressed(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One), Buttons.DPadRight) ||
                InputManager.KeyDirectionPressed(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One), Buttons.LeftThumbstickDown) ||
                InputManager.KeyDirectionPressed(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One), Buttons.DPadDown))) */
                    Game1.audioManager.PlayNavSound();
                }
            //if (InputManager.KeyReleased(Keys.B) || InputManager.KeyReleased(Keys.Escape)) //|| InputManager.ButtonReleased(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One), Buttons.B))
                    //Game1.audioManager.PlayBackSound();
        }
        public virtual void Draw(SpriteBatch spriteBatch, int lineSpacing, bool alwaysHighlight, bool drawPointer = true)
        {
            //Vector2 menuPosition = Vector2.Zero;
            if (IsVisible)
            {
                
                Vector2 menuPosition = position;
                highlight = alwaysHighlight;

                for (int i = 0; i < menuItems.Count; i++)
                {
                    // TODO: revisit hero name
                    //string shownString = menuItems[i];
                    string shownString = menuItems[i].Replace(GamePlayScreen.MC_IDENTIFIER, GamePlayScreen.HERO_NAME);
                    if (i == selectedIndex && (hasFocus == true || highlight == true))
                        spriteBatch.DrawString(spriteFont, shownString, menuPosition, HiliteColor);
                    else
                        spriteBatch.DrawString(spriteFont, shownString, menuPosition, NormalColor);
                    menuPosition.Y += lineSpacing;
                }

                if (HasFocus && drawPointer)
                    spriteBatch.Draw(Game1.pointerTexture, new Vector2(position.X - (Game1.Width / 41), (position.Y + fontCharHeight / 2) + selectedIndex * lineSpacing), Color.White);
            }
        }
        #endregion
    }
}
