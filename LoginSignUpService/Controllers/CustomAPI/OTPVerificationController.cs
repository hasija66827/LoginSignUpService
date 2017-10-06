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
                throw new Exception("OTP Verification should not have been null.");

            string OTP = null;
            if (OTPVerificationDTO.UserID == Guid.Empty)
                OTP = SendOTP(OTPVerificationDTO.ReceiverMobileNo, OTPVerificationDTO.SMSContent);
            else
            {
                var user = await db.Users.FindAsync(OTPVerificationDTO.UserID);
                if (user == null)
                    throw new Exception(String.Format("UserId {0} does not exist.", OTPVerificationDTO.UserID));

                if (user.NumberOfSMS < 1)
                    return BadRequest("Please recharge your SMS Balance by clicking on Settings.");

                OTP = SendOTP(OTPVerificationDTO.ReceiverMobileNo, OTPVerificationDTO.SMSContent);
                user.NumberOfSMS--;
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }
            return Ok(OTP);
        }

        private string SendOTP(string mobileNo, string SMSContent)
        {
            var OTP = GenerateDummyOTP();
            var formattedSMSContent = String.Format(SMSContent, OTP);
            var IsSMSSent = SendSMSDummy(mobileNo, formattedSMSContent);
            if (!IsSMSSent)
                throw new Exception("SMS API failed, Contact Administrator.");
            return OTP;
        }

        private bool SendSMSDummy(string mobileNo, string SMSContent)
        {
            //TODO call actual SMS API
            return true;
        }

        //TODO: Remove it.
        private string GenerateDummyOTP() {
            return "123456";
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