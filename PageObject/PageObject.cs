namespace PageObject
{
    public class usersListPage
    {
        public int pageNum { get; }
        protected Dictionary<int, Tuple<string, string, string>> pageRows;
        public usersListPage(int pageNum)
        {
            pageRows = new Dictionary<int, Tuple<string, string, string>>();
            this.pageNum = pageNum;
        }
        public void AddRow(int userId, Tuple<string, string, string> userData)
        {
            pageRows.Add(userId, userData);
        }
        public void RemoveRow(int userId) 
        {
            pageRows.Remove(userId);    
        }
        public bool IsUserInList(int userId) 
        {
            return pageRows.ContainsKey(userId);
        }
        public string GetNameById(int id)
        {
            return pageRows[id].Item1;
        }
        public string GetBirthDateById(int id) 
        {
            return pageRows[id].Item2;
        }
        public string GetRoleById(int id)
        {
            return pageRows[id].Item3;
        }
        public List<int> GetIdList()
        {
            return pageRows.Keys.ToList();
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
