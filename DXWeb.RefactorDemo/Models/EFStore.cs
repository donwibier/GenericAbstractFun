using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo.Models
{
	public interface IDataStore<TKey, TModel>
		where TKey : IEquatable<TKey>
		where TModel : class, new()
	{
		IMapper Mapper { get; }
		TModel GetByKey(TKey key);
		IQueryable<T> Query<T>() where T : class, new();
		IQueryable<TModel> Query();
		TKey ModelKey(TModel model);
		void SetModelKey(TModel model, TKey key);
		EFResult Create(params TModel[] items);
		Task<EFResult> CreateAsync(params TModel[] items);
		EFResult Update(params TModel[] items);
		Task<EFResult> UpdateAsync(params TModel[] items);
		EFResult Delete(params TKey[] ids);
		Task<EFResult> DeleteAsync(params TKey[] ids);
	}

	public class EFResult
	{
		public bool Success { get; set; }
		public string Error { get; set; }
	}

	public abstract class EFDataStore<TEFContext, TKey, TModel, TDBModel> : IDataStore<TKey, TModel>
		where TEFContext : DbContext, new()
		where TKey : IEquatable<TKey>
		where TModel : class, new()
		where TDBModel : class, new()
	{
		readonly TEFContext context;
		readonly IMapper mapper;
		public EFDataStore(TEFContext context, IMapper mapper)
		{
			this.mapper = mapper;
			this.context = context;
		}

		public IMapper Mapper { get => mapper; }
		public TEFContext DbContext { get => context; }

		public TModel CreateModel()
		{
			return new TModel();
		}
		protected virtual TDBModel EFGetByKey(TKey key)
		{
			return DbContext.Find<TDBModel>(key);
		}

		protected virtual IQueryable<TDBModel> EFQuery()
		{
			return DbContext.Set<TDBModel>();
		}

		public virtual IQueryable<TModel> Query()
		{
			return EFQuery().ProjectTo<TModel>(Mapper.ConfigurationProvider);
		}
		public virtual IQueryable<T> Query<T>() where T : class, new()
		{
			return EFQuery().ProjectTo<T>(Mapper.ConfigurationProvider);
		}

		public virtual TModel GetByKey(TKey key)
		{
			TModel result = CreateModel();
			return Mapper.Map(EFGetByKey(key), result);
		}

		protected virtual T TransactionalExec<T>(
			Func<EFDataStore<TEFContext, TKey, TModel, TDBModel>,
			IDbContextTransaction, T> work,
			bool autoCommit = true)
		{
			T result = default;
			using (var dbTrans = DbContext.Database.BeginTransaction())
			{
				result = work(this, dbTrans);
				if (autoCommit && DbContext.ChangeTracker.HasChanges())
				{
					DbContext.SaveChanges();
					dbTrans.Commit();
				}
			}
			return result;
		}
		protected virtual void TransactionalExec<T>(
			Action<EFDataStore<TEFContext, TKey, TModel, TDBModel>,
			IDbContextTransaction> work, bool autoCommit = true)
		{
			using (var dbTrans = DbContext.Database.BeginTransaction())
			{
				work(this, dbTrans);
				if (autoCommit && DbContext.ChangeTracker.HasChanges())
				{
					DbContext.SaveChanges();
					dbTrans.Commit();
				}
			}
		}

		public abstract void SetModelKey(TModel model, TKey key);

		public abstract TKey ModelKey(TModel model);

		protected abstract TKey DBModelKey(TDBModel model);

		public virtual EFResult Create(params TModel[] items)
		{
			if (items == null)
				throw new ArgumentNullException(nameof(items));

			var result = TransactionalExec(
				(s, t) =>
				{
					foreach (var item in items)
					{
						var newItem = new TDBModel();
						Mapper.Map(item, newItem);
						var r = DbContext.Set<TDBModel>().Add(newItem);
						DbContext.SaveChanges();
						SetModelKey(item, DBModelKey(r.Entity));
					}
					try
					{
						s.DbContext.SaveChanges();
						t.Commit();
						return new EFResult { Success = true };
					}
					catch (Exception e)
					{
						return new EFResult { Success = false, Error = e.InnerException != null ? e.InnerException.Message : e.Message };
					}
				},
				false);
			return result;
		}
		public virtual EFResult Update(params TModel[] items)
		{
			if (items == null)
				throw new ArgumentNullException(nameof(items));

			var result = TransactionalExec((s, t) =>
			{
				foreach (var item in items)
				{
					var key = ModelKey(item);
					var dbModel = EFGetByKey(key);
					if (dbModel == null)
						return new EFResult
						{
							Success = false,
							Error = $"Unable to locate {typeof(TDBModel).Name}({key}) in datastore"
						};
					else
					{
						Mapper.Map(item, dbModel);
						DbContext.Entry(dbModel).State = EntityState.Modified;
						DbContext.SaveChanges();
					}
				}
				try
				{
					s.DbContext.SaveChanges();
					t.Commit();
					return new EFResult { Success = true };
				}
				catch (Exception e)
				{
					return new EFResult { Success = false, Error = e.InnerException != null ? e.InnerException.Message : e.Message };
				}
			}, false);
			return result;
		}

		public virtual EFResult Delete(params TKey[] ids)
		{
			if (ids == null)
				throw new ArgumentNullException(nameof(ids));

			var result = TransactionalExec((s, t) =>
			{
				foreach (var id in ids)
				{
					var dbModel = EFGetByKey(id);
					DbContext.Entry(dbModel).State = EntityState.Deleted;
					DbContext.SaveChanges();
				}
				try
				{
					s.DbContext.SaveChanges();
					t.Commit();
					return new EFResult { Success = true };
				}
				catch (Exception e)
				{
					return new EFResult { Success = false, Error = e.InnerException != null ? e.InnerException.Message : e.Message };
				}

			}, false);
			return result;
		}
		public async virtual Task<EFResult> DeleteAsync(params TKey[] ids)
		{
			return await Task.FromResult(Delete(ids));
		}
		public async virtual Task<EFResult> UpdateAsync(params TModel[] items)
		{
			return await Task.FromResult(Update(items));
		}
		public async virtual Task<EFResult> CreateAsync(params TModel[] items)
		{
			return await Task.FromResult(Create(items));
		}

	}
}
