using Dapper;
using JE_PracticalTask.Logic.Interfaces;
using JE_PracticalTask.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace JE_PracticalTask.Logic
{
    public class HistoryLogic : IHistoryLogic
    {
        private readonly IConfiguration _configuration;

        public HistoryLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void UpdateQueryHistory(string queryString)
        {
            var procedure = "QUERY_HISTORY_UPDATE";
            var values = new { Query_string = queryString };

            await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            await connection.ExecuteAsync(procedure, values, commandType: CommandType.StoredProcedure);
        }

        public List<QueryHistory> GetQueryHistory()
        {
            var sql = "SELECT QUERY_STRING AS QUERYSTRING, QUERY_TIME AS QUERYDATETIME FROM QUERY_HISTORY";
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            return (List<QueryHistory>)connection.Query<QueryHistory>(sql);
        }
    }
}
