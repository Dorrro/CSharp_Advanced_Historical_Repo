namespace CSharp.Advanced.Sii.Trainings.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Xunit;

    public class _4_SimpleExtensionsTests
    {
        [Fact]
        public void MinutesExtMethod_Should_ReturnValueInMinutes()
        {
            var fiveMinutes = 5.Minutes();

            fiveMinutes.Should()
                .Be(TimeSpan.FromMinutes(5));
        }

        [Theory]
        [InlineData(5, 2, 25)]
        [InlineData(5, 3, 125)]
        [InlineData(2, 2, 4)]
        [InlineData(3, 3, 27)]
        public void Power_Should_ReturnPoweredValue(int number, int to, double expected)
        {
            var pow = number.Power(to);

            pow.Should()
                .Be(expected);
        }

        [Fact]
        public void StringJoin_Should_JoinEnumarableOfString_Using_Delimeter()
        {
            var list = new List<string> { "Ala", "ma", "kota" };

//            var result = string.Join(" ", list);
            var result = list.Join(" ");
            result.Should()
                .Be("Ala ma kota");

        }
    }
}