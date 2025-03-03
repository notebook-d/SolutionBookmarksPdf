using PdfTools.Interface;
using PdfTools.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools
{
    public class NoneFormDocums : IFormDocument
    {
        public FormDocums FormDocums { get; set; }
        public int NumberPage { get; set; }

        public NoneFormDocums(FormDocums formDocums, int numberPage) 
        {
            FormDocums = formDocums; 
            NumberPage = numberPage;
        }
        public override string ToString()
        {
            return $"Номер страницы={NumberPage} {FormDocums}";
        }

    }
}
