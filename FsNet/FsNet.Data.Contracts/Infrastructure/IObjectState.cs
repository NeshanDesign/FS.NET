
using System.ComponentModel.DataAnnotations.Schema;

namespace FsNet.Data.Repository.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}