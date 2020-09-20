using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using AutoMapper;
using ItemsApp.API.Data;
using ItemsApp.API.Dtos;
using ItemsApp.API.Models;
using ItemsApp.API.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ItemsApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository itemRepository;
        private readonly IMapper mapper;
        public ItemsController(IItemsRepository itemRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.itemRepository = itemRepository;

        }


        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemForCreationDto itemForCreationDto)
        {
            var itemToCreated = mapper.Map<Item>(itemForCreationDto);

            itemRepository.Add(itemToCreated);

            if (await itemRepository.SaveAll())
                return Ok();

            return BadRequest("Failed to add the item.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await itemRepository.GetItem(id);

            itemRepository.Delete(item);

            if (await itemRepository.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the item.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, ItemForCreationDto itemForCreationDto)
        {
          
            var item = await itemRepository.GetItem(id);
            mapper.Map(itemForCreationDto, item);

            if (await itemRepository.SaveAll())
                return Ok();

            throw new Exception($"Updating item {id} failed on save.");
        }

        [HttpGet("GetMaxPricesForItems")]
        public async Task<IActionResult> GetMaxPricesForItems([FromQuery] ItemParams itemParams)
        {
            var items = await itemRepository.GetMaxPricesForItems(itemParams);

            var itemsToReturn = mapper.Map<IEnumerable<ItemForListDto>>(items);

            Response.AddPagination(items.CurrentPage, items.PageSize, items.TotalCount, items.TotalPages);
            return Ok(itemsToReturn);
        }

        [HttpGet("GetMaxPriceForItem/{itemName}")]
        public async Task<IActionResult> GetMaxPriceForItem(string itemName)
        {
            var maxPrice = await itemRepository.GetMaxPriceForItem(itemName);

            return Ok(maxPrice);
        }

        [HttpGet("GetItemNames")]
        public async Task<IActionResult> GetItemNames()
        {
            var itemNames = await itemRepository.GetItemNames();

            return Ok(itemNames);
        }

        [HttpGet("GetItems")]
        public async Task<IActionResult> GetItems([FromQuery] ItemParams itemParams)
        {
            var items = await itemRepository.GetAllItems(itemParams);

            var itemsToReturn = mapper.Map<IEnumerable<ItemForListDto>>(items);
            
            Response.AddPagination(items.CurrentPage, items.PageSize, items.TotalCount, items.TotalPages);
            return Ok(itemsToReturn);
        }

    }
}