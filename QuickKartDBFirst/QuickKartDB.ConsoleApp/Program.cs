using QuickKartDB.DataAccessLayer;
using QuickKartDB.DataAccessLayer.Models;

class Program
{
    static QuickKartDBContext context;
    static QuickKartRepository repository;

    static Program()
    {
        context = new QuickKartDBContext();
        repository= new QuickKartRepository(context);
    }

    static void Main(string[] args)
    {
        string repeat;
        do
        {
            Console.Write("Choose an CRUD Operation/Stored_Procedure/TVF/Function = ");
            String val = Console.ReadLine();

            switch (val)
            {
                case "Create":
                    Console.WriteLine("Enter the Category Name =");
                    String CategoryName = Console.ReadLine();
                    Category category1 = new Category();
                    category1.CategoryName = CategoryName;
                    bool status = repository.AddCategories(category1);
                    if (status)
                    {
                        Console.WriteLine("Added Successfully");
                    }
                    else
                    {
                        Console.WriteLine("Error Occurred");
                    }

                    break;
                case "Read":
                    Console.Write("Choose an Table (Category/Product) = ");
                    String table = Console.ReadLine();
                    switch (table)
                    {
                        case "Category":
                            var categories = repository.GetAllCategories();
                            foreach (var category in categories)
                            {
                                Console.WriteLine($"{category.CategoryId}\t{category.CategoryName}");
                            }
                            break;
                        case "Product":
                            var products = repository.GetAllProducts();
                            foreach (var product in products)
                            {
                                Console.WriteLine($"{product.ProductId}\t{product.ProductName}\t{product.CategoryId}\t{product.Price}\t{product.QuantityAvailable}");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid Input");
                            break;
                    }

                    break;
                case "Update":
                    Console.Write("Enter the Primary Key value to be Updated = ");
                    byte primaryVal1 = Byte.Parse(Console.ReadLine());
                    Console.Write("Enter the Value that needs to be updated = ");
                    string categoryName = Console.ReadLine();
                    if (repository.UpdateCategory(primaryVal1, categoryName))
                        Console.WriteLine("Updation Success");
                    else
                        Console.WriteLine("Updation Unsuccessful");
                    break;
                case "Delete":
                    Console.WriteLine("Enter the Primary Key value to be Deleted = ");
                    byte primaryVal = Byte.Parse(Console.ReadLine());
                    if (repository.DeleteCategories(primaryVal))
                        Console.WriteLine("Deletion Success");
                    else
                        Console.WriteLine("Deletion Unsuccessful");
                    break;

                case "Stored_Procedure":
                    Console.Write("Enter Category Name to Execute usp_AddCategory = ");
                    string categoryName1= Console.ReadLine();
                    byte categoryID=0;
                    int result=repository.AddCategoryDetailUsingUSP(categoryName1, out categoryID);
                    if(result >0)
                        Console.WriteLine($"Category Name added succesfully with Category ID = {categoryID}");
                    else
                        Console.WriteLine("Execution Failed");
                    break;

                case "TVF":
                    List<ProductCategory> TVFList1, TVFList2;
                    Console.Write("Enter the Category ID to Call Table Valued Function = ");
                    int categoryID1 =Convert.ToInt32(Console.ReadLine());
                    TVFList1 = repository.GetProductCategoryDetailsTVF(categoryID1, out TVFList2);
                    Console.WriteLine("\nUsing FromSqlRaw() ...\n");
                    foreach(var TVF1 in  TVFList1)
                        Console.WriteLine($"{TVF1.ProductId}\t{TVF1.ProductName}\t{TVF1.CategoryName}\t{TVF1.QuantityAvailable}");
                    Console.WriteLine("\nUsing FromSqlInterpolated() ...\n");
                    foreach (var TVF2 in TVFList2)
                        Console.WriteLine($"{TVF2.ProductId}\t{TVF2.ProductName}\t{TVF2.CategoryName}\t{TVF2.QuantityAvailable}");
                    break;

                case "Function":
                    Console.Write("Enter the EmailID to Check the DB = ");
                    string emailID=Console.ReadLine();
                    bool result1 = repository.CheckEmailID(emailID);
                    if(result1)
                        Console.WriteLine("Email ID Doesn't Exist in the Database");
                    else
                        Console.WriteLine("The Email ID Exist in the Database");
                    break ;


                default:
                    Console.WriteLine("Invalid Input Data");
                    break;
            }

            Console.Write("Do you Wish to Continue (Y/N) = ");
            repeat= Console.ReadLine();

        }while(repeat=="Y");
        
    }
}