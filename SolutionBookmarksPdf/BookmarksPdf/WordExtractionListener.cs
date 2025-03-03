using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;


namespace BookmarksPdf
{
    public class WordExtractionListener : IEventListener
    {
        private readonly List<WordWithCoordinates> words = new List<WordWithCoordinates>();

        public IList<WordWithCoordinates> Words => words;

        public void EventOccurred(IEventData data, EventType type)
        {
            if (data is TextRenderInfo textRenderInfo)
            {
                // Получаем текст
                string word = textRenderInfo.GetText();

                // Получаем ограничивающий прямоугольник (bounding box)
                Rectangle rect = textRenderInfo.GetDescentLine().GetBoundingRectangle();

                // Получаем координаты всех 4 точек
                Point bottomLeft = new Point(rect.GetLeft(), rect.GetBottom());
                Point bottomRight = new Point(rect.GetRight(), rect.GetBottom());
                Point topLeft = new Point(rect.GetLeft(), rect.GetTop());
                Point topRight = new Point(rect.GetRight(), rect.GetTop());

                // Добавляем слово с координатами в список
                words.Add(new WordWithCoordinates
                {
                    Text = word,
                    BottomLeft = bottomLeft,
                    BottomRight = bottomRight,
                    TopLeft = topLeft,
                    TopRight = topRight
                });
            }
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return new List<EventType> { EventType.RENDER_TEXT };
        }
    }
}
