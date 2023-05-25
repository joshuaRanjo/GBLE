using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
[System.Serializable]
public class GameData
{
    
    #region Data
    public string playerName;
    public long lastUpdated;
    public int puzzleSet;
    public float completion;
    public int accessLevel;
    public int experience;
    public string lastLoadedScene;
    public SerializableDictionary<string, bool> puzzleDictionary;

    #endregion Data

    public GameData(SerializableDictionary<string, bool> dict, int puzzleSet){
        this.playerName = "Apollo";
        this.puzzleSet = puzzleSet;
        this.completion = 0f;
        this.accessLevel = 0;
        this.experience = 0;
        this.puzzleDictionary = dict;
        this.lastLoadedScene = "WorldSelect";


    }

}
