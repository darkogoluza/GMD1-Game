using System;
using AI.Core;
using UnityEngine;

namespace AI
{
    public class CheckAmmoNode : IBehaviorNode
    {
        private readonly Func<int> _checkAmmo;

        public CheckAmmoNode(Func<int> checkAmmo)
        {
            _checkAmmo = checkAmmo;
        }

        public NodeStatus Tick()
        {
            return _checkAmmo() == 0 ? NodeStatus.Success : NodeStatus.Failure;
        }
    } 
}
