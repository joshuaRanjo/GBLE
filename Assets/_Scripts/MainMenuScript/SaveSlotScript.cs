using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SaveSlotScript : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField]private string profileId = "";
    private string lastLoadedScene = "WorldSelect";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    //[SerializeField] private TextMeshProUGUI percentageCompleteText;
    [SerializeField] private TextMeshProUGUI profileName;
    [SerializeField] private TextMeshProUGUI experienceText;

    [Header("Clear Data Button")]
    [SerializeField] private Button clearButton;

    public bool hasData {get; private set;} = false;

    private Button saveSlotButton;

    private void Awake() {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        // theres no data for this profileID
        if(data== null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearButton.gameObject.SetActive(false);
        }
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearButton.gameObject.SetActive(true);

            profileName.text = data.playerName;
            experienceText.text = "Experience: " + data.experience.ToString(); 
            lastLoadedScene = data.lastLoadedScene;
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

    public string GetLastLoadedScene()
    {
        return this.lastLoadedScene;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearButton.interactable = interactable;
    }
}
