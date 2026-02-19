using UnityEngine;
using UnityEngine.Rendering;

public class walking : MonoBehaviour
{
    public float speed = 30f;
    public bool left = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;
        float currentX = currentRotation.x;
        Vector3 currentSpot = transform.position;
        float spot = transform.position.x;

        if (spot > 1 && left==true)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            left = false;
        }else if (spot < -1 && left ==false)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            left = true;
        }
        if(left ==true){
            speed = 30f;
            transform.Rotate(new Vector3(0,1,0)*speed*Time.deltaTime );
            if (currentX<70)
            {
                transform.eulerAngles = new Vector3(90f, 0f, 0f);
                transform.Translate(new Vector3(15, 0, 0)*speed*Time.deltaTime);
            }
        }
        else
        {
            speed = -30f;
            transform.Rotate(new Vector3(0,1,0)*speed*Time.deltaTime );
            if (currentX<70)
            {
                transform.eulerAngles = new Vector3(90f, 0f, 0f);
                transform.Translate(new Vector3(10, 0, 0)*speed*Time.deltaTime);
            }
        }

    }
}
