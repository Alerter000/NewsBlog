using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public static class Category
    {
        public const string Politics = "Политика";
        public const string Sport = "Спорт";
        public const string Art = "Искусство";
        public const string Entertainment = "Развлекательное";
        public const string Education = "Образование";
        public const string Other = "Прочее";

        private static List<String> _categories = new List<String>();

        public static List<String> Categories
        {
            get
            {
                _categories.Clear();
                _categories.Add(" ");
                _categories.Add(Politics);
                _categories.Add(Sport);
                _categories.Add(Art);
                _categories.Add(Entertainment);
                _categories.Add(Education);
                _categories.Add(Other);
                return _categories;
            }
        }
        
    }
}