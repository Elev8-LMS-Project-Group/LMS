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
        public ILessonRepository Lesson { get; private set; }
        public IContentRepository Content { get; private set; }
        public IUserRepository User { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Course = new CourseRepository(_db);
            Lesson = new LessonRepository(_db);
            Content = new ContentRepository(_db);
            User = new UserRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
