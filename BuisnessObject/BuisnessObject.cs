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

        public userLoginTime loginTime{ get;}

        public userObject(int id, string name, DateOnly birthDate, ROLES role = ROLES.user)
        {
            this.userId = id;
            this.userName = name;   
            this.userRole = role;
            this.userBirthDate = birthDate;
        }


    }
    public class userLoginTime
    {
        public List<DateTime> timeTable { get; set; }
        public userLoginTime()
        {
            this.timeTable = new List<DateTime>();
        }
    }
}
