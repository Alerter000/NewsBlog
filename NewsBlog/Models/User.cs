using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите имя пользователя", AllowEmptyStrings = false)]
        [DisplayName("Имя пользователя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
        [DisplayName("Пароль")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }

        [EmailAddressAttribute]
        [DisplayName("Электронная почта")]
        public String EMail { get; set; }

        [DisplayName("Дата регистрации")]
        public DateTime DateRegistration { get; set; }

        public virtual List<News> Publications { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}