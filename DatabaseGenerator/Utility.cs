using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGenerator
{

    public static class Utility
    {

        public static void DirectoryDeleteIfExists(string path, bool recursive)
        {
            var di = new DirectoryInfo(path);

            if (di.Exists)
            {
                di.Delete(recursive);
            }
        }

        public static void DeleteFiles(string folderPath, string searchPattern, bool resursively)
        {
            Directory
               .GetFiles(folderPath, searchPattern, new EnumerationOptions() { RecurseSubdirectories = resursively })
               .ToList()
               .ForEach(x => File.Delete(x));
        }


        //public static void DeleteFilesRecursively(string folderPath, string searchPattern)
        //{
        //    Directory
        //       .GetFiles(folderPath, searchPattern, new EnumerationOptions() { RecurseSubdirectories = true })
        //       .ToList()
        //       .ForEach(x => File.Delete(x));
        //}

    }

}
