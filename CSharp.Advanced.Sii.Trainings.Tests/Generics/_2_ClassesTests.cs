namespace CSharp.Advanced.Sii.Trainings.Tests.Generics
{
    using FluentAssertions;
    using Xunit;

    public class _2_ClassesTests
    {
        [Fact]
        public void Theory()
        {
            var genericClass = new GenericClass<string>("sadasd");
            genericClass.Write();
        }


        [Fact]
        public void DIY_1()
        {
            var data = new Data<int>();
            data.Set(1);
            data.Get().Should().Be(1);

            // nie powinno się kompilować
//            data.Set("a");

            var data1 = new Data<string>();
            data1.Set("a");
            data1.Get().Should().Be("a");

            // nie powinno się kompilować
//            data1.Set(1);
        }

        private class Data<T>
        {
            private T _o;

            public void Set(T o)
            {
                this._o = o;
            }

            public T Get()
            {
                return this._o;
            }
        }
    }
}