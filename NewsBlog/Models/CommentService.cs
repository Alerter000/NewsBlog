using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class CommentService
    {
        private const String ERROR_OPEN_DATABASE = "Error open table UserFiles in database";
        private const String ERROR_CREATE_COMMENT = "Error create comment in database";
        private const String ERROR_DELETE_COMMENT = "Error delete comment in database";
        private const String ERROR_UPDATE_COMMENT = "Error update comment in database";
        public CommentService(NewsBlogDatabase db)
        {
            if (db == null)
                throw new ArgumentException(ERROR_OPEN_DATABASE);
            _database = db;
        }
        public Comment Create(User user, News publication, String content)
        {
            if (String.IsNullOrEmpty(content) || user == null)
                throw new ArgumentException(ERROR_CREATE_COMMENT);
            Comment comment = new Comment()
            {
                Owner = user,
                Content = content,
                Publication = publication,
                DatePublication = DateTime.Now,
                CommentId = Guid.NewGuid()
            };
            _database.Comments.Add(comment);
            return comment;
        }

        public void Delete(Comment comment)
        {
            if (comment == null)
                throw new ArgumentException(ERROR_DELETE_COMMENT);
            _database.Comments.Remove(comment);
        }

        public void Delete(Guid id)
        {
            CommentRepository rep = new CommentRepository(_database);
            Comment comment = rep.GetCommentById(id);
            if (comment == null)
                throw new ArgumentException(ERROR_DELETE_COMMENT);
            _database.Comments.Remove(comment);
        }

        private readonly NewsBlogDatabase _database;
    }
}