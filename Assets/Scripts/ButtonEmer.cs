using UnityEngine;
using UnityEngine.UI;

public class ButtonEmer : MonoBehaviour
{
    [SerializeField] private Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick()
    {
        Debug.Log("feuivfbiwusbviwbgi");
    }
}
