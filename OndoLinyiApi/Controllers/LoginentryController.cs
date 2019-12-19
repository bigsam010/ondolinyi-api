using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OndoLinyiApi.Models;
namespace OndoLinyiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginentryController : ControllerBase
    {
        ondolinyiContext _context;
        public LoginentryController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<Loginentry[]>> Get()
        {
            return await _context.Loginentry.ToArrayAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Loginentry>> Get(int id)
        {
            var target = await _context.Loginentry.SingleOrDefaultAsync(obj => obj.Id == id);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [Route("[action]/{user}")]
        [HttpGet]
        public async Task<ActionResult<Loginentry[]>> LoginHistory(string user)
        {
            return await _context.Loginentry.Where(le => le.Client == user).ToArrayAsync();
        }

        [Route("[action]/{user}")]
        [HttpGet]
        public async Task<ActionResult<Loginentry>> LastLogin(string user)
        {
            var llogin = await _context.Loginentry.Where(cl => cl.Client == user).OrderByDescending(le => le.Logindate).FirstOrDefaultAsync();
            if (llogin == null)
            {
                return NotFound();
            }
            return llogin;
        }

        [Route("[action]/{user}")]
        [HttpGet]
        public async Task<ActionResult<int>> LoginTotal(string user)
        {
            return await _context.Loginentry.CountAsync(le => le.Client == user);
        }
    }
}