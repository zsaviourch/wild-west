using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class DialogueManager : MonoBehaviour
{
    private const string ID_SEPARATOR = "***";
    private const string NAME_SEPARATOR = ": ";
   
    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private TextMeshProUGUI _descriptionText;

    private Queue<DialogueSentence> _dialogueSentences;

    [SerializeField]
    private TextAsset _dialogueAsset;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Image _speaker;

    [Header("Character Data")]
    private List<DialogueSpeaker> _speakerList;


    private Dictionary<int, Dialogue> _dialogues = new Dictionary<int, Dialogue>();

    private static DialogueManager s_instance;

    public static DialogueManager Instance
    {
        get
        {
            return s_instance;
        }
    }

    private void Awake()
    {
        s_instance = this;

        _dialogueSentences = new Queue<DialogueSentence>();

        _dialogues.Clear();

        string[] lines = _dialogueAsset.text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int currentID = -1;

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (line.Contains(ID_SEPARATOR))
            {
                string[] dialogueInfo = line.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                
                currentID = int.Parse(dialogueInfo[0].Substring(ID_SEPARATOR.Length));
                DialogueTownLevel townLevel = ParseTownLevel(dialogueInfo[1]);
                DialogueTypeEnum dialogueType = ParseDialogueType(dialogueInfo[2], dialogueInfo.Length > 3 ? dialogueInfo[3] : "");
                Debug.Log($"{townLevel} and {(int) dialogueType}");
                _dialogues.Add(currentID, new Dialogue(townLevel, dialogueType));
            }
            else
            {
                if (_dialogues.TryGetValue(currentID, out Dialogue dialogue))
                {
                    DialogueSentence sentence = new DialogueSentence();
                    string[] dialogSplit = line.Split(new string[] { NAME_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                    sentence.Name = dialogSplit[0];
                    sentence.Description = dialogSplit[1];

                    dialogue.Sentences.Add(sentence);
                }
            }
        }
    }

    public void StartDialogue(int id)
    {
        if (_dialogues.Count > 0)
        {
            if (_dialogues.TryGetValue(id, out Dialogue dialogue))
            {
                _animator.SetBool("IsOpen", true);

                _dialogueSentences.Clear();

                foreach (DialogueSentence sentence in dialogue.Sentences)
                {
                    _dialogueSentences.Enqueue(sentence);
                }
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(DialogueTownLevel level, bool isFirstEncounter, bool wasPlayerDead, bool didPlayerDieOnThisFloor, bool didPlayerPassThroughThisFloor, int numDeaths)
    {
        DialogueTypeEnum dialogueType = 0;
        if (isFirstEncounter)
        {
            dialogueType |= DialogueTypeEnum.INITIAL;
        }

        if (wasPlayerDead)
        {
            dialogueType |= DialogueTypeEnum.FORCED;
        }
        else
        {
            dialogueType |= DialogueTypeEnum.NATURAL;
        }

        if (didPlayerDieOnThisFloor)
        {
            dialogueType |= DialogueTypeEnum.CONTIGUOUS_DEATH;
        }

        if (didPlayerPassThroughThisFloor)
        {
            dialogueType |= DialogueTypeEnum.CONTIGUOUS_EXIT;
        }

        if (numDeaths >= 4)
        {
            dialogueType |= DialogueTypeEnum.HARD;
        }

        foreach(int key in _dialogues.Keys)
        {
            Dialogue dialogue = _dialogues[key];
            if (dialogue.DialogueType == dialogueType && level == dialogue.TownLevel)
            {
                StartDialogue(key);
                return;
            }

            Debug.Log($"No key found for enum {dialogueType} and town {level}");
        }
    }

    public void StartDialogueEpilogue(int numDeaths)
    {
        if (numDeaths == 0)
        {
            StartDialogue(16);
        }
        else if (numDeaths < 4)
        {
            StartDialogue(45);
        }
        else
        {
            StartDialogue(71);
        }
    }

    public void DisplayNextSentence()
    {
        if (_dialogueSentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueSentence dialog = _dialogueSentences.Dequeue();
        _nameText.text = dialog.Name;
        _descriptionText.text = dialog.Description;
    }

    public void EndDialogue()
    {
        _animator.SetBool("IsOpen", false);
    }

    private static DialogueTypeEnum ParseDialogueType(string textValue, string additionalValues = "")
    {
        DialogueTypeEnum value = 0;
        if (textValue.Contains("Immovable", StringComparison.CurrentCultureIgnoreCase))
        {
            value |= DialogueTypeEnum.IMMOVABLE;
        }
        
        if (textValue.Contains("Natural", StringComparison.CurrentCultureIgnoreCase))
        {
            value |= DialogueTypeEnum.NATURAL;
        }
        
        if (textValue.Contains("Forced", StringComparison.CurrentCultureIgnoreCase))
        {
            value |= DialogueTypeEnum.FORCED;
        }
        
        if (textValue.Contains("Initial", StringComparison.CurrentCultureIgnoreCase))
        {
            value |= DialogueTypeEnum.INITIAL;
        }
        
        if (textValue.Contains("Final Conditional", StringComparison.CurrentCultureIgnoreCase))
        {
            value |= DialogueTypeEnum.FINAL_CONDITIONAL;
        }
        
        if (textValue.Contains("Contiguous", StringComparison.CurrentCultureIgnoreCase))
        {
            value |= additionalValues.Contains("Exit") ? DialogueTypeEnum.CONTIGUOUS_EXIT : DialogueTypeEnum.CONTIGUOUS_DEATH;
        }
        
        if (textValue.Contains("Difficult", StringComparison.CurrentCultureIgnoreCase))
        {
            value |= DialogueTypeEnum.HARD;
        }

        return value;
    }    

    private static DialogueTownLevel ParseTownLevel(string textValue)
    {
        if (textValue.Contains("0"))
        {
            return DialogueTownLevel.LEVEL0;
        }
        else if (textValue.Contains("1A"))
        {
            return DialogueTownLevel.LEVEL1A;
        }
        else if (textValue.Contains("1B"))
        {
            return DialogueTownLevel.LEVEL1B;
        }
        else if (textValue.Contains("1C"))
        {
            return DialogueTownLevel.LEVEL1C;
        }
        else if (textValue.Contains("2A"))
        {
            return DialogueTownLevel.LEVEL2A;
        }
        else if (textValue.Contains("2B"))
        {
            return DialogueTownLevel.LEVEL2B;
        }
        else if (textValue.Contains("2C"))
        {
            return DialogueTownLevel.LEVEL2C;
        }
        else if (textValue.Contains("3A"))
        {
            return DialogueTownLevel.LEVEL3A;
        }
        else if (textValue.Contains("3B"))
        {
            return DialogueTownLevel.LEVEL3B;
        }
        else if (textValue.Contains("3C"))
        {
            return DialogueTownLevel.LEVEL3C;
        }
        else if (textValue.Contains("4A"))
        {
            return DialogueTownLevel.LEVEL4A;
        }
        else if (textValue.Contains("4B"))
        {
            return DialogueTownLevel.LEVEL4B;
        }
        else if (textValue.Contains("4C"))
        {
            return DialogueTownLevel.LEVEL4C;
        }
        else if (textValue.Contains("E"))
        {
            return DialogueTownLevel.LEVELE;
        }
        else if (textValue.Contains("PC"))
        {
            return DialogueTownLevel.LEVELPC;
        }

        return DialogueTownLevel.LEVEL0;
    }
}
