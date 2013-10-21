using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public virtual Type getUnproxiedTyped()
        {
            return this.GetType();
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
            if (object.ReferenceEquals(obj, this))
            {
                return true;
            }

            //Check keys and types
            if (((BaseModel)obj).CompositeKey != null && this.CompositeKey != null &&
                String.Equals(this.CompositeKey, ((BaseModel)obj).CompositeKey, StringComparison.CurrentCultureIgnoreCase))
            {
                Type otherType = ((BaseModel)obj).getUnproxiedTyped();
                Type thisType = this.getUnproxiedTyped();
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
            if (this == null || this.CompositeKey == null)
            {
                return base.GetHashCode();
            }
            else
            {
                return this.CompositeKey.GetHashCode();
            }
        }

    }
}
