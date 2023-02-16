using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelAtlus", menuName = "ScriptableObjects/Level Atlus")]
public class LevelAtlus : ScriptableObject
{
    [System.Serializable]
    public struct TownSet
    {
        public string TownName;

        public int RoomsInSingleRun;

        public GameObject[] RoomPrefabs;

    }

    public TownSet[] Towns;

}
