using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Authorize(Roles =SD.Role_Admin)]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public readonly IUnitOfWork _IUnitOfWork;
        public CategoryController(IUnitOfWork IUnitOfWork)
        {
            _IUnitOfWork = IUnitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _IUnitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Both Name and DisplayOrder cannot be same");
                //name--inout field name
            }
            if (obj.Name!=null && obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "Name cannot be a test");
                //no field specified so we it will be displayed in asp-validated-summary
            }
            if (ModelState.IsValid)
            {
                _IUnitOfWork.Category.Add(obj);
                _IUnitOfWork.Save();
                TempData["success"] = "sucessfuly created";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            Category cat = _IUnitOfWork.Category.Get(u => u.Id == id);
            if (cat == null) { return NotFound(); }
            return View(cat);
        }

        [HttpPost,ActionName("Edit")]
        public IActionResult _Edit(Category obj)
        { 
            if (ModelState.IsValid)
            {
                _IUnitOfWork.Category.Update(obj);
                _IUnitOfWork.Save();
                TempData["success"] = "sucessfuly Edited";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            Category cat = _IUnitOfWork.Category.Get(u => u.Id == id);
            if (cat == null) { return NotFound(); }
            return View(cat);
        }

        [HttpPost]
        public IActionResult Delete(Category obj)
        {
            _IUnitOfWork.Category.Remove(obj);
            _IUnitOfWork.Save();
            TempData["success"] = "sucessfuly Deleted";
            return RedirectToAction("Index", "Category");
        }
    }
}
