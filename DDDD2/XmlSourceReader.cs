using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace DDDD2
{
    public class XmlSourceReader : ContentTypeReader<XmlSource>
    {
        /// <summary>
        /// Loads an imported shader.
        /// </summary>
        protected override XmlSource Read(ContentReader input, XmlSource existingInstance)
        {
            string xmlData = input.ReadString();

            return new XmlSource(xmlData);
        }
    }
}
