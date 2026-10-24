using System.Collections.Generic;
using System.Linq;
using BulletinBoard.BLL.Exceptions;
using BulletinBoard.BLL.Models;
using BulletinBoard.BLL.Services;
using BulletinBoard.BLL.Tests.Fakes;
using Xunit;

namespace BulletinBoard.BLL.Tests
{
    public class CategoryServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _service = new CategoryService(_uow);
        }

        [Fact]
        public void GetTree_ReturnsDepthFirstOrderWithLevels()
        {
            IReadOnlyList<CategoryModel> tree = _service.GetTree();

            Assert.Equal(new[] { "Електроніка", "Транспорт", "Авто", "Легкові" }, tree.Select(c => c.Name).ToArray());
            Assert.Equal(new[] { 0, 0, 1, 2 }, tree.Select(c => c.Level).ToArray());
        }

        [Fact]
        public void Create_Subcategory_ReturnsLevelTwo()
        {
            CategoryModel category = _service.Create(TestData.Olena, "Вантажні", TestData.Cars);

            Assert.Equal(2, category.Level);
        }

        [Fact]
        public void Create_FourthLevel_ThrowsValidation()
        {
            Assert.Throws<ValidationFailedException>(() => _service.Create(TestData.Olena, "Седани", TestData.PassengerCars));
        }

        [Fact]
        public void Create_DuplicateNameOnSameLevel_ThrowsValidation()
        {
            Assert.Throws<ValidationFailedException>(() => _service.Create(TestData.Olena, "авто", TestData.Transport));
        }

        [Fact]
        public void Create_UnknownParent_ThrowsNotFound()
        {
            Assert.Throws<NotFoundException>(() => _service.Create(TestData.Olena, "Нова", 999));
        }

        [Fact]
        public void Create_UnregisteredUser_ThrowsAccessDenied()
        {
            Assert.Throws<AccessDeniedException>(() => _service.Create(99, "Нова", null));
        }
    }
}
