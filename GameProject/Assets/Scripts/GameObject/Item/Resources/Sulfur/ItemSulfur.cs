using System;

namespace TheIslandKOD
{
    public class ItemSulfur : IInventoryItem
    {
        public IInventoryItemInfo info { get; }

        public IInventoryItemState state { get; }

        public Type type => GetType();

        public ItemSulfur(IInventoryItemInfo info)
        {
            this.info = info;
            state = new InventoryItemState();

        }

        public IInventoryItem Clone()
        {
            var newItem = new ItemSulfur(info);
            newItem.state.amount = state.amount;
            return newItem;
        }

        public void OnDisable()
        {

        }

        public void OnEnable()
        {

        }
    }
}
