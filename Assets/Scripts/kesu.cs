using UnityEngine;
using UnityEngine.SceneManagement;

public class kesu : MonoBehaviour
{
    private bool Tap = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("aeuiuefhuhefuqeh");

        if(Tap)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("afaafada");
                Tap = false;
                SceneManager.LoadScene("Scenes/Stage1", LoadSceneMode.Single);
            }

        }
    }

    public void OnPush()
    {
        Debug.Log("kitaaaaaaaaaaaaa");
    }
}
