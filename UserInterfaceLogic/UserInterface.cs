using BuisnessLayerLogic;
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
        public async Task<ConsoleKey> InputRequest()
        {
            Console.WriteLine("Enter the key...");
            return Console.ReadKey().Key;
        }
        public async Task<int> ChangeKeyRequest()
        {
            Console.WriteLine("Enter number of elements on page:");
            var page_size_from_keyboard = Console.ReadLine();
            int page_size;
            if (int.TryParse(page_size_from_keyboard, out page_size))
                return Convert.ToInt32(page_size);
            else
                Console.WriteLine("Invalid input");
            return 0;
        }
        public async Task<int> IdRequest()
        {
            Console.WriteLine("Enter user ID:");
            var id_from_keyboard = Console.ReadLine();
            int id;
            if (int.TryParse(id_from_keyboard, out id))
                if (id >= 0)
                    return Convert.ToInt32(id);
                else
                    Console.WriteLine("Invalid input");
            else
                Console.WriteLine("Invalid input");
            return 0;
        }
        
        public async Task MainScreen()
        {
            Console.Clear();
            Console.WriteLine("==========================");
            Console.WriteLine("|     Main screen     |");
            Console.WriteLine("===========================");
            Console.WriteLine("1. Open user table");
            Console.WriteLine("2. Settings");
            Console.WriteLine("Esc. Exit");
            Start:
                switch(await InputRequest())
                {
                    case ConsoleKey.Enter:
                        await PageScreen();
                        goto Start;
                    case ConsoleKey.Escape:
                        break;
                }
        }
        public async Task PageScreen()
        {
            int pageNum = 1;
            var page = await buisnessLayer.LoadPage(pageSize, pageNum);
            Start: 
                await GoToPage(page);
                switch(await InputRequest())
                {
                    case ConsoleKey.LeftArrow:
                        if (pageNum -1 >= 1)
                            page = await buisnessLayer.LoadPage(pageSize, pageNum - 1);
                        goto Start;
                    case ConsoleKey.RightArrow:
                        page = await buisnessLayer.LoadPage(pageSize, pageNum + 1);
                        goto Start;
                    case ConsoleKey.Escape:
                        break;
                    case ConsoleKey.S:
                        var newPageSize = await ChangeKeyRequest();
                        if (newPageSize != 0)
                        {
                            SetPageSize(newPageSize);
                            page = await buisnessLayer.LoadPage(pageSize, pageNum);
                        }
                        goto Start;
                    case ConsoleKey.Enter:
                        var id = await IdRequest();
                        if (id >= page.First.userId && id <= page.Last.userId)
                            break;
                        goto Start;
                    default:
                        goto Start;
                }
        }    
        public async Task UserScreen(int id)
        {
            Start: 
                switch(await InputRequest())
                {
                    case ConsoleKey.D:
                        goto Start;
                    case ConsoleKey.E:
                        goto Start;
                    case ConsoleKey.Escape:
                        break;
                    default:
                        goto Start;
                }
        }
        public async Task GoToPage(usersListPage page)
        {
            Console.Clear();
            foreach (var row in page.pageRows) 
            {
                Console.WriteLine(row.Key + " " + row.Value.Item1 + " " + row.Value.Item2 + " " + row.Value.Item3);
            }
            Console.WriteLine("===========================");
            Console.WriteLine("Page:" + page.pageNum);
        }
        public void SetPageSize(int size)
        {
            pageSize = size;
        }

    }
}
