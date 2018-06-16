namespace CSharp.Advanced.Sii.Trainings.Tests.Extensions
{
    using System;

    public static class IntExtensions
    {
        public static double Power(this int number, int to = 2)
        {
            return Math.Pow(number, to);
        }
    }
}