using UnityEngine;

public class focus : MonoBehaviour
{
    public static int StageNumber;
    public GameObject Stage1;
    public GameObject Stage2;
    public GameObject Stage3;
    private float Buttonretrytime;
    public Transform Stageone;
    public Transform Stagetwo;
    public Transform Stagethree;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Stageone = Stage1.transform;
        Stagetwo = Stage2.transform;
        Stagethree = Stage3.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Buttonretrytime > 0.2f)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                StageNumber++;
                Buttonretrytime = 0f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                StageNumber--;
                Buttonretrytime = 0f;
            }


            if (StageNumber < 1)
            {
                StageNumber = 1;
            }
            if (StageNumber > 3)
            {
                StageNumber--;
            }

        }
        Buttonretrytime += Time.deltaTime;

        if (StageNumber == 1)
        {
            rb.MovePosition(Stageone.position);
        }
        if (StageNumber == 2)
        {
            rb.MovePosition(Stagetwo.position);
        }
        if (StageNumber == 3)
        {
            rb.MovePosition(Stagethree.position);
        }
    }
}
