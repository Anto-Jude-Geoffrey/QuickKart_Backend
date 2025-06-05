using QuickKartDB.DataAccessLayer.Models;

namespace QuickKartDB.DataAccessLayer
{
    public interface IQuickKartRepository
    {
        bool AddCategories(Category category);
        public int ValidateLogin(string EmailID, string Password);
        int AddCategoryDetailUsingUSP(string categoryName, out byte categoryID);
        string AddProducts(Product product);
        bool CheckEmailID(string emailID);
        bool DeleteCategories(byte categoryID);
        string DeleteUser(string EmailId, string UserPassword);
        string deleteProducts(string productID);
        List<Category> GetAllCategories();
        List<Product> GetAllProducts();
        List<ProductCategory> GetProductCategoryDetailsTVF(int categoryID, out List<ProductCategory> FromInterpolated);
        Product GetProductDetail(string ProductID);
        bool UpdateCategory(byte categoryID, string categoryName);
        string UpdateProducts(Product product);
    }
}