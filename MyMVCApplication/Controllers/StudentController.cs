﻿using MyMVCApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyMVCApplication.Controllers
{
    public class StudentController : Controller
    {
        public IList<Student> Student = new List<Student>() {
           new Student() { StudentId = 1, StudentName = "John", Age = 18 } ,
           new Student() { StudentId = 2, StudentName = "Steve",  Age = 21 } ,
           new Student() { StudentId = 3, StudentName = "Bill",  Age = 25 } ,
           new Student() { StudentId = 4, StudentName = "Ram" , Age = 20 } ,
           new Student() { StudentId = 5, StudentName = "Ron" , Age = 31 } ,
           new Student() { StudentId = 4, StudentName = "Chris" , Age = 17 } ,
           new Student() { StudentId = 4, StudentName = "Rob" , Age = 19 }
        };

        // GET: Student
        public ActionResult Index()
        {
            return View(Student);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Student studen = Student.SingleOrDefault(x => x.StudentId == id);

            return View(studen);
        }

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            bool check = true;

            // Verifico las property
            if (ModelState.IsValid)
            {
                if (check)
                {
                    // Agrego resumen de errores
                    ModelState.AddModelError(String.Empty, "El dato ya existe en la base de datos");

                    return View(student);
                }

                Student.Add(student);

                return RedirectToAction("Index");
            }

            return View(student);
        }
    }
}