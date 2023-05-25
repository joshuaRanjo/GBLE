using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;

    private UnityAction enterProblem;
    private UnityAction exitProblem;
    private void Awake() {
        enterProblem = new UnityAction(EnterProblem);
        exitProblem = new UnityAction(ExitProblem);
    }

    private void OnEnable() {
        EventManager.StartListening("PuzzleInteracted", enterProblem);
        EventManager.StartListening("PuzzleExited", exitProblem);
    }

    private void OnDisable() {
        EventManager.StopListening("PuzzleInteracted", enterProblem);
        EventManager.StopListening("PuzzleExited", exitProblem);        
    }   
    public void EnterProblem()
    {
        mainCamera.GetComponent<FollowPlayer>().StopFollowingPlayer();
        mainCamera.GetComponent<FocusProblem>().StartFocusingOnProblem();
    }

    public void ExitProblem()
    {
        mainCamera.GetComponent<FollowPlayer>().StartFollowingPlayer();
        mainCamera.GetComponent<FocusProblem>().StopFocusingOnProblem();
    }


}
