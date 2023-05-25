using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Temp

public class PuzzleModule : MonoBehaviour, IDataPersistence
{
    private Camera mainCamera;

    [Header("Puzzle Attributes")]
    private bool completeState = false;
    private bool solved = false;
    public string puzzleID;

    
    [Header("Visuals to change")]
    [SerializeField] private TextMeshPro sign;

    [Header("Related Objects For Scripts")]
    [SerializeField] private Transform problemFocus;



    private void Awake() {
        mainCamera = Camera.main;
    }

    public void PuzzleChangeState(bool state){
        solved = state;
    }

    public void ChangeCompleteState(bool state)
    {
        completeState = state;
    }

    public void LoadData(GameData data)
    {
        this.completeState = data.puzzleDictionary[puzzleID];

        if(this.completeState == true)
        {
            ShowSolvedVisuals();
        }
        else
        {
            ShowUnsolvedVisuals();
        }
    }

    public void SaveData(GameData data){} //No data will be saved, only loaded

    public void Interact()
    {

        if(!completeState) // Can only be interacted if the puzzle isnt complete
        {
            // Set the focus on the FocusProblem script
            mainCamera.GetComponent<FocusProblem>().SetFocus(problemFocus);

            // Broadcast the PuzzleInteracted Event
            EventManager.TriggerEvent("PuzzleInteracted");

            // Make puzzle tools target the current puzzle (Currently Using temp tools)
            #region TEMPORARY
            Button unsolveButton = GameObject.Find("/Canvas/Controls/PuzzleControls/Tools/Unsolve").GetComponent<Button>();
            Button solveButton = GameObject.Find("/Canvas/Controls/PuzzleControls/Tools/Solve").GetComponent<Button>();
            Button submitButton = GameObject.Find("/Canvas/Controls/PuzzleControls/Submit").GetComponent<Button>();

            unsolveButton.onClick.RemoveAllListeners();
            solveButton.onClick.RemoveAllListeners();
            submitButton.onClick.RemoveAllListeners();

            unsolveButton.onClick.AddListener(() => {
                PuzzleChangeState(false);
                ShowUnsolvedVisuals();
            });

            solveButton.onClick.AddListener(() => {
                PuzzleChangeState(true);
                ShowSolvedVisuals();
            });

            submitButton.onClick.AddListener(() => {
                if(solved && !completeState){ // do only if the puzzle hasnt been completed before
                    ChangeCompleteState(true);
                    // Change state in puzzle dictionary      
                    PuzzleManager puzzleManager = GameObject.Find("/_Managers/PuzzleManager").GetComponent<PuzzleManager>();
                    puzzleManager.changeState(puzzleID,true);
                    EventManager.TriggerEvent("PuzzleExited");

                    // Add exp points based off difficulty
                    char diff = puzzleID[2];
                        switch(diff)
                        {
                            case 'E':
                                EventManager.TriggerEvent("EasyPuzzleCompleted");
                                break;
                            case 'M':
                                EventManager.TriggerEvent("MediumPuzzleCompleted");
                                break;
                            case 'H':
                                EventManager.TriggerEvent("HardPuzzleCompleted");
                                break;
                            default:
                                break;
                        }

                    DataPersistenceManager.instance.SaveGame();
                }
                
            });

            #endregion TEMPORARY
        }
    }

    private void ShowUnsolvedVisuals()
    {
        // Will prolly have to write more scripts depending on each puzzle
        // This is just to change sign visuals for temp puzzles
        sign.text = "progress \n unsolved";
    }

    private void ShowSolvedVisuals()
    {
        sign.text = "progress \n solved";
    }
}
