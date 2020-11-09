using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using All4Ole_Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace All4Ole_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        //private readonly IFlightsManager manager;
        private Manager manager = new Manager();
        // GET: UserController
        [HttpGet]
        public ActionResult Index()
        {
            return Ok("Welcome to my server!!!!!!!!");
        }

        // register new user
        [HttpPost]
        [Route("/user/register")]
        public ActionResult Register([FromBody] User user)
        {
            string message = manager.InsertUser(user);
            if (message == "good")
                return Ok(user);
            return BadRequest(message);
        }

        // Login to user's account - todoooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo
        [HttpPost]
        [Route("/user/login")]
        public ActionResult Login([FromBody] User user)
        {
            /*string message = manager.InsertUser(user);
            if (message == "good")
                return Ok("Inserted successfully");*/
            return BadRequest("");
        }

        // POST: UserController/Create
        [HttpGet]
        [Route("/user/Help")]
        public ActionResult LookForHelp([FromQuery(Name = "username")] string userName, [FromQuery(Name = "help")] int help)
        {
            /*Manager manager = new Manager();
            string message = manager.InsertUser(user);
            if (message == "good")
                return Ok("Inserted successfully");*/
            //manager.LookForHelp(userName, help);
            return Ok(manager.LookForHelp(userName, help));
        }


        //todo extract username and help from json!!!!!!!!!!!!!!!!!!!!!!!! and do function
        [HttpPost]
        [Route("/user/setHelp")]
        public ActionResult SetHelp([FromBody] User user)
        {
            /*string message = manager.InsertUser(user);
            if (message == "good")
                return Ok("Inserted successfully");*/
            return BadRequest("");
        }


        //todo extract username and help from json!!!!!!!!!!!!!!!!!!!!!!!! and do function
        [HttpPost]
        [Route("/user/people")]
        public ActionResult PeopleLikeMe([FromQuery(Name = "username")] string userName)
        {
            /*string message = manager.InsertUser(user);
            if (message == "good")
                return Ok("Inserted successfully");*/
            return BadRequest("");
        }

        /*// GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
