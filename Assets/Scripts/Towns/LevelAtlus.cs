using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAtlus : MonoBehaviour
{
    // Singleton instance
    private static LevelAtlus instance;

    public static LevelAtlus Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelAtlus>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(LevelAtlus).Name;
                    instance = obj.AddComponent<LevelAtlus>();
                }
            }
            return instance;
        }
    }


    [System.Serializable]
    public struct TownSet
    {
        public string TownName;

        // public int RoomsInSingleRun;

        // public int numVisits;

        public GameObject room;

        // public GameObject RandomRoom
        // {
        //     get
        //     {
        //         if (PossibleRooms == null || PossibleRooms.Length == 0)
        //         {
        //             Debug.LogError($"Town {TownName} doesn't have any rooms");
        //             return null;
        //         }
        //         System.Random rnd = new System.Random();
        //         int chosenRoom = rnd.Next(PossibleRooms.Length);
        //         return PossibleRooms[chosenRoom];
        //     }
        // }
    }

    public int NumTownRoomsToComplete
    {
        get
        {
            int sum = 0;
            foreach(TownSet town in townPrefabs)
            {
                sum += 1;
            }
            return sum;
        }
    }


    public TownSet[] townPrefabs; 
    private List<TownSet> instantiatedTowns = new List<TownSet>(); 
    private int currentTownIndex = 0; 

    private void Start()
    {
        InstantiateNextTown();

        // Use this code to test town transitions
        // Invoke("ProgressTown", 10.0f);
    }

    private void InstantiateNextTown()
    {
        // If there is a previous town, clean it up
        if (instantiatedTowns.Count > 0)
        {
            Destroy(instantiatedTowns[instantiatedTowns.Count - 1].room);
            instantiatedTowns.RemoveAt(instantiatedTowns.Count - 1);
        }

        // If we haven't instantiated all the town prefabs yet, instantiate the next one
        if (currentTownIndex < townPrefabs.Length)
        {
            GameObject newTown = Instantiate(townPrefabs[currentTownIndex].room, transform);
            TownSet newTownSet = new TownSet();
            newTownSet.TownName = townPrefabs[currentTownIndex].TownName;
            newTownSet.room = newTown;
            instantiatedTowns.Add(newTownSet);
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
