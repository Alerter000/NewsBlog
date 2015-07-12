using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class NewsAndComments
    {
        public News News { get; set; }
        public IEnumerable<Comment> Comments 
        { 
            get
            {
                return _comments.OrderBy(c => c.DatePublication);
            }
            set
            {
                _comments = value;
            }
        }
        public String CurrentComment { get; set; }
        private IEnumerable<Comment> _comments;
    }
}