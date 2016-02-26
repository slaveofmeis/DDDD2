using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContentPipeline.XMLContentShared
{
    [ContentTypeWriter]
    class XmlSourceWriter : ContentTypeWriter<XmlSource>
    {
        protected override void Write(ContentWriter output, XmlSource value)
        {
            /*StringWriter sw=new StringWriter();
            value.Save(sw);
            string content = sw.ToString();*/
            output.Write(value.XmlCode);
        }
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(XmlDocument).AssemblyQualifiedName;
        }
        public override string GetRuntimeReader(
                TargetPlatform targetPlatform)
        {
            return "DDDD2.XmlSourceReader, DDDD2";
        }
    }
}
