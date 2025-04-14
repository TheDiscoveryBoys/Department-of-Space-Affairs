using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Dapper;
using Npgsql;
using IntergalacticPassportAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntergalacticPassportAPI.Data
{
    public abstract class BaseRepository<Model> : IBaseRepository<Model>
    {

        private readonly string _connectionString;
        private readonly string tableName;
        private readonly string PKIdentifier;

        public BaseRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            tableName = GetTableNameFromModel();
            PKIdentifier = GetPrimaryKeyIdentifier();
        }


        protected IDbConnection CreateDBConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public async Task<Model> GetById(object id)
        {
            using var db = CreateDBConnection();
            var typedParam = ConvertToExpectedType(id);
            var sql = $"SELECT * FROM {tableName} WHERE {tableName}.{CamelToSnake(PKIdentifier)} = @id";
            return await db.QueryFirstOrDefaultAsync<Model>(sql, new { id = typedParam });

        }
        public virtual async Task<IEnumerable<Model>> GetAll()
        {
            using var db = CreateDBConnection();
            var sql = $"SELECT * FROM {tableName}";
            return await db.QueryAsync<Model>(sql);
        }
        public virtual async Task<Model> Create(Model model)
        {
            using var db = CreateDBConnection();
            var sql = ModelToSQLInsert(model);
            return await db.QuerySingleAsync<Model>(sql, model);
        }

        public virtual async Task<Model> Update(Model model)
        {
            using var db = CreateDBConnection();
            List<string> reflectedAttributes = GetPropertyNamesFromModel(model);
            string sqlSetCode = "";

            foreach (string column in reflectedAttributes)
            {
                if (!column.Equals(PKIdentifier)) sqlSetCode += CamelToSnake(column) + " = " + "@" + column + ",";
            }

            var sql = $"UPDATE {tableName} SET {truncateComma(sqlSetCode)} WHERE {CamelToSnake(PKIdentifier)} = @{PKIdentifier} RETURNING *;";
            Console.WriteLine(sql);
            return await db.QuerySingleOrDefaultAsync<Model>(sql, model);

        }

        public async Task<bool> Delete(object id)
        {
            using var db = CreateDBConnection();
            var typedParam = ConvertToExpectedType(id);
            var sql = $"DELETE FROM {tableName} WHERE {tableName}.{CamelToSnake(PKIdentifier)} = @id";
            var rowsAffected = await db.ExecuteAsync(sql, new { id = typedParam });

            return rowsAffected > 0;
        }

        public abstract Task<bool> Exists(Model model);

        protected virtual string ModelToSQLInsert(Model model)
        {
            List<string> reflectedAttributes = GetPropertyNamesFromModel(model);
            string sqlCols = "";
            string sqlValues = "";
            for (var i = 0; i < reflectedAttributes.Count; i++)
            {
                if (i == reflectedAttributes.Count - 1)
                {
                    sqlCols += CamelToSnake(reflectedAttributes.ElementAt(i));
                    sqlValues += "@" + reflectedAttributes.ElementAt(i);
                }
                else
                {
                    sqlCols += CamelToSnake(reflectedAttributes.ElementAt(i)) + ", ";
                    sqlValues += "@" + reflectedAttributes.ElementAt(i) + ", ";
                }
            }
            string sql = $"INSERT INTO {tableName} ({sqlCols}) VALUES ({sqlValues}) RETURNING *";
            return sql;
        }

        protected List<string> GetPropertyNamesFromModel(Model model)
        {
            PropertyInfo[] properties = model.GetType().GetProperties();
            List<string> modelAttributes = [];
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                var pkAttr = property.GetCustomAttribute<PrimaryKeyAttribute>();
                if (pkAttr != null)
                {
                    if (!pkAttr.AutoGenerated) modelAttributes.Add(propertyName);
                }
                else
                {
                    modelAttributes.Add(propertyName);
                }

            }
            return modelAttributes;
        }

        private string GetPrimaryKeyIdentifier()
        {
            PropertyInfo[] properties = typeof(Model).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                var pkAttr = property.GetCustomAttribute<PrimaryKeyAttribute>();
                if (pkAttr != null)
                {
                    return propertyName;
                }

            }
            throw new Exception("Model doesn't contain primary key");
        }

        private Type GetPrimaryKeyType()
        {
            PropertyInfo[] properties = typeof(Model).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var pkAttr = property.GetCustomAttribute<PrimaryKeyAttribute>();
                if (pkAttr != null)
                {
                    return property.PropertyType;
                }

            }
            throw new Exception("Model doesn't contain primary key");
        }

        private object ConvertToExpectedType(object id)
        {
            Type expectedType = GetPrimaryKeyType();
            if (expectedType == typeof(int) && id is string str && int.TryParse(str, out var intVal))
                return intVal;

            if (expectedType == typeof(string))
                return id.ToString();

            return id;
        }

        private string truncateEndOfSql(string sqlString)
        {
            return sqlString[..(sqlString.Count() - 2)];
        }

        private string truncateComma(string sqlString)
        {
            return sqlString[..(sqlString.Count() - 1)];
        }

        private static string CamelToSnake(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Insert underscore before each uppercase letter (except the first), then lowercase everything
            string result = Regex.Replace(input, "(?<!^)([A-Z])", "_$1").ToLower();
            return result;
        }

        private string GetTableNameFromModel()
        {
            var tableAttr = typeof(Model).GetCustomAttribute<TableAttribute>();
            return tableAttr.Name;
        }

    }
}