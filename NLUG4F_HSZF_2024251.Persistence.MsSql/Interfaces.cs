using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    //IDataImportExportService
    //Leírás: Adatok importálása és exportálása a programból és programba.
    //Metódusok:
    //Task ImportFromJsonAsync(string jsonFilePath): Adatok beolvasása JSON fájlból.
    //Task ExportToTxtAsync(string directoryPath): Terméklista exportálása .txt fájlba, dátum és timestamp formátummal.
    public interface IDataImportExportService
    {

    }
}
