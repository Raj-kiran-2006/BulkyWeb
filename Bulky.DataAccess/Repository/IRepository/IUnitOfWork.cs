using System;
using System.Collections.Generic;
using System.Text;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        // Add other repositories here as you build them (e.g., IProductRepository Product { get; })
        IProductRepository Product { get; }
        void Save();
    }
}
