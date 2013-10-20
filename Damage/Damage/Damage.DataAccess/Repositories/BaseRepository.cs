﻿using NHibernate;
using System.Collections.Generic;

namespace Damage.DataAccess.Repositories
{
	public abstract class BaseRepository<T>
	{


		protected ISession m_Session = null;
		public BaseRepository(ISession Session)
		{
			m_Session = Session;
		}

		/// <summary>
		/// Removes the specified entity from the NHibernate session.
		/// </summary>
		/// <param name="Entity">The entity.</param>
		public void StopTracking(T Entity)
		{
			m_Session.Evict(Entity);
		}

		/// <summary>
		/// Saves the specified entity.
		/// </summary>
		/// <param name="Entity">The entity.</param>
		/// <param name="OperationType">The type of save operation.</param>
		public virtual void Save(T Entity, SaveOperation OperationType = SaveOperation.Unknown)
		{
			switch (OperationType) {
				case SaveOperation.SaveNew:
					m_Session.Save(Entity);
					break;
				case SaveOperation.Update:
					m_Session.Update(Entity);
					break;
				case SaveOperation.Unknown:
					m_Session.SaveOrUpdate(Entity);
					break;
			}
		}

		/// <summary>
		/// Saves the specified entity set.
		/// </summary>
		/// <param name="EntitySet">The entity set.</param>
		/// <param name="OperationType">The type of save operation.</param>
		public virtual void Save(List<T> EntitySet, SaveOperation OperationType = SaveOperation.Unknown)
		{
			foreach (T Entity in EntitySet) {
				switch (OperationType) {
					case SaveOperation.SaveNew:
						m_Session.Save(Entity);
						break;
					case SaveOperation.Update:
						m_Session.Update(Entity);
						break;
					case SaveOperation.Unknown:
						m_Session.SaveOrUpdate(Entity);
						break;
				}
			}
		}

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		/// <param name="Entity">The entity.</param>
		public virtual void Delete(T Entity)
		{
			m_Session.Delete(Entity);
		}

		/// <summary>
		/// Deletes the specified entity set.
		/// </summary>
		/// <param name="EntitySet">The entity set.</param>
		public virtual void Delete(List<T> EntitySet)
		{
			foreach (T Entity in EntitySet) {
				m_Session.Delete(Entity);
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