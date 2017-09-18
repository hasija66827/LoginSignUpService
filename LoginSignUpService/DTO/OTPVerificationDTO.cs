using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginSignUpService.DTO
{
    public class OTPVerificationDTO
    {
        [Required]
        public Guid? UserID { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string ReceiverMobileNo { get; set; }

        [Required]
        [Range(0, 98765432198765)]
        public decimal? TransactionAmount { get; set; }

        [Required]
        public bool? IsCredit { get; set; }

    }
}