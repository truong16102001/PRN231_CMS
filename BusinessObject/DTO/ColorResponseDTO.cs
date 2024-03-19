using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ColorResponseDTO
    {
        public List<ColorDTO> Colors { get; set; }
    }

    public class ColorDTO
    {
        public string Name { get; set; }
        public string Theme { get; set; }
        public string Group { get; set; }
        public string Hex { get; set; }
        public string Rgb { get; set; }
    }
}
