﻿using Microsoft.Extensions.Logging;
using Npgsql;
using BuisnessObject;
using System.ComponentModel;

namespace DataLayerLogic
{
    public class DataLayer
    {
        string connectionString;
        NpgsqlDataSource dataSource;
        NpgsqlConnection connection;
        public async Task<bool> ConnectToServer()
        {
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = await dataSource.OpenConnectionAsync();
            if (connection == null)
            {
                Console.WriteLine("Error, can't connect to server:" + connectionString);
                return false;
            }
            return true;
        }

        public DataLayer()
        {
            connectionString = "Host=localhost;Port=5549;Username=admin;Password=123;Database=postgres";
        }

        public async Task<int> CountUsers()
        {
            await using var cmd = new NpgsqlCommand("SELECT COUNT(*) \r\n\t" +
                                                    "FROM \"UserSchema\".\"Users\";");
            cmd.Connection = connection;
            await using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();
            return reader.GetInt32(0);
        }

        public async Task InsertData(userObject userToInsert)
        {
            await using var cmd = new NpgsqlCommand("INSERT INTO \"UserSchema\".\"Users\"(\r\n\t\"userName\", \"userDateOfBirth\", \"userRole\")\r\n\tVALUES ($1, $2, $3);")
            {
                Parameters =
            {
                new() { Value = userToInsert.userName},
                new() { Value = userToInsert.userBirthDate},
                new() { Value = (int) userToInsert.userRole }
            }
            };
            cmd.Connection = connection;
            Convert.ToBoolean(await cmd.ExecuteNonQueryAsync());
        }

        public async Task<List<userObject>> GetPage(sortRule rule, int page_size = 5, int page_number = 1)
        {
            await using var cmd = new NpgsqlCommand("SELECT \"userID\" AS \"ID\", \"userName\" AS \"Name\", \"userDateOfBirth\" AS \"Birthday\", \"role\" AS \"Role\"\r\n" +
                                                    "FROM \"UserSchema\".\"Users\"\r\nINNER JOIN \"UserSchema\".\"Roles\"\r\nON \"UserSchema\".\"Users\".\"userRole\" = \"UserSchema\".\"Roles\".\"roleID\"\r\n" +
                                                    "ORDER BY \r\n\t" +
                                                    "\"" + rule.getSortByString() + "\" " +
                                                    rule.getOrderString() + " " +
                                                    "LIMIT $1 OFFSET $2")
            {
                Parameters =
            {
                new() { Value = page_size},
                new() { Value = (page_number - 1) * page_size}

            }

            };
            cmd.Connection = connection;
            await using var reader = await cmd.ExecuteReaderAsync();
            List<userObject> readData = new List<userObject>();
            while (await reader.ReadAsync())
            {
                var ID = reader.GetInt32(0);
                var name = reader.GetString(1);
                var date = DateOnly.FromDateTime(reader.GetDateTime(2));
                userObject.ROLES role = (userObject.ROLES)Enum.Parse(typeof(userObject.ROLES), reader.GetString(3));
                var user = new userObject(ID, name, date, role);
                readData.Add(user);
            }

            return readData;
        }

        public async Task<List<DateTime>> GetLoginTime(int userID)
        {
            await using var cmd = new NpgsqlCommand("SELECT \"time\"\r\n" +
                                                    "FROM \"UserSchema\".\"LoginTime\"\r\n" +
                                                    "INNER JOIN \"UserSchema\".\"Users\"\r\nON \"UserSchema\".\"LoginTime\".\"userID\" = \"UserSchema\".\"Users\".\"userID\"\r\n" +
                                                    "WHERE \"UserSchema\".\"Users\".\"userID\" = $1\r\n" +
                                                    "ORDER BY \"UserSchema\".\"Users\".\"userID\", \"timeID\" ASC")
            {
                Parameters =
            {
                new() { Value = userID}
            }

            };
            cmd.Connection = connection;
            await using var reader = await cmd.ExecuteReaderAsync();
            List<DateTime> userLoginTimes = new List<DateTime>();
            while (await reader.ReadAsync())
            {
                var login_time = reader.GetDateTime(0);
                userLoginTimes.Add(login_time);
            }

            return userLoginTimes;
        }

        public async Task InsertLoginTime(userObject user, userLoginTime loginTime)
        {
            await using var cmd = new NpgsqlCommand("INSERT INTO \"UserSchema\".\"LoginTime\"(\r\n\t\"userID\", \"time\")\r\n\t" +
                                                    "VALUES ($1, $2);")
            {
                Parameters =
                {
                    new() {Value = user.userId},
                    new() {Value = loginTime.time}
                }
            };
            cmd.Connection = connection;
            await cmd.ExecuteNonQueryAsync();
            return;
        }
        public async Task<userObject> UpdateData(userObject user)
        {
            await using var cmd = new NpgsqlCommand("UPDATE \"UserSchema\".\"Users\"\r\n" +
                                                    "SET \"userName\"= $1, \"userDateOfBirth\"= $2, \"userRole\"=$3\r\n" +
                                                    "WHERE \"userID\" = $4;")
            {
                Parameters =
            {
                new() { Value = user.userName },
                new() { Value = user.userBirthDate },
                new() { Value = (int) user.userRole },
                new() { Value = user.userId }
            }

            };
            cmd.Connection = connection;
            await cmd.ExecuteNonQueryAsync();
            return user;
        }

        public async Task<bool> DeleteData(userObject user)
        {
            var cmd = new NpgsqlCommand("DELETE FROM \"UserSchema\".\"LoginTime\"\r\n\tWHERE \"LoginTime\".\"userID\" = $1;\r\n")
            {
                Parameters =
                {
                    new() {Value = user.userId}
                }
            };
            cmd.Connection = connection;
            await cmd.ExecuteNonQueryAsync();
            cmd.CommandText = "DELETE FROM \"UserSchema\".\"Users\"\r\n\tWHERE \"Users\".\"userID\" = $1;";
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<int> SearchUser(userObject user)
        {
            var userName = user.userName;
            var userBirthDay = user.userBirthDate;
            await using var cmd = new NpgsqlCommand("SELECT \"userID\", \"userName\", \"userDateOfBirth\"\r\n" +
                                                    "FROM \"UserSchema\".\"Users\"\r\n" +
                                                    "WHERE \"userName\" = $1 AND \"userDateOfBirth\" = $2")
            {
                Parameters =
            {
                new() { Value = userName},
                new() { Value = userBirthDay }
            }

            };
            cmd.Connection = connection;
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return reader.GetInt32(0);
            return 0;
        }

        public async Task<bool> SearchUserByID(int id)
        {
            await using var cmd = new NpgsqlCommand("SELECT \"userID\", \"userName\", \"userDateOfBirth\"\r\n" +
                                                    "FROM \"UserSchema\".\"Users\"\r\n" +
                                                    "WHERE \"userID\" = $1")
            {
                Parameters =
            {
                new() { Value = id},
            }

            };
            cmd.Connection = connection;
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return true;
            return false;
        }






    }
}