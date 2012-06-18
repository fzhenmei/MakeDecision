using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MakeDecision.Web.Models
{ 
    public class DepartmentUserRepository : IDepartmentUserRepository
    {
        MakeDecisionWebContext context = new MakeDecisionWebContext();

        public IQueryable<DepartmentUser> All
        {
            get { return context.DepartmentUsers; }
        }

        public IQueryable<DepartmentUser> AllIncluding(params Expression<Func<DepartmentUser, object>>[] includeProperties)
        {
            IQueryable<DepartmentUser> query = context.DepartmentUsers;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public DepartmentUser Find(int id)
        {
            return context.DepartmentUsers.Find(id);
        }

        public void InsertOrUpdate(DepartmentUser departmentuser)
        {
            if (departmentuser.Id == default(int)) {
                // New entity
                context.DepartmentUsers.Add(departmentuser);
            } else {
                // Existing entity
                context.Entry(departmentuser).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var departmentuser = context.DepartmentUsers.Find(id);
            context.DepartmentUsers.Remove(departmentuser);
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

    public interface IDepartmentUserRepository : IDisposable
    {
        IQueryable<DepartmentUser> All { get; }
        IQueryable<DepartmentUser> AllIncluding(params Expression<Func<DepartmentUser, object>>[] includeProperties);
        DepartmentUser Find(int id);
        void InsertOrUpdate(DepartmentUser departmentuser);
        void Delete(int id);
        void Save();
    }
}