// * ObjectPooler.cs
// * AUTHOR: Rica Feng
// * DESCRIPTION:
// *    Helper for pooling objects. This is much faster than instantiating and destroying objects.
// * REQUIREMENTS:
// *    Attach anywhere.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    /// <summary>
    /// What object are we pooling?
    /// </summary>
    public GameObject objToPool;

    /// <summary>
    /// How many copies should we start with?
    /// </summary>
    [SerializeField]
    private int numberOfCopies;

    /// <summary>
    /// Where to store pooled objects?
    /// </summary>
    [SerializeField]
    private Transform _storageArea;
    public Transform storageArea
    {
        get { return _storageArea; }
        private set { _storageArea = value; }
    }

    /// <summary>
    /// Number of these objects active
    /// </summary>
    public int numberOfActiveObjects
    {
        get
        {
            int count = 0;
            foreach (GameObject obj in pooledObjects)
            {
                if (obj.activeInHierarchy)
                {
                    count++;
                }
            }
            return count;
        }
    }

    private List<GameObject> pooledObjects = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numberOfCopies; ++i)
        {
            GameObject newObj = addObjToPool();
            newObj.SetActive(false);
        }
    }

    /// <summary>
    /// Retrieve
    /// </summary>
    public GameObject retrieveCopy()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        //All objects are currently in use, so create another.
        GameObject new_obj = addObjToPool();
        new_obj.SetActive(true);
        return new_obj;
    }

    public int CurrentlyActive()
    {
        int count = 0;
        foreach (GameObject obj in pooledObjects)
        {
            if (obj.activeInHierarchy)
                count++;
        }
        return count;
    }

    public void DeactivateAll()
    {
        foreach (GameObject obj in pooledObjects)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// Create a new obj, add to storage area and to the list. Return the new obj
    /// </summary>
    private GameObject addObjToPool()
    {
        GameObject copy = Instantiate(objToPool) as GameObject;
        copy.transform.SetParent(storageArea, false);
        pooledObjects.Add(copy);
        return copy;
    }

}
