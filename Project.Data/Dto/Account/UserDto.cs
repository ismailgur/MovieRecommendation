using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Dto.Account
{
    public class UserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }
    }


    public class UserUpdateDto
    {
        [StringLength(20, ErrorMessage = "İsim en fazla 20 karakter uzunluğunda olabilir")]
        public string FirstName { get; set; }


        [StringLength(20, ErrorMessage = "İsim en fazla 20 karakter uzunluğunda olabilir")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email adresi gerekli")]
        [EmailAddress(ErrorMessage = "Geçersiz mail adresi")]
        public string Email { get; set; }
    }
}
