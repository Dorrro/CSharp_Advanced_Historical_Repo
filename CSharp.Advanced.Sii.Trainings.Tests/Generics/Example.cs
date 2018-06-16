namespace CSharp.Advanced.Sii.Trainings.Tests.Generics
{
    using System;

    public class Example
    {
        public void Write<T>(T text)
        {
            Console.WriteLine(text);
        }
    }
}