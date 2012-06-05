using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MakeDecision.Web.Models
{ 
    public class KeyDataRepository : IKeyDataRepository
    {
        MakeDecisionWebContext context = new MakeDecisionWebContext();

        public IQueryable<KeyData> All
        {
            get { return context.KeyDatas; }
        }

        public IQueryable<KeyData> AllIncluding(params Expression<Func<KeyData, object>>[] includeProperties)
        {
            IQueryable<KeyData> query = context.KeyDatas;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public KeyData Find(int id)
        {
            return context.KeyDatas.Find(id);
        }

        public void InsertOrUpdate(KeyData keydata)
        {
            if (keydata.Id == default(int)) {
                // New entity
                context.KeyDatas.Add(keydata);
            } else {
                // Existing entity
                context.Entry(keydata).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var keydata = context.KeyDatas.Find(id);
            context.KeyDatas.Remove(keydata);
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

    public interface IKeyDataRepository : IDisposable
    {
        IQueryable<KeyData> All { get; }
        IQueryable<KeyData> AllIncluding(params Expression<Func<KeyData, object>>[] includeProperties);
        KeyData Find(int id);
        void InsertOrUpdate(KeyData keydata);
        void Delete(int id);
        void Save();
    }
}