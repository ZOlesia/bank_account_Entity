using System;
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
    public class AccountController: Controller
    {

        private BankContext _context;
 
        public AccountController(BankContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("account/{user_id}")]
        public IActionResult Dashboard(int user_id)
        {
            // int? isLogin = HttpContext.Session.GetInt32("user_id");
            if(HttpContext.Session.GetInt32("current_userid") == null || user_id != HttpContext.Session.GetInt32("current_userid"))
            {
                return RedirectToAction("Login", "Home");
            } 

            // List<Transaction> Transactions = _context.transactions.Include(transaction => transaction.user).Where(u => u.userid == user_id).ToList();

            User user = _context.users.Include(transaction => transaction.transactions).SingleOrDefault(u => u.userid == user_id);

            ViewBag.reverse_transactions = user.transactions.OrderByDescending(d => d.trans_date);

            ViewBag.allAboutUser = user;

            // ViewBag.balance = _context.transactions.Include(transaction => transaction.user).Where(u => u.userid == user_id).Sum(a => a.amount);

            return View("dashboard");
        }

        [HttpPost]
        [Route("account/{user_id}/proccess/transaction")]
        public IActionResult Proccess(int user_id, decimal num)
        {
            if(HttpContext.Session.GetInt32("current_userid") == null || user_id != HttpContext.Session.GetInt32("current_userid"))
            {
                return RedirectToAction("Login", "Home");
            } 
            User user = _context.users.Include(transaction => transaction.transactions).SingleOrDefault(u => u.userid == user_id);
            ViewBag.allAboutUser = user;
            if(-num > user.balance)
            {
                TempData["error"] = "Not enough money";
                return RedirectToAction("Dashboard", new{user_id = user.userid});
            }
            Transaction newTransaction = new Transaction{
                amount = num,
                userid = user.userid
            };
            _context.transactions.Add(newTransaction);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", new{user_id = user.userid});
        }
    }
}


