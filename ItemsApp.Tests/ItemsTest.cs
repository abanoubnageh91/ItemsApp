using System;
using Xunit;
using ItemsApp.API.Data;
using Moq;
using System.Collections.Generic;
using ItemsApp.API.Controllers;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItemsApp.API.Helpers;
using ItemsApp.API.Dtos;

namespace ItemsApp.Tests
{
    public class ItemsTest
    {

        private readonly ItemsController controller;
        public ItemsTest()
        {
            var mockRepo = new Mock<IItemsRepository>();
            
            var mockMapper = new Mock<IMapper>();

            controller = new ItemsController(mockRepo.Object, mockMapper.Object);

            var httpCtxStub = new Mock<Microsoft.AspNetCore.Http.HttpContext>();

            var controllerCtx = new ControllerContext();
            controllerCtx.HttpContext = httpCtxStub.Object;

            controller.ControllerContext = controllerCtx;
        }
        
        [Fact]
        public async void GetItemNames_ReturnOk()
        {
           
            // Act
            var result = await controller.GetItemNames();
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetItems_ReturnOk()
        {
            var itemParams = new ItemParams { PageNumber = 1, PageSize = 5 };
            // Act
            
            var result = await controller.GetItems(itemParams);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetMaxPricesForItems_ReturnOk()
        {

            var itemParams = new ItemParams { PageNumber = 1, PageSize = 5 };

            // Act
            var result = await controller.GetMaxPricesForItems(itemParams);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetMaxPriceForItem_ReturnOk()
        {

            var itemName = "Item 1";

            // Act
            var result = await controller.GetMaxPriceForItem(itemName);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void CreateItem_ReturnsBadRequest()
        {
            // Arrange
            var item = new ItemForCreationDto()
            {
                ItemName="Item 89"
            };
            controller.ModelState.AddModelError("Cost", "Required");

            // Act
            var badResponse = await controller.CreateItem(item);

            // Assert
            Assert.IsType<BadRequestResult>(badResponse);
        }


        [Fact]
        public async void CreateItem_ReturnOk()
        {
            // Arrange
            var item = new ItemForCreationDto()
            {
                ItemName = "Item 89",
                Cost = 12
            };

            // Act
            var createdResponse = await controller.CreateItem(item);

            // Assert
            Assert.IsType<OkObjectResult>(createdResponse);
        }


        [Fact]
        public async void CreateItem_ReturnCreatedItem()
        {
            // Arrange
            var item = new ItemForCreationDto()
            {
                ItemName = "Item 89",
                Cost = 12
            };

            // Act
            var createdResponse = await controller.CreateItem(item) as OkObjectResult;
            var createdItem = createdResponse.Value as ItemToReturnDto;

            // Assert
            Assert.IsType<ItemToReturnDto>(createdItem);
            Assert.Equal("Item 89", createdItem.ItemName);
        }

        [Fact]
        public async void DeleteItem_ReturnOk()
        {
            // Act
            var okResponse = await controller.DeleteItem(3);
            // Assert
            Assert.IsType<OkResult>(okResponse);
        }
        [Fact]
        public async void DeleteItem_ReturnsBadRequest()
        {
            // Act
            var badResponse = await controller.DeleteItem(100);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }
    }
}
