using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewLevelAtlus", menuName = "ScriptableObjects/Level Atlus")]
public class LevelAtlus_Old2 : ScriptableObject
{
    public GameObject StartRoom1, StartRoom2;

    [System.Serializable]
    public struct TownSet
    {
        public string TownName;

        public int RoomsInSingleRun;

        public int numVisits;

        public GameObject[] PossibleRooms;

        public GameObject RandomRoom
        {
            get
            {
                if (PossibleRooms == null || PossibleRooms.Length == 0)
                {
                    Debug.LogError($"Town {TownName} doesn't have any rooms");
                    return null;
                }
                System.Random rnd = new System.Random();
                int chosenRoom = rnd.Next(PossibleRooms.Length);
                return PossibleRooms[chosenRoom];
            }
        }
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

    public string RoomCode(int townIndex, int roomIndex, bool reserveSet)
    {
        if (reserveSet)
        {
            if (townIndex <= 0)
            {
                return "0";
            }
            //else town index is > 0
            if (roomIndex <= 0)
            {
                return "E";
            }
            //else room index is > 0
            return "Pc";
        }
        //else not looking for resrved rooms
        if (townIndex < 0 || townIndex >= Towns.Length || roomIndex < 0 || roomIndex >= Towns[townIndex].RoomsInSingleRun)
        {
            return "N/A";
        }
        //else is a valid town and room index
        return (townIndex + 1).ToString() + ('A' + (char)roomIndex).ToString();
    }

    public TownSet[] Towns;

    public GameObject FinalBossRoom, PostCreditsRoom;

}
