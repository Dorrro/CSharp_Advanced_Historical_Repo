namespace CSharp.Advanced.Sii.Trainings.Tests.Reflection
{
    using System;

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