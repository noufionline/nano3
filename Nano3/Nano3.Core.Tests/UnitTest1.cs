using System;
using Xunit;

namespace Nano3.Core.Tests
{
    public class WhenObjectIsCreatedUsingEntityBase
    {
        [Fact]
        public void DirtyTrackingWorksOnlyWhenExplicitlyStated()
        {
            var sot=new EntityMock();
            sot.Name="Noufal";
            Assert.False(sot.IsDirty);
        }

        [Fact]
        public void DirtyTrackingWorksWhenExplicitlyStated()
        {
            var sot=new EntityMock();
            sot.StartDirtyTracking();
            sot.Name="Noufal";
            Assert.True(sot.IsDirty);
        }
    }


    public class EntityMock : EntityBase<EntityMock>
    {
        public string Name { get; set; }
    }
}
