using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Nano3.Core.Tests
{
    
    public class When_Object_Is_Created_Using_EntityBase
    {
        [Fact(DisplayName ="Entity Does Not Get Dirty By Default")]
        public void EntityDoesNotGetDirtyByDefault()
        {
            var sot = new EntityMock
            {
                Name = "Noufal"
            };
            Assert.False(sot.IsDirty);
        }

        [Fact(DisplayName ="Dirty Tracking Works When Explicitly Stated")]
        public void DirtyTrackingWorksWhenExplicitlyStated()
        {
            var sot = new EntityMock();
            
            Assert.False(sot.IsDirty);

            sot.StartDirtyTracking();

            sot.Name = "Noufal";
           
            Assert.True(sot.IsDirty);
        }


        [Fact(DisplayName ="Dirty Tracking Respects the excluded Properties")]
        public void DirtyTrackingRespectsTheExcludedProperties()
        {
            var sot = new EntityMock();
            
            
            Assert.False(sot.IsDirty);

            sot.StartDirtyTracking();

            sot.Age =25;
           
            Assert.False(sot.IsDirty);
        }
    }


    public class EntityMock : EntityBase<EntityMock>
    {
        public EntityMock()
        {
            ExcludedPropertiesFromDirtyTracking.Add("Age");
        }


        public string Name { get; set; }
        public int Age {get;set;}
    }
}
