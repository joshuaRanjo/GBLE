using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public void PauseGame()
    {
        EventManager.TriggerEvent("GamePaused");
    }
}
