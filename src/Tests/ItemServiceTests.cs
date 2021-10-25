using DomainModels;
using FluentAssertions;
using Moq;
using Repositories;
using Services;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class ItemServiceTests
    {
        [Fact]
        public void GetAll_ShouldReturnAllItem()
        {
            // Arrange
            var stubItemRepo = new Mock<IItemRepository>();
            stubItemRepo.Setup(_ => _.All()).Returns(CreateFakeItemList().AsQueryable());
            const int TOTAL_ITEM_COUNT = 3;

            var itemService = CreateDefaultItemService(stubItemRepo.Object);

            // Act
            IEnumerable<ItemDTO> itemsReturned = itemService.GetAll();

            // Assert
            itemsReturned.Should().HaveCount(TOTAL_ITEM_COUNT, "Because we have declared the stub Repo to return three items.");
        }

        [Theory]
        [InlineData(1, "Hello World")]
        [InlineData(2, "Hello World 2")]
        [InlineData(3, "Hello World 3")]
        public void Get_UsingExistingItemId_ShouldReturnItemDetails(int id, string expectedText)
        {
            // Arrange
            var stubItemRepo = new Mock<IItemRepository>();
            stubItemRepo.Setup(_ => _.All()).Returns(CreateFakeItemList().AsQueryable());

            var itemService = CreateDefaultItemService(stubItemRepo.Object);

            // Act
            ItemDTO itemReturned = itemService.Get(id);

            // Assert
            itemReturned.Should()
                .Match((ItemDTO _) => _.Id == id, "Because service should fetch the item with the specified id.").And
                .Match((ItemDTO _) => string.Equals(_.Text, expectedText), "Because service should get the item with the text specified with the id.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CheckIfItemExists_UsingExistingItemId_ShouldReturnTrue(int id)
        {
            // Arrange
            var stubItemRepo = new Mock<IItemRepository>();
            stubItemRepo.Setup(_ => _.All()).Returns(CreateFakeItemList().AsQueryable());

            var itemService = CreateDefaultItemService(stubItemRepo.Object);

            // Act
            bool itemExists = itemService.CheckIfItemExists(id);

            // Assert
            itemExists.Should().BeTrue("Because there is an existing item with the given id in the fake repository");
        }

        private IItemService CreateDefaultItemService(IItemRepository repository)
        {
            return new ItemService(repository);
        }

        private List<Item> CreateFakeItemList()
        {
            return new List<Item> {
                new Item {
                    Id = 1,
                    Text = "Hello World",
                    CreatedBy = new Guid("52386593-deb3-4005-9bfd-2452465101bc"),
                    DateCreated = new DateTime(2021, 10, 21, 11, 17, 45)
                },
                new Item {
                    Id = 2,
                    Text = "Hello World 2",
                    CreatedBy = new Guid("a5cf22d2-0924-4bc6-a72d-249fc233f9d0"),
                    DateCreated = new DateTime(2021, 10, 22, 11, 10, 02)
                },
                new Item {
                    Id = 3,
                    Text = "Hello World 3",
                    CreatedBy = new Guid("f4fe0509-5edd-4f8f-b5b0-0bd44144c2fb"),
                    DateCreated = new DateTime(2021, 10, 23, 21, 17, 23)
                }
            };
        }
    }
}
