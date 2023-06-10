namespace BuisnessObject
{
    public class userObject
    {
        public enum ROLES
        {
            user = 1,
            admin,
            guest
        }
        public int userId { get; }
        public string userName { get; }
        public ROLES userRole { get; }
        public DateOnly userBirthDate { get; }

        public List<userLoginTime> timeTable{ get;}

        public userObject(int id, string name, DateOnly birthDate, ROLES role = ROLES.user)
        {
            this.userId = id;
            this.userName = name;   
            this.userRole = role;
            this.userBirthDate = birthDate;
            this.timeTable = new List<userLoginTime>();
        }
        public static userObject CreateFromString(int id, string name, string _birthDate, string _role)
        {
            DateOnly birthDate;
            if (!DateOnly.TryParse(_birthDate, out birthDate))
                throw new ArgumentException("Invalid data format");
            userObject.ROLES role;
            if (!Enum.TryParse(_role, out role))
                throw new ArgumentException("Invalid role format");
            return new userObject(id, name, birthDate, role);
        }
    }
    public class userLoginTime
    {
        public DateTime time { get; set; }
        public userLoginTime(DateTime loginTime)
        {
            this.time = loginTime;
        }
        public static userLoginTime CreateFromString(string _loginTime)
        {
            DateTime loginTime;
            if (!DateTime.TryParse(_loginTime, out loginTime))
                throw new ArgumentException("Invalid date/time format");
            return new userLoginTime(loginTime);
        }
    }
    public class sortRule
    {
        public enum COLUMNS
        {
            id = 1,
            name = 2,
            date,
            role
        }
        public bool isDesc;
        public COLUMNS sortBy;
        public sortRule(bool isDesc = false, COLUMNS sortBy = COLUMNS.id) 
        {
            this.isDesc = isDesc;
            this.sortBy = sortBy;
        }
        public string getSortByString()
        {
            switch (sortBy) 
            {
            case COLUMNS.id:
                return "userID";
            case COLUMNS.name:
                return "userName";
            case COLUMNS.date:
                return "userDateOfBirth";
            case COLUMNS.role:
                return "userRole";
            default:
                throw new ArgumentException();
            }
        }
        public string getOrderString()
        {
            if (isDesc)
                return "DESC";
            else
                return "ASC";
        }
    }
}
