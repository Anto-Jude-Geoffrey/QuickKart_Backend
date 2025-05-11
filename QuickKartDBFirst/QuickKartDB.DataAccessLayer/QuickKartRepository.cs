using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickKartDB.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickKartDB.DataAccessLayer
{
    public class QuickKartRepository : IQuickKartRepository
    {
        QuickKartDBContext context;
        public QuickKartRepository(QuickKartDBContext context)
        {
            this.context = context;
        }

        public List<Category> GetAllCategories()
        {
            var result = context.Categories.OrderBy(c => c.CategoryId).ToList();

            return result;
        }

        public int ValidateLogin(string EmailID, string Password)
        {
            if(context.Users.Find(EmailID).Equals(null))
            {  return -1; }
            else
            {
                if(context.Users.Where(u=> u.EmailId==EmailID).Select(u=>u.UserPassword).FirstOrDefault().ToString()==Password)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<Product> GetAllProducts()
        {
            var result = context.Products.AsNoTracking().ToList();

            return result;
        }

        public Product GetProductDetail(string ProductID)
        {
            var result = context.Products.Find(ProductID);

            return result;
        }

        public bool AddCategories(Category category)
        {
            bool status = false;
            try
            {
                context.Categories.Add(category);
                context.SaveChanges();
                status = true;
            }
            catch (Exception ex) { status = false; }
            return status;
        }

        public string AddProducts(Product product)
        {
            string status = "";
            try
            {
                context.Products.Add(product);
                context.SaveChanges();
                status = "Success";
            }
            catch (Exception ex) { status = ex.ToString(); }
            return status;
        }

        public bool DeleteCategories(byte categoryID)
        {
            bool status = false;
            Category category = context.Categories.Find(categoryID);
            try
            {
                context.Categories.Remove(category);
                context.SaveChanges();
                status = true;
            }
            catch (Exception e) { status = false; }
            return status;
        }

        public string deleteProducts(string productID)
        {
            string msg = "";
            Product product;
            try
            {
                product = context.Products.Find(productID);
                context.Products.Remove(product);
                context.SaveChanges();
                msg = "Deletion Successfull";
            }
            catch (Exception ex) { msg = ex.ToString(); }
            return msg;
        }

        public bool UpdateCategory(byte categoryID, string categoryName)
        {
            bool status = false;
            try
            {
                Category category = context.Categories.Find(categoryID);
                category.CategoryName = categoryName;
                using (QuickKartDBContext newcontext = new QuickKartDBContext())
                {
                    newcontext.Categories.Update(category);
                    newcontext.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex) { status = false; }
            return status;
        }

        public string UpdateProducts(Product product)
        {
            string status = "";
            try
            {
                using (QuickKartDBContext newContext = new QuickKartDBContext())
                {
                    newContext.Products.Update(product);
                    newContext.SaveChanges();
                    status = "Successfully Updated";
                }
            }
            catch (Exception ex) { status = ex.Message; }
            return status;

        }

        //Stored Procedure Execution
        public int AddCategoryDetailUsingUSP(string categoryName, out byte categoryID)
        {
            int returnValue = 0;
            categoryID = 0;
            int modifiedRows = 0;
            SqlParameter prmCategoryName = new SqlParameter("@CategoryName", categoryName);
            SqlParameter prmCategoryID = new SqlParameter("@CategoryId", System.Data.SqlDbType.TinyInt);
            prmCategoryID.Direction = System.Data.ParameterDirection.Output;
            SqlParameter prmReturnValue = new SqlParameter("@ReturnValue", System.Data.SqlDbType.Int);
            prmReturnValue.Direction = System.Data.ParameterDirection.Output;
            try
            {
                modifiedRows = context.Database.ExecuteSqlRaw("EXEC @ReturnValue = usp_AddCategory @CategoryName, @CategoryId OUT", prmReturnValue, prmCategoryName, prmCategoryID);
                returnValue = Convert.ToInt32(prmReturnValue.Value);
                categoryID = Convert.ToByte(prmCategoryID.Value);
            }
            catch (Exception ex)
            {
                returnValue = -99;
                modifiedRows = -1;
                categoryID = 0;
            }
            return returnValue;
        }

        //Invoke Table valued Function
        public List<ProductCategory> GetProductCategoryDetailsTVF(int categoryID, out List<ProductCategory> FromInterpolated)
        {
            List<ProductCategory> FromRaw;
            try
            {
                //Using FromSqlRaw()
                SqlParameter prmCategoryID = new SqlParameter("@CategoryId", categoryID);
                FromRaw = context.ProductCategories.FromSqlRaw("SELECT * FROM ufn_GetProductCategoryDetails(@CategoryId)", prmCategoryID).ToList();

                //Using FromSqlInterpolated() => this doesn't need SqlParameter initialization but need $ format in beginning
                FromInterpolated = context.ProductCategories.FromSqlInterpolated($"SELECT * FROM ufn_GetProductCategoryDetails({categoryID})").ToList();
            }
            catch (Exception ex)
            {
                FromRaw = null;
                FromInterpolated = null;
            }
            return FromRaw;

        }

        //Invoke Scalar Function

        public bool CheckEmailID(string emailID)
        {
            bool res = false;
            try
            {
                res = (from s in context.Users select QuickKartDBContext.ufn_CheckEmailId(emailID)).FirstOrDefault();

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }



    }
}
