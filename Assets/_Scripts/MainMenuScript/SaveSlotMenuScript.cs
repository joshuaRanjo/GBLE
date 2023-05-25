using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SaveSlotMenuScript : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;
    [Header("Confirmation PopUp")]
    [SerializeField] private ConfirmationPopUpMenu confirmationPopUpMenu;

    private SaveSlotScript[] saveSlots;
    private bool isLoadingGame = false;
    private bool isLoadingGame2 = false;
    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlotScript>();
    }

    public void OnSaveSlotClicked(SaveSlotScript saveSlot)
    {
        // there is a bug somewhere where isLoadingGame is being changed after clicking the LoadGame button
        if(isLoadingGame != isLoadingGame2)
        {
            isLoadingGame = isLoadingGame2;
        }

        // disable all buttons
        DisableMenuButtons();
        if(isLoadingGame)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            SaveAndLoadScene(saveSlot.GetLastLoadedScene());
        }
        //Case - new game but save slot has data
        else if(saveSlot.hasData)
        {
            confirmationPopUpMenu.ActivateMenu(
                "Starting a New Game with this slot will override the currently saved. Data Are you sure?",
                //Function to execute if we select 'yes'
                () => {
                    DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                    DataPersistenceManager.instance.NewGame();
                    SaveAndLoadScene(saveSlot.GetLastLoadedScene());
                },
                //Function to execute if we select 'cancel'
                () => {
                    this.ActivateMenu(isLoadingGame);
                }
            );
        }
        //case - new game and saveslot has no data
        else
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            DataPersistenceManager.instance.NewGame();
            SaveAndLoadScene(saveSlot.GetLastLoadedScene());
        }
    }

    private void SaveAndLoadScene(string scene){
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();
        // Load the scene
        SceneManager.LoadSceneAsync(scene);
    }

    public void OnClearClicked(SaveSlotScript saveSlot)
    {

        // there is a bug somewhere where isLoadingGame is being changed after clicking the LoadGame button
        if(isLoadingGame != isLoadingGame2)
        {
            isLoadingGame = isLoadingGame2;
        }
        Debug.Log("Clear clicked, isLoading: " + isLoadingGame + " isLoading2: " + isLoadingGame2);
        DisableMenuButtons();
        
        confirmationPopUpMenu.ActivateMenu(
            "Are you sure you want to delete this saved data?",

            () => {
                    DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
                    ActivateMenu(isLoadingGame);    
            },
            () => {
                    ActivateMenu(isLoadingGame);    
            }
        );


    }
    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    private void Start(){
       // ActivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        isLoadingGame2 = isLoadingGame;
        this.gameObject.SetActive(true);

        // Load all existing Profiles
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        // ensure back button is enabled when we activate menu
        backButton.interactable = true;

        // Loop through each save slot in the UI and set appropriately
        GameObject firstSelected = backButton.gameObject;
        foreach(SaveSlotScript saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
            
        }

        // set first selected button
        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach(SaveSlotScript saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
