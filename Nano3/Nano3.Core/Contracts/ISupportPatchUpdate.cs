using Microsoft.AspNetCore.JsonPatch;

namespace Nano3.Core.Contracts
{
    public interface ISupportPatchUpdate
    {
        JsonPatchDocument CreatePatchDocument();
    }
}