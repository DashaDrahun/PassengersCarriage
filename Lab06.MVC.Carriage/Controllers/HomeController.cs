using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.ModelBuilders;
using Lab06.MVC.Carriage.Models;

namespace Lab06.MVC.Carriage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IModelBuilder modelBuilder;

        public HomeController(IModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        [HttpGet]
        public ActionResult Error(HttpStatusCode? id)
        {
            var httpStatusCode = (int)(id ?? HttpStatusCode.InternalServerError);
            Response.StatusCode = httpStatusCode;

            string message;

            switch (id)
            {
                case HttpStatusCode.Unauthorized:
                    message =
                        "The request has not been applied because it lacks valid authentication credentials for the target resource.";
                    break;
                case HttpStatusCode.Forbidden:
                    message = "The server understood the request but refuses to authorize it.";
                    break;
                case HttpStatusCode.NotFound:
                    message =
                        "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.";
                    break;
                default:
                    message =
                        "The server encountered an unexpected condition that prevented it from fulfilling the request.";
                    break;
            }

            var model = new ErrorViewModel
            {
                Title = $"{httpStatusCode} {id}",
                Message = message,
            };

            return View(model);
        }

        public ActionResult Index()
        {
            var model = modelBuilder.Build();

            return View(model);
        }

        public ActionResult Test()
        {
            throw new ArgumentNullException("Test mistake");
        }
    }
}