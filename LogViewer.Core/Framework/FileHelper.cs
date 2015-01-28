using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.Framework
{
    public static class FileHelper
    {
        public static IEnumerable<String> ReadLines(String path)
        {
            using(var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            {
                using(var sr = new StreamReader(fs, true))
                {
                    while (!sr.EndOfStream)
                    {
                        yield return sr.ReadLine();
                    }
                }
            }
        }


        public static IEnumerable<String> ReadLines(String path, int skip)
        {
            return ReadLines(path).Skip(skip);
        }
    }
}
