using System;

namespace TheIslandKOD
{
    public class Apple : IInventoryItem
    {
        public IInventoryItemInfo info { get; }

        public IInfentoryItemState state { get; }

        public Type type => GetType();

        public Apple(IInventoryItemInfo info)
        {
            this.info = info;
            state = new InventoryItemState();
        }

        public IInventoryItem Clone()
        {
            var cloneApple = new Apple(info);
            cloneApple.state.amount = state.amount;
            return cloneApple;
        }
    }
}