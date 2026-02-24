using UnityEngine;
using UnityEngine.Rendering;

public class walking : MonoBehaviour
{
    //how fast to rotate/how much to move forward
    public float speed = 30f;
    //tells what direction they're walking
    public bool left = true;

    //set bounds of the road to know where to turn around
    public float turn1 = 3.15f;
    public float turn2 = -2.7f;

    // Update is called once per frame
    void Update()
    {
        //get the rotation to be able to hinge on it
        Vector3 currentRotation = transform.eulerAngles;
        float currentX = currentRotation.x;
        //get the position to be able to move it
        Vector3 currentSpot = transform.position;
        float spot = transform.position.x;

        //when the walker gets to the sides of the road turn them around and have them walk the other way
        if (spot > turn1 && left==true)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            left = false;
            speed = -speed;
        }else if (spot < turn2 && left ==false)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            left = true;
            speed = -speed;
        }
        //move the walkers based on which way they're going
        if(left ==true){
            
            transform.Rotate(new Vector3(0,1,0)*speed*Time.deltaTime );
            if (currentX<70)
            {
                transform.eulerAngles = new Vector3(90f, 0f, 0f);
                transform.Translate(new Vector3(15, 0, 0)*speed*Time.deltaTime);
            }
        }
        else
        {
            
            transform.Rotate(new Vector3(0,1,0)*speed*Time.deltaTime );
            if (currentX<70)
            {
                transform.eulerAngles = new Vector3(90f, 0f, 0f);
                transform.Translate(new Vector3(10, 0, 0)*speed*Time.deltaTime);
            }
        }

    }
}
