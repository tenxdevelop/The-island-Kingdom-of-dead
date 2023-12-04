using System.Collections;
using TheIslandKOD;
using UnityEngine;

public class Storage : InteractableRaycast
{

    private InventoryWithSlots m_storage;
    private bool isOpen = false;

    private void Start()
    {
        m_storage = new InventoryWithSlots(42);
        this.promptMessage = "Open Storage";
    }

    protected override void Interact()
    {
        if (!isOpen)
        {
            OpenStorage();
        }
        else
        {
            CloseStorage();
        }
    }

    private void OpenStorage()
    {
        UIStorage.instance.SetupStorageUI(m_storage);
        UIStorage.instance.SetVisible(true);
        isOpen = true;
        

    }

    public void CloseStorage()
    {
        UIStorage.instance.UnSetupStorageUI();
        UIStorage.instance.SetVisible(false);
        isOpen = false; 
    }

}
