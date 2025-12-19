using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChatBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Infrastructure
{
    /// <summary>
    /// DbContext that discovers entity types and DbSets via reflection to reduce manual wiring.
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly IReadOnlyDictionary<Type, object> _dynamicSets;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            _dynamicSets = BuildDynamicSets();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in GetEntityTypes())
            {
                modelBuilder.Entity(entityType);
            }

            base.OnModelCreating(modelBuilder);
        }

        private IEnumerable<Type> GetEntityTypes()
        {
            var domainAssembly = Assembly.GetAssembly(typeof(BaseEntity));
            if (domainAssembly == null)
            {
                return Enumerable.Empty<Type>();
            }

            return domainAssembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(BaseEntity).IsAssignableFrom(t));
        }

        private IReadOnlyDictionary<Type, object> BuildDynamicSets()
        {
            var sets = new Dictionary<Type, object>();
            var setMethod = typeof(DbContext).GetMethod(nameof(Set), Type.EmptyTypes)!;

            foreach (var entityType in GetEntityTypes())
            {
                var genericSetMethod = setMethod.MakeGenericMethod(entityType);
                var dbSet = genericSetMethod.Invoke(this, null);
                if (dbSet != null)
                {
                    sets[entityType] = dbSet;
                }
            }

            return sets;
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : BaseEntity => Set<TEntity>();

        public IQueryable Query(Type entityType)
        {
            if (_dynamicSets.TryGetValue(entityType, out var set))
            {
                return (IQueryable)set;
            }

            throw new InvalidOperationException($"No DbSet registered for type {entityType.Name}.");
        }
    }
}
