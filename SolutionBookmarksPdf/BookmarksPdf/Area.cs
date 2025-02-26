using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookmarksPdf
{
    /// <summary>
    /// Класс для хранения области (левая верхняя и правая нижняя точки)
    /// </summary>    
    public class Area
    {
        [XmlElement]
        public Coordinates TopLeft { get; set; }
        [XmlElement]
        public Coordinates BottomRight { get; set; }

        public Area() { }

        public Area(Coordinates topLeft, Coordinates bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        public override string ToString()
        {
            return $"{TopLeft} - {BottomRight}";
        }
    }
}
