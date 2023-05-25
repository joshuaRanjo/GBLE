using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overriedSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId ="test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;
    private string selectedProfileId = "";  // If save data exists, defaults to the most recent updated data

    public static DataPersistenceManager instance {get; private set;}

    private void OnApplicationQuit() {
        SaveGame();    
    }
    private void Awake() {
        if (instance !=null){
            Debug.Log("More than one Data Persistence Manager in the scene. Destroying Newest instance");
            Destroy(this.gameObject);
            return;
        }

        if(disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled");
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        InitializeSelectedProfileId();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    //    SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    //    SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        //Debug.Log("OnSceneLoaded Called");
        
        LoadGame();
    }

/*
    public void OnSceneUnloaded(Scene scene){
        Debug.Log("OnSceneUnloaded Called");
        SaveGame();
    }
*/
    public void ChangeSelectedProfileId(string newProfileId)
    {
        //update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
        //load game, which will use that profile and update the game accordingly
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        dataHandler.Delete(profileId);
        InitializeSelectedProfileId();
        LoadGame();
    }

    private void InitializeSelectedProfileId(){
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();

        //debugging
        if(overriedSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profileid with test id: " + testSelectedProfileId);
        }
    }

    public void NewGame(){   
    
    #region Puzzle Selection 
        //IMPORTANT CHANGE NUMBER OF Problems
        //int problemsToSelect = 1;
        int problemTotal = 2;

        var dict = new SerializableDictionary<string, bool>();

        int puzzleSet = Random.Range(1,4);
        Debug.Log("Puzzle set: " + puzzleSet);
        
        //Puzzle Naming
        List<char> typePrefix = new List<char>(){
            'C','P','E','H'
            
        };
        List<char> difficultyPrefix = new List<char>(){
            'E','M','H'
        };
        // Puzzle ID = Puzzle Set + Type + Difficulty + Number
        foreach(char type in typePrefix)
        {
            foreach(char difficulty in difficultyPrefix)
            {
                for(int i = 1; i < problemTotal+1; i++)
                {
                    string pID = puzzleSet.ToString() +  type.ToString() + difficulty.ToString() + i.ToString(); 
                    dict.Add(pID,false);
                }
            }
        }
    #endregion Puzzle Selection


        this.gameData = new GameData(dict,puzzleSet);

        dataHandler.Save(gameData,selectedProfileId);
    }

    public void LoadGame(){
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        // return immediately if data persistence is disabled
        if(disableDataPersistence)
        {
            return;
        }

        // Load most recently saved data
        this.gameData = dataHandler.Load(selectedProfileId);

        // makes new game if data is null for debugging purposes
        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        //Loads new game if no save data is found
        if(this.gameData == null){
            Debug.Log("No data found. New game should be made before loading");
            return;
        }

        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects){
            
            dataPersistenceObj.LoadData(gameData);
        }
       
    }

    public void SaveGame(){
        // return immediately if data persistence is disabled
        if(disableDataPersistence)
        {
            return;
        }

        // if no new data to save, log warning
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found, new game needs to start before data can be saved");
            return;
        }

        //pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects){
            dataPersistenceObj.SaveData(gameData);
        }

        // timestamp the data for when it was last saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        // save data to a file using the data handler
        dataHandler.Save(gameData,selectedProfileId);
    }

    private  List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
        .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public string GetLastLoadedScene() // For Continue Button, get the lastLoadedScene of the most recently updated game data
    {
        return dataHandler.Load(selectedProfileId).lastLoadedScene;
    }
}
