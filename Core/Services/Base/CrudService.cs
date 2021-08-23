using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using Core.Dto;
using DAL.Models;

namespace Core.Services.Base
{
    public interface ICrudService<T> where T : Entity
    {
        T Create(T entity);

        T Update(Guid id, T entity);

        T Get(Guid id);

        T Delete(Guid id);

        T Update(string slug, T entity);

        T Get(string slug);

        T Delete(string slug);

        T Find(Func<IQueryable<T>, IQueryable<T>> query, bool optional = false);

        List<T> List();

        List<T> List(Func<IQueryable<T>, IQueryable<T>> query);

        Page<T> Page(PageQuery pageQuery);

        Page<T> Page(PageQuery pageQuery, Func<IQueryable<T>, IQueryable<T>> query);
    }

    public abstract class CrudService<T> : ICrudService<T> where T : Entity
    {
        private static readonly Func<IQueryable<T>, IQueryable<T>> NoQuery = query => query;
        protected readonly DbSet<T> DbSet;
        protected readonly DbContext Context;
        protected List<string> DefaultSort = new() { "CreatedAt" };

        protected CrudService(DbContext context)
        {
            DbSet = context.Set<T>();
            Context = context;
        }

        /// Save new Entity. Overrideable 
        public virtual T Create(T entity)
        {
            DbSet.Add(entity);
            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                DbSet.Remove(entity);
                throw new ServiceException(e);
            }

            return Query(DbSet).FirstOrDefault(e => e.Id == entity.Id);
        }

        /// Delete entity. Overrideable
        /// This one is base delete method. others delete method depends on this
        protected virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                DbSet.Attach(entity);
                throw new ServiceException(e);
            }
        }

        /// Update entity. Overrideable
        /// This one is base update method. others update method depends on this
        protected virtual void Update(T current, T entity)
        {
            var entry = Context.Entry(current);
            entity.Id = current.Id;
            entry.CurrentValues.SetValues(entity);
            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                entry.Reload();
                throw new ServiceException(e);
            }
        }

        /// Custom query processing. This help us to add eager loading into all query
        /// and CRUD methods
        protected abstract IQueryable<T> Query(DbSet<T> dbSet);

        /// Find one entity, custom query can be passed
        public virtual T Find(Func<IQueryable<T>, IQueryable<T>> query, bool optional = false)
        {
            var entity = query.Invoke(Query(DbSet)).FirstOrDefault();
            if (optional) return entity;

            return entity ?? throw new DataNotFoundException($"{typeof(T).Name} not found");
        }

        /// Query entities, custom query can be passed
        public virtual List<T> List(Func<IQueryable<T>, IQueryable<T>> query)
        {
            return query.Invoke(Query(DbSet)).ToList();
        }

        /// Query entities result in page, custom query can be passed
        public virtual Page<T> Page(PageQuery pageQuery, Func<IQueryable<T>, IQueryable<T>> query)
        {
            var baseQuery = query.Invoke(Query(DbSet));

            var total = baseQuery.Count();
            if (total == 0)
                return new Page<T>
                {
                    Data = new List<T>(),
                    Meta = new PageMeta
                    {
                        CurrentPage = 1,
                        PerPage = pageQuery.Size,
                        TotalPages = 1,
                        TotalItems = 0
                    }
                };

            var sort = string.Join(",", pageQuery.IsSorted ? pageQuery.Sort : DefaultSort);

            var items = baseQuery
                .OrderBy(sort)
                .Skip(pageQuery.Skip)
                .Take(pageQuery.Take)
                .ToList();

            return new Page<T>
            {
                Data = items,
                Meta = new PageMeta
                {
                    CurrentPage = pageQuery.Page,
                    PerPage = pageQuery.Size,
                    TotalPages = (int)Math.Ceiling((double)total / pageQuery.Size),
                    TotalItems = total
                }
            };
        }

        public T Get(Guid id)
        {
            if (id == null) throw new DataNotFoundException($"{typeof(T).Name} without id not found");
            var found = Query(DbSet).FirstOrDefault(e => e.Id == id);
            return found ?? throw new DataNotFoundException($"{typeof(T).Name} with id {id} not found!");
        }

        public T Update(Guid id, T entity)
        {
            var current = Get(id);
            Update(current, entity);
            return current;
        }

        public T Delete(Guid id)
        {
            var current = Get(id);
            Delete(current);
            return current;
        }

        public T Update(string slug, T entity)
        {
            var current = Get(slug);
            Update(current, entity);
            return current;
        }

        public T Delete(string slug)
        {
            var current = Get(slug);
            Delete(current);
            return current;
        }

        public T Get(string slug)
        {
            if (slug == null) throw new DataNotFoundException($"{typeof(T).Name} without slug not found");
            var found = Query(DbSet).FirstOrDefault(e => e.Slug == slug);
            return found ?? throw new DataNotFoundException($"{typeof(T).Name} with slug {slug} not found!");
        }

        public List<T> List()
        {
            return List(NoQuery);
        }

        public Page<T> Page(PageQuery pageQuery)
        {
            return Page(pageQuery, NoQuery);
        }
    }

    public class SimpleCrudService<T> : CrudService<T> where T : Entity
    {
        protected override IQueryable<T> Query(DbSet<T> dbSet)
        {
            return dbSet.AsQueryable();
        }

        protected SimpleCrudService(DbContext context) : base(context)
        {
        }
    }
}