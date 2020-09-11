using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using System;
using System.IO;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            int a = 10, b = 20;
            int answer = a + b;
            Assert.Equal(30, answer);
        }

        [Theory]
        [InlineData(2, 2, 4)]
        [InlineData(10, 5, 15)]
        [InlineData(12, 12, 24)]
        public void CanAdd(int a, int b, int expected)
        {
            int answer = a + b;

            Assert.Equal(expected, answer);
        }
    }
}
