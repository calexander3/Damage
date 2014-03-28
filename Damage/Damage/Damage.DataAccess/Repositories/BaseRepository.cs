using Damage.DataAccess.Models;
using NHibernate;
using System.Collections.Generic;

namespace Damage.DataAccess.Repositories
{
    /// <summary>
    /// Contains basic repository functionality
    /// </summary>
    /// <typeparam name="T">Model Type. Must inherit from <see cref="BaseModel"/></typeparam>
    public abstract class BaseRepository<T> where T : BaseModel
    {

        protected ISession Session = null;

        protected BaseRepository(ISession session)
        {
            Session = session;
        }

        /// <summary>
        /// Removes the specified entity from the NHibernate session.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void StopTracking(T entity)
        {
            Session.Evict(entity);
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="operationType">The type of save operation.</param>
        public virtual void Save(T entity, SaveOperation operationType = SaveOperation.Unknown)
        {
            using (var tran = Session.BeginTransaction())
            {
                switch (operationType)
                {
                    case SaveOperation.SaveNew:
                        Session.Save(entity);
                        break;
                    case SaveOperation.Update:
                        Session.Update(entity);
                        break;
                    case SaveOperation.Unknown:
                        Session.SaveOrUpdate(entity);
                        break;
                }
                tran.Commit();
            }
        }

        /// <summary>
        /// Saves the specified entity set.
        /// </summary>
        /// <param name="entitySet">The entity set.</param>
        /// <param name="operationType">The type of save operation.</param>
        public virtual void Save(IList<T> entitySet, SaveOperation operationType = SaveOperation.Unknown)
        {
            using (var tran = Session.BeginTransaction())
            {
                foreach (T entity in entitySet)
                {
                    switch (operationType)
                    {
                        case SaveOperation.SaveNew:
                            Session.Save(entity);
                            break;
                        case SaveOperation.Update:
                            Session.Update(entity);
                            break;
                        case SaveOperation.Unknown:
                            Session.SaveOrUpdate(entity);
                            break;
                    }
                }
                tran.Commit();
            }
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Delete(T entity)
        {
            using (var tran = Session.BeginTransaction())
            {
                Session.Delete(entity);
                tran.Commit();
            }
        }

        /// <summary>
        /// Deletes the specified entity set.
        /// </summary>
        /// <param name="entitySet">The entity set.</param>
        public virtual void Delete(List<T> entitySet)
        {
            using (var tran = Session.BeginTransaction())
            {
                foreach (T entity in entitySet)
                {
                    Session.Delete(entity);
                }
                tran.Commit();
            }
        }

        /// <summary>
        /// The types of save operations
        /// </summary>
        public enum SaveOperation
        {
            SaveNew,
            Update,
            Unknown
        }
    }
}