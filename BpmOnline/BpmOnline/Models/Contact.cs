using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BpmOnline.Models
{
    public class Contact
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        [RegularExpression(@"^\+(?:[0-9] ?){6,14}[0-9]$", ErrorMessage = "Not a valid Phone number. Mobile phone should start with a plus sign.")]
        [DataType(DataType.PhoneNumber)]
        public string MobilePhone { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string Dear { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string JobTitle { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public void Init(Guid id, string name, string mobilePhone, string dear, string jobTitle, DateTime? birthDate)
        {
            Id = id;
            Name = name;
            MobilePhone = mobilePhone;
            Dear = dear;
            JobTitle = jobTitle;
            BirthDate = birthDate;
        }
    }
}