namespace CSharp.Advanced.Sii.Trainings.Tests.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using FluentAssertions;
    using Xunit;

    public class _6_ExtractingDataTests
    {
        [Fact]
        public void GetTypesOfObjects()
        {
            var objects = new List<object> {"a", 1};
            var types = this.GetTypes(objects);

            types[0]
                .Should()
                .Be(typeof(string));
            types[1]
                .Should()
                .Be(typeof(int));
        }

        [Fact]
        public void ExtractMethodNames()
        {
            var refl1 = new Refl1();
            var methodInfos = refl1.GetType()
                .GetMethods(
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static)
                .Select(m => m.Name)
                .ToList();

            methodInfos.Should()
                .Contain(new List<string> {"Main", "AddInts", "Output"});
        }

        [Fact]
        public void AddMethodResults_Which_ReturnsString()
        {
            var refl2 = new Refl2();
            var result = refl2
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => m.ReturnType == typeof(string))
                .Select(m => (string)m.Invoke(refl2, null))
                .OrderByDescending(r => r.Length)
                .ToList()
                .Join("");

            result.Should()
                .Be("Test-OutputStark");
        }

        private List<Type> GetTypes(List<object> objects)
        {
            return objects.Select(t => t.GetType())
                .ToList();
        }
    }
}