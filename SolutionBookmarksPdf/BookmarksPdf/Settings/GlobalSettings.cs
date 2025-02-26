using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarksPdf.Settings
{
    /// <summary>
    /// Класс для хранения общих настроек
    /// </summary>
    public class GlobalSettings
    {
        public Area StandardAndFormArea { get; set; } // Координаты области стандарта и формы листа
        public Area TypeDocumentArea { get; set; } // Координаты области стандарта и формы листа
        //public List<int> ExcludedWorkshops { get; set; } // Цеха, которые не нужно выводить отдельно
        //public List<string> WorkshopPairs { get; set; } // Пары цехов, которые не нужно разделять

        public GlobalSettings()
        {
            //ExcludedWorkshops = new List<int>();
            //WorkshopPairs = new List<string>();
        }
    }

}
