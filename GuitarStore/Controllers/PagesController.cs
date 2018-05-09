using GuitarStore.Models.Data;
using GuitarStore.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuitarStore.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index/{page}
        public ActionResult Index(string page = "")
        {
            // Get/Set page slug
            if (page == "")
                page = "home";

            // Declare model and DTO
            PageVM model;
            PageDTO dto;

            // Check if page exists
            using (Db db = new Db())
            {
                if (!db.Pages.Any(x => x.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }

            // Get page DTO
            using (Db db = new Db())
            {
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }

            // Set page title
            ViewBag.PageTitle = dto.Title;

            // Init model
            model = new PageVM(dto);

            return View(model);
        }

        public ActionResult PagesMenuPartial()
        {
            // Declare a list of PageVM
            List<PageVM> pageVMList;

            // Get all pages except home
            using (Db db=new Db())
            {
                pageVMList = db.Pages.ToArray()
                             .OrderBy(x => x.Sorting)
                             .Where(x => x.Slug != "home")
                             .Select(x => new PageVM(x))
                             .ToList();
            }

            // Return partial view with list
            return PartialView(pageVMList);
        }
    }
}