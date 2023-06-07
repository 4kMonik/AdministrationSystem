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

        public async Task goToPage(int pageNum)
        {
            var page = await buisnessLayer.LoadPage(pageSize, pageNum);
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