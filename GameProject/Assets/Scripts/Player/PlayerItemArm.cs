using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class PlayerItemArm : MonoBehaviour
{
    [SerializeField] private List<ItemArm> m_itemArm = new List<ItemArm>();
    public List<ItemArm> ItemArm => m_itemArm;

}


namespace TheIslandKOD
{
    [System.Serializable]
    public sealed class ItemArm
    {
        public string key;
        public GameObject item;

        public ItemArm()
        {

        }

    }
}
