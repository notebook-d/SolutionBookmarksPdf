using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace BookmarksPdf
{
    public class WordWithCoordinates
    {
        public string Text { get; set; }
        public Point BottomLeft { get; set; }  // Нижний левый угол
        public Point BottomRight { get; set; } // Нижний правый угол
        public Point TopLeft { get; set; }     // Верхний левый угол
        public Point TopRight { get; set; }    // Верхний правый угол

        public override string ToString()
        {
            return $"Text: {Text}, BottomLeft: {BottomLeft}, TopLeft: {TopLeft}, BottomRight: {BottomRight}, TopRight: {TopRight}";
        }
    }
}
