using UnityEngine;
using UnityEngine.UI;

public class Class1 : MonoBehavior
{
    [SerializeField] private Button button;

    private void Start()
	{
        button.onClick.AddListener(Resume);
	}
}
