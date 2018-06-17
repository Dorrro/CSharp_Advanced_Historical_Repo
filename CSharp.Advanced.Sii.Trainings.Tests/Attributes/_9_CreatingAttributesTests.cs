namespace CSharp.Advanced.Sii.Trainings.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using FluentAssertions;
    using Xunit;

    public class ClassToBeFiltered
    {
        public int Test1 { get; set; }
        public int Test2 { get; set; }

        [NonFilterable]
        public int Test3 { get; set; }
    }

    public class NonFilterableAttribute : Attribute
    { }

    public enum FilterTypes
    {
        Gt,
        Lt,
        Contains
    }

    public class Filter
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public FilterTypes FilterType { get; set; }

        public Request Request { get; set; }
    }

    public class Request
    {
        public List<Filter> Filters { get; set; }

        public LogicalOperator LogicalOperator { get; set; }
    }

    public enum LogicalOperator
    {
        Or,
        And
    }

    public class _10_UsingAttributes
    {
        [Fact]
        public void MapToViewModelExample()
        {
            var entities = new List<IEntity> {new Model1 {Id = 1}, new Model2 {Id = 2}};
            var list = entities.Select(e => new
                                            {
                                                e.GetType()
                                                    .GetCustomAttribute<MapToViewModelAttribute>()
                                                    .Type,
                                                Entity = e
                                            })
                .Select(a => this.Map(a.Entity, a.Type))
                .ToList();

            list.Should()
                .BeEquivalentTo(new List<IEntity> {new ViewModel2 {Id = 2}, new ViewModel1 {Id = 1}});
        }

        [Fact]
        public void GetDisplayProperty()
        {
            var dictionary = typeof(KlasaZWykorzystaniemDisplay)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(p => p.Name, p => p.GetCustomAttribute<DisplayAttribute>()
                                               .Name);

            dictionary["User"]
                .Should()
                .Be("Użytkownik");
            // 1. stworzyć atrybut Display
            // [Display("Użytkownik")]
            // public string User {get;set;}

            // 2. rezultat = słownik, gdzie klucz = nazwa property, wartość = wartość atrybuty Display
        }

        private IEntity Map(IEntity entity, Type type)
        {
            var viewModel = Activator.CreateInstance(type) as IEntity;
            if (viewModel != null) viewModel.Id = entity.Id;
            return viewModel;
        }

        class KlasaZWykorzystaniemDisplay
        {
            [Display("Użytkownik")]
            public string User { get; set; }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal class DisplayAttribute : Attribute
    {
        public string Name { get; }

        public DisplayAttribute(string name)
        {
            this.Name = name;
        }
    }

    [MapToViewModel(typeof(ViewModel2))]
    public class Model2 : IEntity
    {
        public int Id { get; set; }
    }

    public class MapToViewModelAttribute : Attribute
    {
        public Type Type { get; }

        public MapToViewModelAttribute(Type type)
        {
            this.Type = type;
        }
    }

    public class ViewModel2 : IEntity
    {
        public int Id { get; set; }
    }

    [MapToViewModel(typeof(ViewModel1))]
    public class Model1 : IEntity
    {
        public int Id { get; set; }
    }

    public class ViewModel1 : IEntity
    {
        public int Id { get; set; }
    }

    public interface IEntity
    {
        int Id { get; set; }
    }

    public class _9_CreatingAttributesTests
    {
//        [Atrybut]
        public void TestMethod()
        { }

        [Fact]
        public void Example_1()
        {
            this.LogMessage("asd");
        }

        void LogMessage(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Console.WriteLine(message);
            Console.WriteLine(memberName);
            Console.WriteLine(filePath);
            Console.WriteLine(lineNumber);
        }

        [Atrybut]
//        [Atrybut] - wiele atrybutów - tylko przy AllowMultiple = true
//        [Atrybut]
//        [Atrybut]
//        [Atrybut]
//        [Atrybut]
//        [Atrybut]
//        [Atrybut]
        class Test
        { }
    }

    [Obsolete("Metoda zostanie usunięta 26.06.2018")]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class AtrybutAttribute : Attribute
    { }
}