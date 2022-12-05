using UnityEngine;

public abstract class StorageComposition : MonoBehaviour
{
    private readonly Storage _storage = new Storage();

    public Storage Storage => _storage;

    public abstract void Compose();
}