﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();

            if (id != null)
                frequency = _unitOfWork.Frequency.Get(id.GetValueOrDefault());

            return View(frequency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if (!ModelState.IsValid)
                return View(frequency);

            if (frequency.Id == 0)
            {
                _unitOfWork.Frequency.Add(frequency);
            }
            else
            {
                _unitOfWork.Frequency.Update(frequency);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Frequency.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var FreqFromDb = _unitOfWork.Frequency.Get(id);

            if (FreqFromDb == null)
                return Json(new { success = false, message = "Error while deleting" });

            _unitOfWork.Frequency.Remove(FreqFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Frequency Deleted" });
        }

        #endregion
    }
}
