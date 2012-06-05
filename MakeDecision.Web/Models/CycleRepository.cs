using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MakeDecision.Web.Models
{ 
    public class CycleRepository : ICycleRepository
    {
        MakeDecisionWebContext context = new MakeDecisionWebContext();

        public IQueryable<Cycle> All
        {
            get { return context.Cycles; }
        }

        public IQueryable<Cycle> AllIncluding(params Expression<Func<Cycle, object>>[] includeProperties)
        {
            IQueryable<Cycle> query = context.Cycles;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Cycle Find(int id)
        {
            return context.Cycles.Find(id);
        }

        public void InsertOrUpdate(Cycle cycle)
        {
            if (cycle.Id == default(int)) {
                // New entity
                context.Cycles.Add(cycle);
            } else {
                // Existing entity
                context.Entry(cycle).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var cycle = context.Cycles.Find(id);
            context.Cycles.Remove(cycle);
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

    public interface ICycleRepository : IDisposable
    {
        IQueryable<Cycle> All { get; }
        IQueryable<Cycle> AllIncluding(params Expression<Func<Cycle, object>>[] includeProperties);
        Cycle Find(int id);
        void InsertOrUpdate(Cycle cycle);
        void Delete(int id);
        void Save();
    }
}