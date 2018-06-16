namespace CSharp.Advanced.Sii.Trainings.Tests.Generics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public class _3_ConstraintsTests
    {
        [Fact]
        public void Example_1()
        {
            var model = new Model();
            model.Id = 1;
        }

        [Fact]
        public void DIY_1()
        {
            var models = new List<Model> { new Model { Id = 1 }, new Model { Id = 2 } };
            var viewModels = Mapper<ViewModel, Model, int>.Map(models);

            viewModels.Count.Should()
                .Be(models.Count);
            viewModels.Sum(vm => vm.Id)
                .Should()
                .Be(models.Sum(m => m.Id));
        }

        public class Mapper<TViewModel, TModel, TEntityType> 
            where TViewModel : IEntity<TEntityType>, new()
            where TModel : IEntity<TEntityType>
            where TEntityType : struct, IComparable
        {
            public static List<TViewModel> Map(List<TModel> models)
            {
                var viewModels = new List<TViewModel>();

                foreach (var model in models)
                {
                    viewModels.Add(new TViewModel { Id = model.Id });
                }

                return viewModels;
            }
        }
    }
}