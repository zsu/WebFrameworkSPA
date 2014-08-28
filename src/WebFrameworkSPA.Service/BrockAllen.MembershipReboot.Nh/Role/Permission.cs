using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockAllen.MembershipReboot.Nh
{
    public class Permission
    {
        /// <summary>
        ///     To help ensure hashcode uniqueness, a carefully selected random number multiplier 
        ///     is used within the calculation.  Goodrich and Tamassia's Data Structures and
        ///     Algorithms in Java asserts that 31, 33, 37, 39 and 41 will produce the fewest number
        ///     of collissions.  See http://computinglife.wordpress.com/2008/11/20/why-do-hash-functions-use-prime-numbers/
        ///     for more information.
        /// </summary>
        private const int HashMultiplier = 31;

        private int? cachedHashcode;
        private ICollection<Role> _roles = new HashSet<Role>();
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual long Version { get; protected set; }
        public virtual ICollection<Role> Roles { get { return _roles; } set { _roles = value; } }

        public static bool operator ==(Permission lhs, Permission rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(Permission lhs, Permission rhs)
        {
            return !Equals(lhs, rhs);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Permission;
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (!this.IsTransient() && !other.IsTransient())
            {
                if ( this.Id== other.Id)
                {
                    var otherType = other.GetUnproxiedType();
                    var thisType = this.GetUnproxiedType();
                    return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            // once we have a hashcode we'll never change it
            if (this.cachedHashcode.HasValue)
            {
                return this.cachedHashcode.Value;
            }

            // when this instance is new we use the base hash code
            // and remember it, so an instance can NEVER change its
            // hash code.
            if (this.IsTransient())
            {
                this.cachedHashcode = base.GetHashCode();
            }
            else
            {
                unchecked
                {
                    // It's possible for two objects to return the same hash code based on 
                    // identically valued properties, even if they're of two different types, 
                    // so we include the object's type in the hash calculation
                    var hashCode = this.GetType().GetHashCode();
                    this.cachedHashcode = (hashCode * HashMultiplier)
                                          ^ this.Id.GetHashCode();
                }
            }

            return this.cachedHashcode.Value;
        }

        protected bool IsTransient()
        {
            return this.Version == default(long);
        }

        /// <summary>
        ///     When NHibernate proxies objects, it masks the type of the actual entity object.
        ///     This wrapper burrows into the proxied object to get its actual type.
        /// 
        ///     Although this assumes NHibernate is being used, it doesn't require any NHibernate
        ///     related dependencies and has no bad side effects if NHibernate isn't being used.
        /// 
        ///     Related discussion is at http://groups.google.com/group/sharp-architecture/browse_thread/thread/ddd05f9baede023a ...thanks Jay Oliver!
        /// </summary>
        protected virtual Type GetUnproxiedType()
        {
            return this.GetType();
        }
    }
}
