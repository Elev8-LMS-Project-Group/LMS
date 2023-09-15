using LMS.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICourseRepository Course { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Course = new CourseRepository(_db);
            
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
