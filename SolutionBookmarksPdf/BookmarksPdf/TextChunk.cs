using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarksPdf
{
    public class TextChunk
    {
        public string Text { get; set; }        
        public float X { get; set; }
        public float Y { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }

        public override string ToString()
        {
            return $"Text: {Text}, X: {X}, Y: {Y}, Height: {Height}, Width: {Width}, RectangleCenter={RectangleCenter.X},{RectangleCenter.Y}";
        }
        /// <summary>
        /// Центр прямоугольника в мм
        /// </summary>
        public Coordinates RectangleCenter
        {
            get
            {
                var centerX = (X  + (Width / 2.0)) * 0.3528;
                var centerY = (Y - (Height / 2.0)) * 0.3528;
                return new Coordinates(centerX, centerY);
            }
        }

        //public static Coordinates CalculateRectangleCenter(Area area)
        //{
        //    if (area == null) return null;
        //    // Перевод в миллиметры
        //    //double millimeters = points * 0.3528;
        //    var centerX = ((area.TopLeft.X + area.BottomRight.X) / 2.0) * 0.3528;
        //    var centerY = ((area.TopLeft.Y + area.BottomRight.Y) / 2.0) * 0.3528;
        //    return new Coordinates(centerX, centerY);
        //}

    }
}
