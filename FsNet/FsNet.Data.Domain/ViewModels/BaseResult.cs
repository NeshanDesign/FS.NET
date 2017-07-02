using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsNet.Data.Domain.ViewModels
{
    public class BaseResult
    {
        public bool IsOk { get; set; }
        public bool HasResult { get; set; }
        public bool HasError => Exception != null;
        public string GeneralMessage { get; set; }
        public List<string> Messages { get; set; }
        public Exception Exception { get; set; }
    }
}
