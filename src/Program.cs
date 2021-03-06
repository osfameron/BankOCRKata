using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BankOCR
{
    public class OCR
    {
        public string Source { get; }
        public OCRDigit[] Digits { get; }

        public OCR (string source)
        {
            Source = source;
 
            var chunks = Chunk(source);
            Digits = chunks.Select(c => new OCRDigit(c)).ToArray();  
        }

        public OCR (int value)
        {
            string s = value.ToString();
            Source = $"From int {s}";
            Digits = s.Select(d => new OCRDigit(Int32.Parse(d.ToString()))).ToArray(); 
        }

        public OCR (int[] digits)
        {
            string s = String.Concat(digits.Select(d => d.ToString()).ToArray());
            Source = $"From int {s}";
            Digits = digits.Select(d => new OCRDigit(d)).ToArray(); 
        }

        public override string ToString()
            => String.Concat(Digits.Select(d => d.ToString()).ToArray());

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

        // Monadic Sequence
        public static int[][] Sequence(int[][] xs)
        {
            return xs.Aggregate(new int[][] {new int[] {}}, (int[][] aa, int[] bb) =>     
                (from a in aa
                from b in bb
                select a.Append(b).ToArray()).ToArray()
            );
        }

        public bool Checksum()
        {
            int[] checks = Digits.Reverse()
                        .Select(d => d.Value)
                        .Zip(Enumerable.Range(1,9))
                        .Select(x => x.Item1.Value * x.Item2)
                        .ToArray();
            return checks.Sum() % 11 == 0;
        }

        public string Check()
        {
            if (Digits.Any(c => ! c.Value.HasValue))
            {
                return ToString() + " ILL";
            }
            if (Checksum())
            {
                return ToString();
            }
            else
            {
                return ToString() + " ERR";
            }
        }
        public string[] CheckWithCorrections()
        {
            int[][] dd = Digits.Select(
                            d => d.Value.HasValue ? new int[] {d.Value.Value} : d.Perturb())
                        .ToArray();
            
            return (from d in Sequence(dd)
                   let ocr = new OCR(d)
                   where ocr.Checksum()
                   select ocr.ToString()).ToArray();
        }

    }

    public class OCRDigit {

        public static string rawDigits = @"
 _     _  _     _  _  _  _  _ 
| |  | _| _||_||_ |_   ||_||_|
|_|  ||_  _|  | _||_|  ||_| _|";

        public static string[] digits = OCR.Chunk(rawDigits).ToArray();
        public static Dictionary<int, int> bitDictionary =
            digits
                .Select((s, i) => (s, i))
                .ToDictionary(
                    x => ToBitInt(ToBitArray(x.Item1)), x => x.Item2);

        public string Source { get; }
        public int? Value { get; }
        private BitArray bitArray;

        public OCRDigit(string source, int value)
        {
            Source = source;
            bitArray = ToBitArray(source);
            Value = value;
        }

        public OCRDigit(int value)
        {
            Source = digits[value];
            bitArray = ToBitArray(Source);
            Value = value;
        }

        public OCRDigit(string source)
        {
            Source = source;
            bitArray = ToBitArray(source);
            int v;
            if (bitDictionary.TryGetValue(ToBitInt(bitArray), out v))
            {
                Value = v;
            }
        }

        public override String ToString()
            => Value.HasValue ?
                      Value.ToString() : "?";

        public static char[] ToBitChars(string src)
        {
            // TODO refactor
            string[] lines = src.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);  

            char[] chars = lines[1].ToCharArray()
                                .Concat(lines[2].ToCharArray())
                                .Prepend(lines[0].ToCharArray()[1])
                                .ToArray();

            return chars;    
        }
        public static bool[] ToBits(string src)
        {
            char[] chars = ToBitChars(src);
            bool[] bools =
                (from c in chars
                 select c == ' ' ? false : true)
                .ToArray();
            return bools;
        }

        public static BitArray ToBitArray(string src)
            => new BitArray(ToBits(src));

        public static int ToBitInt(BitArray b)
        {
            int[] i = new int[1];
            b.CopyTo(i, 0);
            return i[0];
        }

        public static int[] ReadBitDigit(BitArray b)
        {
            int i = ToBitInt(b);

            if (bitDictionary.ContainsKey(i))
            {
                return new int[] {bitDictionary[i]};
            }
            else
            {
                return new int[] {};
            }
        }
            
        public int[] Perturb()
        {
            var b = bitArray;
            int[] ret = ReadBitDigit(b);

            for (var pos = new BitArray(new bool[] {true,false,false,false,false,false,false});
                 ToBitInt(pos) > 0;
                 pos.LeftShift(1))
            {
                b.Xor(pos);
                ret = ret.Concat(ReadBitDigit(b)).ToArray();
                b.Xor(pos); // annoyingly Xor is destructive
            }
            return ret;
        }
    }
}
