using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }
            
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
                return View(category);

            category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            if (category.Id == 0)
            {
                _unitOfWork.Category.Add(category);
            }
            else
            {
                _unitOfWork.Category.Update(category);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            //return Json(new { data = _unitOfWork.Category.GetAll()});
            return Json(new { data = _unitOfWork.SP_Calls.RetunList<Category>(SD.usp_GetAllCategory, null)}); // using StoredProcedure
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CategoryFromDb = _unitOfWork.Category.Get(id);
            if (CategoryFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting"});
            }
            _unitOfWork.Category.Remove(CategoryFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Category Deleted" });
        }
        #endregion
    }
}
