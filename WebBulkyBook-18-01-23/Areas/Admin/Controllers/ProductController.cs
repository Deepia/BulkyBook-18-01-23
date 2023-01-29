using Microsoft.AspNetCore.Mvc;
using BulkyBooks.Models;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebBulkyBook_18_01_23.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }
        

        public IActionResult Upsert(int? id)
        {
            Product product = new();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value=u.Id.ToString()
                });
            IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            if (id==null || id==0)
            {
                //Create Product
                ViewBag.CategoryList = CategoryList;
                ViewData["CoverTypeList"] = CoverTypeList;
                return View(product);
            }
            else
            {
                //update product
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType CoverType)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(CoverType);
                _unitOfWork.Save();
                TempData["success"] = "CoverType updated successfuly";
                return RedirectToAction("Index");
            }

            return View(CoverType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var CoverType = _unitOfWork.CoverType.GetFirstOrDefault(u=>u.Id==id);
            if (CoverType == null)
            {
                return NotFound();
            }
            return View(CoverType);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var CoverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == Id);
            if (CoverType == null)
            {
                return NotFound();
            }
            _unitOfWork.CoverType.Remove(CoverType);
            _unitOfWork.Save();
            TempData["success"] = "CoverType deleted successfuly";
            return RedirectToAction("Index");
        }

    }
}
