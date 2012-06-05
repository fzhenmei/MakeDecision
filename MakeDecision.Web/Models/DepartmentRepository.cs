using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MakeDecision.Web.Models
{ 
    public class DepartmentRepository : IDepartmentRepository
    {
        MakeDecisionWebContext context = new MakeDecisionWebContext();

        public IQueryable<Department> All
        {
            get { return context.Departments; }
        }

        public IQueryable<Department> AllIncluding(params Expression<Func<Department, object>>[] includeProperties)
        {
            IQueryable<Department> query = context.Departments;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Department Find(int id)
        {
            return context.Departments.Find(id);
        }

        public void InsertOrUpdate(Department department)
        {
            if (department.Id == default(int)) {
                // New entity
                context.Departments.Add(department);
            } else {
                // Existing entity
                context.Entry(department).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var department = context.Departments.Find(id);
            context.Departments.Remove(department);
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

    public interface IDepartmentRepository : IDisposable
    {
        IQueryable<Department> All { get; }
        IQueryable<Department> AllIncluding(params Expression<Func<Department, object>>[] includeProperties);
        Department Find(int id);
        void InsertOrUpdate(Department department);
        void Delete(int id);
        void Save();
    }
}