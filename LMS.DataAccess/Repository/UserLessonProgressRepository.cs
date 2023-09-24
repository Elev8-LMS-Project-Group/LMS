//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LMS.DataAccess.Repository
//{
//    internal class UserLessonProgressRepository
//    {
//    }
//}
using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Repository
{
    public class UserLessonProgressRepository : Repository<UserLessonProgress>, IUserLessonProgressRepository
    {
        private ApplicationDbContext _db;
        public UserLessonProgressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(UserLessonProgress obj)
        {
            _db.UserLessonProgresses.Update(obj);
        }
    }
}

