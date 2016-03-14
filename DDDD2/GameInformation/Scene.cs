using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDD2.GameInformation
{
    public class Scene
    {
        private string sprite, choices, attributeModify, attributeFork;
        private string nextScene, music;
        public Scene()
        {
            music = "";
            sprite = "";
            choices = "";
            attributeModify = "";
            attributeFork = "";
            nextScene = "-1";
        }
        public string SceneId
        {
            get; set;
        }
        public string Background
        {
            get; set;
        }
        public string Sprite
        {
            get { return sprite; } set { sprite = value; }
        }
        public string Music
        {
            get { return music; } set { music = value;  }
        }
        public string MainDialogue
        {
            get; set;
        }
        public string Choices
        {
            get { return choices; } set { choices = value; }
        }

        public string AttributeModify
        {
            get { return attributeModify; } set { attributeModify = value; }
        }

        public string AttributeFork
        {
            get { return attributeFork; } set { attributeFork = value; }
        }

        public string NextScene
        {
            get { return nextScene; } set { nextScene = value; }
        }
    }
}
