using UnityEngine;
using UnityEngine.UI;

public class KeyboardButtonTrigger : MonoBehaviour
{
    public Button targetButton; // 対象のUIボタン
    public KeyCode triggerKey = KeyCode.Space; // 押すキー（例：スペース）

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (targetButton != null)
            {
                targetButton.onClick.Invoke(); // ボタンのクリックイベントを呼び出す
            }
        }
    }
}
