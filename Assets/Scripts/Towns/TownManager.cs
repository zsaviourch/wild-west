using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    // Singleton instance
    private static TownManager instance;

    public static TownManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TownManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(TownManager).Name;
                    instance = obj.AddComponent<TownManager>();
                }
            }
            return instance;
        }
    }

    public GameObject[] townPrefabs; 
    private List<GameObject> instantiatedTowns = new List<GameObject>(); 
    private int currentTownIndex = 0; 

    private void Start()
    {
        InstantiateNextTown();

        // Use this code to test town transitions
        Invoke("ProgressTown", 10.0f);
    }

    private void InstantiateNextTown()
    {
        // If there is a previous town, clean it up
        if (instantiatedTowns.Count > 0)
        {
            Destroy(instantiatedTowns[instantiatedTowns.Count - 1]);
            instantiatedTowns.RemoveAt(instantiatedTowns.Count - 1);
        }

        // If we haven't instantiated all the town prefabs yet, instantiate the next one
        if (currentTownIndex < townPrefabs.Length)
        {
            GameObject newTown = Instantiate(townPrefabs[currentTownIndex], transform);
            instantiatedTowns.Add(newTown);
            currentTownIndex++;
        }
    }

    public void ProgressTown()
    {
        // If we've instantiated all the town prefabs, do nothing
        if (currentTownIndex == townPrefabs.Length) return;

        InstantiateNextTown();
    }

    private void Awake()
    {
        // Implement the singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
