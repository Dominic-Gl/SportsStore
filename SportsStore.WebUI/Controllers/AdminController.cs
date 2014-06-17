using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
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
    }
}