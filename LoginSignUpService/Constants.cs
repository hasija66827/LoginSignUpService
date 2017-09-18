using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginSignUpService
{
    public class Constants
    {
        public static readonly int OTPLength = 6;
        public static readonly string CreditTransactionSMSFormat = "OTP for Deducting INR {0} from your wallet at {1} is {2}. Please share it with merchant only for security reasons.";
        public static readonly string DebitTransactionSMSFormat = "OTP for Adding INR {0} to your wallet at {1} is {2}. Please share it with merchant only for security reasons.";
    }
}