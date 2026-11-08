
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Bulky.Utility;
using BulkyWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]

    [Area("Admin")]
    public class ProductController : Controller
    {
        public readonly IUnitOfWork _IUnitOfWork;
        public readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(
            IUnitOfWork IUnitOfWork,
            IWebHostEnvironment _WebHostEnvironment,
            UserManager<ApplicationUser> userManager)
        {
            _IUnitOfWork = IUnitOfWork;
            this._WebHostEnvironment = _WebHostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Product> objProductList =
                _IUnitOfWork.Product.GetAll("Category").ToList();

            return View(objProductList);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryList =
                _IUnitOfWork.Category.GetAll().ToList().Select(u =>
                    new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });

            ProductViewModel prod = new ProductViewModel()
            {
                Product = new Product(),
                CategoryList = categoryList
            };

            if (id == 0 || id == null)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                prod.SellerPhoneNumber = currentUser?.PhoneNumber;
                return View(prod);
            }
            else
            {
                prod.Product = _IUnitOfWork.Product.Get(u => u.Id == id);
                if (prod.Product == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrWhiteSpace(prod.Product.SellerId))
                {
                    var seller = await _userManager.FindByIdAsync(prod.Product.SellerId);
                    prod.SellerPhoneNumber = seller?.PhoneNumber;
                }

                return View(prod);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductViewModel productVM, IFormFile? file)
        {
            bool isNewProduct = productVM.Product.Id == 0;

            if (ModelState.IsValid)
            {
                string WebRootPath = _WebHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string FileName = Guid.NewGuid().ToString()
                                    + Path.GetExtension(file.FileName);

                    string ProductPath = Path.Combine(
                        WebRootPath, @"Images\product");

                    if (!Directory.Exists(ProductPath))
                    {
                        Directory.CreateDirectory(ProductPath);
                    }

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // Delete the old image
                        var oldImagePath = Path.Combine(
                            WebRootPath,
                            productVM.Product.ImageUrl.TrimStart('\\', '/'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(
                        Path.Combine(ProductPath, FileName),
                        FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl =
                        @"\Images\product\" + FileName;
                }

                if (isNewProduct)
                {
                    productVM.Product.SellerId = _userManager.GetUserId(User);

                    if (!string.IsNullOrWhiteSpace(productVM.SellerPhoneNumber))
                    {
                        var currentUser = await _userManager.GetUserAsync(User);
                        if (currentUser == null)
                        {
                            return Challenge();
                        }

                        currentUser.PhoneNumber = productVM.SellerPhoneNumber;
                        var phoneUpdateResult = await _userManager.UpdateAsync(currentUser);
                        if (!phoneUpdateResult.Succeeded)
                        {
                            foreach (var error in phoneUpdateResult.Errors)
                            {
                                ModelState.AddModelError(
                                    nameof(productVM.SellerPhoneNumber),
                                    error.Description);
                            }

                            productVM.CategoryList =
                                _IUnitOfWork.Category.GetAll().ToList().Select(category =>
                                    new SelectListItem
                                    {
                                        Text = category.Name,
                                        Value = category.Id.ToString()
                                    });

                            return View(productVM);
                        }
                    }

                    _IUnitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    var existingProduct = _IUnitOfWork.Product.Get(product =>
                        product.Id == productVM.Product.Id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    productVM.Product.SellerId = existingProduct.SellerId;
                    _IUnitOfWork.Product.Update(productVM.Product);
                }

                _IUnitOfWork.Save();

                TempData["success"] = isNewProduct
                    ? "Successfully created"
                    : "Successfully updated";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM.CategoryList =
                    _IUnitOfWork.Category.GetAll().ToList().Select(category =>
                        new SelectListItem
                        {
                            Text = category.Name,
                            Value = category.Id.ToString()
                        });

                return View(productVM);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            Product cat = _IUnitOfWork.Product.Get(u => u.Id == id);

            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        [HttpPost]
        public IActionResult Delete(Product obj)
        {
            var product = _IUnitOfWork.Product.Get(u => u.Id == obj.Id);

            if (product == null)
            {
                return NotFound();
            }

            _IUnitOfWork.Product.Remove(product);
            _IUnitOfWork.Save();

            TempData["success"] = "Successfully deleted";
            return RedirectToAction(nameof(Index));
        }

        #region Api Call
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList =
                _IUnitOfWork.Product.GetAll("Category").ToList();
            return Json(new { data = objProductList });
        }
        #endregion 
    }
}

//using Bulky.DataAccess.Data;
//using Bulky.DataAccess.Repository;
//using Bulky.DataAccess.Repository.IRepository;
//using Bulky.Models;
//using Bulky.Models.ViewModel;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace BulkyWeb.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class ProductController : Controller
//    {
//        public readonly IUnitOfWork _IUnitOfWork;
//        public readonly IWebHostEnvironment _WebHostEnvironment;
//        public ProductController(IUnitOfWork IUnitOfWork, IWebHostEnvironment _WebHostEnvironment)
//        {
//            _IUnitOfWork = IUnitOfWork;
//            this._WebHostEnvironment = _WebHostEnvironment;
//        }
//        public IActionResult Index()
//        {
//            List<Product> objProductList = _IUnitOfWork.Product.GetAll().ToList();
//            return View(objProductList);
//        }
//        public IActionResult Upsert(int? id)
//        {
//            IEnumerable<SelectListItem> categoryList = _IUnitOfWork.Category.GetAll().ToList().Select(u =>
//            new SelectListItem
//            {
//                Text = u.Name,
//                Value = u.Id.ToString()
//            });
//            // ViewBag.CategoryList = categoryList;
//            ProductViewModel prod = new ProductViewModel()
//            {
//                Product = new Product(),
//                CategoryList = categoryList


//            };

//            if (id == 0 || id == null)
//            {
//                return View(prod);
//            }
//            else
//            {
//                prod.Product = _IUnitOfWork.Product.Get(u => u.Id == id);
//                return View(prod);
//            }
            
//        }

//        [HttpPost]
//        public IActionResult Upsert(ProductViewModel productVM,IFormFile file)
//        { 
//            if (ModelState.IsValid)
//            {
//                String WebRootPath = _WebHostEnvironment.WebRootPath;

//                if (file != null)
//                {
//                    String FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
//                    String ProductPath = Path.Combine(WebRootPath, @"Images\product");

//                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
//                    {
//                        // delete the old image
//                        var oldImagePath =
//                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

//                        if (System.IO.File.Exists(oldImagePath))
//                        {
//                            System.IO.File.Delete(oldImagePath);
//                        }
//                    }

//                    using (var fileStream = new FileStream(Path.Combine(ProductPath, FileName), FileMode.Create))
//                    {
//                        file.CopyTo(fileStream);
//                    }
//                    productVM.Product.ImageUrl = @"\images\product\" + FileName;
//                }

//                if (productVM.Product.Id == 0)
//                {
//                    _IUnitOfWork.Product.Add(productVM.Product);
//                }
//                else
//                {
//                    _IUnitOfWork.Product.Update(productVM.Product);
//                }
//                _IUnitOfWork.Save();
//                    TempData["success"] = "sucessfuly created";
//                    return RedirectToAction(nameof(Index));
//             }
//            else
//            {
//                productVM.CategoryList = _IUnitOfWork.Category.GetAll().ToList().Select(category =>
//                    new SelectListItem
//                    {
//                        Text = category.Name,
//                        Value = category.Id.ToString()
//                    });
//                return View(productVM);
//            }
//        }
//        public IActionResult Delete(int? id)
//        {
//            if (id == null) return NotFound();
//            Product cat = _IUnitOfWork.Product.Get(u => u.Id == id);
//            if (cat == null) { return NotFound(); }
//            return View(cat);
//        }

//        [HttpPost]
//        public IActionResult Delete(Product obj)
//        {
//            var product = _IUnitOfWork.Product.Get(u => u.Id == obj.Id);
//            if (product == null) { return NotFound(); }

//            _IUnitOfWork.Product.Remove(product);
//            _IUnitOfWork.Save();
//            TempData["success"] = "sucessfuly Deleted";
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
