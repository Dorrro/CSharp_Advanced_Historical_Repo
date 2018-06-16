namespace CSharp.Advanced.Sii.Trainings.Tests.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using Extensions;
    using FluentAssertions;
    using Xunit;

    public class _7_CreatingDynamicTypes
    {
        [Fact]
        public void DefiningClass()
        {
            var typeBuilder = CreateTypeBuilder();
            var instance = GetInstance(typeBuilder);

            ((string)instance.ToString())
                .Should()
                .Be("Nowa klasa");
        }

        [Fact]
        public void DefiningFields()
        {
            var typeBuilder = CreateTypeBuilder();
            typeBuilder.DefineField("test", typeof(string), FieldAttributes.Public);

            var instance = GetInstance(typeBuilder);
            instance.test = "abc";

            ((string)instance.test).Should()
                .Be("abc");
        }

        [Fact]
        public void DefiningProperty()
        {
            var typeBuilder = CreateTypeBuilder();
            var field = typeBuilder.DefineField("test", typeof(string), FieldAttributes.Private);

            var propertyBuilder = typeBuilder
                .DefineProperty(
                    "Test",
                    PropertyAttributes.None,
                    typeof(string),
                    Type.EmptyTypes);

            var getMethodBuilder = typeBuilder
                .DefineMethod(
                    "get_Test",
                    MethodAttributes.Public | 
                    MethodAttributes.SpecialName | 
                    MethodAttributes.HideBySig, 
                    typeof(string), 
                    Type.EmptyTypes);
            var testGetIl = getMethodBuilder.GetILGenerator();

            testGetIl.Emit(OpCodes.Ldarg_0);
            testGetIl.Emit(OpCodes.Ldfld, field);
            testGetIl.Emit(OpCodes.Ret);

            var setMethodBuilder = typeBuilder
                .DefineMethod(
                    "set_Test",
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    returnType: null,
                    parameterTypes: new [] {typeof(string)});
            var setGetIl = setMethodBuilder.GetILGenerator();

            setGetIl.Emit(OpCodes.Ldarg_0);
            setGetIl.Emit(OpCodes.Ldarg_1);
            setGetIl.Emit(OpCodes.Stfld, field);
            setGetIl.Emit(OpCodes.Ret);
            propertyBuilder.SetSetMethod(setMethodBuilder);
            propertyBuilder.SetGetMethod(getMethodBuilder);

            var instance = GetInstance(typeBuilder);
            instance.Test = "123";

            var test = instance.Test;
        }

        [Fact]
        public void DIY_1()
        {
            // var instance = this.CreateType("test", new Dictionary<string, Type> { {"Test", typeof(string)}, {"Test2", typeof(TestClass)}});
            // instance.Test = "dasdsa";
            // instance.Test2 = new TestClass {Id = 1};
        }

        private static dynamic GetInstance(TypeBuilder typeBuilder)
        {
            var type = typeBuilder.CreateType();
            var instance = Activator.CreateInstance(type);
            return instance;
        }

        private static TypeBuilder CreateTypeBuilder()
        {
            var assemblyName = new AssemblyName("RuntimeAssembly");
            var assemblyBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeBuilder = moduleBuilder.DefineType(
                "Nowa klasa", TypeAttributes.Class |
                              TypeAttributes.Public);
            return typeBuilder;
        }
    }

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

    public class Refl2
    {
        public string Output()
        {
            return "Test-Output";
        }

        public int AddInts(int i1, int i2)
        {
            return i1 + i2;
        }

        public string TonysLastName()
        {
            return "Stark";
        }
    }

    public class Refl1
    {
        public string Output()
        {
            return "Test-Output";
        }

        public int AddInts(int i1, int i2)
        {
            return i1 + i2;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(new Refl1().Output());
            Console.WriteLine(new Refl1().AddInts(1, 2));
        }
    }
}