using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ExperienceCounter : MonoBehaviour,IDataPersistence
{
    [SerializeField] private TextMeshProUGUI expText;
    private int exp;
    private UnityAction ePuzzleCompleted;
    private UnityAction mPuzzleCompleted;
    private UnityAction hPuzzleCompleted;
    public void LoadData(GameData data)
    {
        this.exp = data.experience;
        expText.text = "Exp : " + exp.ToString();
    }

    public void SaveData( GameData data)
    {
        data.experience = this.exp;
    }

    private void Awake() 
    {
        ePuzzleCompleted = new UnityAction(AddPoints1);
        mPuzzleCompleted = new UnityAction(AddPoints2);
        hPuzzleCompleted = new UnityAction(AddPoints3);
    }
    private void OnEnable() 
    {
        EventManager.StartListening("EasyPuzzleCompleted", ePuzzleCompleted);
        EventManager.StartListening("MediumPuzzleCompleted",  mPuzzleCompleted);
        EventManager.StartListening("HardPuzzleCompleted", hPuzzleCompleted);
    }    

    private void OnDisable() 
    {
        EventManager.StopListening("EasyPuzzleCompleted", ePuzzleCompleted);
        EventManager.StopListening("MediumPuzzleCompleted",  mPuzzleCompleted);
        EventManager.StopListening("HardPuzzleCompleted", hPuzzleCompleted);
    }   

    private void UpdateUI()
    {
        expText.text = "Exp : " + exp.ToString();
        DataPersistenceManager.instance.SaveGame();
    }

    private void AddPoints1()
    {
        exp += 1;
         UpdateUI();
    }
    private void AddPoints2()
    {
        exp += 2;
         UpdateUI();
    }
    private void AddPoints3()
    {
        exp += 3;
        UpdateUI();
    }
}
