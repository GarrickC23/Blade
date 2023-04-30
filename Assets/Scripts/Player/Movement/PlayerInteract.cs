using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float interactRadius;
    private PlayerControls playerControls;              //Input System variable

    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls(); 
        playerControls.Ground.Interact.performed += Interact;  
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable(); 
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed) {
            Collider2D[] Hits = Physics2D.OverlapCircleAll(this.transform.position, interactRadius);
            foreach (Collider2D hit in Hits) {
                if (hit.gameObject.tag == "Interactable") {
                    hit.gameObject.GetComponent<Interactable>().Interact();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
