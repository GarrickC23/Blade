using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class NPCInteractable : MonoBehaviour
{
    public bool interactable; 
    public string conversationStartNode;
    private DialogueRunner dialogueRunner; 
    private PlayerControls playerControls;

    void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        playerControls = new PlayerControls();
        playerControls.Ground.Interact.performed += Interact;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void onDisable()
    {
        playerControls.Disable(); 
    }

    private void StartConversation()
    {
        dialogueRunner.StartDialogue(conversationStartNode);
    }

    //private void EndConversation();

    //public void DisableConversation();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("trigger");
        if (collider.gameObject.tag == "Player")
        {
            interactable = true;
        }
        else
        {
            interactable = false; 
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && interactable == true)
        {
            Debug.Log("interact");
            StartConversation(); 
        }
    }
}
