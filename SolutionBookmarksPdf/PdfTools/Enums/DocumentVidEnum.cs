using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools.Enums
{
    public enum DocumentVidEnum
    {
        None,             // Не определен 
        General,          // Общий документ ТП (ТЛ, ЛРИ и т.п.)
        SingleWorkshop,    // Документ с принадлежностью к одному цехозаходу (ОК, КЭ и т.п.)
        MultipleWorkshops // Документ с принадлежностью к нескольким цехозаходам (МК, КТТП, ВТП и т.п.)
    }
}
