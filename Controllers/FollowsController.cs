using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneCs.Data;
using TwitterCloneCs.Models;

namespace TwitterCloneCs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowsController : ControllerBase
    {
        private readonly TwitterCloneContext _context;

        public FollowsController(TwitterCloneContext context)
        {
            _context = context;
        }

        // GET: api/Follows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Follow>>> GetFollow()
        {
            return await _context.Follow.ToListAsync();
        }

        // GET: api/Follows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Follow>> GetFollow(int id)
        {
            var follows =  _context.Follow.Where( x => x.User_id == id);

            if (follows == null)
            {
                return NotFound( new { error = "User does not follow anyone"});
            }

            return Ok( new { follows = follows});
        }

        // PATCH: api/Follows/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PutFollow(int id, Follow follow)
        {
            if (id != follow.Id)
            {
                return BadRequest();
            }

            _context.Entry(follow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowExists(id))
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

        // POST: api/Follows
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Follow>> PostFollow(Follow follow)
        {
            Follow following = await _context.Follow.FirstOrDefaultAsync(x => x.Follows == follow.Follows && x.User_id.ToString() == User.Identity.Name);
            Follow newFollow = follow;

            User user = await _context.User.FirstOrDefaultAsync(x => x.Id == newFollow.Follows);

            if(user == null)
            {
                return BadRequest(new { error = "The user you are trying to follow does not exist" });
            }

            if(newFollow.Follows.ToString() == User.Identity.Name)
            {
                return BadRequest(new { error = "You can not follow yourself"});
            }

            if(following != null)
            {
                return BadRequest(new { error = "You follow this user already" });
            };

            newFollow.User_id = int.Parse(User.Identity.Name);

            _context.Follow.Add(newFollow);
            await _context.SaveChangesAsync();

            return Ok( new { id = newFollow.Id, user_id = User.Identity.Name });
        }

        // DELETE: api/Follows/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Follow>> DeleteFollow(int id)
        {
            var follow = await _context.Follow.FirstOrDefaultAsync( x => x.Follows == id && x.User_id == int.Parse(User.Identity.Name));
            if (follow == null)
            {
                return NotFound( new { error = "You are not following this user"});
            }

            _context.Follow.Remove(follow);
            await _context.SaveChangesAsync();

            return Ok( new { follow = follow});
        }

        private bool FollowExists(int id)
        {
            return _context.Follow.Any(e => e.Id == id);
        }
    }
}
