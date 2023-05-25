using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IDataPersistence
{
    private bool activated = false;
    private bool ready = false;
    [SerializeField] private int accessLevel = 0;

    private void Awake()
    {
        StartCoroutine(WaitToActivate());
    }
    public void teleport(string SceneName){
        if(activated && ready)
            SceneManager.LoadScene(SceneName);
    }

    public void setPortal(bool status){
        activated = status;
    }

    private IEnumerator WaitToActivate()
    {
        yield return new WaitForSeconds(2f); 
        //Debug.Log("ready to teleport");
        ready = true;
    }

    public void LoadData(GameData data)
    {
        if(this.accessLevel <= data.experience)
        {
            this.activated = true;
            this.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else{
            this.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    public void SaveData(GameData data){} //No data will be saved from portals, only loaded
    
}
