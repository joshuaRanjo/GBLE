using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlsUIManager : MonoBehaviour
{

//Controls visibility of On Screen Controls    
[Header("Control Sets")]
[SerializeField] private GameObject playerControls;
[SerializeField] private GameObject puzzleControls;
[SerializeField] private GameObject pauseMenu;


#region EVENT_LISTENERS
// Actions

    private UnityAction puzzleMode;
    private UnityAction movementMode;
    private UnityAction gamePaused;
    private UnityAction gameResumed;
    
    private void Awake() 
    {
        puzzleMode = new UnityAction(EnablePuzzleMode);
        movementMode = new UnityAction(EnableMovementMode);
        gamePaused = new UnityAction(PauseGame);
        gameResumed= new UnityAction(ResumeGame);
    }
    private void OnEnable() 
    {
        EventManager.StartListening("PuzzleInteracted", puzzleMode);
        EventManager.StartListening("PuzzleExited", movementMode);
        EventManager.StartListening("GamePaused", gamePaused);
        EventManager.StartListening("GameResumed", gameResumed);
    }

    private void OnDisable()
    {
        EventManager.StopListening("PuzzleInteracted", puzzleMode);
        EventManager.StopListening("PuzzleExited", movementMode);
        EventManager.StopListening("GamePaused", gamePaused);
        EventManager.StopListening("GameResumed", gameResumed);
    }

#endregion EVENT_LISTENERS

    public void EnablePuzzleMode()
    {
        playerControls.SetActive(false);
        puzzleControls.SetActive(true);
    }

    public void EnableMovementMode()
    {
        playerControls.SetActive(true);
        puzzleControls.SetActive(false);
    }

    public void EnableControls(bool enable)
    {
            playerControls.SetActive(enable);
            puzzleControls.SetActive(enable);
    }
    
    public void PauseGame()
    {
        EnableControls(false);
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        EnableMovementMode();
        pauseMenu.SetActive(false);
    }

}
