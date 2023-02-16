using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int dialog;

    public DialogueTownLevel townLevel;
    public bool isFirstEncounter;
    public bool wasPlayerDead;
    public bool didPlayerDieOnThisFloor;
    public bool didPlayerPassThroughThisFloor;
    public int numDeaths;

    public void TriggerDialogue()
    {
        if (DialogueManager.Instance == null)
        {
            return;
        }

        if (dialog == -1)
        {
            DialogueManager.Instance.StartDialogue(townLevel, isFirstEncounter, wasPlayerDead, didPlayerDieOnThisFloor, didPlayerPassThroughThisFloor, numDeaths);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(dialog);
        }
    }
}
