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
    public class OrderrequestController : ControllerBase
    {
        ondolinyiContext _context;
        public OrderrequestController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<Orderrequest[]>> Get()
        {
            return await _context.Orderrequest.ToArrayAsync();
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> Dispatch([FromBody]Dispatcher dis)
        {
            var target = await _context.Orderrequest.SingleOrDefaultAsync(obj => obj.Orderid == dis.Orderid);
            if (target == null)
            {
                return BadRequest("Invalid orderID");
            }
            if (target.Status.ToLower() != "pending")
            {
                return BadRequest("Order already dispatched, completed or canceled");
            }
            target.Status = "Intransit";
            _context.Dispatcher.Add(dis);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("[action]/{orderid}")]
        [HttpGet]
        public async Task<ActionResult> IsDispatchable(string orderid)
        {
            var target = await _context.Orderrequest.SingleOrDefaultAsync(obj => obj.Orderid == orderid);
            if (target == null)
            {
                return BadRequest("Invalid orderID");
            }
            return Ok(target.Status.ToLower() == "pending");
        }

        [Route("[action]/{orderid}")]
        [HttpGet]
        public async Task<ActionResult> GetDispatcher(string orderid)
        {
            var target = await _context.Orderrequest.SingleOrDefaultAsync(obj => obj.Orderid == orderid);
            if (target == null)
            {
                return BadRequest("Invalid orderID");
            }
            var dispatcher = _context.Dispatcher.SingleOrDefaultAsync(od => od.Orderid == orderid);
            return Ok(dispatcher);
        }

        [Route("[action]/{orderid}")]
        [HttpPost]
        public async Task<ActionResult> MarkComplete(string orderid)
        {
            var target = await _context.Orderrequest.SingleOrDefaultAsync(obj => obj.Orderid == orderid);
            if (target == null)
            {
                return BadRequest("Invalid orderID");
            }
            if (target.Status.ToLower() != "intransit")
            {
                return BadRequest("Order already completed or canceled");
            }
            target.Status = "Success";
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("[action]/{orderid}")]
        [HttpGet]
        public async Task<ActionResult> IsCompleteable(string orderid)
        {
            var target = await _context.Orderrequest.SingleOrDefaultAsync(obj => obj.Orderid == orderid);
            if (target == null)
            {
                return BadRequest("Invalid orderID");
            }
            return Ok(target.Status.ToLower() == "intransit");
        }


        [HttpGet("{orderid}")]
        public async Task<ActionResult<Orderrequest>> Get(string orderid)
        {
            var target = await _context.Orderrequest.SingleOrDefaultAsync(obj => obj.Orderid == orderid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }

        [HttpPost]
        public async Task<ActionResult<Orderrequest>> Post([FromBody] Orderrequest obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Orderrequest.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Orderrequest", obj);
            }
        }

        [HttpPut("{orderid}")]
        public async Task<ActionResult> Put(string orderid, [FromBody] Orderrequest obj)
        {
            var target = await _context.Orderrequest.SingleOrDefaultAsync(nobj => nobj.Orderid == orderid);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{orderid}")]
        public async Task<ActionResult> Delete(string orderid)
        {
            var target = await _context.Orderrequest.SingleOrDefaultAsync(obj => obj.Orderid == orderid);
            if (target != null)
            {
                _context.Orderrequest.Remove(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}