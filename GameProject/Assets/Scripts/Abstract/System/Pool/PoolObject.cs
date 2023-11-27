using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    public void BaseInitialization()
    {
        Initialization();
    }
    protected abstract void Initialization();
}
