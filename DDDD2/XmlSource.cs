using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDD2
{
    public class XmlSource
    {
        private string xmlCode;
        public string XmlCode { get { return xmlCode; } }
        public XmlSource(string xmlCode)
        {
            this.xmlCode = xmlCode;
        }
    }

}
