namespace CatalogService.Api.Core.Application.ViewModels
{
    public class PaginatedItemsViewModel<TEntity> where TEntity : class
    {
        public int PageIndex { get; private set; }//Hangi sayfadayız
        public int PageSize { get; private set; } //Sayfada kaç tane ürün listeleniyor
        public long Count { get; private set; }//Toplamdaki ürün sayımız
        public IEnumerable<TEntity> Data{ get; private set; } 

        public PaginatedItemsViewModel(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
