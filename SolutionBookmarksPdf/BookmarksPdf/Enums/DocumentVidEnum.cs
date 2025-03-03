using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarksPdf.Enums
{
    public enum DocumentVidEnum
    {
        General,          // Общий документ ТП (ТЛ, ЛРИ и т.п.)
        SingleWorkshop,    // Документ с принадлежностью к одному цехозаходу (ОК, КЭ и т.п.)
        MultipleWorkshops // Документ с принадлежностью к нескольким цехозаходам (МК, КТТП, ВТП и т.п.)
    }
}
