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
            return roles;
        }

        public async Task<bool> AssignRoleToUser(string googleId, int roleId)
        {

            using var db = CreateDBConnection();
            var sql = @"
                    INSERT INTO user_roles (user_id, role_id)
                    VALUES (@UserId, @RoleId)
                    ON CONFLICT DO NOTHING;
                ";
            Console.WriteLine(sql);
            var rowsAffected = await db.ExecuteAsync(sql, new { UserId = googleId, RoleId = roleId });
            return rowsAffected > 0;
        }

        public async override Task<bool> Exists(Users model)
        {
             var existingUser = await GetById(model.GoogleId);
              if (existingUser == null){
                    return false;
              } else{
                return true;
              }
        }
    }
}

