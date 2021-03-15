using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitFileIntoSmallChunks
{
    // Reference - https://stackoverflow.com/questions/51310470/splitting-zipfile-into-multiple-small-chunks-and-combining-chunks-again-to-creat

    class Program
    {
        static void Main(string[] args)
        {
            string InputFile = $@"D:\Temp\SplitZipFile\TempFile.zip";
            int chunkSize = 100000;
            string path = $@"D:\Temp\SplitZipFile\Output\";
            SplitFile(InputFile, chunkSize, path);

            string NewCombinedFile = $@"D:\Temp\SplitZipFile\TempFile_Combined.zip";
            Combine(path, NewCombinedFile);

        }
        //310549737 input length

        public static void SplitFile(string inputFile, int chunkSize, string path)
        {
            const int BUFFER_SIZE = 20 * 1024;
            byte[] buffer = new byte[BUFFER_SIZE];

            using (Stream input = File.OpenRead(inputFile))
            {
                int index = 0;
                while (input.Position < input.Length)
                {
                    using (Stream output = File.Create(Path.Combine(path, index.ToString().PadLeft(15, '0'))))
                    {
                        int remaining = chunkSize, bytesRead;
                        while (remaining > 0 && (bytesRead = input.Read(buffer, 0,
                                Math.Min(remaining, BUFFER_SIZE))) > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                            remaining -= bytesRead;
                        }
                    }
                    index++;
                }
            }
        }

        public static void Combine(string directoryPath, string fullFileName)
        {
            if (File.Exists(fullFileName))
                File.Delete(fullFileName);

            using (Stream output = File.Create(fullFileName))
                foreach (string file in Directory.GetFiles(directoryPath).OrderBy(x => x))
                {
                    Console.WriteLine(file);
                    using (Stream input = File.OpenRead(file))
                        input.CopyTo(output);
                }
                    
            
            foreach (string file in Directory.EnumerateFiles(directoryPath))
                System.IO.File.Delete(file);
        }
    }
}
