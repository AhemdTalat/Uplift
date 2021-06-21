using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Models.ViewModels;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _hostEnvironment;

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment HostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = HostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ServiceVM serviceVM = new ServiceVM()
            {
                Service = new Service(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown()
            };

            if (id != null)
                serviceVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());

            return View(serviceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ServiceVM serviceVM)
        {
            if (ModelState.IsValid)
            {
                var webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (serviceVM.Service.Id == 0)
                {
                    var name = Guid.NewGuid().ToString();
                    var path = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(path, name + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    serviceVM.Service.ImageUrl = @"\images\services\" + name + extension;

                    _unitOfWork.Service.Add(serviceVM.Service);
                }
                else
                {
                    var serviceFromDb = _unitOfWork.Service.Get(serviceVM.Service.Id);

                    if (files.Count() > 0)
                    {
                        var name = Guid.NewGuid().ToString();
                        var path = Path.Combine(webRootPath, @"images\services");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var oldImage = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }

                        using (var fileStream = new FileStream(Path.Combine(path, name + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        serviceVM.Service.ImageUrl = @"\images\services\" + name + extension_new;

                    }
                    else
                    {
                        serviceVM.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }

                    _unitOfWork.Service.Update(serviceVM.Service);

                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            serviceVM.CategoryList = _unitOfWork.Category.GetCategoryListForDropDown();
            serviceVM.FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown();

            return View(serviceVM);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(IncludeProperties: "Category,Frequency") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var serviceFromDB = _unitOfWork.Service.Get(id);

            if (serviceFromDB == null)
                return Json(new { success = false, message = "Error while deleting" });

            var webRootPath = _hostEnvironment.WebRootPath;
            var oldImage = Path.Combine(webRootPath, serviceFromDB.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }

            _unitOfWork.Service.Remove(serviceFromDB);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Service Deleted Successfully" });
        }


        #endregion
    }
}
