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
    public abstract class GenericRepository<T> :IGenericRepository<T> where T: class
    {
        protected readonly string TableName;
        
        protected GenericRepository(string tablename)
        {
            TableName = tablename;
        }


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
        
        //TODO: Create connection string in App.config file
        // Or give raw connection string as parameter
        private SqlConnection SqlConnection() => new SqlConnection("Soon™"); 

        protected IDbConnection CreateConnection()
        {
            var connection = SqlConnection();
            connection.Open();
            return connection;
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {TableName} (");
            var properties = GenerateListOfProperties(GetProperties);
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

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {TableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("ID")){
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery
                .Remove(updateQuery.Length - 1, 1)
                .Append(" WHERE ID=@ID");

            return updateQuery.ToString();
        }

    }
}
