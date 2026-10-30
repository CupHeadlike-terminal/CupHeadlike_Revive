using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyboardButtonTrigger : MonoBehaviour
{
    public Button targetButton; // 対象のUIボタン
    public KeyCode triggerKey = KeyCode.Space; // 押すキー（例：スペース）
    [SerializeField] InputActionAsset inputActions;
    InputAction pressAction;

    private void Start()
    {
        pressAction = inputActions.FindActionMap("UI").FindAction("Submit");
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) || pressAction.IsPressed())
        {
            Debug.Log("Key pressed");
            if (targetButton != null)
            {
                targetButton.onClick.Invoke(); // ボタンのクリックイベントを呼び出す
            }
        }
    }
}
