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
using System.Diagnostics;
using DDDD2.GameComponents;

namespace DDDD2.GameComponents
{
    public class DialogueManager : Microsoft.Xna.Framework.GameComponent
    {
        bool drawDialogue, drawPortrait;
        SpriteFont dialogueFont, identifierFont;
        List<String> dialogue;
        Rectangle dialogueRectangle, narrationDialogueRectangle;
        //Rectangle portraitRectangle;
        string identifier = "";
        string tempString = "";
        private int TEXTPADDING;
        private int DIALOGUEHEIGHT;
        private int DIALOGUEWIDTH;
        private int DIALOGUEPOSX;
        private int DIALOGUEPOSY;
        private int PORTRAITHEIGHT;
        private int PORTRAITWIDTH;
        private int NARRATIONPOSY;

        private const int SCROLL_SPEED = 0;
        private int portraitPosX, portraitPosY, textPosX, textPosY,
            identifierPosX, identifierPosY;
        private Stopwatch scrollWatch;
        private string shownString = "";
        private bool scrollFinished = false;
        private bool updateLock = false;
        private Queue<Tuple<string, DialogueEnum>> dialogueQueue;
        private List<string> choiceValues;
        public enum DialogueEnum {  Normal, Attribute, Choice };
        Game myGame;
        Texture2D dialogueTexture, altDialogueTexture;
        //Texture2D portraitBorderTexture, portraitTexture, dialogueTexture, dialogueBorderTexture;
        public DialogueManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            drawDialogue = false;
            drawPortrait = false;
            dialogue = new List<String>();
            myGame = game;
            //DIALOGUEPOSX = (Engine.ViewportWidth) / 2;
            TEXTPADDING = Game1.Height / 80;
            DIALOGUEHEIGHT = (int)(Game1.Height * 0.4);
            DIALOGUEWIDTH = (int)(Game1.Width * 0.875);
            DIALOGUEPOSX = (Game1.Width - DIALOGUEWIDTH) / 2;
            DIALOGUEPOSY = (int)(Game1.Height * 0.55);
            //PORTRAITHEIGHT = Engine.ViewportWidth / 6;
            //PORTRAITWIDTH = Engine.ViewportWidth / 6;
            PORTRAITWIDTH = (int)(Game1.Width * 1.50);
            PORTRAITHEIGHT = (int)(PORTRAITWIDTH * 0.897);
            //PORTRAITWIDTH = 2048;
            //PORTRAITHEIGHT = 1837;
            portraitPosX = -1 * Game1.Width / 8;
            portraitPosY = 0;
            //portraitPosX = DIALOGUEPOSX;
            //portraitPosY = DIALOGUEPOSY - DIALOGUEHEIGHT - 10;
            //textPosX = (Engine.ViewportWidth - DIALOGUEWIDTH + 2 * TEXTPADDING) / 2;
            textPosX = DIALOGUEPOSX + TEXTPADDING * 2;
            identifierPosX = DIALOGUEPOSX + TEXTPADDING * 2;
            textPosY = DIALOGUEPOSY + TEXTPADDING * 6;
            identifierPosY = DIALOGUEPOSY + TEXTPADDING;
            scrollWatch = new Stopwatch();
            dialogueQueue = new Queue<Tuple<string, DialogueEnum>>();
            choiceValues = new List<string>();
            NARRATIONPOSY = (int)(Game1.Height * 0.10) + 2 * TEXTPADDING;
        }

        public MenuComponent ChoiceMenu
        {
            get; set;
        }

        public List<string> ChoiceValues
        {
            get { return choiceValues; }
        }

        public bool DrawDialogue
        {
            get { return drawDialogue; }
            set { drawDialogue = value; }
        }

        public DialogueEnum DialogueType
        {
            get; set;
        }

        public bool DrawPortrait
        {
            get { return drawPortrait; }
        }

