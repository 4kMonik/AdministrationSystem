using BuisnessLayerLogic;

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
        public async Task<ConsoleKeyInfo> InputRequest()
        {
            Console.WriteLine("Enter the key...");
            return Console.ReadKey().Key;
        }
        public async Task<int> ChangeKeyRequest()
        {
            Console.WriteLine("Enter number of elements on page:");
            var page_size = Console.ReadLine();
            if (int.tryParse(page_size))
                return Convert.ToInt32(page_size);
            else
                Console.WriteLine("Invalid input");
            return 0;
        }
        public async Task<int> IdRequest()
        {
            Console.WriteLine("Enter user ID:");
            var page_size = Console.ReadLine();
            if (int.tryParse(page_size))
                return Convert.ToInt32(page_size);
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
                switch(await inputRequest())
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
                await goTopage(page);
                switch(await inputRequest())
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
                        if (newPageSize)
                        {
                            SetPageSize(newPageSize);
                            page = await buisnessLayer.LoadPage(pageSize, pageNum);
                        }
                        goto Start;
                    case ConsoleKey.Enter:
                        var id = await IdRequest();
                        if (id >= page.First.userId && id <= page.Last.userId)
                            
                        goto Start;
                    default:
                        goto Start;
                }
        }    
        public async Task UserScreen(int id)
        {
            Start: 
                switch(await inputRequest())
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
        public async Task goToPage(List<userObject> page)
        {
            Console.Clear();
            foreach (var row in page) 
            {
                Console.WriteLine(row.userId + " " + row.userName + " " + row.userBirthDate + " " + row.userRole);
                Console.WriteLine("---------------------------");
                foreach (var time in row.loginTime)
                    Console.WriteLine("  " + time.ToString());
                Console.WriteLine("---------------------------");
            }
            Console.WriteLine("===========================");
            Console.WriteLine("Page:" + pageNum);
        }
        public void SetPageSize(int size)
        {
            pageSize = size;
        }

    }
}
