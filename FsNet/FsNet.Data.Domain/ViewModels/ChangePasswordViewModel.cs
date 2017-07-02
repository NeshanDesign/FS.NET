using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace FsNet.Data.Domain.ViewModels
{
    public class ChangePasswordResultModel
    {
        public IdentityResult Result { get; set; }
        public bool IsSucceeded => Result != null && Result.Succeeded;

        ManageMessageId _msgId = ManageMessageId.Null;
        public ManageMessageId MessageId { get => _msgId; set => _msgId = value;
        }
    }
}
