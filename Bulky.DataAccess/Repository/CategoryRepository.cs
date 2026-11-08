using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Bulky.DataAccess.Repository
{
    public  class CategoryRepository : Repository<Category> , ICategoryRepository
    {
        public ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext _db):base(_db)
        {
            this._db = _db;
        }

        public void Update(Category obj)
        {
            _db.Update(obj);
        }
    }
}
