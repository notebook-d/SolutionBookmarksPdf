using PdfTools.Interface;
using PdfTools.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools
{
    public class SingleWorkshopFormDocums: IFormDocument
    {
        public FormDocums FormDocums { get; set; }
        public int NumberPage { get; set; }
        public string NumberOper { get; set; }
        public string NumberCeh { get; set; }        

        public SingleWorkshopFormDocums(FormDocums formDocums, int numberPage, string numberCeh, string numberOper) 
        {
            FormDocums = formDocums; 
            NumberOper = numberOper;
            NumberCeh = numberCeh;  
            NumberPage = numberPage;
        }
        public override string ToString()
        {
            return $"Номер страницы={NumberPage} {FormDocums}|{NumberCeh}|{NumberOper}";            
        }

    }
}
