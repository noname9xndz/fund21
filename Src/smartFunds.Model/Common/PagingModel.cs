namespace smartFunds.Model.Common
{
    public class PagingModel
    {
        public PagingModel()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
