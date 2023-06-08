namespace PageObject
{
    public class usersListPage
    {
        public int pageNum { get; }
        public Dictionary<int, Tuple<string, string, string>> pageRows;
        public usersListPage(int pageNum)
        {
            pageRows = new Dictionary<int, Tuple<string, string, string>>();
            this.pageNum = pageNum;
        }
    }
    public class userPage
    {
        public int id { get; }
        public string name { get; }
        public string birthDate { get; }
        public string role { get; }
        public List<string> loginTimes { get; }
        public userPage(int userID, string userName, string userBirthDate, string userRole)
        {
            id = userID;
            name = userName;
            birthDate = userBirthDate;
            role = userRole;
            loginTimes = new List<string>();
        }

    }
}
