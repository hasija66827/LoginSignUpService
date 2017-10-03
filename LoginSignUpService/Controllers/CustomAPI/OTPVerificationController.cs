using LoginSignUpService.DTO;
using LoginSignUpService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace LoginSignUpService.Controllers.CustomAPI
{
    public class OTPVerificationController : ApiController
    {
        private LoginSignUpServiceContext db = new LoginSignUpServiceContext();

        [HttpGet]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> Get(OTPVerificationDTO OTPVerificationDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (OTPVerificationDTO == null)
                throw new Exception("OTP Verification should not have been null");

            if (OTPVerificationDTO.UserID != Guid.Empty)
                return Ok("123456");

            var user = await db.Users.FindAsync(OTPVerificationDTO.UserID);
            if (user == null)
                throw new Exception(String.Format("UserId {0} does not exist.", OTPVerificationDTO.UserID));

            if (user.NumberOfSMS < 1)
                return BadRequest("Please recharge your SMS Balance.\nCall @7987421819");

            var OTP = GenerateOTP();
            var SMSContent = String.Format(OTPVerificationDTO.SMSContent, OTP);

            //TODO: CALL SMS API(mobile number, SMSContent)

            DeductSMSBalance(user);
            await db.SaveChangesAsync();

            return Ok(OTP);
        }

        private void DeductSMSBalance(User user)
        {
            user.NumberOfSMS--;
            db.Entry(user).State = EntityState.Modified;
        }

        private string GenerateOTP()
        {
            var random = new Random();
            string OTP = "";
            for (int i = 0; i < OTPVerificationConstants.OTPLength; i++)
                OTP = String.Concat(OTP, random.Next(10).ToString());
            return OTP;
        }
    }
}