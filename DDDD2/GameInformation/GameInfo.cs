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
        private Dictionary<string, int> statsDict;
        private Dictionary<string, Scene> sceneDict;
        private XmlDocument xd;

        public GameInfo()
        {
            statsDict = new Dictionary<string, int>();
            sceneDict = new Dictionary<string, Scene>();   
        }

        public void LoadContent()
        {

        }

        public Dictionary<string, int> getStatsDict()
        {
            return statsDict;
        }
        
        public void resetStats()
        {
            statsDict.Clear();
        }        

        public Dictionary<string, Scene> getSceneDict()
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
                s.SceneId = elemList[i].SelectSingleNode("SceneNumber").InnerText.Trim();
                s.Background = elemList[i].SelectSingleNode("Background").InnerText.Trim();
                s.Sprite = elemList[i].SelectSingleNode("Sprite").InnerText.Trim();
                s.MainDialogue = elemList[i].SelectSingleNode("MainDialogue").InnerText.Trim().Replace('\n', '^');
                if (elemList[i].SelectSingleNode("ChoiceText") != null)
                {
                    XmlNodeList choiceList = elemList[i].SelectNodes("Choice");
                    if (!elemList[i].SelectSingleNode("ChoiceText").InnerText.Trim().Equals(""))
                    {
                        s.Choices += elemList[i].SelectSingleNode("ChoiceText").InnerText.Trim() + "^";
                    }
                    for (int j = 0; j < choiceList.Count; j++)
                    {
                        if (choiceList[j].InnerText.Trim().Equals(""))
                        {
                        }
                        else
                        {
                            s.Choices += choiceList[j].InnerText;
                            if (j != choiceList.Count - 1)
                            {
                                s.Choices += "^";
                            }
                        }
                    }
                }
                s.AttributeModify = elemList[i].SelectSingleNode("AttributeModify").InnerText.Trim();
                s.AttributeFork = elemList[i].SelectSingleNode("AttributeFork").InnerText.Trim();
                if(!elemList[i].SelectSingleNode("NextScene").InnerText.Trim().Equals(""))
                    s.NextScene = elemList[i].SelectSingleNode("NextScene").InnerText.Trim();
                sceneDict.Add(s.SceneId, s);
            }
        }
    }
}
