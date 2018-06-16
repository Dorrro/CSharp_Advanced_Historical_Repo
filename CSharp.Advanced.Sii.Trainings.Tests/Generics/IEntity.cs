namespace CSharp.Advanced.Sii.Trainings.Tests.Generics
{
    using System;

    public interface IEntity<T> 
        where T : struct, IComparable 
    {
        T Id { get; set; }
    }
}