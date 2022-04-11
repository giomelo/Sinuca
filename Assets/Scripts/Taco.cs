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
    public float backSpeed = 4f;
    private bool canRotate = true;
    private bool setPos = false;
    private bool canMove = true;
    public float force = 13f;
    public Transform rotateOrigin;
    public Collider collider;
    private float initialForce;
    Rigidbody body;

    public Transform WhiteBall;

    private Vector3 _intialPos;
    // Start is called before the first frame update
    private void Start()
    {
        _intialPos = WhiteBall.position;
        initialForce = force;
        minPos = transform.localPosition.z;
        t = 0;
        body = WhiteBall.GetComponent<Rigidbody>();
        maxPos = minPos - posVariation;
    }

    private void CheckifBallStoped()
    {
        if (canMove) return;
        if (body.velocity.magnitude <= 0.1)
        {
            Debug.LogWarning("BallStoped");
            ResetTaco();
        }
    }

    void Update ()
    {
        CheckifBallStoped();
         
         
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
                 //minPos = transform.localPosition.z;
                 setPos = true;
             }
             //Debug.Log(maxPos);
             //Debug.Log(Mathf.Abs(transform.localPosition.z - maxPos));
             if(transform.localPosition.z - maxPos >= 0)
             {
                 transform.Translate(0,0, -backSpeed * Time.deltaTime);
                 t += Time.deltaTime;
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

    private void OnCollisionEnter(Collision other) {
        if(other.transform.CompareTag("Branca"))
        {
            collider.enabled = false;
            other.transform.parent = null;
            force *= t;
            body.AddRelativeForce(Vector3.forward * force, ForceMode.Impulse);
        }
    }
    

    private void ResetTaco()
    {
      
        rotateOrigin.position = WhiteBall.position;
        body.isKinematic = true;
        WhiteBall.parent = rotateOrigin;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        force = initialForce;
        t = 0;

        transform.localPosition = new Vector3(0,0,minPos);
        WhiteBall.localRotation = Quaternion.Euler(0, 0, 0);
        canRotate = true;
        canMove = true;
        collider.enabled = true;
        
        StartCoroutine(voltar());
       
        
    }

    public void ResetWhite()
    {
        WhiteBall.position = _intialPos;
        ResetTaco();
    }

    IEnumerator voltar()
    {
        yield return new WaitForSeconds(0.5f);
       
        body.isKinematic = false;
    }
     
 }

