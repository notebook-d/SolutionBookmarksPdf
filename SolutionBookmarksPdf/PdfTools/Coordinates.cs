using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PdfTools
{
    /// <summary>
    /// Класс для хранения координат (X, Y)
    /// </summary>
    public class Coordinates
    {
        [XmlAttribute]
        public double X { get; set; }
        [XmlAttribute]
        public double Y { get; set; }

        public Coordinates() { }

        public Coordinates(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"[{X}; {Y}]";
        }
    }
}
