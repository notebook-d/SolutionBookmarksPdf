using BookmarksPdf.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
using System.Windows;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Outline;

namespace BookmarksPdf
{
    public class HelperPdf
    {
        /// <summary>
        /// Загружаем документ
        /// </summary>
        /// <param name="file"></param>
        public void LoadInfoDocuments(string file, ConfigurationFile configurationFile)
        {
            var gostForma = configurationFile.GlobalSettings.StandardAndFormArea;
            var forma821 = configurationFile.Forms.FirstOrDefault();

            List<string> result = new List<string>();            

            using (UglyToad.PdfPig.PdfDocument document = UglyToad.PdfPig.PdfDocument.Open(file))
            {

                var isGetBook = document.TryGetBookmarks(out Bookmarks bookmarks);

                foreach (Page page in document.GetPages())
                {
                    var pageSize = page.Size;
                    var pageHeight = page.Height;
                    var pageWidth = page.Width;


                    IEnumerable<Word> words = page.GetWords();

                    var pageInfo = GetFormaDocument(words, configurationFile.GlobalSettings.StandardAndFormArea, configurationFile.GlobalSettings.TypeDocumentArea);

                    result.Add($"Страница {page.Number} {pageInfo.DocumentType}|{pageInfo.Standard}|{pageInfo.FormName}");


                    //GetFormaDocuments(page, configurationFile);

                    //IReadOnlyList<Letter> letters = page.Letters;
                    //string example = string.Join(string.Empty, letters.Select(x => x.Value));



                    //foreach (var word in words)
                    //{
                    //    var tst = word.Text;


                    //}

                    //IEnumerable<IPdfImage> images = page.GetImages();
                }
            }

            MessageBox.Show(string.Join(Environment.NewLine, result));
        }

        /// <summary>
        /// Определяет стандарт, форму и тип документа листа
        /// </summary>
        public FormDocums GetFormaDocument(IEnumerable<Word> words, Area standardAndFormArea, Area typeDocumentArea)
        {
            if (words == null || standardAndFormArea == null || typeDocumentArea == null) return null;

            List<string> standardAndFormList = new List<string>();
            var typeDocument = string.Empty;

            foreach (var word in words)
            {
                Area area = new Area(new Coordinates(word.BoundingBox.TopLeft.X, word.BoundingBox.TopLeft.Y), 
                    new Coordinates(word.BoundingBox.BottomRight.X, word.BoundingBox.BottomRight.Y));

                var rectangleCenter = CalculateRectangleCenter(area);

                var isPointInRectangle = IsPointInRotatedRectangle(standardAndFormArea, rectangleCenter);
                if (isPointInRectangle) 
                {
                    standardAndFormList.Add(word.Text);
                }

                var isPointInRectangleTypeDocument = IsPointInRotatedRectangle(typeDocumentArea, rectangleCenter);
                if (isPointInRectangleTypeDocument) typeDocument = word.Text;
            }

            var resultStandardAndForm = GetStringsAroundForm(standardAndFormList);

            FormDocums reseltFormDocums = new FormDocums 
            { 
                DocumentType = typeDocument,
                Standard = resultStandardAndForm.Item1,
                FormName = resultStandardAndForm.Item2
            };

            return reseltFormDocums;
        }

        // Метод для нахождения строк до и после слова "Форма"
        static (string, string) GetStringsAroundForm(List<string> list)
        {
            // Находим индекс слова "Форма"
            int formIndex = list.IndexOf("Форма");

            // Если слово "Форма" не найдено, возвращаем пустые строки
            if (formIndex == -1)
            {
                return (string.Empty, string.Empty);
            }

            // Суммируем строки до слова "Форма"
            string beforeForm = string.Join(" ", list.Take(formIndex));

            // Суммируем строки после слова "Форма" (включая его)
            string afterForm = string.Join(" ", list.Skip(formIndex));

            return (beforeForm, afterForm);
        }

