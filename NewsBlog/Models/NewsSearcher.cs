using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class NewsSearcher
    {
        [DisplayName("Заголовок")]
        public String Title { get; set; }

        [DisplayName("Категория")]
        public String Category { get; set; }

        [DisplayName("Ключевые слова")]
        public String KeyWords { get; set; }
        public IEnumerable<News> Results { get; set; }

        public bool Find()
        {
            if (KeyWords != null)
                KeyWords = KeyWords.ToLower();
            if (Title != null)
                Title = Title.ToLower();
            using (UnitOfWork unit = new UnitOfWork())
            {
                NewsRepository newsRep = new NewsRepository(unit.DataContext);
                Results = newsRep.GetAllNews().OrderBy(n => n.DatePublication);
                if (KeyWords != null)
                {
                    String[] keys = KeyWords.Split(' ');
                    List<News> temp = new List<News>();
                    foreach(var k in keys)
                    {
                        temp.AddRange(Results.Where(c => c.Content.ToLower().Contains(k)));
                    }
                    Results = temp;
                }
                if (Category != null && Category != String.Empty)
                    Results = Results.Where(n => n.Category.Equals(Category));
                if (Title != null && Title != String.Empty)
                    Results = Results.Where(n => n.Title.ToLower().Contains(Title));
                return Results.Count() > 0;
            }
        }
    }
}