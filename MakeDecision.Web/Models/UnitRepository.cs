using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MakeDecision.Web.Models
{ 
    public class UnitRepository : IUnitRepository
    {
        MakeDecisionWebContext context = new MakeDecisionWebContext();

        public IQueryable<Unit> All
        {
            get { return context.Units; }
        }

        public IQueryable<Unit> AllIncluding(params Expression<Func<Unit, object>>[] includeProperties)
        {
            IQueryable<Unit> query = context.Units;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Unit Find(int id)
        {
            return context.Units.Find(id);
        }

        public void InsertOrUpdate(Unit unit)
        {
            if (unit.Id == default(int)) {
                // New entity
                context.Units.Add(unit);
            } else {
                // Existing entity
                context.Entry(unit).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var unit = context.Units.Find(id);
            context.Units.Remove(unit);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IUnitRepository : IDisposable
    {
        IQueryable<Unit> All { get; }
        IQueryable<Unit> AllIncluding(params Expression<Func<Unit, object>>[] includeProperties);
        Unit Find(int id);
        void InsertOrUpdate(Unit unit);
        void Delete(int id);
        void Save();
    }
}