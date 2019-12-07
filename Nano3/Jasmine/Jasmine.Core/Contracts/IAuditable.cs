using System;

namespace Jasmine.Core.Contracts
{
    public interface IAuditable
    {
        string CreatedUser { get; set; }
        DateTime CreatedDate { get; set; }
        string ModifiedUser { get; set; }
        DateTime? ModifiedDate { get; set; }
        byte[] RowVersion { get; set; }
    }
}