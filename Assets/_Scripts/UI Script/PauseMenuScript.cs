using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;


    private void Awake() {
        resumeButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();

        resumeButton.onClick.AddListener(() => {
            DeactivateMenu();
            
        });
        quitButton.onClick.AddListener(() => {
            QuitGame();
        });
    }
    private void DeactivateMenu()
    {
        EventManager.TriggerEvent("GameResumed");
    }

    private void QuitGame()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync(0);
    }
}
