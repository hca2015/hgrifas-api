using System;
using System.Runtime.CompilerServices;
using System.Transactions;
using api_lrpd.Util.Attribute;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace api_lrpd.Models
{
    [Transient]
    public class BaseRepository<T> where T : class, IEntidade
    {
        private readonly ApplicationDbContext context;
        protected readonly ContextoExecucao contextoExecucao;

        protected DatabaseFacade Database => context.Database;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(ApplicationDbContext applicationDbContext, ContextoExecucao contextoExecucao)
        {
            context = applicationDbContext;
            this.contextoExecucao = contextoExecucao;
            dbSet = context.Set<T>();
        }

        protected virtual async Task<bool> BeforeInsertAsync(IEntidade entity)
        {
            return await Task.FromResult(true);
        }

        protected virtual async Task<bool> BeforeUpdateAsync(IEntidade entity)
        {
            return await Task.FromResult(true);
        }

        protected virtual async Task<bool> BeforeDeleteAsync(IEntidade entity)
        {
            return await Task.FromResult(true);
        }

        protected virtual async Task<bool> AfterInsertAsync(IEntidade entity)
        {
            return await Task.FromResult(true);
        }

        protected virtual async Task<bool> AfterUpdateAsync(IEntidade entity)
        {
            return await Task.FromResult(true);
        }

        protected virtual async Task<bool> AfterDeleteAsync(IEntidade entity)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> InsertAsync(T entity)
        {
            if (await BeforeInsertAsync(entity))
            {
                SetarCamposControleCriado(entity);
                dbSet.Update(entity);
                context.SaveChanges();
                if (await AfterInsertAsync(entity))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            if (await BeforeUpdateAsync(entity))
            {
                SetarCamposControleAtualizado(entity);
                dbSet.Update(entity);
                context.SaveChanges();
                if (await AfterUpdateAsync(entity))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (await BeforeDeleteAsync(entity))
            {
                dbSet.Remove(entity);
                context.SaveChanges();
                if (await AfterDeleteAsync(entity))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            return await DeleteAsync(await dbSet.FindAsync(id));
        }

        private void SetarCamposControleCriado(IEntidade entity)
        {
            entity.setCriado();
            entity.setCriadoPor(contextoExecucao.GetLoginUsuario());
        }

        private void SetarCamposControleAtualizado(IEntidade entity)
        {
            entity.setAtualizado();
            entity.setAtualizadoPor(contextoExecucao.GetLoginUsuario());
        }

        public virtual async Task<T> GetIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }        

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }
    }
}

