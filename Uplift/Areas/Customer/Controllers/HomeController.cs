﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeViewModel HomeVM = new HomeViewModel()
            {
                CategoryList = _unitOfWork.Category.GetAll(),
                ServiceList = _unitOfWork.Service.GetAll(IncludeProperties: "Frequency")
            };

            return View(HomeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var service = _unitOfWork.Service.GetFirstOrDefault(IncludeProperties: "Category,Frequency", filter: s => s.Id == id);
            return View(service);
        }

        public IActionResult AddToCart(int serviceId)
        {
            List<int> sessionList = new List<int>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                sessionList.Add(serviceId);
                HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            }
            else
            {
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                if (!sessionList.Contains(serviceId))
                {
                    sessionList.Add(serviceId);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
