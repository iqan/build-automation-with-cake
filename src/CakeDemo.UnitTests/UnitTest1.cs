using System;
using CakeDemo.Lib;
using FluentAssertions;
using Xunit;

namespace CakeDemo.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var result = Class1.GetSomething();
            result.Should().Be("xD");
        }
    }
}
