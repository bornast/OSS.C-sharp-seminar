﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sindikat.Identity.Application.Interfaces;

namespace Sindikat.Identity.Persistence.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbContext Db;
        protected readonly DbSet<T> DbSet;

        public BaseRepository(IdentityDbContext db)
        {
            Db = db;
            DbSet = Db.Set<T>();
        }

        public IQueryable<T> Query()
        {
            return this.DbSet;
        }

        public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> whereExpression)
        {
            var entities = await this.DbSet.Where(whereExpression).ToListAsync();

            return entities;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereExpression)
        {
            var entity = await this.DbSet.FirstOrDefaultAsync(whereExpression);

            return entity;
        }

        public async Task<T> FindAsync(int id)
        {
            var entity = await this.DbSet.FindAsync(id);

            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await this.DbSet.ToListAsync();

            return entities;
        }

        public T Single(int id)
        {
            return this.DbSet.Find(id);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Persist(T entity)
        {
            DbSet.Add(entity);
        }

        public void PersistRange(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
        }

        public void Flush()
        {
            Db.SaveChanges();
        }

        public async Task FlushAsync()
        {
            await Db.SaveChangesAsync();
        }
    }
}