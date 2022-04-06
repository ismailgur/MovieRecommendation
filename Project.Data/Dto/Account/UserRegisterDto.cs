using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Dto.Account
{
    public class UserRegisterDto
    {
        [StringLength(20, MinimumLength = 5,ErrorMessage = "Kullanıcı adını minimum 5, maksimum 20 karakter uzunluğunda olabilir")]
        public string Username { get; set; }


        [StringLength(20, MinimumLength = 5, ErrorMessage = "Şifre minimum 5, maksimum 20 karakter uzunluğunda olabilir")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Email adresi gerekli")]
        [EmailAddress(ErrorMessage = "Geçersiz mail adresi")]
        public string Email { get; set; }


        [StringLength(20, ErrorMessage = "İsim en fazla 20 karakter uzunluğunda olabilir")]
        public string FirstName { get; set; }


        [StringLength(20, ErrorMessage = "Soyisim en fazla 20 karakter uzunluğunda olabilir")]
        public string LastName { get; set; }
    }
}
