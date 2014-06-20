using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        
        public ViewResult Index()
        {

            return View(repository.Products);
        }

        public ViewResult Edit(int productid)
        {
            Product product = repository.Products.FirstOrDefault(item => item.ProductID == productid);

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            ActionResult action;

            if(ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                action = RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data value
                action = View(product);
            }
             
            return action;
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product detletedProduct = repository.DelteProduct(productId);
            if(detletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted", detletedProduct.Name);
            }

            return RedirectToAction("Index");
        }
    }
}