using System.Collections;
using TheIslandKOD;
using UnityEngine;

public class Storage : InteractableRaycast
{
    private InputManager m_inputManager;
    private InventoryWithSlots m_storage;
    private Coroutine m_coroutine;
    private bool isOpen = false;
    private void Start()
    {
        m_inputManager = ReferenceSystem.instance.player.GetComponent<InputManager>();
        m_storage = new InventoryWithSlots(42);
        this.promptMessage = "Open Storage";
    }

    protected override void Interact()
    {
        if (!isOpen)
        {
            OpenStorage();
        }
    }

    private void OpenStorage()
    {
        UIStorage.instance.SetupStorageUI(m_storage);
        UIStorage.instance.SetVisible(true);
        isOpen = true;
        m_coroutine = CoroutineSystem.StartRoutine(OpenStorageUpdate());
        
    }

    public void CloseStorage()
    {
        UIStorage.instance.UnSetupStorageUI();
        UIStorage.instance.SetVisible(false);
        isOpen = false;
        CoroutineSystem.StopRoutine(m_coroutine);
        m_coroutine = null;
    }

    private IEnumerator OpenStorageUpdate()
    {
        
        while (true)
        {
            if (m_inputManager.Inventory.Close.IsPressed())
            {
                CloseStorage();
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
