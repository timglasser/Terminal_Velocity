using UnityEngine;
using System.Collections.Generic;

//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights to
//Unity ObjectPool C# Class (modified from an old JS Unity Demo).
//This work is published from:
//California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice


// the Object Pool pattern implements the interface of a Factory design pattern i.e.Create and Destroy
// It demostrates the concepts of static variables, Data Structures, Encapsulation,  
public class ObjectPool : MonoBehaviour {

    // demonstrate a singleton pattern for the object pool
    // never make public just to show in the inspector
    [SerializeField]
    private static ObjectPool Instance;
    [SerializeField]
    protected GameObject prefab;
    [SerializeField]
    private int poolSize = 10;
    //  where the objects are pooled and stored
    [SerializeField]
    private GameObject[] objects;
    private int poolIndex = 0;
    private Dictionary<GameObject, bool> activeCachedObjects = new Dictionary<GameObject, bool>();

    private void Initialize()
    {
        objects = new GameObject[poolSize];

        // Instantiate the objects in the array and set them to be inactive
        for (var i = 0; i < poolSize; i++)
        {
                objects[i] = Instantiate(prefab) as GameObject;
                objects[i].name = objects[i].name + i; // rename object based on position in the cache
            objects[i].SetActive(false);
            activeCachedObjects.Add(objects[i], false); //set key objects in the dictionary to the false value
                
        }
    }

    private static GameObject GetNextObjectInCache()
    {
            GameObject obj = null;

            // The poolIndex starts out at the position of the object created
            // the longest time ago, so that one is usually free,
            // but in case not, loop through the cache until we find a free one.
            for (int i = 0; i < Instance.poolSize; i++)
            {
                obj = Instance.objects[Instance.poolIndex];

                // If we found the oldest inactive object in the pool/cache, use that.
                if (!obj.activeSelf)
                    break;

                // If not, increment index and make it loop around
                // if it exceeds the size of the cache
                Instance.poolIndex = (Instance.poolIndex + 1) % Instance.poolSize;
            }

            // The object should be inactive. If it's not, log a warning and use
            // the object created the longest ago even though it's still active.
            if (obj.activeSelf)
            {
                Debug.LogWarning(
                    "Spawn of " + Instance.prefab.name +
                    " exceeds pool size of " + Instance.poolSize +
                    "! Reusing already active object.", obj);
                ObjectPool.Destroy(obj);
            }

            // Increment index and make it loop around
            // if it exceeds the size of the pool
            Instance.poolIndex = (Instance.poolIndex + 1) % Instance.poolSize;

            return obj;
    }
 

    // The public Factory Interface to the Singleton ObjectPool(Create and Destroy)
    public static GameObject Create(Vector3 position, Quaternion rotation)
    {
        // Find the next object in the pool
        GameObject obj = GetNextObjectInCache();

        // Set the position and rotation of the object
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        // Set the object to be active
        obj.SetActive(true);
        Instance.activeCachedObjects[obj] = true;

        return obj;
    }

    public static void Destroy(GameObject objectToDestroy)
    {
        if (Instance && Instance.activeCachedObjects.ContainsKey(objectToDestroy))
        {
            objectToDestroy.SetActive(false);
            Instance.activeCachedObjects[objectToDestroy] = false;
        }
        else {
            objectToDestroy.SetActive(false);
        }
    }


    // Use this for Unity initialization
    void Awake()
    {
        // Set the Singleton
        Instance = this;
 
        // Initialize the pool
        Initialize();

        // Create a hashtable/Dictionary for active/nonactive objects
        activeCachedObjects = new Dictionary<GameObject, bool>();
    }

}