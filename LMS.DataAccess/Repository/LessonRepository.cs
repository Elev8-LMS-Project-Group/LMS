using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Repository
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        private ApplicationDbContext _db;
        public LessonRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
