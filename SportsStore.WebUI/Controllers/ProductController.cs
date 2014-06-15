using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        internal int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }


        public ViewResult List(int page = 1)
        {
            return View(repository.Products.OrderBy(product => product.ProductID).Skip((page - 1) * PageSize).Take(PageSize));
        }


    }
}