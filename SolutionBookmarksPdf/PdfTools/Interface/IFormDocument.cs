using PdfTools.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools.Interface
{
    public interface IFormDocument
    {
        int NumberPage { get; set; }
        FormDocums FormDocums { get; set; }
    }
}
