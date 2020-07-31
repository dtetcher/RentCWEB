using RentC.WebUI.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RentC.WebUI.Infrastructure.Abstract
{
    public abstract class Repository<T> : IRepository<T>
        where T : class
    {
        protected RentCEntities _ctx = new RentCEntities();
        private IEnumerable<T> _local;

        public DbSet<T> All => _ctx.Set<T>();
        public IEnumerable<T> Local { 
            get {
                if (_local == null)
                {
                    _local = new List<T>();
                }
                return _local;
            }
            set { _local = value; }
        }

        public void Add(T element)
        {
            _ctx.Set<T>().Add(element);
        }

        public void AddRange(IEnumerable<T> elements)
        {
            _ctx.Set<T>().AddRange(elements);
        }

        public IEnumerable<T> Filter(Func<T, bool> _delegate)
        {
            foreach(var e in All)
            {
                if (_delegate(e))
                {
                    yield return e;
                }
            }
        }

        public void Remove(T element)
        {
            _ctx.Set<T>().Remove(element);
        }

        public void RemoveRange(IEnumerable<T> elements)
        {
            _ctx.Set<T>().RemoveRange(elements);
        }

        public DbEntityEntry<T> GetEntryFor(T entity)
        {
            return _ctx.Entry(entity);
        }

        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}