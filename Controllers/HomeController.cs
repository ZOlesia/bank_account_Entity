﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bank_accounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace bank_accounts.Controllers
{
    public class HomeController : Controller
    {
        private BankContext _context;
 
        public HomeController(BankContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("")]
        public IActionResult Register()
        {
            return View("register");
        }

        [HttpPost]
        [Route("/newUser")]
        public IActionResult CreateUser(RegisterViewModel model)
        {
            var checkEmail = _context.users.SingleOrDefault(e => e.email == model.email);
            
            if(ModelState.IsValid)
            {
                if(checkEmail != null)
                {
                    TempData["error"] = "Email already in use";
                    return View("register");
                }
                User newUser = new User{
                    first_name = model.first_name,
                    last_name = model.last_name,
                    email = model.email,
                    password = model.password
                };
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                newUser.password = hasher.HashPassword(newUser, newUser.password);
                _context.users.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("current_userid", newUser.userid);
                // var id = HttpContext.Session.GetInt32("user_id");
                return RedirectToAction("Dashboard", "Account", new{user_id = newUser.userid});
            }
            return View("register");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            
            return View("login");
        }

        [HttpPost]
        [Route("logIn")]
        public IActionResult SignIn(string email, string password)
        {

            var checkEmail = _context.users.SingleOrDefault(e => e.email == email);
            if(checkEmail==null)
            {
                TempData["email_error"] = "Please check your email otherwie go to register";
                return View("login");
            }

            if(checkEmail!=null && password!= null)
            {
                var hasher = new PasswordHasher<User>();
                if(0 != hasher.VerifyHashedPassword(checkEmail, checkEmail.password, password))
                {
                    HttpContext.Session.SetInt32("current_userid", checkEmail.userid);
                    // var id = HttpContext.Session.GetInt32("userid");
                    return RedirectToAction("Dashboard", "Account", new{user_id = checkEmail.userid});
                }
            }
                TempData["psw_error"] = "Password is incorrect";
                return RedirectToAction("Login");
        }


        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("register");
        }
    }
}
