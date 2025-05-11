using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickKartDB.DataAccessLayer;
using System.Collections.Generic;
using QuickKartDB.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;



namespace QuickKartServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IQuickKartRepository repository;
        private readonly TokenService _tokenService;

        public ProductController(IQuickKartRepository repository, TokenService tokenService)
        {
            this.repository = repository;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult Login(User request)
        {
            if (repository.ValidateLogin(request.EmailId,request.UserPassword)==1)
            {
                var token = _tokenService.GenerateToken(request.EmailId);
                return Ok(new { Token = token });
            }
            else { return Unauthorized(); }
            
        }

        
        [HttpGet]
        public JsonResult GetAllCategories()
        {
            List<Category> category = new List<Category>();
            try
            {
                category = repository.GetAllCategories();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                category = null;
            }
            return Json(category);
        }

        [HttpGet]
        public JsonResult GetAllProducts()
        {
            List<Product> product = new List<Product>();
            try
            {
                product = repository.GetAllProducts();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                product = null;
            }
            return Json(product);
        }

        [HttpGet]
        public JsonResult GetProductDetails(string ProductId)
        {
            Product product;
            try
            {
                product = repository.GetProductDetail(ProductId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                product = null;
            }
            return Json(product);
        }

        [HttpPost]
        public JsonResult AddProducts(Product product)
        {
            bool status=false;
            string msg="";
            try
            {
                msg = repository.AddProducts(product);
       
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(msg);
        }

        [HttpPut]
        public JsonResult UpdateProduct(Product product) 
        {
            //bool status=false;
            string msg;
            try
            {
                msg= repository.UpdateProducts(product);
            }catch(Exception ex)
            {
                //status = false;
                msg = ex.Message;
            }
            return Json(msg);
        }

        [HttpDelete]
        public JsonResult DeleteProduct(string ProductId) 
        {
            string msg;
            try
            {
                repository.deleteProducts(ProductId);
                msg = "Delete Successful";
            }catch(Exception ex)
            { msg = ex.Message; }
            return Json(msg);
        }
            

    }
}
