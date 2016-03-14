using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DDDD2.GameComponents
{
    public enum DrawMode { Center, Fill }
    class BackgroundComponent
    {
        Rectangle screenRectangle;
        Rectangle destination;
        Texture2D image;
        DrawMode drawMode;
        //string backgroundName;
        public bool Visible
        {
            get;
            set;
        }
        /*public string Name
        {
            get { return backgroundName; }
        }*/
        public BackgroundComponent(Game game, Texture2D image, DrawMode drawMode)
        {
            Visible = true;
            this.image = image;
            this.drawMode = drawMode;
            //backgroundName = image.Name;
            screenRectangle = new Rectangle(
            0,
            0,
            Game1.Width,
            Game1.Height);
            switch (drawMode)
            {
                case DrawMode.Center:
                    destination = new Rectangle(
                    (screenRectangle.Width - image.Width) / 2,
                    (screenRectangle.Height - image.Height) / 2,
                    image.Width,
                    image.Height);
                    break;
                case DrawMode.Fill:
                    destination = new Rectangle(
                    screenRectangle.X,
                    screenRectangle.Y,
                    screenRectangle.Width,
                    screenRectangle.Height);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(image, destination, Color.White);
        }

    }
}
