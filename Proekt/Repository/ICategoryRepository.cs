using Proekt.Entites;

namespace Proekt.Repository
{
    public interface ICategoryRepository
    {
        public void CreateCategory(Category category);
        public IEnumerable<Category> GetAllCategoris();
        public IEnumerable<TaskE> GetCategoryById(int categoriId, int userId);

    }
}
