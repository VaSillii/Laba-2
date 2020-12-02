using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_2.Base
{
    public class BaseInicialize
    {
        public static readonly string nameSaveFile = "data.json"; 
        public static readonly string url = "https://bdu.fstec.ru/files/documents/thrlist.xlsx";
        public static readonly string basePathSave = Directory.GetCurrentDirectory(); 

        public static string GetFullNameFileExcel() => Path.Combine(BaseInicialize.basePathSave, BaseInicialize.nameSaveFile);
    }
}
