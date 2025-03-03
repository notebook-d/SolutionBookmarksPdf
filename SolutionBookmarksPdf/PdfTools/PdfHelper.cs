using PdfTools.Interface;
using PdfTools.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Outline;

namespace PdfTools
{
    public class PdfHelper
    {
        public void LoadInfoDocuments(string file, ConfigurationFile configurationFile)
        {
            //List<string> result = new List<string>();

            using (UglyToad.PdfPig.PdfDocument document = UglyToad.PdfPig.PdfDocument.Open(file))
            {
                var isGetBook = document.TryGetBookmarks(out Bookmarks bookmarks);

                foreach (Page page in document.GetPages())
                {
                    IEnumerable<Word> words = page.GetWords();
                    var pageInfo = GetFormaDocument(page.Number, words, configurationFile);                    
                    Console.WriteLine($"Страница {pageInfo}");
                }
            }
        }

        /// <summary>
        /// Определяет стандарт, форму и тип документа листа
        /// </summary>
        public IFormDocument GetFormaDocument(int numberPage, IEnumerable<Word> words, ConfigurationFile configurationFile)
        {
            if (words == null || configurationFile == null) return null;
            var standardAndFormArea = configurationFile.GlobalSettings.StandardAndFormArea;
            var typeDocumentArea = configurationFile.GlobalSettings.TypeDocumentArea;
            if (standardAndFormArea == null || typeDocumentArea == null) return null; 

            List<string> standardAndFormList = new List<string>();
            var typeDocument = string.Empty;

            foreach (var word in words)
            {
                Area area = new Area(new Coordinates(word.BoundingBox.TopLeft.X, word.BoundingBox.TopLeft.Y), new Coordinates(word.BoundingBox.BottomRight.X, word.BoundingBox.BottomRight.Y));

                var isPointInRectangle = IsPointInRotatedRectangle(standardAndFormArea, area.RectangleCenter);
                if (isPointInRectangle) standardAndFormList.Add(word.Text);

                var isPointInRectangleTypeDocument = IsPointInRotatedRectangle(typeDocumentArea, area.RectangleCenter);
                if (isPointInRectangleTypeDocument) typeDocument = word.Text;
            }

            var (standard, formName) = GetStringsAroundForm(standardAndFormList);

            var configForm = configurationFile.Forms.FirstOrDefault(n => n.Standard == standard
                    && n.FormName == formName
                    && n.DocumentType == typeDocument);

            if (configForm == null) return new NoneFormDocums(new FormDocums { DocumentType = typeDocument, Standard = standard, FormName = formName }, numberPage);

            if (configForm.DocumentVid == Enums.DocumentVidEnum.General)
            {
                //Для находим номер цеха и номер операции
                var singleWorkshopFormDocums = new GeneralFormDocums(configForm, numberPage);
                return singleWorkshopFormDocums;
            }
            else
            if (configForm.DocumentVid == Enums.DocumentVidEnum.SingleWorkshop)
            {
                //Для находим номер цеха и номер операции
                string numberCeh = string.Empty;
                string numberOper = string.Empty;

                foreach (var word in words)
                {
                    Area area = new Area(new Coordinates(word.BoundingBox.TopLeft.X, word.BoundingBox.TopLeft.Y), new Coordinates(word.BoundingBox.BottomRight.X, word.BoundingBox.BottomRight.Y));
                    var isPointInRectangle = IsPointInRotatedRectangle(configForm.CehArea, area.RectangleCenter);
                    if (isPointInRectangle) numberCeh = word.Text;
                    var isOperPointInRectangle = IsPointInRotatedRectangle(configForm.OperArea, area.RectangleCenter);
                    if (isOperPointInRectangle) numberOper = word.Text;
                }

                var singleWorkshopFormDocums = new SingleWorkshopFormDocums(configForm, numberPage, numberCeh, numberOper); 
                return singleWorkshopFormDocums;
            }
            else
            if (configForm.DocumentVid == Enums.DocumentVidEnum.MultipleWorkshops)
            {
                return new MultipleWorkshopFormDocums(new FormDocums { DocumentType = typeDocument, Standard = standard, FormName = formName }, numberPage);
            }
            
            return new NoneFormDocums(new FormDocums { DocumentType = typeDocument, Standard = standard, FormName = formName }, numberPage );
        }

        // <summary>
        /// Метод для проверки, попадает ли точка в прямоугольник
        /// </summary>
        /// <param name="areaTopLeftBottomRight"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static bool IsPointInRotatedRectangle(Area area, Coordinates point)
        {
            if (area == null || point == null)
                return false;

            var (x1, y1) = (area.TopLeft.X, area.BottomRight.Y);
            var (x2, y2) = (area.TopLeft.X, area.TopLeft.Y);
            var (x3, y3) = (area.BottomRight.X, area.TopLeft.Y);
            var (x4, y4) = (area.BottomRight.X, area.BottomRight.Y);

            double CrossProduct(double ax, double ay, double bx, double by, double cx, double cy)
            {
                return (bx - ax) * (cy - ay) - (by - ay) * (cx - ax);
            }

            double d1 = CrossProduct(x1, y1, x2, y2, point.X, point.Y);
            double d2 = CrossProduct(x2, y2, x3, y3, point.X, point.Y);
            double d3 = CrossProduct(x3, y3, x4, y4, point.X, point.Y);
            double d4 = CrossProduct(x4, y4, x1, y1, point.X, point.Y);

            return (d1 >= 0 && d2 >= 0 && d3 >= 0 && d4 >= 0) || (d1 <= 0 && d2 <= 0 && d3 <= 0 && d4 <= 0);
        }

        /// <summary>
        /// Метод для нахождения строк до и после слова "Форма"
        /// </summary>
        public static (string Standard, string FormName) GetStringsAroundForm(List<string> list)
        {
            int formIndex = list.IndexOf("Форма");

            if (formIndex == -1)
                return (string.Empty, string.Empty);

            string beforeForm = string.Join(" ", list.Take(formIndex));
            string afterForm = string.Join(" ", list.Skip(formIndex));

            return (beforeForm, afterForm);
        }
    }
}
