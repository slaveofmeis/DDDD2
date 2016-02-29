using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDD2.GameInformation
{
    public class Scene
    {
        private List<Tuple<string, int>> choiceList;

        public Scene()
        {
            choiceList = new List<Tuple<string, int>>();
        }
        public int SceneNumber
        {
            get; set;
        }
        public string Background
        {
            get; set;
        }
        public string Sprite
        {
            get; set;
        }
        public string MainDialogue
        {
            get; set;
        }
        public List<Tuple<string, int>> ChoiceList
        {
            get { return choiceList; }
        }

        public string AttributeModify
        {
            get; set;
        }

        public string AttributeFork
        {
            get; set;
        }

        public int NextScene
        {
            get; set;
        }
    }
}
