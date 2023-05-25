using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Test1 : MonoBehaviour,IDataPersistence
{
    // Start is called before the first frame update
    private string pName;
    void Start()
    {
        pName = GetComponent<TMP_Text>().text;
        
    }

    private void textChange(){
        GetComponent<TMP_Text>().text = pName;
    }

    public void LoadData(GameData data){
        this.pName = data.playerName;
        textChange();
    }

    public void SaveData(GameData data){
        data.playerName = "pollo";
        
    }

}
