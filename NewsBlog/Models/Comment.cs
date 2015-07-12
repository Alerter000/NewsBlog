using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }
        [DisplayName("Дата публикации")]
        public DateTime DatePublication { get; set; }

        [DisplayName("Комментарий")]
        public string Content { get; set; }

        public virtual News Publication { get; set; }
        public virtual User Owner { get; set; }
    }
}