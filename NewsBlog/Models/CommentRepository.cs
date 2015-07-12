using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class CommentRepository
    {
        private const String ERROR_OPEN_DATABASE = "Error open table Comments in database";
        public Comment GetCommentById(Guid commentId)
        {
            return _database.Comments.SingleOrDefault(x => x.CommentId.Equals(commentId));
        }

        public IEnumerable<Comment> GetCommentsByUserId(Guid userId)
        {
            return _database.Comments.Where(x => x.Owner.UserId.Equals(userId)).ToList();
        }

        public IEnumerable<Comment> GetCommentsByPublicationId(Guid publicationId)
        {
            return _database.Comments.Where(x => x.Publication.NewsId.Equals(publicationId)).ToList();
        }
        public CommentRepository(NewsBlogDatabase db)
        {
            if (db == null)
                throw new ArgumentException(ERROR_OPEN_DATABASE);
            _database = db;
        }

        private readonly NewsBlogDatabase _database;
    }
}