namespace CSharp.Advanced.Sii.Trainings.Tests.Extensions
{
    using System.Collections.Generic;

    public static class ListStringExtensions
    {
        public static string Join(this List<string> list, string separator)
        {
            return string.Join(separator, list);
        }
    }
}