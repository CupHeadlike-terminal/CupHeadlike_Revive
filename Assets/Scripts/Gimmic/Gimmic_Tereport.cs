using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
    public Vector3 teleportPosition; // テレポート先の座標

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Actor")) // プレイヤーがトリガーに触れた場合
        {
            other.transform.position = teleportPosition;
        }
    }
}