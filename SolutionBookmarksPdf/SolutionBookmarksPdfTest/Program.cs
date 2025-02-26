using BookmarksPdf;
using BookmarksPdf.Settings;
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

            ConfigurationFile configurationFile = new ConfigurationFile();

            configurationFile.Forms.Add(new FormDocums { DocumentType = "МК", Standard = "ГОСТ 3.1118-82", FormName = "Форма 1" });
            configurationFile.Forms.Add(new FormDocums { DocumentType = "МК", Standard = "ГОСТ 3.1118-82", FormName = "Форма 1б" });


            Area standardAndFormArea = new Area();
            standardAndFormArea.TopLeft = new Coordinates(240.15, 208.3);
            standardAndFormArea.BottomRight = new Coordinates(291.5, 204.05);

            Area typeDocumentArea = new Area();
            typeDocumentArea.TopLeft = new Coordinates(5.5, 8.5);
            typeDocumentArea.BottomRight = new Coordinates(28.9, 0);


            GlobalSettings globalSettings = new GlobalSettings();
            globalSettings.StandardAndFormArea = standardAndFormArea;
            globalSettings.TypeDocumentArea = typeDocumentArea;
            

            configurationFile.GlobalSettings = globalSettings;

            ConfigurationFile.SaveXml<ConfigurationFile>(configurationFile, "D:\\BookmarksPdf.xml");



            //// Загружаем настроечный файл
            var configFile2 = ConfigurationFile.ReadXml<ConfigurationFile>("BookmarksPdf.xml");

            ////Находим Тип, Стандарт, Форма
            HelperPdf helperPdf = new HelperPdf();
            helperPdf.LoadInfoDocuments("ИТП без закладок.pdf", configFile2);

        }
    }
}
