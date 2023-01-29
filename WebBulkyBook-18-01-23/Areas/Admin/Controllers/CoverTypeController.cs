using Microsoft.AspNetCore.Mvc;
using BulkyBooks.Models;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;

namespace WebBulkyBook_18_01_23.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType CoverType)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(CoverType);
                _unitOfWork.Save();
                TempData["success"] = "CoverType inserted successfuly";
                return RedirectToAction("Index");
            }
            
            return View(CoverType);
        }

        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            var CoverType = _unitOfWork.CoverType.GetFirstOrDefault(x=>x.Id==id);
            if (CoverType == null)
            {
                return NotFound();
            }
            return View(CoverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType CoverType)
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
