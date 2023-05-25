using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour,IDataPersistence
{

    SerializableDictionary<string, bool> puzzleDictionary;

    private int puzzleSet;
    private bool puzzlesLoaded = false;
    public void LoadData(GameData data){
        this.puzzleDictionary = data.puzzleDictionary;
        //Checks if its a puzzle level
        if(SceneManager.GetActiveScene().name != "WorldSelect" && !puzzlesLoaded)
        {
            
            puzzleSet = data.puzzleSet;
            string set = "Set" + puzzleSet.ToString();

            GameObject.Find("/Level/Interactables/Puzzles/"+set).SetActive(true);
            puzzlesLoaded = true;
            DataPersistenceManager.instance.LoadGame();
        }
        
        // Set Active the puzzle set of the level
        
    }

    public void SaveData( GameData data){
        data.puzzleDictionary = this.puzzleDictionary;
    }

    public void changeState(string id, bool state){
        this.puzzleDictionary[id] = state;
    }


}
