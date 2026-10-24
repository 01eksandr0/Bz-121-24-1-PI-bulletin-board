using System.Collections.Generic;
using System.Linq;
using BulletinBoard.BLL.Exceptions;
using BulletinBoard.BLL.Models;
using BulletinBoard.BLL.Services;
using BulletinBoard.BLL.Tests.Fakes;
using Xunit;

namespace BulletinBoard.BLL.Tests
{
    public class AdServiceTests
    {
        private readonly InMemoryUnitOfWork _uow = TestData.Create();
        private readonly AdService _service;

        public AdServiceTests()
        {
            _service = new AdService(_uow);
        }

        private static AdInputModel ValidInput(params int[] tagIds)
        {
            return new AdInputModel
            {
                Title = "Продам диван",
                Description = "Майже новий",
                Price = 4500m,
                CategoryId = TestData.Electronics,
                TagIds = tagIds.ToList()
            };
        }

        [Fact]
        public void Create_ValidInput_AddsActiveAdWithTags()
        {
            AdModel ad = _service.Create(TestData.Olena, ValidInput(1, 2));

            Assert.True(ad.IsActive);
            Assert.Equal("Олена", ad.AuthorName);
            Assert.Equal(2, ad.Tags.Count);
            Assert.Equal(3, _uow.Ads.Count);
            Assert.Equal(1, _uow.SaveCount);
        }

        [Fact]
        public void Create_UnregisteredUser_ThrowsAccessDenied()
        {
            Assert.Throws<AccessDeniedException>(() => _service.Create(99, ValidInput()));
        }

        [Fact]
        public void Create_MoreThanFiveTags_ThrowsValidation()
        {
            Assert.Throws<ValidationFailedException>(() => _service.Create(TestData.Olena, ValidInput(1, 2, 3, 4, 5, 6)));
        }

        [Fact]
        public void Create_UnknownCategory_ThrowsNotFound()
        {
            AdInputModel input = ValidInput();
            input.CategoryId = 999;

            Assert.Throws<NotFoundException>(() => _service.Create(TestData.Olena, input));
        }

        [Fact]
        public void Create_ShortTitle_ThrowsValidationAndDoesNotSave()
        {
            AdInputModel input = ValidInput();
            input.Title = "ок";

            Assert.Throws<ValidationFailedException>(() => _service.Create(TestData.Olena, input));
            Assert.Equal(0, _uow.SaveCount);
        }

        [Fact]
        public void Update_ByAuthor_ChangesTitleAndTags()
        {
            AdInputModel input = ValidInput(4);
            input.Title = "Продам Golf, торг";

            AdModel ad = _service.Update(TestData.PetroAd, TestData.Petro, input);

            Assert.Equal("Продам Golf, торг", ad.Title);
            Assert.Equal(new[] { "б/в" }, ad.Tags.ToArray());
        }

        [Fact]
        public void Update_ByOtherUser_ThrowsAccessDenied()
        {
            Assert.Throws<AccessDeniedException>(() => _service.Update(TestData.PetroAd, TestData.Olena, ValidInput()));
        }

        [Fact]
        public void Deactivate_ByAuthor_MakesAdInactive()
        {
            _service.Deactivate(TestData.OlenaAd, TestData.Olena);

            Assert.False(_uow.Ads.GetById(TestData.OlenaAd).IsActive);
        }

        [Fact]
        public void Deactivate_ByOtherUser_ThrowsAccessDeniedAndKeepsActive()
        {
            Assert.Throws<AccessDeniedException>(() => _service.Deactivate(TestData.OlenaAd, TestData.Petro));
            Assert.True(_uow.Ads.GetById(TestData.OlenaAd).IsActive);
        }

        [Fact]
        public void Deactivate_Twice_ThrowsValidation()
        {
            _service.Deactivate(TestData.OlenaAd, TestData.Olena);

            Assert.Throws<ValidationFailedException>(() => _service.Deactivate(TestData.OlenaAd, TestData.Olena));
        }

        [Fact]
        public void Activate_AfterDeactivate_MakesAdActiveAgain()
        {
            _service.Deactivate(TestData.OlenaAd, TestData.Olena);
            _service.Activate(TestData.OlenaAd, TestData.Olena);

            Assert.True(_uow.Ads.GetById(TestData.OlenaAd).IsActive);
        }

        [Fact]
        public void Delete_ByAuthor_RemovesAd()
        {
            _service.Delete(TestData.PetroAd, TestData.Petro);

            Assert.Null(_uow.Ads.GetById(TestData.PetroAd));
        }

        [Fact]
        public void Delete_ByOtherUser_ThrowsAccessDeniedAndKeepsAd()
        {
            Assert.Throws<AccessDeniedException>(() => _service.Delete(TestData.PetroAd, TestData.Olena));
            Assert.NotNull(_uow.Ads.GetById(TestData.PetroAd));
        }

        [Fact]
        public void Search_ByRootCategory_IncludesSubcategories()
        {
            IReadOnlyList<AdModel> ads = _service.Search(new AdSearchFilter { CategoryId = TestData.Transport });

            Assert.Single(ads);
            Assert.Equal(TestData.PetroAd, ads[0].Id);
        }

        [Fact]
        public void Search_ByTextIgnoringCase_FindsAd()
        {
            IReadOnlyList<AdModel> ads = _service.Search(new AdSearchFilter { Text = "НОУТБУК" });

            Assert.Single(ads);
            Assert.Equal(TestData.OlenaAd, ads[0].Id);
        }

        [Fact]
        public void Search_DoesNotReturnInactiveAds()
        {
            _service.Deactivate(TestData.OlenaAd, TestData.Olena);

            IReadOnlyList<AdModel> ads = _service.Search(new AdSearchFilter());

            Assert.DoesNotContain(ads, a => a.Id == TestData.OlenaAd);
        }

        [Fact]
        public void GetById_InactiveAdForStranger_ThrowsNotFound()
        {
            _service.Deactivate(TestData.OlenaAd, TestData.Olena);

            Assert.Throws<NotFoundException>(() => _service.GetById(TestData.OlenaAd, TestData.Petro));
            Assert.Throws<NotFoundException>(() => _service.GetById(TestData.OlenaAd, null));
        }

        [Fact]
        public void GetById_InactiveAdForAuthor_ReturnsAd()
        {
            _service.Deactivate(TestData.OlenaAd, TestData.Olena);

            AdModel ad = _service.GetById(TestData.OlenaAd, TestData.Olena);

            Assert.False(ad.IsActive);
        }
    }
}
