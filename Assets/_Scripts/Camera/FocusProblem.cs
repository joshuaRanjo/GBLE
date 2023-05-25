using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusProblem : MonoBehaviour
{
    [SerializeField] private Transform problemFocus = null;
    private Vector3 cameraOffset;

    [SerializeField] private bool focusedOnProblem = false;
    private void Awake() {
        gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(problemFocus != null && focusedOnProblem)
        {
            MoveCamera();
        }
    }

    public void StartFocusingOnProblem()
    {
        focusedOnProblem = true;
    }

    public void StopFocusingOnProblem()
    {
        focusedOnProblem = false;
        RemoveFocus();
    }

    public void SetFocus(Transform focus){
        problemFocus = focus;
    }

    public void RemoveFocus()
    {
        problemFocus = null;
    }

    private void MoveCamera(){
        Vector3 lerpPosition = Vector3.Lerp(transform.position, new Vector3(problemFocus.position.x,problemFocus.position.y, -10f), 0.05f);
        transform.position = lerpPosition;
    }
}
