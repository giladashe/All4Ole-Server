﻿using System.Collections.Generic;
using All4Ole_Server.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace All4Ole_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IManager manager;

        public UserController(IManager manager)
        {
            this.manager = manager;
        }


        // GET: UserController
        [HttpGet]
        public ActionResult Index()
        {
            return Ok("Welcome to all4ole db");
        }

        // Registers a new user to the MySQL db
        [HttpPost("[action]")]
        public ActionResult Register([FromBody] User user)
        {
            string message = manager.InsertUser(user);
            if (message == "Good")
                return Ok(user);
            return BadRequest(message);
        }

        // Login to user's account
        [HttpPost("[action]")]
        public ActionResult Login([FromBody] JObject loginUser)
        {
            string userName = loginUser.Value<string>("user_name");
            string password = loginUser.Value<string>("password");
            User user = manager.Login(userName, password);

            if (user != null)
                return Ok(user);
            return BadRequest(null);

        }


        // Changes the things the user can help with
        [HttpGet("[action]")]
        public ActionResult SetHelp([FromQuery(Name = "username")] string userName, [FromQuery(Name = "help")] int help)
        {
            string message = manager.SetHelp(userName, help);
            if (message == "success")
                return Ok(message);
            return BadRequest(message);
        }

        // Searches for people who can help the user
        [HttpGet]
        [Route("/user/Help")]
        public ActionResult LookForHelp([FromQuery(Name = "username")] string userName, [FromQuery(Name = "help")] int help)
        {
            List<User> users = manager.LookForHelp(userName, help);
            if (users == null)
            {
                return BadRequest("Connection failure");
            }
            return Ok(users);
        }


        // Finds people that have hobbies like the user
        [HttpGet]
        [Route("/user/hobby")]
        public ActionResult FindFriendsForHobbies([FromQuery(Name = "username")] string userName, [FromQuery(Name = "hobbies")] int hobbies)
        {

            List<User> users = manager.FindFriendsForHobbies(userName, hobbies);
            if (users == null)
            {
                return BadRequest("Connection failure");
            }
            return Ok(users);
        }


        // Searches for people who are similar to the user
        [HttpGet]
        [Route("/user/people")]
        public ActionResult PeopleLikeMe([FromQuery(Name = "username")] string userName)
        {
            List<User> users = manager.PeopleLikeMe(userName);
            if (users == null)
            {
                return BadRequest("Connection failure");
            }
            return Ok(users);
        }

    }
}
