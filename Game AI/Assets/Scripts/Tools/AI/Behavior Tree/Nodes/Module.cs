using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class Module<T> : Node
    {
        public T source { get; private set; }

        public Module(T source)
        {
            this.source = source;
        }
    }
}
