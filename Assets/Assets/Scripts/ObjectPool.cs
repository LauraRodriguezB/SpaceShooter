using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : IPooleable
{
    public List<T> Availables;
    public List<T> InUse;
    public GameObject PreFab;
    Transform _parentTransform;

    public ObjectPool(GameObject preFab, int poolSize, Transform parentTransform = null)
    {
        Availables = new List<T>();
        InUse = new List<T>();
        this.PreFab = preFab;
        _parentTransform = parentTransform;

        for (int i = 0; i < poolSize; i++)
        {
            CreateObjects();
        }
    }

    public T GetNext()
    {
        if (Availables.Count > 0)
        {
            T obj = Availables[0];
            Availables.RemoveAt(0);
            InUse.Add(obj);
            obj.OnActivate();
            return obj;
        }

        // Create a new one if there's no bullet available
        CreateObjects();
        return GetNext();
    }

    public void ReturnToAvailable(T obj)
    {
        obj.OnDeactivate();
        InUse.Remove(obj);
        Availables.Add(obj);
    }

    private void CreateObjects()
    {
        GameObject temp = GameObject.Instantiate(PreFab, _parentTransform);
        T obj = temp.GetComponent<T>();
        obj.OnDeactivate();
        Availables.Add(obj);
    }
}
