using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    //public PlayerManager mainCharacter;

    [ContextMenu("Hot Test")]
    public void HotTestValues()
    {
        switch (currentProgress)
        {
            case ProgressState.PRE_RUN:
                Debug.Log($"currently in Pre-Run, player is in room {numRoomsCompletedInCurrentTown}. Data shows\nNumber of towns cleared: {numTownsCompleted}, index of starting town and current town {startTownIndex} -> {CurrentTownIndex}({UnmodulatedTownIndex})");
                break;
            case ProgressState.MID_RUN:
                Debug.Log($"currently in Mid-Run, player started in town {startTownIndex} and is now in room {numRoomsCompletedInCurrentTown} of town {CurrentTownIndex}({UnmodulatedTownIndex}).\nData shows: Number of towns cleared: {numTownsCompleted}, total number of rooms cleared: {TotalNumRoomsCompleted}");
                break;
            default:
                string scene = numRoomsCompletedInCurrentTown <= 0 ? "Facing final boss" : "Viewing post credits";
                Debug.Log($"Now in final segment {scene}, player started in town {startTownIndex}\nData shows: current town: {CurrentTownIndex}({UnmodulatedTownIndex}). Number of towns cleared: {numTownsCompleted}, total number of rooms cleared: {TotalNumRoomsCompleted}");
                break;
        }
    }
    [SerializeField][Tooltip("Editor only field, no bearing on game")] private bool TestingPlayerLived;
    [ContextMenu("Hot Test Transition, Level beat")]
    public void HotTestChange()
    {
        HotTestValues();
        string MCstatus = TestingPlayerLived ? "cleared the room" : "died";
        Debug.Log($"Moving to next room given player {MCstatus}");
        _ = ToNextRoom(TestingPlayerLived);
        HotTestValues();
    }

    public enum ProgressState
    {
        PRE_RUN, MID_RUN, ALL_TOWNS_CLEAR
    }

    public LevelAtlus_Old levelAtlus;

    public int numRoomsCompletedInCurrentTown;
    public ProgressState currentProgress;

    public int startTownIndex;

    [field: SerializeField] public int UnmodulatedTownIndex { get; private set; }
    public int CurrentTownIndex => UnmodulatedTownIndex % levelAtlus.Towns.Length;

    public int numDeaths { get; private set; }

    public int numTownsCompleted
    {
        get
        {
            if (currentProgress == ProgressState.PRE_RUN)
            {
                return 0;
            }
            if (currentProgress == ProgressState.MID_RUN)
            {
                return UnmodulatedTownIndex - startTownIndex;
            }
            //if current progress shows all towns cleared
            return levelAtlus.Towns.Length;
        }
    }
    public int TotalNumRoomsCompleted
    {
        get
        {
            int completedRooms = 0;
            if(currentProgress == ProgressState.PRE_RUN)
            {
                return 0;
            }
            if (currentProgress == ProgressState.MID_RUN)
            {
                int TownIndex = startTownIndex;
                while (TownIndex != CurrentTownIndex)
                {
                    completedRooms += levelAtlus.Towns[CurrentTownIndex].RoomsInSingleRun;
                    TownIndex++;
                    if (TownIndex >= levelAtlus.Towns.Length)
                    {
                        TownIndex = 0;
                    }
                }
            }
            else //if (currentProgress == ProgressState.ALL_TOWNS_CLEAR)
            {
                foreach (LevelAtlus_Old.TownSet town in levelAtlus.Towns)
                {
                    completedRooms += town.RoomsInSingleRun;
                }
            }
            return completedRooms + numRoomsCompletedInCurrentTown;
        }
    }

    public GameObject StartRun()
    {
        //load the first room in the game
        numRoomsCompletedInCurrentTown = 0;
        startTownIndex = -1;
        UnmodulatedTownIndex = -1;
        currentProgress = ProgressState.PRE_RUN;
        return levelAtlus.StartRoom1;
    }

    public GameObject ToNextRoom(bool PlayerLived)
    {
        switch (currentProgress)
        {
            case ProgressState.PRE_RUN:
                if (numRoomsCompletedInCurrentTown <= 0)
                {
                    numRoomsCompletedInCurrentTown = 1;
                    return levelAtlus.StartRoom2;
                }
                // else on the secoond room
                numRoomsCompletedInCurrentTown = 0;
                startTownIndex = 0;
                UnmodulatedTownIndex = 0;
                currentProgress = ProgressState.MID_RUN;
                return levelAtlus.Towns[0].RandomRoom;

            case ProgressState.MID_RUN:
                if (PlayerLived)
                {
                    numRoomsCompletedInCurrentTown++;
                    if(numRoomsCompletedInCurrentTown >= levelAtlus.Towns[CurrentTownIndex].RoomsInSingleRun)
                    {
                        ToNextTown();
                        if (numTownsCompleted >= levelAtlus.Towns.Length)
                        {
                            currentProgress = ProgressState.ALL_TOWNS_CLEAR;
                            numRoomsCompletedInCurrentTown = 0;
                            return levelAtlus.FinalBossRoom;
                        }
                    }
                }
                else //player died
                {
                    numDeaths++;
                    ToNextTown();
                    startTownIndex = CurrentTownIndex;
                    UnmodulatedTownIndex = startTownIndex;
                }
                return levelAtlus.Towns[CurrentTownIndex].RandomRoom;

            default: //case progressState.ALL_TOWNS_CLEAR
                if (PlayerLived)
                {
                    numRoomsCompletedInCurrentTown++;
                    return levelAtlus.PostCreditsRoom;
                }
                //else the player died
                numDeaths++;
                currentProgress = ProgressState.MID_RUN;
                ToNextTown();
                startTownIndex = CurrentTownIndex;
                UnmodulatedTownIndex = startTownIndex;
                return levelAtlus.Towns[CurrentTownIndex].RandomRoom;
        }

        void ToNextTown()
        {
            UnmodulatedTownIndex++;
            numRoomsCompletedInCurrentTown = 0;
        }
    }

    private void Awake()
    {
        numDeaths = 0;
        numRoomsCompletedInCurrentTown = 0;
        startTownIndex = -1;
        UnmodulatedTownIndex = -1;
        currentProgress = ProgressState.PRE_RUN;
    }
}
