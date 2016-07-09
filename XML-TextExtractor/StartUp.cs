using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace XML_TextExtractor
{
    public class StartUp
    {
        public static Regex  textFilter = new Regex(@"<.*>(.*)<.*>");
        public static void Main(string[] args)
        {
            Console.WriteLine("TYPE TARGET DIRECTORY:");
            string inputPath = Console.ReadLine();
            
            ProcessFiles(inputPath);
        }

        private static void ProcessFiles(string inputPath)
        {
            string[] fileNames = Directory.GetFiles(inputPath);
           
            foreach (var filepath in fileNames)
            {
                Queue<string> extractedText = new Queue<string>();
                if (filepath.EndsWith(".xml"))
                {
                    string newFilePath = filepath.Replace(".xml", ".txt");
                    using (StreamReader reader = new StreamReader(filepath))
                    {
                        string line = string.Empty;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (textFilter.IsMatch(line))
                            {
                                Match matcher = textFilter.Match(line);
                                extractedText.Enqueue(matcher.Groups[1].Value);
                            }
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(newFilePath))
                    {
                        while (extractedText.Count > 0)
                        {
                            string line = extractedText.Dequeue();
                            writer.WriteLine(line);
                        }
                        Console.WriteLine("Writing to file completed!");

                    }
                }
                
            }
        }
    }
}
