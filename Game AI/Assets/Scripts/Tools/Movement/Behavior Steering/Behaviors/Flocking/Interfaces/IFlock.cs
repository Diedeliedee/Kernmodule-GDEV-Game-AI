using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joeri.Tools.Movement
{
    public interface IFlock
    {
        public List<IBoid> GetPeerBoids(IBoid _exclusion);
    }
}
