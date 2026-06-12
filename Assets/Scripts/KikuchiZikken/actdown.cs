using UnityEngine;

public class actdown : MonoBehaviour
{
    public BoxCollider2D col;
    private float time;
    private bool timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer)
        {
            time += Time.deltaTime;
            if (time > 1f)
            {
                timer = false;
                col.enabled = true;
            }
            Debug.Log("timer");
            Debug.Log(timer);
            Debug.Log(time);
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Actor")
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (!timer)
                {
                    time = 0f;
                    timer = true;
                    col.enabled = false;

                }
            }
        }
    }
}
