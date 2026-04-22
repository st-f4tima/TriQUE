using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using TriQue.Enums;
using TriQue.Models;

namespace TriQue.Data.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public UserRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public User GetByUsername(string username)
        {
            string query = "SELECT * FROM User WHERE Username = @username LIMIT 1";
            var parameters = new[] { new SqliteParameter("@username", username) };

            using (var reader = _dbHelper.ExecuteReader(query, parameters))
            { 
                if (reader.Read())
                {
                    string role = reader["RoleName"].ToString() ?? string.Empty;
                    User user;

                    if (role == "Admin")
                    {
                        user = new Admin();
                    }
                    else
                    {
                        user = new Driver();
                    }

                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.Username = reader["Username"].ToString() ?? string.Empty;
                    user.PasswordHash = reader["PasswordHash"].ToString() ?? string.Empty;
                    Enum.Parse<UserRole>(role);

                    return user;
                }
            }
            return null;
        }
    }
}
