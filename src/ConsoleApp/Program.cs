using System;
using System.Drawing;
using CompactExifLib;
using DotNetCoords;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
            }
            else
            {
                try
                {
                    AddOsGrid(args[0]);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Don't understand (no GPS?, not an image?).");
                }
            }
        }


        /// <summary>
        /// Print a suitable usage message
        /// </summary>
        private static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("AddOSGrid imagefile.jpg");
            Console.WriteLine();
            Console.WriteLine("Renames file to OSXXXXXX_imagefile.jpg and changes EXIF description.");
        }

        /// <summary>
        /// Stick the OS grid reference on the filename and in the EXIF description.
        /// </summary>
        /// <param name="filename"></param>
        private static void AddOsGrid(String filename)
        {
            ExifData TestExif;
            TestExif = new ExifData(filename);
            DateTime DateTaken;
            TestExif.GetTagValue(ExifTag.DateTimeOriginal, out DateTaken);
            Console.WriteLine();
        }
    }
}
