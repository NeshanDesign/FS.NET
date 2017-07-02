using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using FsNet.Data.Contracts.Model;

namespace FsNet.Data.Domain.Model
{
    public class Entity<T> : IEntity<T>, IComparable<Entity<T>>
    {
        private Entity()
        {
        }

        private Entity(T key)
        {
            Id = key;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; protected set; }

        private DateTime? _creationDate;

        public virtual DateTime? CreationDate
        {
            get => _creationDate;
            set => _creationDate = value ?? DateTime.Now;
        }

        private DateTime? _modifiedDate;

        public virtual DateTime? ModifiedDate
        {
            get => _modifiedDate;
            set => _modifiedDate = value ?? DateTime.Now;
        }

        [StringLength(50, ErrorMessage = @"Too many characters. length exceeded")]
        public virtual string CreatedBy { get; set; }

        [StringLength(50, ErrorMessage = @"Too many characters. length exceeded")]
        public virtual string ModifiedBy { get; set; }

 #region Equality check

        public override bool Equals(object entity)
        {
            return entity != null && (entity as Entity<T>) != null && this == (Entity<T>)entity;
        }

        
        public static bool operator ==(Entity<T> entity1, Entity<T> entity2)
        {
            return ((object)entity1 == null && 
                    (object)entity2 == null ) ||
                   ((object)entity1 != null &&
                     entity1.Id != null &&
                    (object)entity2 != null && 
                    entity1.Id.Equals(entity2.Id));
        }

        public static bool operator !=(Entity<T> entity1, Entity<T> entity2)
        {
            return !(entity1 == entity2);
        }
        
        public static bool operator >(Entity<T> entity1, Entity<T> entity2)
        {
            if (entity1 != entity2)
            {
                if (entity1.Id is long || entity1.Id is int || entity1.Id is short)
                    return Convert.ToInt64(entity1.Id) > Convert.ToInt64(entity2.Id);
                if (entity1.Id is float || entity1.Id is double)
                    return Convert.ToDouble(entity1.Id) > Convert.ToDouble(entity2.Id);
                if (entity1.Id is byte)
                    return Convert.ToByte(entity1.Id) > Convert.ToByte(entity2.Id);
                if (entity1.Id is string)
                {
                    var id1 = entity1.Id as string;
                    var id2 = entity2.Id as string;
                    return string.Compare(id1, id2, StringComparison.OrdinalIgnoreCase) == 1;
                }
                if (entity1.Id is Guid)
                {
                    var id1 = Guid.Parse(entity1.Id.ToString()) ;
                    var id2 = Guid.Parse(entity2.Id.ToString());
                    return id1.CompareTo(id2) == 1;
                }
            }

            throw new NotSupportedException("Type is not supported.");
        }

        public static bool operator <(Entity<T> entity1, Entity<T> entity2)
        {
            return !(entity1 > entity2) && entity1 != entity2;
        }

        public static bool operator <=(Entity<T> entity1, Entity<T> entity2)
        {
            return entity1 < entity2 || entity1 == entity2;
        }
        public static bool operator >=(Entity<T> entity1, Entity<T> entity2)
        {
            return entity1 > entity2 || entity1 == entity2;
        }

        public int CompareTo(Entity<T> otherEntity)
        {
            if (this == otherEntity)
                return 0;
            if (this > otherEntity)
                return 1;
            if (this < otherEntity)
                return -1;
            throw new NotSupportedException("one of the sides is not comparable.");
        }
      
        public override int GetHashCode()
        {
            try
            {
                Contract.Assert(Id != null, "Id != null");
                return Id.GetHashCode();
            }
            catch (Exception ex)
            {
                throw new Exception("GetHashCode()", ex);
            }
        }

#endregion Equality check

    }
}
