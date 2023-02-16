using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Dialogue
{
    public DialogueTownLevel TownLevel;
    public DialogueTypeEnum DialogueType;
    public List<DialogueSentence> Sentences;

    public Dialogue(DialogueTownLevel townLevel, DialogueTypeEnum dialogueType)
    {
        TownLevel = townLevel;
        DialogueType = dialogueType;
        Sentences = new List<DialogueSentence>();
    }

    ~Dialogue() 
    {
        Sentences.Clear();
    }
}

[Serializable]
public class DialogueSentence
{
    public string Name;
    public string Description;
}

[Serializable]
public class DialogueSpeaker
{
    public string Name;
    public Sprite Speaker;
    public Color Color;
}

public enum DialogueTownLevel
{
    LEVEL0,
    LEVEL1A,
    LEVEL1B,
    LEVEL1C,
    LEVEL2A,
    LEVEL2B,
    LEVEL2C,
    LEVEL3A,
    LEVEL3B,
    LEVEL3C,
    LEVEL4A,
    LEVEL4B,
    LEVEL4C,
    LEVELE,
    LEVELPC
}

public enum DialogueTypeEnum
{
    IMMOVABLE = 1, // has to happen
    NATURAL = 2, // when you complete the previous level
    FORCED = 4, // reach level by death; offered weapon
    INITIAL = 8, // first meeting
    FINAL_CONDITIONAL = 16, // final scene dialogue
    CONTIGUOUS_EXIT = 32, // meet player already
    CONTIGUOUS_DEATH = 64,
    HARD = 128 // hard mode (based on deaths)
}

/*
 * immovable
 * natural initial
 * final conditional (0, >1, >4)
 * forced initial
 * natural contiguous - player exit
 * natural contiguous - player death
 * forced contiguous - player exit
 * forced contiguous - player death
 * natural difficult
 * forced difficult
 */

/*
 * immovable: can be changed to intro dialogue
 * natural initial: is prev stage complete and is first time
 * final conditional (0, >1, >4): is game terminated
 * forced initial: has previously died
 * natural contiguous - player exit: not is first time and is prev stage complete
 * natural contiguous - player death: not is first time and not is prev stage complete
 * forced contiguous - player exit: new level
 * forced contiguous - player death: new level
 * natural difficult: is prev stage complete and hard
 * forced difficult: is dead and hard
 * 
 * Start_Dialogue(int index)
 * Start_Dialogue(level, bool isFirstEncounter, bool wasPlayerDead, bool didPlayerDieOnThisFloor, bool didPlayerPassThroughThisFloor, int numDeaths)
 * Start_Dialogue_Epilogue(int numDeaths)
 * 
 * true, false, false, false, 0 - natural initial
 * true, true, false, false, 0 - forced initial
 * false, false, true/false, true/false, >0 - natural contiguous
 * false, true, true/false, true/false, >0 - forced contiguous
 * false, false, true/false, true/false, >4 - natural diff
 * false, true, true/false, true/false, >4 - forced diff
 */
