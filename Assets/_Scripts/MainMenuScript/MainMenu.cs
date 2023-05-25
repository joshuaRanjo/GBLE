using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotMenuScript saveSlotMenu;

    [Header("Menu Buttons")] 
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadGamebutton;

    private void Start() {
        DisableButtonsDependingOnData();
    }

    private void DisableButtonsDependingOnData()
    {
        if(!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGamebutton.interactable = false;
        }
    }

    public void OnNewGameClicked(){
        /*
        DisableMenuButtons();
        Debug.Log("New Game!");
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("SampleScene");
        */
        this.DeactivateMenu();

        saveSlotMenu.ActivateMenu(false);
    }

    public void OnLoadGameClicked(){
        this.DeactivateMenu();
        saveSlotMenu.ActivateMenu(true);
        
    }

    public void OnContinueGameClicked(){

        DisableMenuButtons();
        
        DataPersistenceManager.instance.SaveGame();

        string lastLoadedScene = DataPersistenceManager.instance.GetLastLoadedScene();

        SceneManager.LoadSceneAsync(lastLoadedScene);
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    
    private void EnableMenuButtons()
    {
        newGameButton.interactable = true;
        continueGameButton.interactable = true;
    }

    public void ActivateMenu()
    {
        EnableMenuButtons();
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
        
    }

    public void DeactivateMenu()
    {
        //Debug.Log("Deactivated");
        this.gameObject.SetActive(false);
        DisableMenuButtons();
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
