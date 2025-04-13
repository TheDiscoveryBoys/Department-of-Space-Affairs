using Dapper;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public class UsersRepository(IConfiguration config) : BaseRepository<Users>(config, "users")
    { 
        
        public async Task<IEnumerable<Roles>> GetUserRoles(string googleId)
        {
            using var db = CreateDBConnection();
            var sql = @"
                    SELECT r.id, r.role AS role
                    FROM user_roles ur
                    JOIN roles r ON ur.role_id = r.id
                    WHERE ur.user_id = @GoogleId;
                    ";
            var roles = await db.QueryAsync<Roles>(sql, new { GoogleId = googleId });
            //Console.WriteLine(roles.ElementAt(0).Role);
            return roles;
        }

        public async Task<Users?> GetUserByEmail(string email)
        {
            using (var db = CreateDBConnection())
            {
                var sql = "SELECT * FROM users WHERE email = @Email;";
                var result = await db.QueryAsync<Users>(sql, new { Email = email });
                return result.FirstOrDefault();
            }
        }

        public async Task<bool> AssignRoleToUser(string googleId, int roleId)
        {

            using var db = CreateDBConnection();

            var sqlCheck = @"
                SELECT * FROM user_roles 
                WHERE user_id = @GoogleId
                AND role_id = @RoleId;
            ";
            Console.WriteLine(sqlCheck);
            var resultRows = await db.QueryAsync<UserRoles>(sqlCheck, new { GoogleId = googleId, RoleId = roleId });

            if (resultRows.Count() == 0) {
                var sql = @"
                    INSERT INTO user_roles (user_id, role_id)
                    VALUES (@GoogleId, @RoleId)
                    ON CONFLICT DO NOTHING;
                ";
            Console.WriteLine(sql);
            
            var rowsAffected = await db.ExecuteAsync(sql, new { GoogleId = googleId, RoleId = roleId });
            return rowsAffected > 0;
            } else {
                return false;
            }
        }

        public async override Task<bool> Exists(Users model)
        {
             var existingUser = await GetById(model.google_id ?? throw new Exception("No google id"));
             
              if (existingUser == null){
                    return false;
              } else{
                return true;
              }
        }
    }
}

