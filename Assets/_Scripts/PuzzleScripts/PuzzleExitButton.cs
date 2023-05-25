using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleExitButton : MonoBehaviour
{
    public void ExitPuzzle()
    {
        EventManager.TriggerEvent("PuzzleExited");
    }
}
