using PdfTools;
using PdfTools.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionBookmarksPdfTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //ConfigurationFile configurationFile = new ConfigurationFile();

            //configurationFile.Forms.Add(new FormDocums { DocumentType = "МК", Standard = "ГОСТ 3.1118-82", FormName = "Форма 1", DocumentVid = BookmarksPdf.Enums.DocumentVidEnum.MultipleWorkshops });
            //configurationFile.Forms.Add(new FormDocums { DocumentType = "МК", Standard = "ГОСТ 3.1118-82", FormName = "Форма 1б", DocumentVid = BookmarksPdf.Enums.DocumentVidEnum.MultipleWorkshops });


            //Area standardAndFormArea = new Area();
            //standardAndFormArea.TopLeft = new Coordinates(240.15, 208.3);
            //standardAndFormArea.BottomRight = new Coordinates(291.5, 204.05);

            //Area typeDocumentArea = new Area();
            //typeDocumentArea.TopLeft = new Coordinates(5.5, 8.5);
            //typeDocumentArea.BottomRight = new Coordinates(28.9, 0);


            //GlobalSettings globalSettings = new GlobalSettings();
            //globalSettings.StandardAndFormArea = standardAndFormArea;
            //globalSettings.TypeDocumentArea = typeDocumentArea;
            

            //configurationFile.GlobalSettings = globalSettings;

            //ConfigurationFile.SaveXml<ConfigurationFile>(configurationFile, "D:\\BookmarksPdf.xml");



            ////// Загружаем настроечный файл
            var configFile2 = ConfigurationFile.ReadXml<ConfigurationFile>("BookmarksPdf.xml");

            //////Находим Тип, Стандарт, Форма
            //HelperPdf helperPdf = new HelperPdf();
            //helperPdf.ExtractTextWithCoordinatesNew("D:\\ИТП без закладок.pdf", configFile2);

            //helperPdf.LoadInfoDocuments("ИТП без закладок.pdf", configFile2);
            //HelperPdf helperPdf = new HelperPdf();
            //helperPdf.LoadInfoListITextSharp("ИТП без закладок.pdf", configFile2);
            //helperPdf.LoadInfoListITextSharp2("ИТП без закладок.pdf");
            //Создаем закладку
            //helperPdf.CreateBookmarks();

            PdfHelper helperPdf = new PdfHelper();
            helperPdf.LoadInfoDocuments("D:\\ИТП без закладок.pdf", configFile2);

        }
    }
}
