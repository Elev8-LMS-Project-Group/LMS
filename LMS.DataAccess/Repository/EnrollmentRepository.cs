using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Repository
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        private ApplicationDbContext _db;
        public EnrollmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
