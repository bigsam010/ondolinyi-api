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
    public class UsersController : ControllerBase
    {
        ondolinyiContext _context;
        public UsersController(ondolinyiContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Users.Count();
            var records = await _context.Users.
                OrderBy(u => u.Joindate).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Users>(records, pageNo, pageSize, total));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> GetPendingInvites(int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Users.Where(u => u.Firstname == null).Count();
            var records = await _context.Users.
                Where(u => u.Firstname == null).
                OrderBy(u => u.Joindate).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Users>(records, pageNo, pageSize, total));
        }
        [HttpGet("{email}")]
        public async Task<ActionResult<Users>> Get(string email)
        {
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target == null)
            {
                return NotFound();
            }
            return target;
        }



        [HttpPut("{email}")]
        public async Task<ActionResult> Put(string email, [FromBody] Users obj)
        {
            var target = await _context.Users.SingleOrDefaultAsync(nobj => nobj.Email == email);
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
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target != null)
            {
                target.Status = "Blocked";
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
        [Route("[action]/{email}")]
        [HttpPost]
        public async Task<ActionResult> Unblock(string email)
        {
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
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
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
            if (target != null)
            {
                return target.Status == "Blocked";
            }
            return NotFound();
        }
        [Route("[action]/{email}/{role}")]
        [HttpPost]
        public async Task<ActionResult> Invite(string email, string role)
        {
            try
            {
                if (role.ToLower() != "sales" && role.ToLower() != "admin")
                {
                    return BadRequest("Invalid role");
                }

                if (!new EmailAddressAttribute().IsValid(email))
                {
                    return BadRequest("Invalid email");
                }
                var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
                var target3 = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
                if (target != null || target3 != null)
                {
                    return BadRequest("User already exist");
                }
                Util.SendMail("Hi,<br>You have been invited to OndoLinyin E-Commerce Platform. Kindly click <a href='https://www.ondolinyi.com/users/signup?email=" + email + "'>here</a> to complete your signup.", "ONDO LINYI INVITE", email);
                Users usr = new Users
                {
                    Email = email,
                    Role = role

                };
                _context.Users.Add(usr);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [Route("[action]/{email}/{oldpassword}/{newpassword}")]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(string email, string oldpassword, string newpassword)
        {
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email && obj.Status != "Blocked");
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
                var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email.ToLower() == email.ToLower());
                if (target == null)
                {
                    return NotFound("User not found");
                }
                string tk = Util.GenToken();
                Util.SendMail("Click <a href='https://www.ondolinyi.com/users/resetpassword?email=" + email + "&token=" + tk + "'>here</a> to reset your password.", "Password Reset", email);
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


        [Route("[action]/{email}")]
        [HttpGet]
        public async Task<ActionResult> Exists(string email, string role)
        {
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
            var target3 = await _context.Customer.SingleOrDefaultAsync(obj => obj.Email == email);
            return Ok(target != null || target3 != null);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> Signup([FromBody]Users user)
        {
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == user.Email);
            if (target == null)
            {
                return BadRequest("User not found");
            }
            target.Firstname = user.Firstname;
            target.Lastname = user.Lastname;
            target.Password = Util.Encrypt(user.Password);
            target.Phone = user.Phone;
            target.Bio = user.Bio;
            await _context.SaveChangesAsync();
            //string[] aemails = _context.Users.Where(u => u.Role == "Admin").Select(u => u.Email).ToArray();
            //email2 = target.Lastname + " " + target.Firstname + " just accepted the LMS invite.";
            //subject = "Invite Acceptance Alert";
            //bccs = aemails;
            //ThreadStart ts = new ThreadStart(dispatchMail);
            //Thread t1 = new Thread(ts);
            // t1.Start();
            return Ok();
        }

        [Route("[action]/{email}/{password}")]
        [HttpGet]
        public async Task<ActionResult<bool>> Authenticate(string email, string password)
        {
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email && obj.Status != "Blocked");
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
        [Route("[action]/{email}/{token}/{newpassword}")]
        [HttpPost]
        public async Task<ActionResult> ResetPassword(string email, string token, string newpassword)
        {
            var target = await _context.Passwordreset.SingleOrDefaultAsync(obj => obj.Customer == email && obj.Token.ToString() == token && DateTime.Now <= obj.Expdate);
            if (target != null)
            {
                var user = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email && obj.Status != "Blocked");
                user.Password = Util.Encrypt(newpassword);
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
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
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
                    return BadRequest();
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
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
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
            var target = await _context.Users.SingleOrDefaultAsync(obj => obj.Email == email);
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

        [Route("[action]/{salesrep}")]
        [HttpGet]
        public async Task<ActionResult> GetAverageReview(string salesrep)
        {
            var target = await _context.Users.SingleOrDefaultAsync(s => s.Email == salesrep);

            if (target == null)
            {
                return BadRequest("Invalid salesrep");
            }

            var surveys = _context.Salesrepsurvey.Where(s => s.SalesRep == salesrep);
            if (surveys.Count() == 0)
            {
                return Ok(0);
            }
            return Ok(surveys.AverageAsync(s => s.Rating));
        }

        [Route("[action]/{salesrep}")]
        [HttpGet]
        public async Task<ActionResult> GetRepReviews(string salesrep, int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Salesrepsurvey.Where(s => s.SalesRep == salesrep).Count();
            var records = await _context.Salesrepsurvey.Where(s => s.SalesRep == salesrep).
                OrderByDescending(s => s.Surveydate).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Salesrepsurvey>(records, pageNo, pageSize, total));
        }

        [Route("[action]/{customer}")]
        [HttpGet]
        public async Task<ActionResult> GetCustomerReviews(string customer, int pageNo = 1, int pageSize = 10)
        {
            int skip = (pageNo - 1) * pageSize;
            int total = _context.Salesrepsurvey.Where(s => s.Surveyby == customer).Count();
            var records = await _context.Salesrepsurvey.Where(s => s.Surveyby == customer).
                OrderByDescending(s => s.Surveydate).
                Skip(skip).
                Take(pageSize).
                ToListAsync();
            return Ok(new PagedResult<Salesrepsurvey>(records, pageNo, pageSize, total));
        }


        [Route("[action]/{customer}/{salesrep}")]
        [HttpGet]
        public async Task<ActionResult> GetRepCustomerReview(string customer, string salesrep)
        {
            var target = await _context.Salesrepsurvey.SingleOrDefaultAsync(s => s.Surveyby == customer && s.SalesRep == salesrep);
            return Ok(target);
        }
        [Route("([action])")]
        [HttpPost]
        public async Task<ActionResult<Salesrepsurvey>> ReviewSalesRep([FromBody] Salesrepsurvey obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }

            else
            {
                var target = await _context.Salesrepsurvey.SingleOrDefaultAsync(s => s.SalesRep == obj.SalesRep && s.Surveyby == obj.Surveyby);
                if (target != null)
                {
                    target.Rating = obj.Rating;
                    await _context.SaveChangesAsync();
                    return Ok(target);
                }
                _context.Salesrepsurvey.Add(obj);
                await _context.SaveChangesAsync();
                return Created("api/Salesrepsurvey", obj);
            }
        }

    }
}