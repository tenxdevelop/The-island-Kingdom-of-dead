
namespace TheIslandKOD
{
    public interface IInventoryItemState
    {
        bool isEquipped { get; set; }
        int amount { get; set; }

        IInventoryItemState Clone();
    }   
}
