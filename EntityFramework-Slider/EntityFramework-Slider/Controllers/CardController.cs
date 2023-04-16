using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;

namespace EntityFramework_Slider.Controllers
{
    public class CardController : Controller
    {
        private readonly AppDbContext _context;
        public CardController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            
            List <BasketVM> basketProducts; 

            if (Request.Cookies["basket"] != null)
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]); //eyer coockide data varsa yani null deyilse coockide olan datani goturub = DeserializeObject<List<BasketVM>>.esayn edirik elmizde olan List<BasketVM>e
            }
            else
            {
                basketProducts = new List<BasketVM>();   /*yoxdusa data teze List yaradir*/
            }
            List<BasketDetailVM> basketDetails = new();

            foreach(var product in basketProducts)
            {
                Product dbProduct =_context.Products.Include(m=>m.Images).Include(m => m.Category).FirstOrDefault(m => m.Id == product.Id);

                basketDetails.Add(new BasketDetailVM
                {

                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Description = dbProduct.Description,
                    Price = dbProduct.Price,
                    Image = dbProduct.Images.Where(m => m.IsMain).FirstOrDefault().Image,
                    Count = product.Count,
                    Total = dbProduct.Price * product.Count,               
                   CategoryName = dbProduct.Category.Name
                }); 
            }
            
            return View(basketDetails);

            //ThenInclude-Include elediyimizin icindeki Datani Include etmek ucun
        }

        #region Delete Product From Basket

        //[ActionName("Delete")]
        public IActionResult DeleteProductFromBasket(int? id)
        {
            if (id == null) return BadRequest();

            List<BasketVM> basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            BasketVM deletedProduct = basketProducts.FirstOrDefault(m => m.Id == id);

            basketProducts.Remove(deletedProduct);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

            return Ok();
        }
        #endregion


        public IActionResult DecreaseCountProductFromBasket(int? id)
        {

            List<BasketVM> basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            BasketVM decreaseProduct = basketProducts.Find(m => m.Id == id);

            if(decreaseProduct.Count > 1)
            {
                var countProduct = --decreaseProduct.Count;

                Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

                return Ok(countProduct);
            }

            return Ok();
        }

        public IActionResult IncreaseCountProductFromBasket(int? id)
        {

            List<BasketVM> basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            BasketVM? increaseProduct = basketProducts.Find(m => m.Id == id);
          
            
             var countProduct =  ++increaseProduct.Count;

                Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

           
            return Ok(countProduct);

        }
    }
}

