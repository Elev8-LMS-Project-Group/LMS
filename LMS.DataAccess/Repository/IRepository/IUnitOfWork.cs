﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICourseRepository Course { get; }
        ILessonRepository Lesson { get; }
        IContentRepository Content { get; }
        IUserRepository User { get; }
        IEnrollmentRepository Enrollment { get; }
        IUserLessonProgressRepository UserLessonProgress { get; }

        void Save();
    }
}
