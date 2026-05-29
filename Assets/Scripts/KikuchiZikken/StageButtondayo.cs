using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButtondayo : MonoBehaviour
{
    public int stage;
    public string Name;

  


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("hahahahaha");
        if(Input.GetKey(KeyCode.Space))
        {
            Debug.Log(this.gameObject.name);
            Data.instance.nowStageID = stage;
            SceneManager.LoadScene(Name, LoadSceneMode.Single);
        }
        
    }
}
