using NUnit.Framework;
using BankOCR;
using System;

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
            
            Assert.AreEqual(OCR.digits[0], chunkedZeros[1]);
        }
        [Test]
        public void DigitsDictTest()
        {
            Assert.AreEqual(0, OCR.digitsDictionary[OCR.digits[0]]);
            Assert.AreEqual(3, OCR.digitsDictionary[OCR.digits[3]]);
            Assert.AreEqual(9, OCR.digitsDictionary[OCR.digits[9]]);
        }

        [Test]
        public void ReadDigitsTest()
        {
            Assert.AreEqual("0123456789", OCR.ReadDigits(OCR.rawDigits));
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
    }

        [Test]
        public void CheckSumTest()
        {
            Assert.True(OCR.Checksum("457508000"));
            Assert.False(OCR.Checksum("457508001"));
        }
    }
}
