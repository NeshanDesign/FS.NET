using System;

namespace FsNet.Data.Contracts.Model
{
    public interface IEntity<out T>
    { 
        T Id { get; }
        DateTime? CreationDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
    }
}
