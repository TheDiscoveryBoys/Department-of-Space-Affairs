using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using Dapper;
using Npgsql;

namespace IntergalacticPassportAPI.Data
{
    public abstract class BaseRepository<Model>(IConfiguration config, string tableName)
    {

        private readonly string _connectionString = config.GetConnectionString("DefaultConnection");

        protected IDbConnection CreateDBConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public async Task<Model> GetById(string id)
        {
            string PKIdentifier = tableName.Equals("users") ? "google_id" : "id";
            using var db = CreateDBConnection();
            var sql = $"SELECT * FROM {tableName} WHERE {tableName}.{PKIdentifier} = '{id}'";
            return await db.QueryFirstOrDefaultAsync<Model>(sql);

        }
        public async Task<IEnumerable<Model>> GetAll()
        {
            using var db = CreateDBConnection();
            var sql = $"SELECT * FROM {tableName}";
            return await db.QueryAsync<Model>(sql);
        }
        public async Task<Model> Create(Model model)
        {
            using var db = CreateDBConnection();
            var sql = ModelToSQLInsert(model);
            return await db.QuerySingleAsync<Model>(sql, model);
        }

        public abstract Task<bool> Exists(Model model);

        protected string ModelToSQLInsert(Model model)
        {
            PropertyInfo[] properties = model.GetType().GetProperties();

            string colNames = "";
            string values = "";
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                if (!propertyName.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                {
                    colNames += propertyName + ", ";
                    values += "@" + propertyName + ", ";
                }

            }
            colNames = colNames[..(colNames.Count() - 2)];
            values = values[..(values.Count() - 2)];
            string sql = $"INSERT INTO {tableName} ({colNames}) VALUES ({values}) RETURNING *";
            return sql;
        }

        private static string CamelToSnake(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Insert underscore before each uppercase letter (except the first), then lowercase everything
            string result = Regex.Replace(input, "(?<!^)([A-Z])", "_$1").ToLower();
            return result;
        }

        private string GetTableNameFromModel(Model model)
        {
            PropertyInfo prop = model.GetType().GetProperty("tableName");
            return prop?.GetValue(model)?.ToString();
        }

    }
}
