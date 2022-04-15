using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Taco : MonoBehaviour
{
    //Luz e particulas
    public Light BallLight;
    public ParticleSystem ForceParticles;
    
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

    private const float DOTSPEED = 0.02f;
    public LineRenderer Path;
    public Gradient rayColor;
    public Gradient rayTransparent; //� mais f�cil isso ser p�blico do que fazer 500 linhas definindo gradiente no c�digo
    public Material dotMaterial;
    Vector3[] pathPos = new Vector3[2];
    Vector2 dotDistance = Vector2.one;
    Vector2 dotCrawl = Vector2.zero;

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
        
        
        //Muda Luz e particula
        BallLight.intensity = t * 10;
        
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
             //canRotate = false;
             
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

    private void FixedUpdate()
    {
        if (canMove)
        {
            Path.colorGradient = rayColor;
            RaycastHit hit;
            Ray rayDistance = new Ray(WhiteBall.position, gameObject.transform.forward);
            if (Physics.Raycast(rayDistance, out hit))
            {
                dotDistance.x = hit.distance;
                dotCrawl.x -= DOTSPEED;
                dotMaterial.mainTextureScale = dotDistance;
                dotMaterial.mainTextureOffset = dotCrawl;
                pathPos[0] = WhiteBall.position;
                pathPos[1] = hit.point;
                Path.SetPositions(pathPos);
            }
        }
        else
        {
            Path.colorGradient = rayTransparent;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.CompareTag("Branca"))
        {
            RigidbodyConstraints constraints;
            constraints = RigidbodyConstraints.FreezePositionY;
            body.constraints = constraints;
            collider.enabled = false;
            other.transform.parent = null;
            force *= t;
            body.AddRelativeForce(Vector3.forward * force, ForceMode.Impulse);
            t = 0;
        }
    }
    

    private void ResetTaco()
    {
      
        rotateOrigin.position = WhiteBall.position;
        body.isKinematic = true;
        WhiteBall.parent = rotateOrigin;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        RigidbodyConstraints constraints;
        constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        body.constraints = constraints;
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
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        WhiteBall.position = _intialPos;
        ResetTaco();
    }

    IEnumerator voltar()
    {
        yield return new WaitForSeconds(0.5f);
       
        body.isKinematic = false;
    }
     
 }

