using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Joeri.Tools.Movement
{
    public interface IBoid
    {
        public Vector3 position { get; }
        public Vector3 velocity { get; }
    }
}
