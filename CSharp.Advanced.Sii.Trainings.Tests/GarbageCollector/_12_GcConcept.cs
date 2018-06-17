namespace CSharp.Advanced.Sii.Trainings.Tests.GarbageCollector
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xunit;
    using Xunit.Sdk;

    public class _12_GcConcept
    {
        [Fact]
        public void Ex_1()
        {
            var a = new Test();
            var b = new Test();
            a = b; // b jest przypisany do => a i b wskazują na to samo miejsce w pamięci
            // miejsce w pamięci na które wskazywał a pozostało bez mocnej więzi
            // GC kiedy się uruchomi spróbuje zwolnić pamięć
        }

        [Fact]
        public void Ex_2()
        {
            var klasaZDestruktorem = new KlasaZDestruktorem();
        }

        [Fact]
        public void Ex_3()
        {
            using (var klasaDisposable = new KlasaDisposable())
            {

            }
        }
    }

    public class _13_GcCollectTests
    {
//        [Fact]
//        public void Ex_1()
//        {
//            var streamWriter = File.CreateText("plik.txt");
//            streamWriter.Write("hello!");
//
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//
//            File.Delete("plik.txt");
//        }

        [Fact]
        public void Ex_2()
        {
            new MyClass();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Delete("plikt.txt");
        }

        [Fact]
        public void WeakReference()
        {
            var myClass = new MyClass();
            var weakReference = new WeakReference(myClass);
            weakReference.IsAlive.Should()
                .BeTrue();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            weakReference.IsAlive.Should()
                .BeFalse();
        }
    }

    public class MyClass
    {
        public MyClass()
        {
            var streamWriter = File.CreateText("plik.txt");
            streamWriter.Write("hello!");
        }
    }

    public class KlasaDisposable : IDisposable
    {
        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~KlasaDisposable()
        {
            ReleaseUnmanagedResources();
        }
    }

    public class KlasaZDestruktorem
    {
        ~KlasaZDestruktorem()
        {
            // w tym miejscu robimy dispose połączenia bazodanowego
        }
    }

    public class Test
    { }
}
