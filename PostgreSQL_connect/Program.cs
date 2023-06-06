using Microsoft.Extensions.Logging;
using Npgsql;
using BuisnessObject;
using System.ComponentModel;

DataLayer dataLayer = new DataLayer();

userObject userToAdd = new userObject(0, "Akimov VV", new DateOnly(2000, 5, 23), userObject.ROLES.admin);
//await dataLayer.InsertData(userToAdd);

var page = await dataLayer.GetPage(10,1);

foreach (var row in page)
{
    Console.WriteLine(row.userId + " " + row.userName + " " + row.userBirthDate + " " + row.userRole);
    row.loginTime = await dataLayer.GetLoginTime(row.userId);
    foreach (var time in row.loginTime)
    {
        Console.WriteLine("    " + time.ToString());
    }
}
    
class DataLayer
{
    string connectionString;
    NpgsqlDataSource dataSource;
    NpgsqlConnection connection;
    void ConnectToServer()
    {
        dataSource = NpgsqlDataSource.Create(connectionString);
        connection = dataSource.OpenConnection();
    }

    public DataLayer()
    {
        connectionString = "Host=localhost;Port=5549;Username=postgres;Password=31497;Database=postgres";
        ConnectToServer();
    }

    public async Task InsertData(userObject userToInsert)
    {
        await using var cmd = new NpgsqlCommand("INSERT INTO \"UserSchema\".\"Users\"(\r\n\t\"userName\", \"userDateOfBirth\", \"userRole\")\r\n\tVALUES ($1, $2, $3);", connection)
        {
            Parameters =
            {
                new() { Value = userToInsert.userName},
                new() { Value = userToInsert.userBirthDate},
                new() { Value = (int) userToInsert.userRole }
            }
        };
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<userObject>> GetPage(int page_size = 5, int page_number = 1)
    {
        await using var cmd = new NpgsqlCommand("SELECT \"userID\" AS \"ID\", \"userName\" AS \"Name\", \"userDateOfBirth\" AS \"Birthday\", \"role\" AS \"Role\"\r\n\r\n" +
                                                "FROM \"UserSchema\".\"Users\"\r\n" +
                                                "INNER JOIN \"UserSchema\".\"Roles\"\r\n" +
                                                "ON \"UserSchema\".\"Users\".\"userRole\" = \"UserSchema\".\"Roles\".\"roleID\"\r\n" +
                                                "ORDER BY \"userID\" ASC " +
                                                "LIMIT $1 OFFSET $2")
        {
            Parameters =
            {
                new() { Value = page_size},
                new() { Value = page_number - 1}
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
            userObject.ROLES role = (userObject.ROLES) Enum.Parse(typeof(userObject.ROLES), reader.GetString(3));
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




}



