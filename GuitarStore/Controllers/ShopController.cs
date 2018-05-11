using GuitarStore.Models.Data;
using GuitarStore.Models.ViewModels.Shop;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuitarStore.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            // Declare list of CategoryVM
            List<CategoryVM> categoryVMList;

            // Init the list
            using (Db db = new Db())
            {
                categoryVMList = db.Categories.ToArray()
                                 .OrderBy(x => x.Sorting)
                                 .Select(x => new CategoryVM(x))
                                 .ToList();
            }

            return PartialView(categoryVMList);
        }

        // Get: shop/category/name
        public ActionResult Category(string name)
        {
            // Declare a list of ProductVM
            List<ProductVM> productVMList;

            using (Db db = new Db())
            {
                // Get category id
                CategoryDTO categoryDTO = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;

                // Init the list
                productVMList = db.Products.ToArray().Where(x => x.CategoryId == catId).Select(x => new ProductVM(x)).ToList();

                // Get category name
                var productCat = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();
                ViewBag.CategoryName = productCat.CategoryName;
            }

            return View(productVMList);
        }

        // Get: shop/product-details/name
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            // Declare the VM and DTO
            ProductVM model;
            ProductDTO dto;

            // Init product id
            int id = 0;

            using (Db db = new Db())
            {
                // Check if product exists
                if (!db.Products.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                // Init productDTO
                dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();

                // Get id
                id = dto.Id;

                // Init model
                model = new ProductVM(dto);
            }

            // Get gallery images
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                              .Select(fn => Path.GetFileName(fn));

            return View("ProductDetails", model);
        }

        // Get: shop/allproducts
        public ActionResult AllProducts()
        {
            // Declare a list of ProductVM
            List<ProductVM> productVMList;

            using (Db db = new Db())
            {
                // Init the list
                productVMList = db.Products.ToArray().Select(x => new ProductVM(x)).ToList();
            }

            return View(productVMList);
        }

        public ActionResult Search(string searchString)
        {
            List<ProductVM> productVMList;

            using (Db db = new Db())
            {
                if (!db.Products.Any(x => x.Slug.Contains(searchString)))
                {
                    TempData["SM"] = "No results found.";
                    return RedirectToAction("Index", "Shop");
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    productVMList = db.Products.ToArray().Where(x => x.Slug.Contains(searchString)).Select(x => new ProductVM(x)).ToList();
                }
                else
                {
                    productVMList = db.Products.ToArray().Select(x => new ProductVM(x)).ToList();
                }

            }
            return View(productVMList);
        }
    }
}