using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using LoginSignUpService.Models;
using LoginSignUpService.DTO;

namespace LoginSignUpService.Controllers
{
    public enum EAuthenticationFactor
    {
        NotAuthenticated,
        OneFactorAuthenticated,
        TwoFactorAuthenticated
    }

    public class AuthenticationToken
    {
        public EAuthenticationFactor AuthenticationFactor { get; set; }
        public User User { get; set; }
    }

    public class UsersController : ApiController
    {
        private LoginSignUpServiceContext db = new LoginSignUpServiceContext();

        [HttpGet]
        [ResponseType(typeof(AuthenticationToken))]
        public IHttpActionResult AuthenticateUser(AuthenticateUserDTO authenticateUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (authenticateUserDTO == null)
                return BadRequest("userDTO should not have been null");

            var authenitcationFactor = EAuthenticationFactor.NotAuthenticated;
            var user = db.Users.Where(u => u.MobileNo == authenticateUserDTO.MobileNo).FirstOrDefault();
            if (user != null)
            {
                if (string.Compare(user.Password, authenticateUserDTO.Password) == 0)
                {
                    authenitcationFactor = EAuthenticationFactor.OneFactorAuthenticated;
                    if (string.Compare(user.DeviceId, authenticateUserDTO.DeviceId) == 0)
                        authenitcationFactor = EAuthenticationFactor.TwoFactorAuthenticated;
                }
            }
            var authenticationToken = new AuthenticationToken()
            {
                AuthenticationFactor = authenitcationFactor,
                User = user
            };
            return Ok(authenticationToken);
        }

        // POST: api/Users
        [HttpPost]
        [ResponseType(typeof(AuthenticationToken))]
        public async Task<IHttpActionResult> Post(CreateUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userDTO == null)
                return BadRequest("userDTO should not have been null");

            var user = new User()
            {
                UserId = Guid.NewGuid(),
                NumberOfSMS = 50,
                AddressLine = userDTO.BI.AddressLine,
                BusinessName = userDTO.BI.BusinessName,
                BusinessType = userDTO.BI.BusinessType,
                City = userDTO.BI.City,
                DateOfBirth = userDTO.PI.DateOfBirth,
                EmailId = userDTO.PI.EmailId,
                FirstName = userDTO.PI.FirstName,
                GSTIN = userDTO.BI.GSTIN,
                IsRegisteredUser = false,
                LastName = userDTO.PI.LastName,
                Latitude = userDTO.BI.Cordinates.Latitude,
                Longitude = userDTO.BI.Cordinates.Longitude,
                MobileNo = userDTO.PI.MobileNo,
                Passcode = userDTO.PI.Passcode,
                Password = userDTO.PI.Password,
                PinCode = userDTO.BI.PinCode,
                State = userDTO.BI.State,
                DeviceId = userDTO.DeviceId,
            };
            db.Users.Add(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(userDTO.PI.MobileNo))
                    return BadRequest(String.Format("The mobileNo {0} you entered already exist with another account. Please try with another number", userDTO.PI.MobileNo));
                throw;
            }

            var authenticationToken = new AuthenticationToken()
            {
                AuthenticationFactor = EAuthenticationFactor.NotAuthenticated,
                User = user
            };
            return Ok(authenticationToken);
        }

        [HttpPut]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> Put(Guid id, UpdateUserDTO updateUserDTO)
        {
            if (updateUserDTO == null)
                throw new Exception("updateUserDTO should not have been null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await db.Users.FindAsync(id);

            if (user == null)
                throw new Exception(String.Format("User with id {0} not found", id));

            user.Passcode = updateUserDTO.Passcode;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string mobileNo)
        {
            return db.Users.Count(e => e.MobileNo == mobileNo) > 0;
        }
    }
}