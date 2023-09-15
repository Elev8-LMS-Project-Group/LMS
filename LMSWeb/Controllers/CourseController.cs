﻿using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMSWeb.Controllers
{
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll();
            return View(courseList);
        }
    }
}