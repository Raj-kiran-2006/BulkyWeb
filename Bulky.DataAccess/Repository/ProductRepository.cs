using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Bulky.DataAccess.Repository
{
    public  class ProductRepository : Repository<Product> , IProductRepository
    {
        public ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext _db):base(_db)
        {
            this._db = _db;

        }

        public void Update(Product obj)
        {
            var trackedProduct = _db.Products.Local
                .FirstOrDefault(product => product.Id == obj.Id);

            if (trackedProduct != null)
            {
                _db.Entry(trackedProduct).CurrentValues.SetValues(obj);
                return;
            }

            _db.Products.Update(obj);
        }
    }
}
