using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContentPipeline.XMLContentShared
{
    public class XmlSource
    {
        public XmlSource(string xmlCode)
        {
            this.xmlCode = xmlCode;
        }

        private string xmlCode;
        public string XmlCode { get { return xmlCode; } }
    }

}
