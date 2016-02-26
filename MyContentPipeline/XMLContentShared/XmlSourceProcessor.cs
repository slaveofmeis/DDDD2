using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MyContentPipeline.XMLContentShared
{
    [ContentProcessor(DisplayName = "XmlSourceProcessor")]
    public class XmlSourceProcessor : ContentProcessor<XmlSource, XmlSource>
    {
        public override XmlSource Process(XmlSource input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
