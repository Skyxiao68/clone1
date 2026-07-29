using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;

    private int dialogueIndex; //which line we are on
    private bool isTyping;
    private bool isDialogueActive;

    private bool playerInRange;
    public GameObject exclamationInteract;

    void Start()
    {
        exclamationInteract.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //NPC in range and interact with player
            playerInRange = true;
            exclamationInteract.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            exclamationInteract.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isDialogueActive)
            {
                StartDialogue();
            }
            else
            {
                NextLine();
            }
            
            exclamationInteract.SetActive(false);
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;
        dialogueText.text = dialogueData.dialogueLines[dialogueIndex];

        //TypeLine of Dialogue 
        StartCoroutine(TypeLine());

    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            //EndDialogue
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

            if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
            {
                yield return new WaitForSeconds(dialogueData.autoProgressDelay);
                //Display Lines
                NextLine();
            }
        
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        exclamationInteract.SetActive(playerInRange);
        
    }
    
}
