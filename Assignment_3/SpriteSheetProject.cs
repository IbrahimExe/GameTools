using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_3
{
    [Serializable]
    public class SpriteSheetProject
    {
        public string OutputDirectory { get; set; } = string.Empty;
        public string OutputFile { get; set; } = string.Empty;
        public List<string> ImagePaths { get; set; } = new List<string>();
        public bool IncludeMetaData { get; set; } = true;
    }
}
