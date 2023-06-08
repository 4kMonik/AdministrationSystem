﻿using BuisnessLayerLogic;
using PageObject;

namespace UserInterfaceLogic
{
    public class UserInterface
    {
        private BuisnessLayer buisnessLayer;
        private int pageSize;
        public UserInterface() 
        {
            buisnessLayer = new BuisnessLayer();
            pageSize = 10;
        }

        public async Task<bool> UserInterfaceInitialize()
        {
            if (!await buisnessLayer.LayerInitialize())
                return false;
            return true;
        }
        private static ConsoleKey InputRequest()
        {
            Console.WriteLine("\nEnter the key...");
            return Console.ReadKey(true).Key;
        }
        private static int ChangeKeyRequest()
        {
            Console.WriteLine("\nEnter number of elements on page:");
            var page_size_from_keyboard = Console.ReadLine();
            int page_size;
            if (int.TryParse(page_size_from_keyboard, out page_size))
                return Convert.ToInt32(page_size);
            else
                Console.WriteLine("Invalid input");
            return 0;
        }
        private static int IdRequest()
        {
            Console.WriteLine("\nEnter user ID:");
            var id_from_keyboard = Console.ReadLine();
            int id;
            if (int.TryParse(id_from_keyboard, out id))
                if (id >= 0)
                    return id;
                else
                    Console.WriteLine("Invalid input");
            else
                Console.WriteLine("Invalid input");
            return 0;
        }
        private static int PageNumRequest()
        {
            Console.WriteLine("\nEnter page number:");
            var page_num_from_keyboard = Console.ReadLine();
            int page_num;
            if (int.TryParse(page_num_from_keyboard, out page_num))
                return page_num;
            else
                Console.WriteLine("Invalid input");
            return 0;
        }

        private static userPage EditUserRequest(int id)
        {
            Console.WriteLine("Enter name");
            var name = Console.ReadLine();
            Console.WriteLine("Enter birthdate");
            var date = Console.ReadLine();
            Console.WriteLine("Enter role");
            var role = Console.ReadLine();
            userPage editUser = new userPage(id, name, date, role);
            return editUser;
        }
        private static void ExceptionRequest()
        {
            Console.WriteLine("Enter any key to repeat");
            Console.ReadKey(true);
        }
        public async Task MainScreen()
        {
        Start:
            DisplayMainScreen();
            switch(InputRequest())
            {
                case ConsoleKey.Enter:
                    await PageScreen();
                    goto Start;
                case ConsoleKey.Escape:
                    break;
                default:
                    goto Start;
            }
        }
        public async Task PageScreen(int startPageNum = 1)
        {
            int pageNum = startPageNum;
            usersListPage page;
        Start:
            page = await buisnessLayer.LoadPage(pageSize, pageNum);
            DisplayPage(page);
            switch (InputRequest())
            {
                case ConsoleKey.LeftArrow:
                    try
                    {
                        page = await buisnessLayer.LoadPage(pageSize, pageNum - 1);
                        pageNum--;
                    }
                    catch(ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message);
                        ExceptionRequest();
                    }
                    goto Start;
                case ConsoleKey.RightArrow:
                    try
                    {
                        page = await buisnessLayer.LoadPage(pageSize, pageNum + 1);
                        pageNum++;
                    }
                    catch(ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message);
                        ExceptionRequest();
                    }
                    goto Start;
                case ConsoleKey.Escape:
                    break;
                case ConsoleKey.S:
                    var newPageSize = ChangeKeyRequest();
                    try
                    {
                        pageNum = 1;
                        page = await buisnessLayer.LoadPage(newPageSize, pageNum);
                        SetPageSize(newPageSize);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message);
                        ExceptionRequest();
                    }
                    goto Start;
                case ConsoleKey.P:
                    var new_pageNum = PageNumRequest();
                    try
                    {
                        page = await buisnessLayer.LoadPage(pageSize, new_pageNum);
                        pageNum = new_pageNum;
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message);
                        ExceptionRequest();
                    }
                    goto Start;
                case ConsoleKey.Enter:
                    var id = IdRequest();
                    if (page.IsUserInList(id))
                        await UserScreen(id, page);
                    else
                    {
                        Console.WriteLine("Invalid ID");
                        ExceptionRequest();
                    }
                    goto Start;
                default:
                    goto Start;
            }
        }    
        public async Task UserScreen(int userId, usersListPage  userList)
        {
            var user = await buisnessLayer.LoadUser(userId, userList);
        Start:
            DisplayUser(user);
            switch(InputRequest())
            {
                case ConsoleKey.D:
                    goto Start;
                case ConsoleKey.E:
                    var editUser = EditUserRequest(user.id);
                    try
                    {
                        await buisnessLayer.EditUser(user.id, editUser.name, editUser.birthDate, editUser.role);
                    }
                    catch (ArgumentException arg)
                    {
                        Console.WriteLine(arg.Message);
                        ExceptionRequest();
                        goto Start;
                    }
                    break;
                case ConsoleKey.Escape:
                    break;
                default:
                    goto Start;
            }
        }



        protected void DisplayMainScreen()
        {
            Console.Clear();
            Console.WriteLine("==========================");
            Console.WriteLine("|     Main screen     |");
            Console.WriteLine("===========================");
            Console.WriteLine("Enter. Open user table");
            Console.WriteLine("Esc. Exit\n");
        }
        protected void DisplayPage(usersListPage page)
        {
            Console.Clear();
            foreach (var id in page.GetIdList()) 
            {
                Console.WriteLine(id + " " + page.GetNameById(id) + " " + page.GetBirthDateById(id) + " " + page.GetRoleById(id));
            }
            Console.WriteLine("===========================");
            Console.WriteLine("Page:" + page.pageNum);
        }

        protected void DisplayUser(userPage user)
        {
            Console.Clear();
            Console.WriteLine("Name: {0}\nDate of birth: {1}\nRole: {2}\n|User activity|",user.name, user.birthDate, user.role);
            Console.WriteLine("===========================");
            foreach (var login in user.loginTimes)
            {
                Console.WriteLine(login);
                Console.WriteLine("---------------------------");
            }

        }

        public void SetPageSize(int size)
        {
            pageSize = size;
        }

    }
}
