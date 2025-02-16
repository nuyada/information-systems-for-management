using Proekt.Entites;
using Proekt.Repository;

namespace Proekt.Service
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void CreateCategory(string name)
        {
            var category = new Category { Name = name };
            _categoryRepository.CreateCategory(category);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategoris();
        }

        public IEnumerable<TaskE> GetCategoryById(int categoryId,int userId)
        {
            return _categoryRepository.GetCategoryById(categoryId,userId);
        }
    }
}