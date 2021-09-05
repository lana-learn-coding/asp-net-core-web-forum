using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using Core.Dto;
using DAL.Models;

namespace Core.Services.Base
{
    public interface ICrudService<W, R> where W : Entity where R : IIdentified
    {
        R Create(W entity);

        R Update(string slug, W entity);

        void Delete(string slug);

        R Get(string slug);

        R Find(Func<IQueryable<W>, IQueryable<W>> query, bool optional = false);

        List<R> List();

        List<R> List(Func<IQueryable<W>, IQueryable<W>> query);

        Page<R> Page(PageQuery pageQuery);

        Page<R> Page(PageQuery pageQuery, Func<IQueryable<W>, IQueryable<W>> query);
    }

    public abstract class CrudService<W, R> : ICrudService<W, R> where W : Entity where R : IIdentified
    {
        private static readonly Func<IQueryable<W>, IQueryable<W>> NoQuery = query => query;
        protected readonly DbContext Context;
        protected readonly DbSet<W> DbSet;
        protected List<string> DefaultSort = new() { "CreatedAt" };

        protected CrudService(DbContext context)
        {
            DbSet = context.Set<W>();
            Context = context;
        }

        /// Save new Entity. Overrideable 
        public virtual R Create(W entity)
        {
            if (entity.Id.Equals(Guid.Empty))
                throw new ConflictException($"Cannot create {typeof(W).Name} with default id");
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

            return Get(entity.Id.ToString());
        }

        /// Find one entity by id or slug
        public virtual R Get(string slug)
        {
            if (slug == null) throw new DataNotFoundException($"{typeof(W).Name} without id/slug not found");

            if (Guid.TryParse(slug, out var id))
            {
                var foundById = Query(DbSet.Where(e => e.Id == id)).FirstOrDefault();
                return foundById ?? throw new DataNotFoundException($"{typeof(W).Name} with id {slug} not found!");
            }

            var found = Query(DbSet.Where(e => e.Slug == slug)).FirstOrDefault();
            return found ?? throw new DataNotFoundException($"{typeof(W).Name} with slug {slug} not found!");
        }

        /// Find one entity, custom query can be passed
        public virtual R Find(Func<IQueryable<W>, IQueryable<W>> query, bool optional = false)
        {
            var entity = Query(query.Invoke(DbSet)).FirstOrDefault();
            if (optional) return entity;

            return entity ?? throw new DataNotFoundException($"{typeof(W).Name} not found");
        }

        /// Query entities, custom query can be passed
        public virtual List<R> List(Func<IQueryable<W>, IQueryable<W>> query)
        {
            return Query(query.Invoke(DbSet)).ToList();
        }

        /// Query entities result in page, custom query can be passed
        public virtual Page<R> Page(PageQuery pageQuery, Func<IQueryable<W>, IQueryable<W>> query)
        {
            var baseQuery = Query(query.Invoke(DbSet));

            var total = baseQuery.Count();
            if (total == 0)
                return new Page<R>
                {
                    Data = new List<R>(),
                    Meta = new PageMeta
                    {
                        CurrentPage = 1,
                        PerPage = pageQuery.Size,
                        TotalPages = 1,
                        TotalItems = 0
                    }
                };

            var sort = string.Join(",", pageQuery.IsSorted ? pageQuery.Sorts : DefaultSort);

            var items = baseQuery
                .OrderBy(sort)
                .Skip(pageQuery.Skip)
                .Take(pageQuery.Take)
                .ToList();

            return new Page<R>
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

        public R Update(string slug, W entity)
        {
            var current = GetForWrite(slug);
            Update(current, entity);
            return Get(current.Id.ToString());
        }

        public void Delete(string slug)
        {
            var current = GetForWrite(slug);
            Delete(current);
        }

        public List<R> List()
        {
            return List(NoQuery);
        }

        public Page<R> Page(PageQuery pageQuery)
        {
            return Page(pageQuery, NoQuery);
        }


        /// Query entities, return IQueryable for further extend
        /// Note that this method return READ queryable, not like List and Page which accept WRITE queryable
        public virtual IQueryable<R> Query()
        {
            return Query(DbSet.AsQueryable());
        }

        /// Delete entity. Overrideable
        /// This one is base delete method. others delete method depends on this
        protected virtual void Delete(W entity)
        {
            if (entity.Id.Equals(Guid.Empty)) throw new ConflictException($"Cannot delete default {typeof(W).Name}");
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
        protected virtual void Update(W current, W entity)
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
        protected abstract IQueryable<R> Query(IQueryable<W> queryable);

        protected W GetForWrite(string slug)
        {
            if (slug == null) throw new DataNotFoundException($"{typeof(W).Name} without id/slug not found");

            if (Guid.TryParse(slug, out var id))
            {
                var foundById = DbSet.FirstOrDefault(e => e.Id == id);
                return foundById ?? throw new DataNotFoundException($"{typeof(W).Name} with id {slug} not found!");
            }

            var found = DbSet.FirstOrDefault(e => e.Slug == slug);
            return found ?? throw new DataNotFoundException($"{typeof(W).Name} with slug {slug} not found!");
        }
    }

    public abstract class CrudService<T> : CrudService<T, T> where T : Entity
    {
        protected CrudService(DbContext context) : base(context)
        {
        }
    }

    public class SimpleCrudService<T> : CrudService<T, T> where T : Entity
    {
        protected SimpleCrudService(DbContext context) : base(context)
        {
        }

        protected override IQueryable<T> Query(IQueryable<T> queryable)
        {
            return queryable;
        }
    }
}