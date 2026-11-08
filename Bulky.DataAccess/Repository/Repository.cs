using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> set;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.set = db.Set<T>();
            _db.Products.Include(u => u.Category);
        }
        

        public void Add(T entity)
        {
            set.Add(entity);
           
        }

        public T Get(Expression<Func<T, bool>> filter, String? includeProperties = null)
        {
            IQueryable<T> query = set;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault(filter);

        }

        public IEnumerable<T> GetAll(String? includeProperties=null )
        {
            IQueryable<T> query = set; 
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
           
        }

        public void Remove(T entity)
        {
            set.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            set.RemoveRange(entity);
        }
    }
}
