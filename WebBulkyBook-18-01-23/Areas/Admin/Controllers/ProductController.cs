using Microsoft.AspNetCore.Mvc;
using BulkyBooks.Models;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBooks.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace WebBulkyBook_18_01_23.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            if (id==null || id==0)
            {
                //Create Product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                productVM.product=_unitOfWork.Product.GetFirstOrDefault(u=>u.Id==id);
                return View(productVM);
                //update product
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    if(obj.product.ImageUrl != null)
                    {
                        var oldimagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldimagePath))
                        {
                            System.IO.File.Delete(oldimagePath);
                        }
                    }
                    using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if (obj.product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.product);
                }
                
                _unitOfWork.Save();
                TempData["success"] = "Product created successfuly";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

      
        

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new {data=productList});
        }

        
        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == Id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
            var oldimagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldimagePath))
            {
                System.IO.File.Delete(oldimagePath);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful." });
        }
        #endregion

    }
}
