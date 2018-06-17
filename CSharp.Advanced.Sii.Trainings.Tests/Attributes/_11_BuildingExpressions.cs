namespace CSharp.Advanced.Sii.Trainings.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using FluentAssertions;
    using Xunit;

    public class _11_BuildingExpressions
    {
        [Fact]
        public void T1()
        {
            Expression<Func<int, int, int>> addNumbers = (a, b) => a + b;
            addNumbers.ToString()
                .Should()
                .Be("(a, b) => (a + b)");

            addNumbers.Parameters[0]
                .Name.Should()
                .Be("a");
            addNumbers.Parameters[1]
                .Name.Should()
                .Be("b");

            addNumbers.Parameters[0]
                .Type.Should()
                .Be<int>();

            // ParameterExpression
            // ConstantExpression

            var binaryExpression = addNumbers.Body as BinaryExpression;
            var left = binaryExpression?.Left as ParameterExpression;
            left?.Name.Should()
                .Be("a");

            (binaryExpression?.Right as ParameterExpression)?.Name.Should()
                .Be("b");
//            var numbers = addNumbers(1,2);
//            numbers.Should()
//                .Be(3);
        }

        [Fact]
        public void T2()
        {
            // a => a % 2
            var a = Expression.Parameter(typeof(int), "a");
            var dwa = Expression.Constant(2);
            var body = Expression.Modulo(a, dwa);
            var lambda = Expression.Lambda<Func<int, int>>(body, a);

            lambda.ToString()
                .Should()
                .Be("a => (a % 2)");

            var func = lambda.Compile();
            func(10)
                .Should()
                .Be(0);
            func(11)
                .Should()
                .Be(1);
        }

        [Fact]
        public void DIY_1()
        {
            // (a,b) => a > b && b > 0
            var a = Expression.Parameter(typeof(int), "a");
            var b = Expression.Parameter(typeof(int), "b");
            var zero = Expression.Constant(0);
            var left = Expression.GreaterThan(a, b);
            var right = Expression.GreaterThan(b, zero);
            var body = Expression.AndAlso(left, right);

            var lambda = Expression.Lambda<Func<int, int, bool>>(body, a, b);
            var func = lambda.Compile();
            func(2, 3)
                .Should()
                .BeFalse();
            func(3, 2)
                .Should()
                .BeTrue();
            func(2, 0)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void T3()
        {
            // a => a.Test1

            var classToBeFiltered = new ClassToBeFiltered();
            var a = Expression.Parameter(classToBeFiltered.GetType(), "a");
            var body = Expression.Property(a, "Test1");
            var lambda = Expression.Lambda<Func<ClassToBeFiltered, int>>(body, a);

            lambda.ToString()
                .Should()
                .Be("a => a.Test1");
        }

        [Fact]
        public void TheProblem()
        {
            var request = new Request
                          {
                              Filters = new List<Filter>
                                        {
                                            new Filter
                                            {
                                                Field = "Test1",
                                                Value = "1",
                                                FilterType = FilterTypes.Gt
                                            },
                                            new Filter
                                            {
                                                Field = "Test2",
                                                Value = "2",
                                                FilterType = FilterTypes.Lt
                                            }
                                        }
                          };

            // ??

            // x.Test1 > 1
            // x.Test2 < 2
            // x => x.Test1 > 1 AndAlso x.Test2 < 2

            var x = Expression.Parameter(typeof(ClassToBeFiltered), "x");

            Expression expression = null;
            foreach (var filter in request.Filters)
            {
                if (expression == null)
                    expression = GetExpression<ClassToBeFiltered>(filter, x);
                else
                {
                    if(request.LogicalOperator == LogicalOperator.And)
                        expression = Expression.AndAlso(expression, GetExpression<ClassToBeFiltered>(filter, x));
                    else
                        expression = Expression.Or(expression, GetExpression<ClassToBeFiltered>(filter, x));
                }
            }

            var ok = new ClassToBeFiltered {  Test1 = 2, Test2 = 0};
            var classToBeFiltereds = new List<ClassToBeFiltered> {  ok, new ClassToBeFiltered {  Test1 = 0, Test2 = 3} }.AsQueryable();
            classToBeFiltereds.Where(Expression.Lambda<Func<ClassToBeFiltered, bool>>(expression, x))
                .ToList()
                .Should()
                .BeEquivalentTo(new List<ClassToBeFiltered> {ok});

//            Convert.ChangeType()
            // Expression
            // Expression z Filter0 AndAlso Expression z Filter1
            // ClassToBeFilter jest obiektem na którym ma działać filtr
            // Test1 odwoluje sie do property Test1 z ClassToBeFiltered itd.
            // List<ClassToFiltered>().AsQueryable().Where(WynikNaszejOperacji);

            // Metoda przyjmuje filter, zwraca expression
            // Zamiast Filter0, Filter1 -> Lista filtrów, request ma mieć listę filtrów
            // Sprawdzenie czy field rzeczywiście istnieje przed stworzniem filtra - OK
            // atrybut, który mówi, czy property jest filtrowalne - OK
            // spróbować stworzyć filtr "Contains", "StartsWith" -> to ma działać tylko na stringach - troszkę google'a
            // ^ => Expression.Call(...)
            // request zawiera property z informacją, o tym czy filtry mają być łączone andem czy orem
        }

        private static Expression GetExpression<T>(Filter request, ParameterExpression parameterExpression)
        {
            var x = parameterExpression;
            var propertyInfo = typeof(T).GetProperty(request.Field);
            if (propertyInfo == null)
            {
                throw new ArgumentException("Field does not exist");
            }

            if (propertyInfo.GetCustomAttribute<NonFilterableAttribute>() != null)
            {
                throw new ArgumentException("Field is not filterable");
            }

            var left = Expression.Property(x, request.Field);

            var propertyType = propertyInfo
                ?
                .PropertyType;

            if (propertyType == null)
                throw new InvalidOperationException();

            var constant = Expression.Constant(
                Convert.ChangeType(
                    request.Value,
                    propertyType));

            switch (request.FilterType)
            {
                case FilterTypes.Gt:
                    return Expression.GreaterThan(left, constant);
                case FilterTypes.Lt:
                    return Expression.LessThan(left, constant);
                case FilterTypes.Contains:
                    return Expression.Call(left, typeof(string).GetMethod("Contains", new [] { typeof(string)}), constant);
            }

            throw new InvalidOperationException();
        }
    }
}