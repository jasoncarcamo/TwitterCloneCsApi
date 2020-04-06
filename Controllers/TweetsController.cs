﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneCs.Data;
using TwitterCloneCs.Models;

namespace TwitterCloneCs.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly TwitterCloneContext _context;

        public TweetsController(TwitterCloneContext context)
        {
            _context = context;
        }

        // GET: api/Tweets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweet()
        {
            return await _context.Tweet.ToListAsync();
        }

        // GET: api/Tweets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tweet>> GetTweet(int id)
        {
            var tweet = await _context.Tweet.FindAsync(id);

            if (tweet == null)
            {
                return NotFound();
            }

            return tweet;
        }

        // PATCH: api/Tweets/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PutTweet(int id, Tweet tweet)
        {
            if (id != tweet.Id)
            {
                return BadRequest();
            }

            _context.Entry(tweet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TweetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tweets
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Tweet>> PostTweet(Tweet tweet)
        {
            Tweet newTweet = tweet;

            newTweet.User_id = int.Parse(User.Identity.Name);

            _context.Tweet.Add(newTweet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTweet", new { id = tweet.Id }, tweet);
        }

        // DELETE: api/Tweets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tweet>> DeleteTweet(int id)
        {
            var tweet = await _context.Tweet.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }

            _context.Tweet.Remove(tweet);
            await _context.SaveChangesAsync();

            return tweet;
        }

        private bool TweetExists(int id)
        {
            return _context.Tweet.Any(e => e.Id == id);
        }
    }
}
