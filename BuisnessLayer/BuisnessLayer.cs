using BuisnessObject;
using PageObject;
using DataLayerLogic;

namespace BuisnessLayerLogic
{
    public class BuisnessLayer
    {
        private DataLayer dataLayer;
        public BuisnessLayer() 
        {
            dataLayer = new DataLayer();
        }
        public async Task<bool> LayerInitialize()
        {
            if (!await dataLayer.ConnectToServer())
            {
                return false;
            }
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

        public async Task<bool> EditUser(int id, string name, string birthDate, string role)
        {
            userObject userToEdit = new userObject(
                                       id,
                                       name,
                                       DateOnly.Parse(birthDate),
                                       (userObject.ROLES)Enum.Parse(typeof(userObject.ROLES), role));
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
            usersListPage page = new usersListPage(pageNum);
            var userList = await dataLayer.GetPage(pageSize, pageNum);
            foreach (var user in userList)
            {
                page.pageRows.Add(user.userId, Tuple.Create(user.userName, user.userBirthDate.ToString(), user.userRole.ToString()));
            }
            return page;
        }


    }
}