using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Compiling
{
    public   class ParserContext
    {
    
        public Dictionary<string ,PropertyMap> Properties { get; set; }
        public  Tokenizer Tokenizer { get; internal set; }

        public ParserContext()
        {
            Properties = new Dictionary<string, PropertyMap>();
        }

    }
}
