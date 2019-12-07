namespace GrpcService.Contracts
{
    public interface IAbsConnectionStringProvider
    {
        string ConnectionString { get; }

        string GetConnectionString(string db);
    }
}
