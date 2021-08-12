using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
