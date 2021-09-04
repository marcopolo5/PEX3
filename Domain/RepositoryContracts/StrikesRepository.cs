using System.Threading.Tasks;
using Dapper;
using Domain.Models;

namespace Domain.RepositoryContracts
{
    public class StrikesRepository : GenericRepository<Strikes>
    {

        public StrikesRepository() : base("Strikes") { }

        public async Task<Strikes> ReadByUserId(int userid)
        {
            var sqlGetStrikes = @"select * from Strikes where userid=@Id";
            using (var connection = CreateConnection())
            {
                var strikeobj = await connection.QueryFirstOrDefaultAsync<Strikes>(sqlGetStrikes, new { Id = userid });
                return strikeobj;
            }
        }

        public async Task<(int id, int userid, int reporteduserid)?> ReadUserReportAsync(int userid, int reporteduserid)
        {
            var sqlGetUserReport = @"select * from UserReports where (userid=@Userid and reporteduserid=@Reporteduserid";
            using (var connection = CreateConnection())
            {
                var userreport = await connection.QueryFirstOrDefaultAsync<(int, int, int)>(sqlGetUserReport, new { Userid = userid, Reporteduserid = reporteduserid });
                return userreport;
            }
        }
    }
}
