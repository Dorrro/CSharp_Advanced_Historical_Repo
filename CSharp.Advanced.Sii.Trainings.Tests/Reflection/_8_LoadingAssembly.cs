namespace CSharp.Advanced.Sii.Trainings.Tests.Reflection
{
    using System;
    using System.Reflection;
    using Xunit;

    public class _8_LoadingAssembly
    {
        [Fact]
        public void Showtime()
        {
            var assembly = Assembly.LoadFile(
                @"C:\Users\dsobackx\Source\Repos\CSharp.Advanced.Sii.Trainings\CSharp.Advanced.Sii.Trainings.AssemblyToLoad\bin\Debug\CSharp.Advanced.Sii.Trainings.AssemblyToLoad.dll");
            var type = assembly.GetType("CSharp.Advanced.Sii.Trainings.AssemblyToLoad.Class1");
            var instance = (dynamic)Activator.CreateInstance(type);

            instance.Id = 1;
        }

        [Fact]
        public void DIY_1()
        {
            // load all assemblies from folder X
            // get all types which are subclasses of interface IWriter
            // execute all the Write methods
        }
    }
}