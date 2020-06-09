using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMovement : MonoBehaviour {

    public float ApointX = -43f;
    public float BPointX = 43f;

    public int MovX;

    public float Vel;

    // Start is called before the first frame update
    void Start()
    {
        if (ApointX == BPointX) {
            print("ERROR: POINTS CANNOT BE THE SAME");
        }
        if (ApointX >= BPointX) {
            print("CHANGING VALUES: A HAS TO BE MINOR THAN B");

            float aux = ApointX;

            ApointX = BPointX;
            BPointX = aux;
        }

        Vel = Random.Range(1f, 2.5f);
        float dice = Random.value;

        MovX = dice >= 0.5 ? -1 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (ApointX <= transform.position.x && transform.position.x <= BPointX)
        {
            float DeltaPos = Vel * Mathf.Sign(MovX) * Time.deltaTime;

            transform.Translate(new Vector3(DeltaPos, 0));
        }
        else {

            float dice = Random.value;

            MovX = dice >= 0.5 ? -1 : 1;

            if (MovX > 0)
            {
                transform.position = new Vector3(ApointX, transform.position.y, transform.position.z);
                Vel = Random.Range(1f, 2.5f);
            }
            else if (MovX < 0) {
                transform.position = new Vector3(BPointX, transform.position.y, transform.position.z);
                Vel = Random.Range(1f, 2.5f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (ApointX != BPointX && ApointX < BPointX)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            Vector2 AP = new Vector2(ApointX, transform.position.y);
            Vector2 BP = new Vector2(BPointX, transform.position.y);

            //Draw A point
            Gizmos.DrawLine(AP - Vector2.up * size, AP + Vector2.up * size);
            Gizmos.DrawLine(AP - Vector2.left * size, AP + Vector2.left * size);

            //Draw B Point
            Gizmos.DrawLine(BP - Vector2.up * size, BP + Vector2.up * size);
            Gizmos.DrawLine(BP - Vector2.left * size, BP + Vector2.left * size);
        }
    }
}
