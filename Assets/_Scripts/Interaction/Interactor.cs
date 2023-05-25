using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{

    private bool canInteract = false;
    [SerializeField] private BoxCollider2D interactionCollider;
    private GameObject interactable;
    public void DoInteraction(InputAction.CallbackContext context){
        if(canInteract && interactable != null){
            interactable.GetComponent<InteractableObject>().DoAction();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
            if(collision.gameObject.CompareTag("Interactable")){
                canInteract = true;
                interactable = collision.gameObject;
               // Debug.Log("Can Interact");
            }
    }

    private void OnTriggerExit2D(Collider2D collision) {
            if(collision.gameObject.CompareTag("Interactable")){
                canInteract = false;
               // Debug.Log("Cant Interact");
                interactable = null;
            }
    }
}
