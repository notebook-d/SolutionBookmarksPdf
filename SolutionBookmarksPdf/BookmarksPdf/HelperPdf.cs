using BookmarksPdf.Settings;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
using System.Windows;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Outline;
using iText.Kernel.Pdf.Navigation;
using PdfSharp.Pdf.IO;

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
                    if (pageInfo.DocumentVid == Enums.DocumentVidEnum.General)
                    {
                        //Создаём закладку-документ.Привязываем закладку к рассматриваемому листу.
                        //Имя закладки создаётся по шаблону: «Тип документа». Например, «ТЛ»;
                        GetBookmarks(document);
                    }

                    //result.Add($"Страница {page.Number} {pageInfo.DocumentType}|{pageInfo.Standard}|{pageInfo.FormName}");
                    Console.WriteLine($"Страница {page.Number} {pageInfo.DocumentType}|{pageInfo.Standard}|{pageInfo.FormName}");
                    
                }
            }

            //MessageBox.Show(string.Join(Environment.NewLine, result));
        }

        public void GetBookmarks(UglyToad.PdfPig.PdfDocument document)
        {
            var getBook = document.TryGetBookmarks(out Bookmarks bookmarks);
            
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

        #region itext7

        public void LoadInfoListITextSharp(string file, ConfigurationFile configurationFile)
        {

            List<TextChunk> textChunks = ExtractTextWithCoordinatesNew(file, configurationFile);

            foreach (var chunk in textChunks)
            {
                Console.WriteLine(chunk);
            }


            //using (PdfReader reader = new PdfReader(file))
            //using (PdfDocument document = new PdfDocument(reader))
            //{
            //    string text = "";
            //    for (int i = 1; i <= document.GetNumberOfPages(); i++)
            //    {
            //        text += iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(document.GetPage(i));
            //    }                
            //}

        }

        //public static List<TextChunk> ExtractTextWithCoordinates(string pdfPath)
        //{
        //    var textChunks = new List<TextChunk>();

        //    using (PdfReader reader = new PdfReader(pdfPath))
        //    using (PdfDocument document = new PdfDocument(reader))
        //    {
        //        for (int i = 1; i <= document.GetNumberOfPages(); i++)
        //        {
        //            var strategy = new TextLocationStrategy();
        //            PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
        //            parser.ProcessPageContent(document.GetPage(i));

        //            textChunks.AddRange(strategy.GetTextChunks());
        //        }
        //    }

        //    return textChunks;
        //}

        public List<TextChunk> ExtractTextWithCoordinatesNew(string pdfPath, ConfigurationFile configurationFile)
        {
            var textChunks = new List<TextChunk>();

            string outputPdf = "D:\\ИТП без закладок 2.pdf";

            using (iText.Kernel.Pdf.PdfReader reader = new iText.Kernel.Pdf.PdfReader(pdfPath))
            using (PdfWriter writer = new PdfWriter(outputPdf))
            using (PdfDocument document = new PdfDocument(reader, writer))
            {
                for (int i = 1; i <= document.GetNumberOfPages(); i++)
                {
                    var strategy = new TextLocationStrategy();
                    PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
                    PdfPage pdfPage = document.GetPage(i);
                    parser.ProcessPageContent(pdfPage);

                    FormDocums formDocums = ExtractFormPage(i, strategy.GetTextChunks(), configurationFile);
                    if (formDocums != null)
                    {
                        Console.WriteLine($"{formDocums}");
                        if (formDocums.DocumentVid == Enums.DocumentVidEnum.General)
                        {
                            //Создаем закладку
                            //CreateBookmarks(document, pdfPage);
                        }
                    }
                }
            }

            return textChunks;
        }
        /// <summary>
        /// Создает закладку
        /// </summary>
        public void CreateBookmarks(PdfDocument pdfDoc, PdfPage pdfPage)
        {
            // Получаем или создаем корневую структуру закладок
            PdfOutline rootOutline = pdfDoc.GetOutlines(false); // Пытаемся получить существующие закладки
            // Если структура закладок отсутствует, создаем ее
            if (rootOutline == null)
            {
                rootOutline = pdfDoc.GetOutlines(true); // Создаем корневую структуру закладок
            }

            // Добавляем новую закладку
            PdfOutline bookmark = rootOutline.AddOutline("Моя закладка");


            // Указываем, куда ведет закладка (на первую страницу)
            PdfDestination destination = PdfExplicitDestination.CreateFit(pdfPage);
            bookmark.AddDestination(destination);
        }

        /// <summary>
        /// Возвращает Тип, ГОСТ и Форму листа
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        public FormDocums ExtractFormPage(int numberPage, List<TextChunk> textChunks, ConfigurationFile configurationFile)
        {
            var gostForma = configurationFile.GlobalSettings.StandardAndFormArea;
            var typeDocums = configurationFile.GlobalSettings.TypeDocumentArea;

            var standartForma = string.Empty;
            var documentType = string.Empty;

            foreach (var textChunk in textChunks)
            {
                
                var isPointInRectangle = IsPointInRotatedRectangle(gostForma, textChunk.RectangleCenter);
                if (isPointInRectangle)
                {
                    standartForma = textChunk.Text;
                    Console.WriteLine($"Страница {numberPage}     {textChunk}   {gostForma}");
                }

                var isPointInRectangleTypeDocument = IsPointInRotatedRectangle(typeDocums, textChunk.RectangleCenter);
                if (isPointInRectangleTypeDocument)
                {
                    documentType = textChunk.Text;
                    Console.WriteLine($"Страница {numberPage}     {textChunk}   {gostForma}");
                }
            }

            if (!string.IsNullOrEmpty(documentType) && 
                !string.IsNullOrEmpty(standartForma))
            {
                var standartFormaList = GetStandartForma(standartForma);                
                var result = configurationFile.Forms.FirstOrDefault(n => 
                n.DocumentType == documentType 
                && n.Standard == standartFormaList.Item1
                && n.FormName == standartFormaList.Item2
                );

                return result;
            }

            return null;
        }

        public (string, string) GetStandartForma(string standartForma)
        {
            if (string.IsNullOrEmpty(standartForma)) return (string.Empty, string.Empty);

            // Находим индекс начала слова "Форма"
            int indexOfForma = standartForma.IndexOf("Форма");

            if (indexOfForma == -1) return (string.Empty, string.Empty);

            // Извлекаем первую часть строки до "Форма"
            string gost = standartForma.Substring(0, indexOfForma).Trim();

            // Извлекаем вторую часть строки, начиная с "Форма"
            string forma = standartForma.Substring(indexOfForma).Trim();

            return (gost, forma);
        }

        #endregion

        #region itext7-2

        public void LoadInfoListITextSharp2(string file)
        {
            using (PdfDocument pdfDoc = new PdfDocument(new iText.Kernel.Pdf.PdfReader(file)))
            {
                // Создаем слушатель для извлечения слов
                var listener = new WordExtractionListener();

                // Обрабатываем каждую страницу
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    PdfPage page = pdfDoc.GetPage(i);
                    PdfCanvasProcessor parser = new PdfCanvasProcessor(listener);
                    parser.ProcessPageContent(page);
                }

                // Выводим извлеченные слова с координатами
                foreach (var word in listener.Words)
                {
                    Console.WriteLine($"Текст: {word.Text} Нижний левый: ({word.BottomLeft.GetX()}, {word.BottomLeft.GetY()}) Нижний правый: ({word.BottomRight.GetX()}, {word.BottomRight.GetY()}) Верхний левый: ({word.TopLeft.GetX()}, {word.TopLeft.GetY()}) Верхний правый: ({word.TopRight.GetX()}, {word.TopRight.GetY()})");                    
                    Console.WriteLine();
                }
            }

        }
               

        #endregion

        public void CreateBookmarks()
        {
            // Путь к существующему PDF
            string inputPdf = "D:\\ИТП без закладок.pdf";
            // Путь для сохранения измененного PDF
            string outputPdf = "D:\\ИТП без закладок.pdf";

            // Открываем существующий PDF
            PdfSharp.Pdf.PdfDocument document = PdfSharp.Pdf.IO.PdfReader.Open(inputPdf, PdfDocumentOpenMode.Modify);
            // Добавляем закладку
            PdfSharp.Pdf.PdfOutline outline = document.Outlines.Add("Моя закладка", document.Pages[1], true);

            // Сохраняем изменения
            document.Save(outputPdf);
        }



    }
}
