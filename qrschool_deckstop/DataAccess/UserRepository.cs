using Dapper;
using qrschool_deckstop.Models;
using System.Collections.Generic;
using System.Linq;

namespace qrschool_deckstop.DataAccess
{
    public class UserRepository
    {
        public User GetByUsername(string username)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                var sql = @"SELECT id,
                                   username,
                                   password_hash AS PasswordHash,
                                   password_plain AS PasswordPlain,
                                   full_name AS FullName,
                                   role,
                                   created_at AS CreatedAt,
                                   last_login AS LastLogin
                              FROM users
                             WHERE username = @Username";
                return connection.QueryFirstOrDefault<User>(sql, new { Username = username });
            }
        }

        public void UpdateLastLogin(int userId)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                var sql = "UPDATE users SET last_login = NOW() WHERE id = @UserId";
                connection.Execute(sql, new { UserId = userId });
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                var sql = @"SELECT id,
                                   username,
                                   password_hash AS PasswordHash,
                                   password_plain AS PasswordPlain,
                                   full_name AS FullName,
                                   role,
                                   created_at AS CreatedAt,
                                   last_login AS LastLogin
                              FROM users
                          ORDER BY id";
                return connection.Query<User>(sql);
            }
        }

        public int Create(User user)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                var sql = @"INSERT INTO users (username, password_hash, full_name, role, created_at)
                            VALUES (@Username, @PasswordHash, @FullName, @Role, @CreatedAt)
                            RETURNING id";
                return connection.ExecuteScalar<int>(sql, new
                {
                    Username = user.Username,
                    PasswordHash = user.PasswordHash,
                    FullName = user.FullName,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                });
            }
        }

        // Проверка пароля: сначала plain (для разработки), затем хеш
        public bool ValidatePassword(User user, string password)
        {
            if (user == null) return false;

            if (!string.IsNullOrEmpty(user.PasswordPlain) && user.PasswordPlain == password)
            {
                return true;
            }

            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                var passwordHash = qrschool_deckstop.Helpers.PasswordHelper.HashPassword(password);
                // сравниваем без учёта регистра и с тримом
                return string.Equals(user.PasswordHash?.Trim(), passwordHash?.Trim(), System.StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        // Создание пользователя (с сохранением открытого пароля для тестирования)
        public int CreateUser(string username, string password, string fullName, string role)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                var passwordHash = qrschool_deckstop.Helpers.PasswordHelper.HashPassword(password);
                var sql = @"
                    INSERT INTO users (username, password_hash, password_plain, full_name, role, created_at) 
                    VALUES (@Username, @PasswordHash, @PasswordPlain, @FullName, @Role, @CreatedAt) 
                    RETURNING id";

                return connection.QuerySingle<int>(sql, new
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    PasswordPlain = password,
                    FullName = fullName,
                    Role = role,
                    CreatedAt = System.DateTime.Now
                });
            }
        }

        public void UpdatePassword(int userId, string passwordHash)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                var sql = "UPDATE users SET password_hash = @PasswordHash WHERE id = @UserId";
                connection.Execute(sql, new { PasswordHash = passwordHash, UserId = userId });
            }
        }
    }
}
