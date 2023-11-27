using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheIslandKOD
{
    public struct CraftInfoCoroutine 
    {
        public readonly int timeCraft;
        public readonly Text updateText;
        public readonly Type itemCraft;
        public readonly GameObject queueObject;
        public readonly int amount;
        public CraftInfoCoroutine(int timeCraft, Text updateText, Type itemCraft, int amount, GameObject queueObject)
        {
            this.timeCraft = timeCraft;
            this.updateText = updateText;
            this.itemCraft = itemCraft;
            this.amount = amount;
            this.queueObject = queueObject;
        }
    }
}
