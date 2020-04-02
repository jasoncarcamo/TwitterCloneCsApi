﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneCs.Data;
using TwitterCloneCs.Models;
using TwitterCloneCs.Services;
using Microsoft.Extensions.Configuration;

namespace TwitterCloneCs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly TwitterCloneContext _context;
        public IConfiguration Configuration {get;}
        public RegisterController(TwitterCloneContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // POST: api/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            UserService userService = new UserService(Configuration);
            User dbUser = _context.User.SingleOrDefault(x => x.Email == user.Email);
            User newUser = user;

            if(dbUser != null)
            {
                return BadRequest(new { error = "A user exists with this email already" });
            }

            newUser.Password = userService.hashPassword(user, newUser.Password);


            _context.User.Add(newUser);
            await _context.SaveChangesAsync();
            

            return Ok( new { token = userService.createToken(newUser) });
        }

    }
}
