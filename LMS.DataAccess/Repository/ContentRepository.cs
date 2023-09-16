using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Repository
{
    public class ContentRepository : Repository<Content>, IContentRepository
    {
        private ApplicationDbContext _db;
        public ContentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
