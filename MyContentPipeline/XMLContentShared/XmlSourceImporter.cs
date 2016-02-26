using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContentPipeline.XMLContentShared
{
    [ContentImporter(".xml", DefaultProcessor = "None", DisplayName = "Xml Source Importer")]
    class XmlSourceImporter : ContentImporter<XmlSource>
    {
        public override XmlSource Import(string filename, ContentImporterContext context)
        {
            string sourceCode = System.IO.File.ReadAllText(filename);
            return new XmlSource(sourceCode);
        }
    }

}
