using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{

    public bool isInRange;
    public UnityEvent interaction;
    private InputAction.CallbackContext context;

    public void DoAction(){
        interaction.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player")){
            isInRange = true;
        }
    }

        private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player")){
            isInRange = false;
        }
    }
}
