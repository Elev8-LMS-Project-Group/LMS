using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Repository.IRepository
{
    public interface IUserLessonProgressRepository : IRepository<UserLessonProgress>
    {
        void Update(UserLessonProgress obj);
    }
}
