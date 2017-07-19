using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void CanSelectCategories()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] 
            {
                new Product {Id = 1, Name = "P1", Category = "Apples"},
                new Product {Id = 2, Name = "P2", Category = "Apples"},
                new Product {Id = 3, Name = "P3", Category = "Plums"},
                new Product {Id = 4, Name = "P4", Category = "Oranges"}
            });

            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);

            // Act - get the set of categories
            string[] results = ((IEnumerable<string>)(target.Invoke() 
                as ViewViewComponentResult).ViewData.Model).ToArray();

            // Assert
            Assert.True(Enumerable.SequenceEqual(new string[] 
            {
                "Apples", "Oranges", "Plums"
            }, results));
        }

        [Fact]
        public void IndicatesSelectedCategory()
        {
            // Arrange 
            string categoryToSelect = "Apples";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] 
            {
                new Product {Id = 1, Name = "P1", Category = "Apples"},
                new Product {Id = 2, Name = "P2", Category = "Oranges"}
            });

            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext
                {
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;

            // Action
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            // Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
