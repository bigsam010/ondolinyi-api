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
    public class CustomersettingsController : ControllerBase
    {
        ondolinyiContext _context;
        public CustomersettingsController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        //[HttpGet]
        //public async Task<ActionResult<Customersettings[]>> Get()
        //{
        //    return await _context.Customersettings.ToArrayAsync();
        //}
        // [Route("[action]/{customer}")]
        [HttpGet("{customer}")]
        public async Task<ActionResult> Get(string customer)
        {
            var cus = await _context.Customer.SingleOrDefaultAsync(c => c.Email == customer);
            if (cus == null)
            {
                return BadRequest("Invalid customer");
            }
            var target = await _context.Customersettings.SingleOrDefaultAsync(obj => obj.Customer == customer);
            if (target == null)
            {
                return Ok(new Customersettings(customer));
            }
            return Ok(target);
        }
        // [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Customersettings obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }
            else
            {
                var cus = await _context.Customer.SingleOrDefaultAsync(c => c.Email == obj.Customer);
                if (cus == null)
                {
                    return BadRequest("Invalid customer");
                }
                var target = await _context.Customersettings.SingleOrDefaultAsync(cs => cs.Customer == obj.Customer);
                if (target != null)
                {
                    target.Allowemail = obj.Allowemail;
                    target.Allowsms = obj.Allowsms;
                    target.Notifyorderstatus = obj.Notifyorderstatus;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                _context.Customersettings.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Customersettings", obj);
            }
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put(int id, [FromBody] Customersettings obj)
        //{
        //    var target = await _context.Customersettings.SingleOrDefaultAsync(nobj => nobj.Id == id);
        //    if (target != null && ModelState.IsValid)
        //    {
        //        _context.Entry(target).CurrentValues.SetValues(obj);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return BadRequest();
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var target = await _context.Customersettings.SingleOrDefaultAsync(obj => obj.Id == id);
        //    if (target != null)
        //    {
        //        _context.Customersettings.Remove(target);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return NotFound();
        //}
    }
}