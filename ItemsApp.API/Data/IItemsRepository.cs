using System.Collections.Generic;
using System.Threading.Tasks;
using ItemsApp.API.Helpers;
using ItemsApp.API.Models;

namespace ItemsApp.API.Data
{
    public interface IItemsRepository
    {
        void Add<T> (T entity) where T:class;
        void Delete<T> (T entity) where T:class;
        Task<bool> SaveAll();
        Task<PagedList<Item>> GetAllItems(ItemParams itemParams);
        Task<PagedList<Item>> GetMaxPricesForItems(ItemParams itemParams);
        Task<int> GetMaxPriceForItem(string itemName);
        Task<Item> GetItem(int id);
        Task<Item> CreateItem(Item item);

        Task<List<string>> GetItemNames();
        
    }
}