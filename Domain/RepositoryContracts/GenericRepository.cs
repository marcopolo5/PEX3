using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Generic data access layer class for database transactions.
    /// </summary>
    /// <typeparam name="T">Domain level class</typeparam>
    /// <see cref="IGenericRepository{T}"/> for undocumented methods
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly string TableName;
        protected readonly string ConnectionString;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tablename">Table's name in the database</param>
        /// <param name="connectionstring">Database's connection string. Defaulted to local db.</param>

        protected GenericRepository(string tablename, string connectionstring = @"Data Source=DESKTOP-CBF6VS1;Initial Catalog=GeoChat_DB;Integrated Security=True")
        {
            TableName = tablename;
            ConnectionString = connectionstring;
        }
        /*protected GenericRepository(string tablename, string connectionstring = @"Data Source=.\MSSQLSERVER02;Initial Catalog=GeoChat_DB;Integrated Security=True")
        {
            TableName = tablename;
            ConnectionString = connectionstring;
        }*/


        #region Interface defined methods.
        public async Task CreateAsync(T t)
        {
            var insertQuery = GenerateInsertQuery();
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, t);
            }
        }
        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>($"SELECT * FROM {TableName}");
            }
        }
        public async Task<T> ReadAsync(int id)
        {
            using(var connection = CreateConnection())
            {
                var entity = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {TableName} WHERE ID={id}");
                if (entity == null)
                    throw new KeyNotFoundException($"{TableName} with id {id} was not found");
                return entity;
            }
        }
        public async Task UpdateAsync(T t)
        {
            var updateQuery = GenerateUpdateQuery();
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, t);
            }
        }
        public async Task DeleteAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync($"DELETE FROM {TableName} WHERE ID={id}");
            }
        }
        #endregion


        /// <summary>
        /// Async method. Gets the next availalbe id for 'TableName' table in the database.
        /// Table's 'Id' column (PK) must be set to identity.
        /// </summary>
        /// <returns>Next available id</returns>
        public async Task<int> GetAvailableId()
        {
            string sql = $"SELECT IDENT_CURRENT('{TableName}')+1";
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<int>(sql);
            }
        }


        /// <summary>
        /// Creates and returns a new SqlConnection object.
        /// </summary>
        /// <returns>SqlConnection object referring to the specified connection string</returns>
        /// <remarks>
        /// TODO:
        /// Create connection string in App.config file
        /// or give raw connection string as parameter
        /// </remarks>
        private SqlConnection SqlConnection() => new SqlConnection(ConnectionString); 


        /// <summary>
        /// Opens a new SqlConnection.
        /// </summary>
        /// <returns>SqlConnection object with an opened connection</returns>
        protected IDbConnection CreateConnection()
        {
            var connection = SqlConnection();
            connection.Open();
            return connection;
        }


        /// <summary>
        /// Generates an IEnumerable<PropertyInfo> with a generic class entity's properties.
        /// </summary>
        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();


        /// <summary>
        /// Transforms a IEnumerable of PropertyInfo objects containing an entity's properties
        /// into a list.
        /// </summary>
        /// <param name="listOfProperties"></param>
        /// <returns></returns>
        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }


        /// <summary>
        /// Generates an insert query.
        /// </summary>
        /// <returns>Insert query string</returns>
        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {TableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);
            properties.Remove("Id");
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        
        /// <summary>
        /// Generates an update query.
        /// </summary>
        /// <returns>Update query string</returns>
        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {TableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id")){
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery
                .Remove(updateQuery.Length - 1, 1)
                .Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }
    }
}
