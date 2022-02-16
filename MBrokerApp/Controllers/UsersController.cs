using MBrokerApp.Repository.Entities;
using MBrokerApp.Repository.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBrokerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserManager userManager;

        public UsersController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllUsers()
        {
            return Ok(userManager.GetAllUsers());
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateUser([FromBody] User user)
        {
            return Ok();
        }
    }
}
