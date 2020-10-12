using FvpWebApp.Infrastructure;
using System;
using Xunit;

namespace FvpWebAppTests
{
    public class RandomValueTest
    {
        [Fact]
        public void EncodingDecodingTest()
        {
            var randomString = RandomValues.RandomString(20);
            Console.WriteLine($"RandomValueTest - RandomString : {randomString}");
            Assert.Equal(20, randomString.Length);
        }
    }
}