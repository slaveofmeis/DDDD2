using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DDDD2.GameInformation
{
    public class GameInfo
    {
        private Dictionary<string, int> affectionDict;
        private Dictionary<int, Scene> sceneDict;
        private XmlDocument xd;

        public GameInfo()
        {
            Strength = 0;
            Intelligence = 0;
            Charisma = 0;
            affectionDict = new Dictionary<string, int>();
            sceneDict = new Dictionary<int, Scene>();   
        }

        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Charisma { get; set; }

        public void LoadContent()
        {

        }

        public Dictionary<string, int> getAffectionDict()
        {
            return affectionDict;
        }        

        public Dictionary<int, Scene> getSceneDict()
        {
            return sceneDict;
        }

        public void parseScenes(XmlSource xs)
        {
            xd = new XmlDocument();
            xd.LoadXml(xs.XmlCode);
            XmlNodeList elemList = xd.GetElementsByTagName("Item");
            for (int i = 0; i < elemList.Count; i++)
            {
                Scene s = new Scene();
                //xd.LoadXml("<Item>"+elemList[i].InnerXml+"</Item>");
                //XmlNodeList subList = xd.GetElementsByTagName("SceneNumber");
                s.SceneNumber = Int32.Parse(elemList[i].SelectSingleNode("SceneNumber").InnerText.Trim());
                s.Background = elemList[i].SelectSingleNode("Background").InnerText.Trim();
                s.Sprite = elemList[i].SelectSingleNode("Sprite").InnerText.Trim();
                s.MainDialogue = elemList[i].SelectSingleNode("MainDialogue").InnerText.Trim().Replace('\n', '^');
                XmlNodeList choiceList = elemList[i].SelectNodes("Choice");
                foreach (XmlNode n in choiceList)
                {
                    if (n.InnerText.Trim().Equals(""))
                    {
                    }
                    else
                    {
                        string[] splitArray = n.InnerText.Split('|');
                        s.ChoiceList.Add(new Tuple<string,int>(splitArray[0].Trim(), Int32.Parse(splitArray[1].Trim())));
                    }
                }
                s.AttributeModify = elemList[i].SelectSingleNode("AttributeModify").InnerText.Trim();
                s.AttributeFork = elemList[i].SelectSingleNode("AttributeFork").InnerText.Trim();
                if(!elemList[i].SelectSingleNode("NextScene").InnerText.Trim().Equals(""))
                    s.NextScene = Int32.Parse(elemList[i].SelectSingleNode("NextScene").InnerText.Trim());
                sceneDict.Add(s.SceneNumber, s);
            }
        }
    }
}
