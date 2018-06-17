namespace CSharp.Advanced.Sii.Trainings.Tests.Reflection
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
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
}