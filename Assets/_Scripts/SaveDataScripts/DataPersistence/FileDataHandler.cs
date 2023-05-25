using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    
    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileId )
    {
        // base case - if profileId is null return right away
        if(profileId == null)
        {
            return null;
        }

        string fullPath = Path.Combine(dataDirPath,profileId,dataFileName);

        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath,FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data,string profileId)
    {
         // base case - if profileId is null return right away
        if(profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath,profileId,dataFileName);

        try{
            //create directory if it does no exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize teh c# game data object into json;
            string dataToStore = JsonUtility.ToJson(data,true);

            //write serialized data to file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        } 
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileId)
    {
        if(profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath,profileId, dataFileName);

        try
        {
            if(File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath),true);
            }
            else
            {
                Debug.LogWarning("Failed to delete profile data, but data was not found at path: " +fullPath);
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to delete profile data for profileId: " + profileId + " at path: " +fullPath+"\n" + e);
        }
    }

    public Dictionary<string,GameData> LoadAllProfiles()
    {
        Dictionary<string,GameData> profileDictionary = new Dictionary<string,GameData>();

        //Loop through all directory names in data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();

        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // check if data file exists
            // if none exists, this folder is not a profile
            string fullPath = Path.Combine(dataDirPath,profileId,dataFileName);
            if(!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory because it does not contain data: " + profileId);
                continue;
            }

            //Load data and put to dictionary
            GameData profileData = Load(profileId);
            //check if data is not null, if null something went wrong
            if(profileData != null)
            {
                profileDictionary.Add(profileId,profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong ProfileID : " + profileId);
            }
            

        }


        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecentProfileId = null;
        Dictionary<string,GameData> profilesGameData = LoadAllProfiles();
        foreach(KeyValuePair<string,GameData> pair in profilesGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if(gameData == null)
            {
                continue;
            }

            // if first data exists, its the first so far
            if(mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }
            // check the rest if exists and is the latest
            else{
                DateTime mostRecemtDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                // greatest time value is the most recent

                if(newDateTime > mostRecemtDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }
        }
        return mostRecentProfileId;
    }
}
