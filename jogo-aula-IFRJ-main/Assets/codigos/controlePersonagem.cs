using UnityEngine;

public class controllerPersonagem : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float moveX;
    public float jumpForce = 10f;
    private Rigidbody2D rb2d;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    public Transform visual;
    private Animator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = visual.GetComponent<Animator>();
        
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
       moveX = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump") && isGrounded){
            Jump();
        }
        Move();
        //walking animation
        anim.SetBool("isRunning",Mathf.Abs(moveX) > 0f && isGrounded);
        if (moveX > 0.01f)
        {
            visual.localScale = new Vector3(1, 1, 1);
        }
        else if (moveX < -0.01f)
        {
            visual.localScale = new Vector3 (-1, 1, 1);
        }
    }
    void Move (){
        rb2d.linearVelocity = new Vector2 (moveX * moveSpeed, rb2d.linearVelocity.y);
    }   

    void Jump(){
        rb2d.linearVelocity =new Vector2(rb2d.linearVelocity.x , jumpForce);
    }

    }


