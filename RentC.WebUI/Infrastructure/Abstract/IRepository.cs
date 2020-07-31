using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.WebUI.Infrastructure.Abstract
{
    public interface IRepository<T> where T : class
    {
        public DbSet<T> All { get; }
        public IEnumerable<T> Local { get; set; }

        public void Add(T element);
        public void AddRange(IEnumerable<T> elements);

        public void Remove(T element);
        public void RemoveRange(IEnumerable<T> elements);

        public DbEntityEntry<T> GetEntryFor(T entity);

        public IEnumerable<T> Filter(Func<T, bool> _delegate);
        public void SaveChanges();
        public Task SaveChangesAsync();
    }
}
