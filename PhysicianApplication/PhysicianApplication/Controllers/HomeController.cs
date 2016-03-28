﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhysicianApplication.Service;
using System.Net;
using System.Net.Http;

namespace PhysicianApplication.Controllers
{
    public class HomeController : Controller
    {
        PhysicianEntities context;
        PhysicianViewService physicianViewService;
        PhysicianDeleteService physicianDeleteService;

        public ActionResult Index()
        {
            physicianViewService = new PhysicianViewService();
            var dataToView = physicianViewService.ViewAllPhysicians();
            return View(dataToView);
        }
        public ActionResult Delete(Guid id)
        {
            physicianDeleteService = new PhysicianDeleteService();
            physicianDeleteService.DeleteForID(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            context = new PhysicianEntities();
            var toEditRecord = context.Physicians.Single(x => x.ID == id);
            ViewBag.HospitalRecord = context.Hospitals;
            ViewBag.SpecialtyRecord = context.Specialties;
            return View("Form", toEditRecord);
        }

        [HttpPost]
        public ActionResult Edit(Physician a)
        {
            using (context = new PhysicianEntities())
            {
                if (ModelState.IsValid)
                {
                    context.Physicians.Attach(a);
                    context.Entry<Physician>(a).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.HospitalRecord = context.Hospitals;
                    ViewBag.SpecialtyRecord = context.Specialties;
                    return View("Form", a);
                }
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            var context = new PhysicianEntities();
            ViewBag.HospitalRecord = context.Hospitals;
            ViewBag.SpecialtyRecord = context.Specialties;
            return View("Form");
        }

        [HttpPost]
        public ActionResult Create(Physician a)
        {
            using (context = new PhysicianEntities())
            {
                if (ModelState.IsValid)
                {
                    a.ID = Guid.NewGuid();
                    context.Physicians.Add(a);
                    context.Entry<Physician>(a).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var dataFetchContext = new PhysicianEntities();
                    ViewBag.HospitalRecord = dataFetchContext.Hospitals;
                    ViewBag.SpecialtyRecord = dataFetchContext.Specialties;
                    return View("Form", a);
                }
            }
        }
    }
}
