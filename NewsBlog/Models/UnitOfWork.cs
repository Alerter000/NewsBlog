using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork()
        {
            _database = new NewsBlogDatabase();
        }
        public void Commit()
        {
            try
            {
                _database.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
        }
        public NewsBlogDatabase DataContext
        {
            get
            {
                return _database;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                    _database.Dispose();
                _disposed = true;
            }
        }

        private bool _disposed;
        private readonly NewsBlogDatabase _database;
    }
}