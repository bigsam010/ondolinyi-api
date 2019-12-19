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
    public class RfqController : ControllerBase
    {
        ondolinyiContext _context;
        public RfqController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<Rfq[]>> Get()
        {
            return await _context.Rfq.ToArrayAsync();
        }

        [HttpGet("{rfqid}")]
        public async Task<ActionResult<Rfq>> Get(string rfqid)
        {
            var target = await _context.Rfq.SingleOrDefaultAsync(obj => obj.Rfqid == rfqid);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }
        [Route("[action]/{customer}")]
        [HttpGet]
        public async Task<ActionResult> CustomerInquiries(string customer)
        {
            return Ok(await _context.Rfq.Where(rf => rf.Customer == customer).OrderByDescending(rf => rf.Requestdate).ToListAsync());
        }

        [Route("[action]/{rfqid}")]
        [HttpGet]
        public async Task<ActionResult<Rfqchatresponse[]>> GetResponses(string rfqid)
        {
            var chat = _context.Rfqchatlog.SingleOrDefaultAsync(rf => rf.Rfqid == rfqid);
            if (chat == null)
            {
                return BadRequest("Rfq has not been answered");
            }
            return await _context.Rfqchatresponse.Where(rf => rf.Chatid == chat.Id).ToArrayAsync();
        }
        [Route("[action]/{salesrep}")]
        [HttpGet]
        public async Task<ActionResult> SalesRepInquiries(string salesrep)
        {
            return Ok(await _context.Rfq.Where(rf => rf.Salesrep == salesrep).OrderByDescending(rf => rf.Requestdate).ToListAsync());
        }
        [Route("[action]/{rfqid}")]
        [HttpGet]
        public async Task<ActionResult> IsAnswered(string rfqid)
        {
            var target = await _context.Rfqchatlog.SingleOrDefaultAsync(rf => rf.Rfqid == rfqid);
            return Ok(target != null);
        }
        [Route("[action]/{rfqid}")]
        [HttpGet]
        public async Task<ActionResult> GetAnswer(string rfqid)
        {
            var target = await _context.Rfqchatlog.SingleOrDefaultAsync(rf => rf.Rfqid == rfqid);
            if (target == null)
            {
                return BadRequest("No answer found for this rfq");
            }
            return Ok(target);
        }

        [HttpPost]
        public async Task<ActionResult> AnswerInquiry([FromBody]Rfqchatlog obj)
        {
            var target = await _context.Rfqchatlog.SingleOrDefaultAsync(rf => rf.Rfqid == obj.Rfqid);
            if (target != null)
            {

                return BadRequest("Rfq already answered");
            }
            _context.Rfqchatlog.Add(obj);
            await _context.SaveChangesAsync();
            return Created("api/rfqchatlog", obj);
        }

        [HttpGet]
        public async Task<ActionResult> IsAnsweredBy(string rfqid, string salesrep)
        {
            var target = await _context.Rfqchatlog.SingleOrDefaultAsync(rf => rf.Rfqid == rfqid && rf.Salesrep == salesrep);
            return Ok(target != null);
        }
        [Route("[action]/{customer}")]
        [HttpGet]
        public ActionResult PagedCustomerInquiries(string customer, int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Rfq.Where(rf => rf.Customer == customer).Count();
            var records = _context.Rfq.Where(rf => rf.Customer == customer).OrderByDescending(rf => rf.Requestdate).Skip(skip).Take(pageSize).ToList();
            return Ok(new PagedResult<Rfq>(records, pageNo, pageSize, total));

        }

        [HttpPost]
        public async Task<ActionResult<Rfq>> Post([FromBody] Rfq obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }
            else
            {
                var product = await _context.Product.SingleOrDefaultAsync(p => p.Productid == obj.Productid);
                if (product == null)
                {
                    return BadRequest("Invalid product");
                }
                if (obj.Qty < product.Moq)
                {
                    return BadRequest("Order quantity less than product's MOQ.");
                }

                var cusrfqs = _context.Rfq.Where(rf => rf.Customer == obj.Customer).Select(rf => rf.Rfqid);
                var cusorders = _context.Orderrequest.Where(co => cusrfqs.Contains(co.Rfqid) && co.Status == "Success");
                //       if (cusorders.Count() >= 3)
                //       {
                //           //choose rep by quality
                //       }
                //       else
                //       {
                //           //select count(rfqid),salesrep from rfqchatlog group by salesrep
                //           // _context.Rfq.GroupBy(rf => rf.Salesrep).Count();
                //           userInfos.GroupBy(userInfo => userInfo.metric)
                //.OrderBy(group => group.Key)
                //.Select(group => Tuple.Create(group.Key, group.Count()));
                //       }
                //       _context.Rfq.GroupBy();
                //       _context.Rfq.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Rfq", obj);
            }
        }

        [HttpPut("{rfqid}")]
        public async Task<ActionResult> Put(string rfqid, [FromBody] Rfq obj)
        {
            var target = await _context.Rfq.SingleOrDefaultAsync(nobj => nobj.Rfqid == rfqid);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{rfqid}")]
        public async Task<ActionResult> Delete(string rfqid)
        {
            var target = await _context.Rfq.SingleOrDefaultAsync(obj => obj.Rfqid == rfqid);
            if (target != null)
            {
                _context.Rfq.Remove(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}