using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Geom;

namespace BookmarksPdf
{
    public class TextLocationStrategy : IEventListener
    {
        private List<TextChunk> textChunks = new List<TextChunk>();

        public void EventOccurred(IEventData data, EventType type)
        {
            if (type == EventType.RENDER_TEXT)
            {
                TextRenderInfo renderInfo = (TextRenderInfo)data;
                string text = renderInfo.GetText();
                var baseline = renderInfo.GetBaseline();
                var startPoint = baseline.GetStartPoint();

                // Получаем ограничивающий прямоугольник (bounding box)
                var rectTop = renderInfo.GetAscentLine().GetBoundingRectangle();
                var rectBottom = renderInfo.GetDescentLine().GetBoundingRectangle();
                
                Point bottomLeft = new Point(rectBottom.GetLeft(), rectBottom.GetBottom());
                Point bottomRight = new Point(rectBottom.GetRight(), rectBottom.GetBottom());
                Point topLeft = new Point(rectTop.GetLeft(), rectTop.GetTop());
                Point topRight = new Point(rectTop.GetRight(), rectTop.GetTop());

                var endPoint = baseline.GetEndPoint();

                var length = baseline.GetLength();

                var rect = baseline.GetBoundingRectangle();

                var temp = new TextChunk
                {
                    Text = text,
                    X = startPoint.Get(0),
                    Y = startPoint.Get(1)
                };

                textChunks.Add(new TextChunk
                {
                    Text = text,
                    X = rectTop.GetX(),
                    Y = rectTop.GetY(),
                    Height = rectTop.GetY() - rectBottom.GetY(),
                    Width = rectTop.GetWidth()
                });
            }
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return new List<EventType> { EventType.RENDER_TEXT };
        }

        public List<TextChunk> GetTextChunks()
        {
            return textChunks;
        }
    }
}
