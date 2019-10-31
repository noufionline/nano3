using Moq;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Nano3.Core.Tests
{

    public class When_Object_Is_Created_Using_EntityBase
    {
        [Fact(DisplayName = "Entity Does Not Get Dirty By Default")]
        public void EntityDoesNotGetDirtyByDefault()
        {
            var sot = new EntityMock
            {
                Name = "Noufal"
            };
            Assert.False(sot.IsDirty);
        }

        //[Fact(DisplayName ="Dirty Tracking Works When Explicitly Stated")]
        //public void DirtyTrackingWorksWhenExplicitlyStated()
        //{
        //    var sot = new EntityMock();

        //    Assert.False(sot.IsDirty);

        // //   sot.StartDirtyTracking();

        //    sot.Name = "Noufal";

        //    Assert.True(sot.IsDirty);
        //}

        [Fact]
        public void DirtyTrackingWorksWhenExplicitlyStated()
        {
            var sut = new EntityMock();

            Assert.False(sut.IsDirty);

            sut.StartDirtyTracking();

            sut.Name = "Noufal";

            Assert.True(sut.IsDirty);
        }


        [Fact(DisplayName = "Dirty Tracking Respects the excluded Properties")]
        public void DirtyTrackingRespectsTheExcludedProperties()
        {
            var sot = new EntityMock();


            Assert.False(sot.IsDirty);

            sot.StartDirtyTracking();

            sot.Age = 25;

            Assert.False(sot.IsDirty);
        }

        [Fact]
        public void ShouldPrintNashathNasserWhenGivenNameIsJustNashath()
        {
            var sut = new EntityMock();
            sut.Name = "Nashath";
            Assert.Equal("Nashath Nasser", sut.ToString());
        }

        [Fact]
        public void ShouldCallSaveWhenUpdate()
        {
            var mockRepository=new Mock<ICustomerRepository>();
            var vm=new CustomerViewModel(mockRepository.Object);
            vm.Update();
            mockRepository.Verify(x=> x.Save(),Times.Once);
        }


    }


    public interface ICustomerRepository
    {
        void Save();
    }


    public class CustomerViewModel
    {
        private readonly ICustomerRepository _repository;

        public CustomerViewModel(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public void Update()
        {
           _repository.Save();
        }
    }
    public class CustomerRepository : ICustomerRepository
    {
        public void Save()
        {
            throw new InvalidOperationException("Database call");
        }
    }

    public class EntityMock : EntityBase<EntityMock>
    {
        private string _name;

        public EntityMock()
        {
            ExcludedPropertiesFromDirtyTracking.Add("Age");
        }


        public string Name
        {
            get => _name;
            set
            {

                _name = value;
            }
        }
        public int Age { get; set; }

        public override string ToString()
        {
            if (string.Equals("Nashath", Name))
            {
                return "Nashath Nasser";
            }
            return base.ToString();
        }
    }
}
