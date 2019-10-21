using System;

namespace Jasmine.AbsCore.Entities
{
    public interface IAuditable
    {
        string CreatedUser { get; set; }

        DateTime CreatedDate { get; set; }

        string ModifiedUser { get; set; }

        DateTime? ModifiedDate { get; set; }

    }
}