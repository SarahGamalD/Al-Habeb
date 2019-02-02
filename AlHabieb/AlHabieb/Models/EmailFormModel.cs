using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace AlHabieb.Models
{
    public class EmailFormModel
    {
        [Required(ErrorMessage = "Enter Your Name"), Display(Name = "Name")]
        public string FromName { get; set; }
        [Required(ErrorMessage = "Enter Your Email"), Display(Name = "Email")
            , EmailAddress(ErrorMessage = "Email Address Is Not Valid")]
        public string FromEmail { get; set; }
        [Required(ErrorMessage = "Enter Your Message")]
        public string Message { get; set; }
        [Required(ErrorMessage ="Enter Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Enter Telephone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{5})$", ErrorMessage = "Invalid Phone number")]
        public string Telephone { get; set; }
        public string ImagePath
        {
            get
            {
                return "~/Content/Images/logo.png";
            }

        }

    }
}