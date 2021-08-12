using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Models;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Data access layer class for 'Message' model. 
    /// Corresponding to 'Messages' table.
    /// </summary>
    public class MessageRepository : GenericRepository<Message> { 
    
        /// <summary>
        /// Constructor
        /// </summary>
        public MessageRepository() : base("Messages") { }


        /// <summary>
        /// Parameterized constructor. Used in DAL.Tests project.
        /// </summary>
        /// <param name="tablename">Database's table name</param>
        /// <param name="connectionstring">Database's connection string</param>
        public MessageRepository(string tablename, string connectionstring) : base(tablename, connectionstring) { }



        /// <summary>
        /// Async method. Reads all messages from the database.
        /// </summary>
        /// <returns>IEnumerable of messages</returns>
        public new async Task<IEnumerable<Message>> ReadAllAsync()
        {
            string sql = @"SELECT * FROM Messages ORDER BY CreatedAt ASC";
            using (var connection = CreateConnection())
            {
                var messages = await connection.QueryAsync<Message>(sql);
                return messages;
            }
        }
    }
}
