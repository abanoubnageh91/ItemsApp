using System;
using System.Threading.Tasks;
using ItemsApp.API.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using ItemsApp.API.Helpers;
using System.Linq;
using ItemsApp.API.Dtos;
using System.Collections.Generic;

namespace ItemsApp.API.Data
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly DataContext dataContext;
        public ItemsRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;

        }

        public void Add<T>(T entity) where T : class
        {
            dataContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            dataContext.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public async Task<Item> CreateItem(Item item)
        {
            await dataContext.Items.AddAsync(item);
            await dataContext.SaveChangesAsync();

            return item;
        }

        public async Task<Item> GetItem(int id)
        {
            return await dataContext.Items.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetMaxPriceForItem(string itemName)
        {
            var maxPrice = await dataContext.Items.Where(i => i.ItemName == itemName).GroupBy(i => i.ItemName)
                      .Select(y => y.Max(i => i.Cost)).FirstOrDefaultAsync();

            return maxPrice;
        }

        public async Task<PagedList<Item>> GetMaxPricesForItems(ItemParams itemParams)
        {
            var items = dataContext.Items.GroupBy(i => i.ItemName)
                      .Select(y => new Item
                      {
                          ItemName = y.Key,
                          Cost = y.Max(i => i.Cost)
                      });

            return await PagedList<Item>.CreateAsync(items, itemParams.PageNumber, itemParams.PageSize);
        }


        public async Task<PagedList<Item>> GetAllItems(ItemParams itemParams)
        {
            var items = dataContext.Items.AsQueryable();

            return await PagedList<Item>.CreateAsync(items, itemParams.PageNumber, itemParams.PageSize);
        }

        public async Task<List<string>> GetItemNames()
        {
            var items = await dataContext.Items.GroupBy(i => i.ItemName)
                      .Select(y => y.Key).ToListAsync();

            return items;
        }
    }
}