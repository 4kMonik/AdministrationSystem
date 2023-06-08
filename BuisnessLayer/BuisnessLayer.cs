using BuisnessObject;
using PageObject;
using DataLayerLogic;

namespace BuisnessLayerLogic
{
    public class BuisnessLayer
    {
        private DataLayer dataLayer;
        public int userCount;
        public BuisnessLayer() 
        {
            dataLayer = new DataLayer();
            userCount = 0;
        }
        public async Task<bool> LayerInitialize()
        {
            if (!await dataLayer.ConnectToServer())
            {
                return false;
            }
            userCount = await dataLayer.CountUsers();
            return true;
        }
        public async Task<bool> AddUser(string name, string birthDate, string role)
        {
            userObject userToAdd = new userObject(
                                       0,
                                       name,
                                       DateOnly.Parse(birthDate),
                                       (userObject.ROLES) Enum.Parse(typeof(userObject.ROLES), role));
            var id = await dataLayer.SearchUser(userToAdd);
            if (!Convert.ToBoolean(id))
            {
                await dataLayer.InsertData(userToAdd);
                return true;
            }
            else
            {
                Console.WriteLine("User already exists");
                return false;
            }
        }

        public async Task<bool> EditUser(int id, string name, string _birthDate, string _role)
        {
            DateOnly birthDate;
            if (!DateOnly.TryParse(_birthDate, out birthDate))
                throw new ArgumentException("Invalid data format");
            userObject.ROLES role;
            if (!Enum.TryParse(_role, out role))
                throw new ArgumentException("Invalid role format");
            userObject userToEdit = new userObject(
                                       id,
                                       name,
                                       birthDate,
                                       role);
            if (!await dataLayer.SearchUserByID(id))
            {
                Console.WriteLine("User does not exist");
                return false;
            }
            await dataLayer.UpdateData(userToEdit);
            return true;
        }

        public async Task<bool> EditUser(userObject userToEdit)
        {
            var id = await dataLayer.SearchUser(userToEdit);
            if (!Convert.ToBoolean(id))
            {
                Console.WriteLine("User does not exist");
                return false;
            }
            await dataLayer.UpdateData(userToEdit);
            return true;
        }

        public async Task<bool> DeleteUser(userObject userToDelete)
        {
            var id = await dataLayer.SearchUser(userToDelete);
            if (!Convert.ToBoolean(id))
            {
                Console.WriteLine("User does not exist");
                return false;
            }
            await dataLayer.DeleteData(userToDelete);
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            if (!await dataLayer.SearchUserByID(id))
            {
                Console.WriteLine("User does not exist");
                return false;
            }
            userObject userToDelete = new userObject(id, "", new DateOnly(0, 0, 0));
            await dataLayer.DeleteData(userToDelete);
            return true;
        }

        public async Task<usersListPage> LoadPage(int pageSize = 5, int pageNum = 1)
        {
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("Invalid page size");
            int additionalPage = 0;
            if (userCount % pageSize != 0)
                additionalPage = 1;
            if (pageNum < 1 || pageNum > userCount/pageSize + additionalPage)
                throw new ArgumentOutOfRangeException("Invalid number of pages");
            usersListPage page = new usersListPage(pageNum);
            var userList = await dataLayer.GetPage(pageSize, pageNum);
            foreach (var user in userList)
            {
                page.AddRow(user.userId, Tuple.Create(user.userName, user.userBirthDate.ToString(), user.userRole.ToString()));
            }
            return page;
        }

        public async Task<userPage> LoadUser(int userID, usersListPage page)
        {
            userPage user = new userPage(userID, page.GetNameById(userID), page.GetBirthDateById(userID), page.GetBirthDateById(userID));
            foreach (var login in await dataLayer.GetLoginTime(userID))
            {
                user.loginTimes.Add(login.ToString());
            }
            return user; 
        }


    }
}