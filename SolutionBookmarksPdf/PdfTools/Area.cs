using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PdfTools
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
            return $"{TopLeft} - {BottomRight} Center = {RectangleCenter.X} {RectangleCenter.Y}";
        }

        public Coordinates RectangleCenter
        {
            get 
            {
                //double millimeters = points * 0.3528;
                var centerX = (TopLeft.X + BottomRight.X) / 2.0 * 0.3528;
                var centerY = (TopLeft.Y + BottomRight.Y) / 2.0 * 0.3528;
                return new Coordinates(centerX, centerY);
            }
        }
    }
}
