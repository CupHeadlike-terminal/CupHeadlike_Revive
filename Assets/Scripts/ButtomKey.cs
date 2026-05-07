using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyboardButtonTrigger : MonoBehaviour
{
    public Button targetButton; // 対象のUIボタン
    public KeyCode triggerKey = KeyCode.Space; // 押すキー（例：スペース）
    [SerializeField] InputAction inputAction;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) || inputAction.IsPressed())
        {
            if (targetButton != null)
            {
                targetButton.onClick.Invoke(); // ボタンのクリックイベントを呼び出す
            }
        }
    }
}
