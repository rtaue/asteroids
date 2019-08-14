using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;
}
public class PoolingManager : MonoBehaviour
{
    #region Singleton

    public static PoolingManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PoolingManager found!");
            return;
        }

        instance = this;
    }

    #endregion

    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

    // Start is called before the first frame update
    void Start()
    {
        //Create PooledObjects list and objects to it;
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

    }

    //Get a specific object from PooledObject list;
    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            //Check if pooled object is not active and matchs tag, then return it;
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
                return pooledObjects[i];
        }

        //If all pooled objects are in use, then check if it should be expanded then return it;
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    //Disable every pooled object in list;
    public void DisablePooledObjects()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy)
                pooledObjects[i].SetActive(false);
        }  
    }
}
