namespace CSharp.Advanced.Sii.Trainings.Tests.Generics
{
    using System.Net.Configuration;
    using FluentAssertions;
    using Xunit;


    public class _1_MethodsTests
    {
        [Fact]
        public void Theory()
        {
            var example = new Example();

            example.Write(4);
            example.Write("sadsadsaddsadsa");
        }

        [Fact]
        public void DIY_1()
        {
            GenericMethods.Compare(1, 1).Should().BeTrue();
            GenericMethods.Compare("a", "a").Should().BeTrue();
            GenericMethods.Compare(1d, 1d).Should().BeTrue();
            GenericMethods.Compare("a", "b").Should().BeFalse();

            // nie powinno się kompilować
//            GenericMethods.Compare(1, "a").Should().BeFalse();
        }

        public class GenericMethods
        {
            public static bool Compare<T>(T a, T b)
            {
                return a.Equals(b);
            }
        }
    }
}
