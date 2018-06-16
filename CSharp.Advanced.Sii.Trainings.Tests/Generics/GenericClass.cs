namespace CSharp.Advanced.Sii.Trainings.Tests.Generics
{
    using System;

    public class GenericClass<T>
    {
        private readonly T _param;

        public GenericClass(T param)
        {
            this._param = param;
        }

        public void Write()
        {
            Console.WriteLine(this._param);
        }
    }
}