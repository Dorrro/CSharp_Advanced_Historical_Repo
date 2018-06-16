namespace CSharp.Advanced.Sii.Trainings.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Generics;
    using Xunit;

    public class _5_AdvancedExtensionsTests
    {
        [Fact]
        public void FirstOrDefault_Should_HaveCustomDefaultValue()
        {
            var typs = new List<Typ>();

            typs.FirstOrDefault(t => t.Id == 2, new Typ())
                .Id.Should()
                .Be(0);
        }

        [Fact]
        public void WhenThereAreMoreElementsThanNeeded_Should_ReturnOnlyNeededElements()
        {
            var enumerable = Enumerable.Range(1, 5);

            var take = enumerable.MakeSureToTake(3);

            take.Count()
                .Should()
                .Be(3);
        }

        [Fact]
        public void WhenThereAreLessElementsThanNeeded_Should_ThrowException()
        {
            var enumerable = Enumerable.Range(1, 2);

            Action a = () => enumerable.MakeSureToTake(3);

            a.Should()
                .NotThrow<ArgumentException>();
        }
    }
}