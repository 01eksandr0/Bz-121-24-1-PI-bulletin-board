using BulletinBoard.BLL.Exceptions;
using BulletinBoard.BLL.Models;
using BulletinBoard.BLL.Services;
using BulletinBoard.BLL.Tests.Fakes;
using Xunit;

namespace BulletinBoard.BLL.Tests
{
    public class UserAndTagServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();

        [Fact]
        public void Register_NewUser_StoresLowercaseEmail()
        {
            UserModel user = new UserService(_uow).Register("Марія", "  Maria@Mail.com ");

            Assert.Equal("maria@mail.com", user.Email);
        }

        [Fact]
        public void Register_ExistingEmailInOtherCase_ThrowsValidation()
        {
            Assert.Throws<ValidationFailedException>(() => new UserService(_uow).Register("Інша", "OLENA@mail.com"));
        }

        [Fact]
        public void Register_InvalidEmail_ThrowsValidation()
        {
            Assert.Throws<ValidationFailedException>(() => new UserService(_uow).Register("Марія", "maria.mail.com"));
        }

        [Fact]
        public void GetById_Unknown_ThrowsNotFound()
        {
            Assert.Throws<NotFoundException>(() => new UserService(_uow).GetById(99));
        }

        [Fact]
        public void CreateTag_Duplicate_ThrowsValidation()
        {
            Assert.Throws<ValidationFailedException>(() => new TagService(_uow).Create(TestData.Olena, "ТОРГ"));
        }

        [Fact]
        public void CreateTag_New_StoresLowercase()
        {
            TagModel tag = new TagService(_uow).Create(TestData.Olena, "Самовивіз");

            Assert.Equal("самовивіз", tag.Name);
        }
    }
}
