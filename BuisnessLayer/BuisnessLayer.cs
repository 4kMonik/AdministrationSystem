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
            userObject userToAdd = userObject.CreateFromString(0, name, birthDate, role);
            var id = await dataLayer.SearchUser(userToAdd);
            if (id == 0)
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

        public async Task<bool> EditUser(int id, string name, string birthDate, string role)
        {
            userObject userToEdit = userObject.CreateFromString(id, name, birthDate, role);
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
                throw new ArgumentException("Invalid id");
            }
            userObject userToDelete = new userObject(id, "", new DateOnly());
            await dataLayer.DeleteData(userToDelete);
            return true;
        }

        public async Task<usersListPage> LoadPage(int pageSize = 5, int pageNum = 1, string orderBy = "userID", string order = "ASC")
        {
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("Invalid page size");
            int additionalPage = 0;
            if (userCount % pageSize != 0)
                additionalPage = 1;
            if (pageNum < 1 || pageNum > userCount / pageSize + additionalPage)
                throw new ArgumentOutOfRangeException("Invalid number of pages");
            usersListPage page = new usersListPage(pageNum);
            var userList = await dataLayer.GetPage(pageSize, pageNum, orderBy, order);
            foreach (var user in userList)
            {
                page.AddRow(user.userId, Tuple.Create(user.userName, user.userBirthDate.ToString(), user.userRole.ToString()));
            }
            return page;
        }

        public async Task<userPage> LoadUser(int userID, usersListPage page)
        {
            userPage user = new userPage(userID, page.GetNameById(userID), page.GetBirthDateById(userID), page.GetRoleById(userID));
            foreach (var login in await dataLayer.GetLoginTime(userID))
            {
                user.loginTimes.Add(login.ToString());
            }
            return user;
        }

        public async Task LogInUser(userPage user, string loginTime)
        {
            await dataLayer.InsertLoginTime(userObject.CreateFromString(user.id, user.name, user.birthDate, user.role), userLoginTime.CreateFromString(loginTime));
        }
    }
}