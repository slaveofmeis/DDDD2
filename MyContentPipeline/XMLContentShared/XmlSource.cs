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
            //this.xmlCode = xmlCode;
            this.xmlCode = obfuscate(xmlCode,4940);
        }

        private string obfuscate(string source, Int16 shift)
        {
            var maxChar = Convert.ToInt32(char.MaxValue);
            var minChar = Convert.ToInt32(char.MinValue);

            var buffer = source.ToCharArray();

            for (var i = 0; i < buffer.Length; i++)
            {
                var shifted = Convert.ToInt32(buffer[i]) + shift;

                if (shifted > maxChar)
                {
                    shifted -= maxChar;
                }
                else if (shifted < minChar)
                {
                    shifted += maxChar;
                }

                buffer[i] = Convert.ToChar(shifted);
            }

            return new string(buffer);
        }

        private string xmlCode;
        public string XmlCode { get { return xmlCode; } }
    }

}
