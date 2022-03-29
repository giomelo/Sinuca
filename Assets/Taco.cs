using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taco : MonoBehaviour
{
    private float rotateSpeed = 30f;
    private float minPos, maxPos;
    private float posVariation = 4f;
    private float t = 0;
    private bool isHolding = false;
    private float backSpeed = 6f;
    private bool canRotate = true;
    private bool setPos = false;
    private bool canMove = true;
    public float force = 13f;
    public GameObject rotateOrigin;
    public Collider collider;

    Rigidbody body; 
    // Start is called before the first frame update
    void Start()
    {
        minPos = transform.localPosition.z;
        t = 0;
        maxPos = minPos - posVariation;
    }

    void Update () 
     {  
         if(canRotate)
         {
            var moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            var moveVelocity = moveInput * rotateSpeed;
 
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;
 
            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
             Vector3 pointToLook = cameraRay.GetPoint(rayLength);
             Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);
 
             rotateOrigin.transform.LookAt(new Vector3(pointToLook.x, rotateOrigin.transform.position.y, pointToLook.z));
            }
         }
        
        //acertar bola
        if(Input.GetMouseButton(0) && canMove)
        {
 
            isHolding = true;
            canRotate = false;
        }else
        {
            isHolding = false;
        }
        

        //mover taco
        if(isHolding)
        {
            if(!setPos)
            {
                minPos = transform.localPosition.z;
                maxPos = minPos - posVariation;
                setPos = true;
            }
            //Debug.Log(maxPos);
            //Debug.Log(Mathf.Abs(transform.localPosition.z - maxPos));
            if(transform.localPosition.z - maxPos >= 0)
            {
                transform.Translate(0,0, -backSpeed * Time.deltaTime);
                t += Time.deltaTime;
                Debug.Log(t);
            }
            // Vector3 teste = transform.position;
            //transform.localPosition = Vector3.Lerp(teste, new Vector3(transform.localPosition.x, transform.localPosition.y,maxPos), Time.deltaTime * backSpeed);
            
        }else
        {
            if(setPos)
            {
                if(transform.localPosition.z - (minPos + 2) <=0)
                {
                    transform.Translate(0,0, + backSpeed * Time.deltaTime * 5);

                }else
                {
                    canMove = false;
                    //canRotate = true;
                    setPos = false;
                    //t = 0;
                }
             
            }
        }

      
     }
       void OnCollisionEnter(Collision other) {
            if(other.transform.CompareTag("Branca"))
            {
                collider.enabled = false;
                Debug.Log("body");
                transform.parent = null;
                force *= t;
                body = other.transform.GetComponent<Rigidbody>();
                body.AddRelativeForce(Vector3.forward * force, ForceMode.Impulse);
            }
            
        }
    

        private void ResetTaco()
        {

        }
     
 }

