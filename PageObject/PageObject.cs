namespace PageObject
{
    class Page
    {
        Dictionary<int, Tuple<string, string, string, List<string>>> pageRows;
        public Page()
        {
            pageRows = new Dictionary<int, Tuple<string, string, string, List<string>>>();
        }
    }
}
