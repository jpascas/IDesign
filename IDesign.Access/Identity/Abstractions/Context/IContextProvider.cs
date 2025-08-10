using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDesign.Access.Identity
{
    public interface IContextProvider
    {
        public long GetCurrentUserId();
    }
}
