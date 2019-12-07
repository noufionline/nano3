namespace Jasmine.Core.Security
{
    public interface IAbsIdentity
    {
        int EmployeeId { get; }
        string Email { get; }
        string Name { get; }
        string Division { get; }
        int DivisionId { get; }

        string Position { get; }
        string Store { get; }
        string[] Roles { get; }

        string AccessToken { get; }
    
    }
}