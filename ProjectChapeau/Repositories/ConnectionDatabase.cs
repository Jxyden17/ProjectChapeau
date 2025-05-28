namespace ProjectChapeau.Repositories
{
    public class ConnectionDatabase
    {
        protected readonly string? _connectionString;

        protected ConnectionDatabase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProjectChapeau");
        }
    }
}
