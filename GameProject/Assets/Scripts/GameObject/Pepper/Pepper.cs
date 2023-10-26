using System;
using TheIslandKOD;
using UnityEngine;

namespace TheIslandKOD
{
    public class Pepper : IInventoryItem
    {
        public IInventoryItemInfo info { get; }

        public IInfentoryItemState state { get; }

        public Type type => GetType();

        public Pepper(IInventoryItemInfo info)
        {
            this.info = info;
            state = new InventoryItemState();
        }

        public IInventoryItem Clone()
        {
            var clonePepper = new Pepper(info);
            clonePepper.state.amount = state.amount;
            return clonePepper;
        }
    }
}
