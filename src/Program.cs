using System;
using System.Collections.Generic;
using System.Linq;

namespace BankOCR
{
    public class OCR
    {
        public static string rawDigits = @"
 _     _  _     _  _  _  _  _ 
| |  | _| _||_||_ |_   ||_||_|
|_|  ||_  _|  | _||_|  ||_| _|";

        public static string[] digits = Chunk(rawDigits).ToArray();

        public static Dictionary<string, int> digitsDictionary =
            digits.Select((s, i) => (s, i)).ToDictionary(x => x.Item1, x => x.Item2);

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public static List<string> Chunk(string src)
        {
            string[] lines = src.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var output = new List<string>();
            while (lines.All(l => l.Length >= 3))
            {
                string head = String.Join('\n', lines.Select(l => l.Substring(0, 3)).ToArray());
                output.Add(head);

                lines = lines.Select(l => l.Substring(3)).ToArray();
            }
            return output;
        }

        public static string ReadDigits(string src)
        {
            var chunks = Chunk(src);
            var ds = chunks.Select(c => digitsDictionary.ContainsKey(c) ?
                                    digitsDictionary[c].ToString() : "?");
            return String.Concat(ds.ToArray());
        }

        public static bool Checksum(string d)
        {
            int[] checks = d.Reverse()
                        .Zip(Enumerable.Range(1,9))
                        .Select(x => Int32.Parse(x.Item1.ToString()) * x.Item2)
                        .ToArray();
            return checks.Sum() % 11 == 0;
        }

        public static string Check(string src)
        {
            string d = ReadDigits(src);
            return d;
        }
    }
}
