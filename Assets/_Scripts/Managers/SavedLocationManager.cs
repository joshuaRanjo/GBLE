using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavedLocationManager : MonoBehaviour, IDataPersistence
{
    private string locationName = "";
   public void LoadData(GameData data)
   {
        this.locationName = SceneManager.GetActiveScene().name;
        
   }

    public void SaveData(GameData data)
    {
        data.lastLoadedScene = locationName;
    }
}
