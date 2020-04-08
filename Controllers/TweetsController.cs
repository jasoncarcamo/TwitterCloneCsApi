using System;
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
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly TwitterCloneContext _context;

        public TweetsController(TwitterCloneContext context)
        {
            _context = context;
        }

        // GET: api/Tweets
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweet()
        {
            var tweets = _context.Tweet.Where(x => x.User_id == int.Parse(User.Identity.Name));

            if(tweets == null)
            {
                return NotFound(new { error = "User does not have any tweets yet" });
            }

            return Ok( new { userTweets = tweets});
        }

        // GET: api/Tweets/5

        [HttpGet("{id}")]
        public async Task<ActionResult<Tweet>> GetTweet(int id)
        {
            var tweets = _context.Tweet.Where( x => x.User_id == id);

            if (tweets == null)
            {
                return NotFound();
            }

            return Ok( new { userTweets = tweets });
        }

        // PATCH: api/Tweets/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTweet(int id, Tweet tweet)
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
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Tweet>> PostTweet(Tweet tweet)
        {
            Tweet newTweet = tweet;

            newTweet.DateCreated = DateTime.Now;
            newTweet.User_id = int.Parse(User.Identity.Name);

            _context.Tweet.Add(newTweet);
            await _context.SaveChangesAsync();

            return Ok( new { tweet_id = newTweet.Id });
        }

        // DELETE: api/Tweets/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tweet>> DeleteTweet(int id)
        {
            var tweet = await _context.Tweet.FindAsync(id);

            if (tweet == null)
            {
                return NotFound();
            };

            _context.Tweet.Remove(tweet);
            await _context.SaveChangesAsync();

            return Ok( new { success = $"You unfollowed {tweet.Id} "});
        }

        private bool TweetExists(int id)
        {
            return _context.Tweet.Any(e => e.Id == id);
        }
    }
}
