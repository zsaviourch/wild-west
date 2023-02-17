using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewLevelAtlus", menuName = "ScriptableObjects/Level Atlus")]
public class LevelAtlus : ScriptableObject
{
    public GameObject StartRoom1, StartRoom2;

    [System.Serializable]
    public struct TownSet
    {
        public string TownName;

        public int RoomsInSingleRun;

        public GameObject[] PossibleRooms;
    }

    public int NumRoomsToComplete
    {
        get
        {
            int sum = 0;
            foreach(TownSet town in Towns)
            {
                sum += town.RoomsInSingleRun;
            }
            return sum;
        }
    }

    public TownSet[] Towns;

    public GameObject FinalBossRoom, PostCreditsRoom;

}
