using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OndoLinyiApi.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
namespace OndoLinyiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ondolinyiContext _context;
        public CustomerController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            long total = _context.Customer.Count();
            var records = await _context.Customer.
                OrderBy(c => c.Lastname).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Customer>(records, pageNo, pageSize, total));
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<Customer>> Get(string email)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target == null)
            {
                return BadRequest("Invalid customer");
            }
            return target;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Post([FromBody] Customer obj)
        {
            if (!new EmailAddressAttribute().IsValid(obj.Email))
            {
                return BadRequest("Invalid email");
            }
            var target = await _context.Users.SingleOrDefaultAsync(o => o.Email == obj.Email);
            var target3 = await _context.Customer.SingleOrDefaultAsync(o => o.Email == obj.Email);
            if (target != null || target3 != null)
            {
                return BadRequest("Customer already exist");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }
            obj.Password = Util.Encrypt(obj.Password);
            obj.Isverified = 0;
            string tk = Util.GenToken();
            obj.Verificationtoken = tk;
            Util.SendMail("Hi " + obj.Firstname + ",<br>You are welcome on board. Kindly click <a href='https://www.ondolinyi.com/customer/verify?email=" + obj.Email + "&token=" + tk + "'>here</a> to verify your email.", "OndoLinyi", obj.Email);
            _context.Customer.Add(obj);
            await _context.SaveChangesAsync();
            return Created("api/Customer", obj);
        }

        [Route("[action]/{email}/{password}")]
        [HttpGet]
        public async Task<ActionResult<bool>> Authenticate(string email, string password)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email && obj.Status != "Blocked");
            if (target == null)
            {
                return false;
            }
            var res = target.Password == Util.Encrypt(password);
            if (res)
            {
                var le = new Loginentry
                {
                    Client = email
                };
                await _context.AddAsync(le);
                await _context.SaveChangesAsync();
            }
            return res;

        }
        [Route("[action]/{email}")]
        [HttpGet]
        public async Task<ActionResult<bool>> Isverified(string email)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target == null)
            {
                return false;
            }
            return target.Isverified == 1;

        }

        [Route("[action]/{email}")]
        [HttpGet]
        public async Task<ActionResult> GetAddresses(string email)
        {

            return Ok(await _context.Customeraddress.Where(ca => ca.Customer == email).ToListAsync());

        }

        [Route("[action]/{email}")]
        [HttpGet]
        public ActionResult GetDefaultAddress(string email)
        {
            var da = _context.Customeraddress.Where(ca => ca.Customer == email && ca.Isdefault == 1);
            if (da == null)
            {
                return BadRequest("No default address");
            }
            return Ok(da);

        }

        [Route("[action]/{id}")]
        [HttpPost]
        public async Task<ActionResult> SetAsDefaultAddress(int id)
        {
            var target = await _context.Customeraddress.SingleOrDefaultAsync(ca => ca.Id == id);
            if (target == null)
            {
                return BadRequest("Invalid request");
            }
            var addresses = _context.Customeraddress.Where(ca => ca.Customer == target.Customer);
            foreach (CustomerAddress ca in addresses)
            {
                ca.Isdefault = 0;
            }
            target.Isdefault = 1;
            await _context.SaveChangesAsync();
            return Ok();

        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> AddAddress([FromBody] CustomerAddress address)
        {
            var cus = await _context.Customer.SingleOrDefaultAsync(c => c.Email == address.Customer);
            if (cus == null)
            {
                return BadRequest("Invlaid customer");
            }
            _context.Customeraddress.Add(address);
            if (address.Isdefault == 1)
            {
                var addresses = _context.Customeraddress.Where(ca => ca.Customer == address.Customer);
                foreach (CustomerAddress ca in addresses)
                {
                    ca.Isdefault = 0;
                }
            }
            await _context.SaveChangesAsync();
            return Created("api/Customeraddress", address);

        }

        [Route("[action]/{id}")]
        [HttpDelete]
        public async Task<ActionResult> RemoveAddress(int id)
        {
            var target = await _context.Customeraddress.SingleOrDefaultAsync(cs => cs.Id == id);
            if (target == null)
            {
                return BadRequest("Invalid id");
            }
            _context.Customeraddress.Remove(target);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [Route("[action]/{email}/{token}")]
        [HttpPost]
        public async Task<ActionResult> Verify(string email, string token)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target == null)
            {
                return NotFound();
            }
            if (target.Verificationtoken != token)
            {
                return BadRequest("Invalid token");
            }
            target.Isverified = 1;
            await _context.SaveChangesAsync();
            return Ok();

        }
        [Route("[action]/{email}")]
        [HttpGet]
        public async Task<ActionResult<bool>> Exists(string email)
        {
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
            var target3 = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            return target != null || target3 != null;
        }

        [Route("[action]/{email}")]
        [HttpPost]
        public async Task<ActionResult> Unblock(string email)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target != null)
            {
                target.Status = "Active";
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        [Route("[action]/{email}")]
        [HttpGet]
        public async Task<ActionResult<bool>> Isblocked(string email)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target != null)
            {
                return target.Status == "Blocked";
            }
            return NotFound();
        }

        [Route("[action]/{email}/{oldpassword}/{newpassword}")]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(string email, string oldpassword, string newpassword)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email && obj.Status != "Blocked");
            if (target != null)
            {
                if (target.Password == Util.Encrypt(oldpassword))
                {
                    target.Password = Util.Encrypt(newpassword);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid password");
                }
            }
            return NotFound();
        }
        [Route("[action]/{email}")]
        [HttpPost]
        public async Task<ActionResult> SendResetLink(string email)
        {
            try
            {
                if (!new EmailAddressAttribute().IsValid(email))
                {
                    return BadRequest("Invalid email");
                }
                var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
                if (target == null)
                {
                    return NotFound("User not found");
                }
                string tk = Util.GenToken();
                Util.SendMail("Click <a href='https://www.ondolinyi.com/customer/resetpassword?email=" + email + "&token=" + tk + "'>here</a> to reset your password.", "Password Reset", email);
                var rreq = new Passwordreset
                {
                    Customer = email,
                    Token = tk,
                    Expdate = DateTime.Now.AddHours(1)

                };
                _context.Passwordreset.Add(rreq);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }
        [Route("[action]/{email}/{token}/{newpassword}")]
        [HttpPost]
        public async Task<ActionResult> ResetPassword(string email, string token, string newpassword)
        {
            var target = await _context.Passwordreset.SingleOrDefaultAsync(obj => obj.Customer == email && obj.Token.ToString() == token && DateTime.Now <= obj.Expdate);
            if (target != null)
            {
                var cus = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email && obj.Status != "Blocked");
                cus.Password = Util.Encrypt(newpassword);
                _context.Passwordreset.Remove(target);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Invalid or expired token");
        }

        [Route("[action]/{email}")]
        [HttpPost]
        public async Task<ActionResult> UploadDp(string email)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target == null || Request.Form.Files.Count == 0)
            {
                return BadRequest("User not found or request file missing");
            }
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Res", "Dps");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fullPath = Path.Combine(pathToSave, email.ToLower() + ".png");
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Ok();
                }
                else
                {
                    return BadRequest("Empty file");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Route("[action]/{email}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetDp(string email)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target == null)
            {
                return NotFound();
            }
            var folderName = Path.Combine("Res", "Dps");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, email.ToLower() + ".png");
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return Path.Combine(folderName, email.ToLower() + ".png"); ;
        }

        [Route("[action]/{email}")]
        [HttpGet]
        public async Task<ActionResult<byte[]>> GetDpRaw(string email)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target == null)
            {
                return NotFound();
            }
            var folderName = Path.Combine("Res", "Dps");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, email.ToLower() + ".png");
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }
            return await System.IO.File.ReadAllBytesAsync(Path.Combine(folderName, email.ToLower() + ".png").ToString());
        }

        [HttpPut("{email}")]
        public async Task<ActionResult> Put(string email, [FromBody] Customer obj)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(nobj => nobj.Email == email);
            if (target != null && ModelState.IsValid)
            {
                _context.Entry(target).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult> Delete(string email)
        {
            var target = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target != null)
            {
                target.Status = "Blocked";
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Customer not found");
        }
    }
}