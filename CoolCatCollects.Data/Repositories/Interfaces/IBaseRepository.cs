using CoolCatCollects.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolCatCollects.Data.Repositories
{
	public interface IBaseRepository<T> where T : BaseEntity
	{
		T Add(T entity);
		Task AddAsync(T entity);
		T Attach(T entity);
		void Dispose();
		IEnumerable<T> Find(Expression<Func<T, bool>> conditions);
		IEnumerable<T> FindAll();
		Task<IEnumerable<T>> FindAllAsync();
		T FindOne(Expression<Func<T, bool>> conditions);
		T FindOne(int id);
		Task<T> FindOneAsync(int id);
		IQueryable<T> Queryable();
		void Remove(T entity);
		Task RemoveAsync(T entity);
		void RemoveMany(IEnumerable<T> entities);
		Task RemoveManyAsync(IEnumerable<T> entities);
		T Update(T entity);
		Task UpdateAsync(T entity);
	}
}