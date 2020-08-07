using NUnit.Framework;
using BankOCR;
using System.Collections;

namespace BankOCRTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        static string zero = @"
 _ 
| |
|_|";
        static string zeros = @"
 _  _ 
| || |
|_||_|";

        [Test]
        public void BasicChunkTest()
        {
            var chunkedZeros = OCR.Chunk(zeros);
            Assert.AreEqual(zero, "\n" + chunkedZeros[0]);
            Assert.AreEqual(zero, "\n" + chunkedZeros[1]);
            
            Assert.AreEqual(OCRDigit.digits[0], chunkedZeros[1]);
        }

        [Test]
        public void NewOCRTest()
        {
            Assert.AreEqual("0123456789", new OCR(@"
 _     _  _     _  _  _  _  _ 
| |  | _| _||_||_ |_   ||_||_|
|_|  ||_  _|  | _||_|  ||_| _|").ToString());

            Assert.AreEqual("000000000", new OCR(@"
 _  _  _  _  _  _  _  _  _ 
| || || || || || || || || |
|_||_||_||_||_||_||_||_||_|").ToString());
            Assert.AreEqual("111111111", new OCR(@"
                           
  |  |  |  |  |  |  |  |  |
  |  |  |  |  |  |  |  |  |").ToString());
            Assert.AreEqual("222222222", new OCR(@"
 _  _  _  _  _  _  _  _  _ 
 _| _| _| _| _| _| _| _| _|
|_ |_ |_ |_ |_ |_ |_ |_ |_ ").ToString());
            Assert.AreEqual("333333333", new OCR(@"
 _  _  _  _  _  _  _  _  _ 
 _| _| _| _| _| _| _| _| _|
 _| _| _| _| _| _| _| _| _|").ToString());
            Assert.AreEqual("444444444", new OCR(@"
                           
|_||_||_||_||_||_||_||_||_|
  |  |  |  |  |  |  |  |  |").ToString());
            Assert.AreEqual("555555555", new OCR(@"
 _  _  _  _  _  _  _  _  _ 
|_ |_ |_ |_ |_ |_ |_ |_ |_ 
 _| _| _| _| _| _| _| _| _|").ToString());
            Assert.AreEqual("666666666", new OCR(@"
 _  _  _  _  _  _  _  _  _ 
|_ |_ |_ |_ |_ |_ |_ |_ |_ 
|_||_||_||_||_||_||_||_||_|").ToString());
            Assert.AreEqual("777777777", new OCR(@"
 _  _  _  _  _  _  _  _  _ 
  |  |  |  |  |  |  |  |  |
  |  |  |  |  |  |  |  |  |").ToString());
            Assert.AreEqual("888888888", new OCR(@"
 _  _  _  _  _  _  _  _  _ 
|_||_||_||_||_||_||_||_||_|
|_||_||_||_||_||_||_||_||_|").ToString());
            Assert.AreEqual("999999999", new OCR(@"
 _  _  _  _  _  _  _  _  _ 
|_||_||_||_||_||_||_||_||_|
 _| _| _| _| _| _| _| _| _|").ToString());
            Assert.AreEqual("000000051", new OCR(@"
 _  _  _  _  _  _  _  _    
| || || || || || || ||_   |
|_||_||_||_||_||_||_| _|  |").ToString());
        }


        [Test]
        public void ChecksumTest()
        {
            Assert.True(new OCR(457508000).Checksum());
            Assert.False(new OCR(457508001).Checksum());
        }

        [Test]
        public void CheckTest()
        {
            Assert.AreEqual("000000051", new OCR(@"
 _  _  _  _  _  _  _  _    
| || || || || || || ||_   |
|_||_||_||_||_||_||_| _|  |").Check());

            Assert.AreEqual("000000851 ERR", new OCR(@"
 _  _  _  _  _  _  _  _    
| || || || || || ||_||_   |
|_||_||_||_||_||_||_| _|  |").Check());
            Assert.AreEqual("1234?678? ILL", new OCR(@"
    _  _     _  _  _  _  _ 
  | _| _||_| _ |_   ||_||_|
  ||_  _|  | _||_|  ||_| _ ").Check());
        }


        [Test]
        public void BitArrayTest()
        {
            bool[] bits = {true, 
                           true, false, true,
                           false, true, false};
            string shape = @"
 _ 
| |
 _ ";
            char[] chars = {'_',
                            '|',' ','|',
                            ' ','_',' '};
            Assert.AreEqual(chars, OCRDigit.ToBitChars(shape));
            Assert.AreEqual(bits, OCRDigit.ToBits(shape));
            Assert.AreEqual(new BitArray(bits), OCRDigit.ToBitArray(shape));
            Assert.AreEqual(43, OCRDigit.ToBitInt(OCRDigit.ToBitArray(shape)));
        }

        [Test]
        public void TestPerturb() {

            Assert.AreEqual(new int[] {0, 8}, new OCRDigit(0).Perturb());
            Assert.AreEqual(new int[] {1, 7}, new OCRDigit(1).Perturb());
            Assert.AreEqual(new int[] {2}, new OCRDigit(2).Perturb());
            Assert.AreEqual(new int[] {3, 9}, new OCRDigit(3).Perturb());
            Assert.AreEqual(new int[] {4}, new OCRDigit(4).Perturb());
            Assert.AreEqual(new int[] {5, 9, 6}, new OCRDigit(5).Perturb());
            Assert.AreEqual(new int[] {6, 8, 5}, new OCRDigit(6).Perturb());
            Assert.AreEqual(new int[] {7, 1}, new OCRDigit(7).Perturb());
            Assert.AreEqual(new int[] {8, 0, 6, 9}, new OCRDigit(8).Perturb());
            Assert.AreEqual(new int[] {9, 3, 5, 8}, new OCRDigit(9).Perturb());
        }
    }

}