        /// <summary>
        /// Определяет стандарт, форму и тип документа листа
        /// </summary>
        public void GetFormaDocuments(Page pagePdf, ConfigurationFile configurationFile)
        {
            if (pagePdf == null || configurationFile == null) return;

            var gostForma = configurationFile.GlobalSettings.StandardAndFormArea;
            var typeDocums = configurationFile.GlobalSettings.TypeDocumentArea;
            var forma821 = configurationFile.Forms.FirstOrDefault();

            List<string> result = new List<string>();
            result.Add($"Страница {pagePdf.Number}");


            IEnumerable<Word> words = pagePdf.GetWords();

            foreach (var word in words)
            {
                var orient = word.TextOrientation;
                var boundBox = word.BoundingBox;

                var tst = word.Text;
                //if (tst == "3.1118-82")
                //{
                    //Определяем координаты середины прямоугольника
                Area area = new Area(new Coordinates(word.BoundingBox.TopLeft.X, word.BoundingBox.TopLeft.Y), new Coordinates(word.BoundingBox.BottomRight.X, word.BoundingBox.BottomRight.Y));    
                
                var rectangleCenter = CalculateRectangleCenter(area);
                var isPointInRectangle = IsPointInRotatedRectangle(gostForma, rectangleCenter);
                if (isPointInRectangle)
                {
                    result.Add(word.Text);
                }

                var isPointInRectangleTypeDocument = IsPointInRotatedRectangle(typeDocums, rectangleCenter);
                if (isPointInRectangleTypeDocument)
                {
                    result.Add(word.Text);
                }

                //}
                //"3.1105-2011"
                //3.1118-82

                //Собираем текст по местоположению

            }

            //MessageBox.Show(string.Join(";",result));

        }
        /// <summary>
        /// Расчитыает координаты середины прямоугольника Результат в мм возвращает
        /// </summary>        
        public static Coordinates CalculateRectangleCenter(Area area)
        {
            if (area == null) return null;
            // Перевод в миллиметры
            //double millimeters = points * 0.3528;
            var centerX = ((area.TopLeft.X + area.BottomRight.X) / 2.0) * 0.3528;
            var centerY = ((area.TopLeft.Y + area.BottomRight.Y) / 2.0) * 0.3528;
            return new Coordinates(centerX, centerY);
        }

        /// <summary>
        /// Метод для проверки, попадает ли точка в прямоугольник
        /// </summary>
        /// <param name="areaTopLeftBottomRight"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static bool IsPointInRotatedRectangle(Area areaTopLeftBottomRight, Coordinates coordinates)
        {
            if (areaTopLeftBottomRight == null || coordinates == null) return false;

            var x1 = areaTopLeftBottomRight.TopLeft.X;
            var x2 = areaTopLeftBottomRight.TopLeft.X;
            var x3 = areaTopLeftBottomRight.BottomRight.X;
            var x4 = areaTopLeftBottomRight.BottomRight.X;

            var y1 = areaTopLeftBottomRight.BottomRight.Y;
            var y2 = areaTopLeftBottomRight.TopLeft.Y;
            var y3 = areaTopLeftBottomRight.TopLeft.Y;
            var y4 = areaTopLeftBottomRight.BottomRight.Y;

            // Функция для вычисления векторного произведения
            double CrossProduct(double ax, double ay, double bx, double by, double cx, double cy)
            {
                return (bx - ax) * (cy - ay) - (by - ay) * (cx - ax);
            }

            // Вычисляем векторные произведения для каждой стороны
            double d1 = CrossProduct(x1, y1, x2, y2, coordinates.X, coordinates.Y);
            double d2 = CrossProduct(x2, y2, x3, y3, coordinates.X, coordinates.Y);
            double d3 = CrossProduct(x3, y3, x4, y4, coordinates.X, coordinates.Y);
            double d4 = CrossProduct(x4, y4, x1, y1, coordinates.X, coordinates.Y);

            // Проверяем, находятся ли все произведения с одной стороны
            return (d1 >= 0 && d2 >= 0 && d3 >= 0 && d4 >= 0) || (d1 <= 0 && d2 <= 0 && d3 <= 0 && d4 <= 0);
        }
    }
}
