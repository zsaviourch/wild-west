using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private bool _useID = false;

    [SerializeField]
    private int _id = -1;

    [SerializeField]
    private InteractiveNPCEnum _npcName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_useID)
        {
            DialogueManager.Instance.StartDialogue(_id);
        }
        else
        {
            //DialogueManager.Instance.StartDialogue(DialogueTownLevel.LEVEL1A, isFirstEncounter, wasPlayerDead, didPlayerDieOnThisFloor, didPlayerPassThroughThisFloor, numDeaths)
        }
    }
}
