using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    //public PlayerManager mainCharacter;

    public LevelAtlus levelAtlus;

    public void StartRun()
    {
        //load the first room in the game
    }

    public int numRoomsCompletedInTown;

    public int StartTownIndex;

    public int CurrentTownIndex;

    public int numDeaths { get; private set; }

    public int TotalNumRoomsCompleted
    {
        get
        {
            int TownIndex = CurrentTownIndex;
            int completedRooms = 0;
            while (TownIndex != CurrentTownIndex)
            {
                completedRooms += levelAtlus.Towns[CurrentTownIndex].RoomsInSingleRun;
                TownIndex++;
                if (TownIndex >= levelAtlus.Towns.Length)
                {
                    TownIndex = 0;
                }
            }
            return completedRooms + numRoomsCompletedInTown;
        }
    }

    public void ToNextRoom(bool PlayerLived)
    {
        if (PlayerLived)
        {
            numRoomsCompletedInTown++;
            if(numRoomsCompletedInTown >= levelAtlus.Towns[CurrentTownIndex].RoomsInSingleRun)
            {
                numRoomsCompletedInTown = 0;
                ToNextTown();
            }
        }
        else
        {
            numDeaths++;
            numRoomsCompletedInTown = 0;
            ToNextTown();
            StartTownIndex = CurrentTownIndex;
        }

        void ToNextTown()
        {
            CurrentTownIndex++;
            if (CurrentTownIndex >= levelAtlus.Towns.Length)
            {
                CurrentTownIndex = 0;
            }
        }
    }

    private void Awake()
    {
        numDeaths = 0;
    }
}
