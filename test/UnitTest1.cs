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
        public void ReadDigitsTest()
        {
            Assert.AreEqual("0123456789", new OCR(@"
 _     _  _     _  _  _  _  _ 
| |  | _| _||_||_ |_   ||_||_|
|_|  ||_  _|  | _||_|  ||_| _|").ToString());

            /*
            Assert.AreEqual("000000000", OCR.ReadDigits(@"
 _  _  _  _  _  _  _  _  _ 
| || || || || || || || || |
|_||_||_||_||_||_||_||_||_|"));
            Assert.AreEqual("111111111", OCR.ReadDigits(@"
                           
  |  |  |  |  |  |  |  |  |
  |  |  |  |  |  |  |  |  |"));
            Assert.AreEqual("222222222", OCR.ReadDigits(@"
 _  _  _  _  _  _  _  _  _ 
 _| _| _| _| _| _| _| _| _|
|_ |_ |_ |_ |_ |_ |_ |_ |_ "));
            Assert.AreEqual("333333333", OCR.ReadDigits(@"
 _  _  _  _  _  _  _  _  _ 
 _| _| _| _| _| _| _| _| _|
 _| _| _| _| _| _| _| _| _|"));
            Assert.AreEqual("444444444", OCR.ReadDigits(@"
                           
|_||_||_||_||_||_||_||_||_|
  |  |  |  |  |  |  |  |  |"));
            Assert.AreEqual("555555555", OCR.ReadDigits(@"
 _  _  _  _  _  _  _  _  _ 
|_ |_ |_ |_ |_ |_ |_ |_ |_ 
 _| _| _| _| _| _| _| _| _|"));
            Assert.AreEqual("666666666", OCR.ReadDigits(@"
 _  _  _  _  _  _  _  _  _ 
|_ |_ |_ |_ |_ |_ |_ |_ |_ 
|_||_||_||_||_||_||_||_||_|"));
            Assert.AreEqual("777777777", OCR.ReadDigits(@"
 _  _  _  _  _  _  _  _  _ 
  |  |  |  |  |  |  |  |  |
  |  |  |  |  |  |  |  |  |"));
            Assert.AreEqual("888888888", OCR.ReadDigits(@"
 _  _  _  _  _  _  _  _  _ 
|_||_||_||_||_||_||_||_||_|
|_||_||_||_||_||_||_||_||_|"));
            Assert.AreEqual("999999999", OCR.ReadDigits(@"
 _  _  _  _  _  _  _  _  _ 
|_||_||_||_||_||_||_||_||_|
 _| _| _| _| _| _| _| _| _|"));
            Assert.AreEqual("000000051", OCR.ReadDigits(@"
 _  _  _  _  _  _  _  _    
| || || || || || || ||_   |
|_||_||_||_||_||_||_| _|  |"));
            */
        }
        /*

        [Test]
        public void CheckSumTest()
        {
            Assert.True(OCR.Checksum("457508000"));
            Assert.False(OCR.Checksum("457508001"));
        }

        [Test]
        public void CheckTest()
        {
            Assert.AreEqual("000000051", OCR.Check(@"
 _  _  _  _  _  _  _  _    
| || || || || || || ||_   |
|_||_||_||_||_||_||_| _|  |"));
            Assert.AreEqual("000000851 ERR", OCR.Check(@"
 _  _  _  _  _  _  _  _    
| || || || || || ||_||_   |
|_||_||_||_||_||_||_| _|  |"));
            Assert.AreEqual("1234?678? ILL", OCR.Check(@"
    _  _     _  _  _  _  _ 
  | _| _||_| _ |_   ||_||_|
  ||_  _|  | _||_|  ||_| _ "));
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
            char[] chars = {'_','|',' ','|',' ','_',' '};
            Assert.AreEqual(chars, OCR.ToBitChars(shape));
            Assert.AreEqual(bits, OCR.ToBits(shape));
            Assert.AreEqual(new BitArray(bits), OCR.ToBitArray(shape));
            Assert.AreEqual(43, OCR.ToBitInt(OCR.ToBitArray(shape)));
        }

        [Test]
        public void TestPerturb() {

            Assert.AreEqual(new int[] {8, 0, 6, 9}, OCR.Perturb(OCR.digits[8]));
        }
        */
    }

}
