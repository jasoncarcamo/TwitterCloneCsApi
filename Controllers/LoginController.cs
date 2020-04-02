using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneCs.Data;
using TwitterCloneCs.Models;
using Microsoft.Extensions.Configuration;
using TwitterCloneCs.Services;

namespace TwitterCloneCs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly TwitterCloneContext _context;

        public IConfiguration Configuration { get; }
        public LoginController(TwitterCloneContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // POST: api/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            UserService userService = new UserService(Configuration);
            User newUser = user;
            User dbUser = _context.User.SingleOrDefault(x => x.Email == user.Email);

            if(dbUser == null)
            {
                return BadRequest(new { error = "No user was found with this email" });
            }

            //Failed: 0
            //Success: 1
            //SuccessRehashNeeeded: 2
            var matches = userService.comparePassword(user, user.Password, dbUser.Password);

            if(!matches)
            {
                return BadRequest(new { error = "Wrong password"});
            }            

            return Ok( new { 
                token = userService.createToken(user)
            });
        }
    }
}
