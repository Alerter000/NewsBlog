using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class News
    {
        [Key]
        public Guid NewsId { get; set; }

        [DisplayName("Категория")]
        [Required(ErrorMessage="Выберите категорию")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Введите заголовок")]
        [DisplayName("Заголовок")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Заголовок может содержать не более 200 символов и не менее 5")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введите новость (не менее 20 символов)")]
        [DisplayName("Новость")]
        [StringLength(5000, MinimumLength = 20, ErrorMessage = "Текст должен содержать не менее 20 символов")]
        public string Content { get; set; }

        [DisplayName("Дата публикации")]
        public DateTime DatePublication { get; set; }

        [DisplayName("Фото")]
        public byte[] Image { get; set; }
        public virtual User Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}