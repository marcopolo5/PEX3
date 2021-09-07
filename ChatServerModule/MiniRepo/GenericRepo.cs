using Microsoft.Extensions.Configuration;

namespace ChatServerModule.MiniRepo
{
    public class GenericRepo
    {
        protected readonly string _connectionString;
        public GenericRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConn");
        }
    }
}
