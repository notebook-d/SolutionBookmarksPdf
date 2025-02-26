using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace BookmarksPdf.Settings
{
    /// <summary>
    /// Класс, представляющий настроечный файл
    /// </summary>
    [XmlRoot("Config")]
    public class ConfigurationFile
    {
        [XmlArray("Forms")]
        [XmlArrayItem("Form")]
        public List<FormDocums> Forms { get; set; }        
        public GlobalSettings GlobalSettings { get; set; }

        public ConfigurationFile()
        {
            Forms = new List<FormDocums>();
            GlobalSettings = new GlobalSettings();
        }

        #region Serilization\Deserilization
        ///<summary>
        ///Записываем в XML
        ///</summary>
        public static void SaveXml<T>(T xmlObject, string puthfile)
        {
            var writer = new XmlSerializer(typeof(T));
            var file = new StreamWriter(puthfile);
            writer.Serialize(file, xmlObject);
            file.Close();
        }

        ///<summary>
        ///Читаем из XML
        ///</summary>
        public static T ReadXml<T>(string puthfile)
        {
            var reader = new XmlSerializer(typeof(T));
            var obj = default(T);
            try
            {
                var file = new StreamReader(puthfile);
                obj = (T)reader.Deserialize(file);
                file.Close();
            }
            catch
            {
                return obj;
            }
            return obj;
        }

        #endregion

    }
}
