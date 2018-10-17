using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.Models;
using Microsoft.Owin.Security;

namespace Lab06.MVC.Carriage.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService identityService;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public AccountController(IIdentityService identityService)
        {
            this.identityService = identityService
                                  ?? throw new ArgumentNullException(nameof(identityService));
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel userModel = new UserModel
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Role = "User"
                };
                OperationDetails operationDetails = await identityService.Create(userModel);

                if (operationDetails.Succedeed)
                {
                    ClaimsIdentity claim = await identityService.Authenticate(userModel);

                    if (claim == null)
                    {
                        ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                    }
                    else
                    {
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(
                            new AuthenticationProperties
                            {
                                IsPersistent = false
                            },
                            claim);

                        return RedirectToAction("Start", "RegisteredUser");
                    }
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                }
            }

            return View(model);
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserModel userModel = new UserModel { Email = model.Email, Password = model.Password, Name = model.Name };
                ClaimsIdentity claim = await identityService.Authenticate(userModel);

                if (claim == null)
                {
                    ModelState.AddModelError(String.Empty, "Incorrect login or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(
                        new AuthenticationProperties
                        {
                            IsPersistent = false
                        },
                        claim);

                    if (String.IsNullOrEmpty(returnUrl))
                    {

                        if (claim.Claims.FirstOrDefault(c => c.Value == "Admin") != null)
                        {
                            return RedirectToAction("TripsWork", "Admin");
                        }

                        return RedirectToAction("Start", "RegisteredUser");
                    }
                }
            }

            ViewBag.returnUrl = returnUrl;

            return View(new RegisterViewModel { Email = model.Email, Password = model.Password, Name = model.Name });
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}