        public List<String> DialogueList
        {
            get { return dialogue; }
            set { dialogue = value; }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void LoadContent()
        {
            identifierFont = Game.Content.Load<SpriteFont>("Fonts/menuFont");
            dialogueFont = Game.Content.Load<SpriteFont>("Fonts/dialogueFont");
            ChoiceMenu = new MenuComponent(dialogueFont, 0);
            ChoiceMenu.IsVisible = false;
            //TODO: REVISIT FOR AESTHETICS
            //dialogueBorderTexture = Game.Content.Load<Texture2D>(@"Sprites\UI\dialogueBorder");
            //portraitBorderTexture = Game.Content.Load<Texture2D>(@"Backgrounds\portrait");
            dialogueTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            altDialogueTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //dialogueTexture.SetData<Color>(new Color[] { new Color(0, 58, 73, 140) });
            dialogueTexture.SetData<Color>(new Color[] { new Color(0, 0, 30, 140) });
            altDialogueTexture.SetData<Color>(new Color[] { new Color(0, 0, 0, 140) });
            dialogueRectangle = new Rectangle(DIALOGUEPOSX, DIALOGUEPOSY, DIALOGUEWIDTH, DIALOGUEHEIGHT);
            narrationDialogueRectangle = new Rectangle(DIALOGUEPOSX, (int)(Game1.Height * 0.10), DIALOGUEWIDTH, (int)(Game1.Height * 0.80));
            //portraitRectangle = new Rectangle(portraitPosX, portraitPosY, PORTRAITWIDTH, PORTRAITHEIGHT);
        }

        public void ClearChoices()
        {
            ChoiceValues.Clear();
            ChoiceMenu.MenuItems.Clear();
            ChoiceMenu.SelectedIndex = 0;
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (ChoiceMenu.MenuItems.Count != 0)
            {
                identifier = "[Choice]";
                ChoiceMenu.Update();
                ChoiceMenu.SetPosition(new Vector2(textPosX + 4*TEXTPADDING, textPosY + (int)dialogueFont.MeasureString(dialogue[0]).Y + TEXTPADDING));
                ChoiceMenu.IsVisible = true;
            }
            // TODO: Add your update code here
            if(dialogueQueue.Count != 0 && dialogue.Count == 0)
            {
                Tuple<string, DialogueEnum> t = dialogueQueue.Dequeue();
                DialogueType = t.Item2;
                drawDialogue = true;
                string[] tempArray = t.Item1.Replace(GamePlayScreen.MC_IDENTIFIER, GamePlayScreen.HERO_NAME).Split('^');
                if (DialogueType != DialogueEnum.Choice)
                {
                    ParseDialogue(tempArray);
                }
                else
                {
                    // Setting up the choices
                    ParseDialogue(tempArray[0].Split('^'));
                    List<string> choiceText = new List<string>();
                    for (int i = 1; i < tempArray.Count(); i++)
                    {
                        if (!tempArray[i].Equals(""))
                        {
                            //Console.WriteLine(s.Trim());
                            choiceText.Add(tempArray[i].Split('|')[0].Trim());
                            choiceValues.Add(tempArray[i].Split('|')[1].Trim());
                        }
                    }
                    ChoiceMenu.SetMenuItems(choiceText);
                }
            }
            if (dialogue.Count != 0)
            {
                /*if (!scrollWatch.IsRunning)
                {
                    scrollWatch.Start();
                }*/
                if (dialogue[0].Equals("") || dialogue.Count == 0)
                {
                    scrollFinished = true;
                    //scrollWatch.Stop();
                    //scrollWatch.Reset();
                }
                else
                {
                    if (DialogueType == DialogueEnum.Attribute || DialogueType == DialogueEnum.Choice) //no scrolling
                    {
                        if (DialogueType == DialogueEnum.Attribute && !dialogue[0].Contains("Affection")) // Affection is a hidden stat gain
                        {
                            shownString = "Gained " + dialogue[0].Trim() + "!";
                            Game1.audioManager.PlayPositiveSound();

                        }
                        else { shownString = dialogue[0].Trim(); }
                        dialogue[0] = "";
                    }
                    else // scrolling
                    { 
                        scrollFinished = false;
                        if (!updateLock)
                        {
                            //if (scrollWatch.ElapsedMilliseconds > SCROLL_SPEED)
                            //{
                            if (dialogue[0].Contains('|')) // && identifier.Equals(""))
                            {
                                // Extract the identifier, this seems to be a very shitty solution
                                int index = dialogue[0].IndexOf('|');
                                identifier = dialogue[0].Split('|')[0];
                                dialogue[0] = dialogue[0].Remove(0, index+1).Trim();
                            }
                            shownString += dialogue[0][0];
                            dialogue[0] = dialogue[0].Remove(0, 1);
                            //scrollWatch.Reset();
                            //}
                        }
                    }
                 }
                if (InputManager.KeyReleased(Keys.Space))
                {
                    if (scrollFinished)
                    {
                        shownString = "";
                        dialogue.RemoveAt(0);
                        //identifier = "";
                    }
                    else
                    {
                        updateLock = true;
                        shownString += dialogue[0];
                        dialogue[0] = "";
                        updateLock = false;
                    }
                }
            }
            if (dialogue.Count == 0)
            {
                drawDialogue = false;
            }
            /*if (dialogue.Count == 0)
            {
                scrollFinished = true;
                //scrollWatch.Stop();
                //scrollWatch.Reset();
            }
            else if(dialogue.Count == 1 && dialogue[0].Equals(""))
            {
                scrollFinished = true;
            }*/
            base.Update(gameTime);
        }

        public void ParseDialogue(string[] tempArray)
        {
            //string[] tempArray = command[1].Split('^');
            for (int i = 0; i < tempArray.Length; i++)
            {
                string buildingString = "";
                string[] tempStringArray = tempArray[i].Trim().Split(' ');
                foreach (string s in tempStringArray)
                {
                    buildingString = s.Replace("\n", "").Trim();
                    if (!buildingString.Equals(""))
                    {
                        if (dialogueFont.MeasureString(tempString + buildingString + " ").X >= (DIALOGUEWIDTH - 4 * TEXTPADDING))
                        {
                            if (dialogueFont.MeasureString(tempString + "\n" + buildingString + " ").Y >= (DIALOGUEHEIGHT - 8 * TEXTPADDING))
                            {
                                dialogue.Add(tempString);
                                tempString = buildingString + " ";
                            }
                            else
                            {
                                tempString += "\n" + buildingString + " ";
                            }
                        }
                        else
                        {
                            tempString += buildingString + " ";
                        }
                    }
                }
                //dialogue.Add(tempArray[i].Trim());
                dialogue.Add(tempString);
                tempString = "";
            }
        }

        public void ShowNPCDialogue(string rawDialogue, DialogueEnum dialogueType)
        {
            dialogueQueue.Enqueue(new Tuple<string,DialogueEnum>(rawDialogue, dialogueType));
        }

        public bool hasJobsLeft()
        {
            if (dialogueQueue.Count == 0 && dialogue.Count == 0)
            {
                return false;
            }
            else
                return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (drawDialogue == true)
            {
                if (DialogueType == DialogueEnum.Attribute)
                {
                    Rectangle itemRectangle = new Rectangle(
                        ((int)(Game1.Width -
                        dialogueFont.MeasureString(shownString).X) / 2) - TEXTPADDING,
                        ((int)Game1.Height - TEXTPADDING) / 2,
                        (int)dialogueFont.MeasureString(shownString).X + 2*TEXTPADDING,
                        (int)dialogueFont.MeasureString(shownString).Y + TEXTPADDING);
                    Game1.spriteBatch.Draw(dialogueTexture, itemRectangle, Color.White);
                    //Game1.spriteBatch.Draw(portraitBorderTexture, itemRectangle, Color.White);
                    Game1.spriteBatch.DrawString(dialogueFont, shownString,
                        new Vector2(((int)(Game1.Width -
                        dialogueFont.MeasureString(shownString).X) / 2),
                            ((int)Game1.Height / 2)), Color.White);
                }
                else
                {
                    //if (dialogue.Count > 0)
                    //{
                        if (identifier.Equals("[Thought]"))
                        {
                            Game1.spriteBatch.Draw(altDialogueTexture, dialogueRectangle, Color.White);
                            Game1.spriteBatch.DrawString(dialogueFont, "( " + shownString + " )", new Vector2(identifierPosX, identifierPosY + TEXTPADDING), Color.White);
                        }
                        else if (identifier.Equals("[Narrator]"))
                        {
                            Game1.spriteBatch.Draw(altDialogueTexture, narrationDialogueRectangle, Color.White);
                            Game1.spriteBatch.DrawString(dialogueFont, shownString, new Vector2(identifierPosX + TEXTPADDING, NARRATIONPOSY), Color.White);
                        }
                        else if (identifier.Equals("[Choice]"))
                        {
                            Game1.spriteBatch.Draw(dialogueTexture, dialogueRectangle, Color.White);
                            Game1.spriteBatch.DrawString(dialogueFont, shownString, new Vector2(identifierPosX, identifierPosY + TEXTPADDING), Color.White);
                        }
                        else if (identifier.Equals("[End]"))
                        {
                            Game1.spriteBatch.Draw(altDialogueTexture, dialogueRectangle, Color.White);
                            Game1.spriteBatch.DrawString(dialogueFont, shownString, new Vector2(identifierPosX, identifierPosY + TEXTPADDING), Color.White);
                        }
                        else if (!identifier.Equals(""))
                        {
                            Game1.spriteBatch.Draw(dialogueTexture, dialogueRectangle, Color.White);
                            Game1.spriteBatch.DrawString(identifierFont, identifier, new Vector2(identifierPosX, identifierPosY), Color.White);
                            Game1.spriteBatch.DrawString(dialogueFont, shownString, new Vector2(identifierPosX + 2 * TEXTPADDING, identifierPosY + 5 * TEXTPADDING), Color.White);
                        }
                        //Game1.spriteBatch.Draw(dialogueTexture, dialogueRectangle, Color.White);
                        // Game1.spriteBatch.DrawString(identifierFont, identifier, new Vector2(identifierPosX, identifierPosY), Color.White);
                        //Game1.spriteBatch.DrawString(dialogueFont, shownString, new Vector2(identifierPosX + 2 * TEXTPADDING, identifierPosY + 5 * TEXTPADDING), Color.White);
                        if (ChoiceMenu.MenuItems.Count != 0 && ChoiceMenu.IsVisible)
                            ChoiceMenu.Draw(Game1.spriteBatch, TEXTPADDING * 4, true);
                    //}
                }
            }
            //Game1.spriteBatch.DrawString(identifierFont, TEXTPADDING.ToString(), Vector2.Zero, Color.White);
        }
    }
}
