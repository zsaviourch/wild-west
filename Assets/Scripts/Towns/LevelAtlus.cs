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

        public int numVisits;

        public GameObject[] roomPrefabs;

        public int CurrentInstantiatedIndex { get; private set; }
        //to un-instanciate all rooms in the town, simply pass in a negative number
        public void SetInstantiatedIndex(int newIndex)
        {
            //clean up any previous towns
            //foreach(GameObject townPrefab in instantiatedTowns)
            //{
            //    Destroy(townPrefab);
            //}
            //instantiatedTowns.Clear();
            if (CurrentInstantiatedTown != null)
            {
                Destroy(CurrentInstantiatedTown);
            }
            if (newIndex < 0 || newIndex >= roomPrefabs.Length)
            {
                CurrentInstantiatedIndex = newIndex;
                CurrentInstantiatedTown = null;
                return;
            }
            CurrentInstantiatedTown = Instantiate(roomPrefabs[newIndex], Instance.transform);
        }

        public GameObject CurrentInstantiatedTown { get; private set; }

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
            foreach (TownSet town in towns)
            {
                sum += town.roomPrefabs.Length;
            }
            return sum;
        }
    }


    public TownSet[] towns;

    //out-of-bounds indices for the list of town are acceptable, just use the modulus to loop it around
    public int UnmodulatedTownIndex { get => unmodulatedTownIndex; private set => unmodulatedTownIndex = value; }
    [SerializeField] private int unmodulatedTownIndex = 0;
    public int CurrentTownIndex
    {
        get
        {
            int remainder = UnmodulatedTownIndex % towns.Length;
            //modulus operator doesn't work right for negative divisors; correct it with some addition
            if (remainder < 0)
            {
                remainder += towns.Length;
            }
            return remainder;
        }
    }

    public int NumRoomsCompleated => unmodulatedTownIndex - startTownIndex;

    private void Start()
    {
        towns[CurrentTownIndex].SetInstantiatedIndex(0);

        // Use this code to test town transitions
        // Invoke("ProgressTown", 10.0f);
    }

    private void ToNextRoom()
    {
        int nextRoomIndex = towns[CurrentTownIndex].CurrentInstantiatedIndex + 1;
        //if the room we have now is the last in our current town, move to the next
        if (nextRoomIndex >= towns[CurrentTownIndex].roomPrefabs.Length)
        {
            GoToTown(unmodulatedTownIndex + 1);
            return;
        }
        //else just move to the next room in the same town
        towns[CurrentTownIndex].SetInstantiatedIndex(nextRoomIndex);
    }
    //
    private void GoToTown(int givenIndex)
    {
        //un-instantiate the last town
        towns[CurrentTownIndex].SetInstantiatedIndex(-1);
        //update the town index
        unmodulatedTownIndex = givenIndex;
        //instantiate the first room in the new town
        towns[CurrentTownIndex].SetInstantiatedIndex(0);
        towns[CurrentTownIndex].numVisits++;
    }

    [Header("Player Progress")]
    [SerializeField] private int startTownIndex;
    public int deathCount;


    public void ProgressTown(bool playerLived)
    {
        //if the player defeated all enemies
        //      he gets to move on to the next room
        //      he keeps all of his progress
        //      if he beat all rooms in all the towns, he gets to face the final boss
        if (playerLived)
        {
            ToNextRoom();
        }
        //else if he died
        //      he gets kicked straight to the next room (index is modulated)
        //          this town will also be considered his first
        //      his death count goes up
        else
        {
            startTownIndex = CurrentTownIndex + 1;
            GoToTown(startTownIndex);
            deathCount++;
        }
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
