using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    Vector2[] dirarray = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    int dirindex = 0;
    Rigidbody2D rb;
    Animator ghostanim;
    Collider2D ghostcol;
    bool eyesMove = false;

    public GameController controller;
    public float speed = 1f;
    public Vector2 curdir;
    public float raydistance;
    public LayerMask raylayer;
    public Vector3 jailPoint;
    List<Dot> path;

    static public bool scared;
    
    // Start is called before the first frame update
    void Start()
    {
        scared = false;
        rb = GetComponent<Rigidbody2D>();
        ghostanim = GetComponent<Animator>();
        ghostcol = GetComponent<Collider2D>();
        curdir = dirarray[dirindex];
 //       path = controller.GetComponent<Astar>().GhostPath;
    }

    // Update is called once per frame
    void Update()
    {
        if (!eyesMove)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, curdir, raydistance, raylayer);

            Vector3 endpoint = curdir * raydistance;
            Debug.DrawLine(transform.position, transform.position + endpoint, Color.red);

            if (hit2D.collider != null)
            {

                if (hit2D.collider.gameObject.CompareTag("Ghost"))
                {

                }
                else
                {
                   
                    ChangeDirection();
                }
            }
        }
    }
        
    private void FixedUpdate()
    {
        if (!eyesMove)
        {

            //normal movement
            transform.Translate(curdir * speed * Time.deltaTime);
        }
        else if(eyesMove) 
        {
            //movement for Eyes 
           
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), jailPoint, speed * 2.5f * Time.deltaTime);
            if(transform.position == jailPoint)
            {
                eyesMove = false;
                ghostcol.isTrigger = false;
                ghostanim.SetBool("Eaten", false);
            }
        }
        
          

        // Animation
        GetComponent<Animator>().SetFloat("DirX", curdir.x);
        GetComponent<Animator>().SetFloat("DirY", curdir.y);
    }

    void ChangeDirection()
    {
        dirindex += Random.Range(0, 3) * 2 - 1;
        dirindex = dirindex % dirarray.Length;
        if(dirindex < 0)
        {
            dirindex = dirarray.Length + dirindex;
        }
        
        curdir = dirarray[dirindex];
    }

    public void SetNotEdible()
    {
        ghostanim.SetBool("Isedible", false);   
        ghostcol.isTrigger = false;
        
        scared = false;
    }

    public void SetEdible()
    {

        scared = true;
        ghostanim.SetBool("Isedible", true);
        ghostcol.isTrigger = true;
        
    }

     public void SetEyes()
    {
        
        eyesMove = true;
        ghostcol.isTrigger = true;
        scared = false;
        ghostanim.SetBool("Isedible", false);
        ghostanim.SetBool("Eaten", true);
        
    }



    private void OnTriggerEnter2D(Collider2D other)
    {     
        if (other.gameObject.CompareTag("Player")) 
        {
            if (scared)
            {
                SetEyes();
                
                other.GetComponent<PacmanMovement>().AddGhostScore();
            }
        }

        if (other.gameObject.CompareTag("Teleport"))
        {
            rb.transform.position = new Vector3(rb.transform.position.x * -.95f, rb.transform.position.y, rb.transform.position.z);
        }
    }
}
