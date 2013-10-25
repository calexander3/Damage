using System;

namespace Damage.DataAccess.Models
{
    /// <summary>
    /// Contains basic model functionality
    /// </summary>
    public abstract class BaseModel
    {

        /// <summary>
        /// Gets a string that represents all of the entity keys concatenated together.
        /// </summary>
        /// <value>
        /// The composite key.
        /// </value>
        public abstract string CompositeKey { get; }

        /// <summary>
        /// Gets the unproxied typed.
        /// </summary>
        /// <returns></returns>
        public virtual Type GetUnproxiedTyped()
        {
            return GetType();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Damage.DataAccess.Models.BaseModel" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="Damage.DataAccess.Models.BaseModel" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="Damage.DataAccess.Models.BaseModel" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            //Check existence
            if (obj == null)
            {
                return false;
            }

            //Check memory reference
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            //Check keys and types
            if (((BaseModel)obj).CompositeKey != null && CompositeKey != null &&
                String.Equals(CompositeKey, ((BaseModel)obj).CompositeKey, StringComparison.CurrentCultureIgnoreCase))
            {
                var otherType = ((BaseModel)obj).GetUnproxiedTyped();
                var thisType = GetUnproxiedTyped();
                return (thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType));
            }

            return false;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            if (CompositeKey == null)
            {
                return base.GetHashCode();
            }
            return CompositeKey.GetHashCode();
        }
    }
}
