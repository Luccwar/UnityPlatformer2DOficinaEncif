using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float speed;
    public float jumpForce;
    public float maxSpeed;
    private Rigidbody2D rigidbody;
    private Animator anim;
    public LayerMask layer;
    
    public bool isJumping;
    public bool doubleJump;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    void FixedUpdate() {
        Move();
    }

    void Move()
    {
        //Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        //transform.position += movement * Time.deltaTime * speed;

        float movement = Input.GetAxisRaw("Horizontal");

        rigidbody.velocity = new Vector2(movement * speed, rigidbody.velocity.y);

        if(Input.GetAxisRaw("Horizontal") != 0)
            anim.SetBool("walk", true);
        else
        {
            anim.SetBool("walk", false);
        }
        if(Input.GetAxisRaw("Horizontal") > 0f)
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        else if(Input.GetAxisRaw("Horizontal") < 0f)
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }

    void Jump()
    {
        if(grounded())
        {
            isJumping = false;
            doubleJump = true;
            anim.SetBool("doubleJump", true);
        }
        else if(!grounded())
        {
            isJumping = true;
        }
        if(Input.GetButtonDown("Jump") && isJumping && doubleJump)
            {
                doubleJump = false;
                anim.SetBool("doubleJump", false);
                rigidbody.velocity = new Vector2(rigidbody.velocity.x , 0f);
                rigidbody.AddForce(new Vector2(0f, jumpForce/1.5f), ForceMode2D.Impulse);
                /*if(rigidbody.velocity.y >= 0)
                    rigidbody.AddForce(new Vector2(0f, jumpForce/1.5f + rigidbody.velocity.y), ForceMode2D.Impulse);
                else if(rigidbody.velocity.y < 0)
                    rigidbody.AddForce(new Vector2(0f, jumpForce/1.5f + (rigidbody.velocity.y * -1)), ForceMode2D.Impulse);*/
            }
        else if(Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x , 0f);
                rigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
                anim.SetBool("doubleJump", true);
            }
        }
        if(rigidbody.velocity.y < 0){
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        /*if(rigidbody.velocity.magnitude > maxSpeed && rigidbody.velocity.y >= 0){
             rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
        }*/
        if(rigidbody.velocity.magnitude > (maxSpeed * 1.8f) && rigidbody.velocity.y < 0){
             rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, (maxSpeed * 1.8f));
        }
        if(rigidbody.velocity.y > 0.35f)
        {
            anim.SetBool("fall", false);
            anim.SetBool("jump", true);
        }
        else if(rigidbody.velocity.y < -0.35f)
        {
            anim.SetBool("jump", false);
            anim.SetBool("fall", true);
            anim.SetBool("doubleJump", true);
        }
        else
        {
            anim.SetBool("jump", false);
            anim.SetBool("fall", false);
        }
        
    }

    private bool grounded(){
        float boxHeight = 0.05f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size - new Vector3(0.1f, 0f, 0f), 0f, Vector2.down, boxHeight, 1 << LayerMask.NameToLayer("Ground"));
        Color rayColor;
        if(raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else{
            rayColor = Color.red;
        }
        /*Debug.DrawRay(gameObject.GetComponent<BoxCollider2D>().bounds.center + new Vector3(gameObject.GetComponent<BoxCollider2D>().bounds.extents.x, 0), Vector2.down * (gameObject.GetComponent<BoxCollider2D>().bounds.extents.y + boxHeight), rayColor);
        Debug.DrawRay(gameObject.GetComponent<BoxCollider2D>().bounds.center - new Vector3(gameObject.GetComponent<BoxCollider2D>().bounds.extents.x, 0), Vector2.down * (gameObject.GetComponent<BoxCollider2D>().bounds.extents.y + boxHeight), rayColor);
        Debug.DrawRay(gameObject.GetComponent<BoxCollider2D>().bounds.center - new Vector3(gameObject.GetComponent<BoxCollider2D>().bounds.extents.x, gameObject.GetComponent<BoxCollider2D>().bounds.extents.y + boxHeight), Vector3.right * (gameObject.GetComponent<BoxCollider2D>().bounds.extents.x) * 2, rayColor);*/
        return raycastHit.collider != null;
    }

    /*void OnGUI()
    {
         GUI.Label(new Rect(20, 20, 200, 200), "rigidbody velocity: " + rigidbody.velocity);
    }*/

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*if(collision.gameObject.layer == 6)
        {
            isJumping = false;
            doubleJump = true;
            anim.SetBool("doubleJump", true);
        }*/
        if(collision.gameObject.tag == "Danger")
        {
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Finish")
        {
            GameController.instance.ChangeScene(GameController.instance.nextStage);
        }
        if(collision.gameObject.tag == "Trampoline")
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("tocou");
            rigidbody.AddForce(new Vector2(0f, collision.gameObject.GetComponent<Trampoline>().trampolineForce), ForceMode2D.Impulse);
            isJumping = true;
            doubleJump = true;
        }
        if(collision.gameObject.tag == "Collectible")
        {
            if(collision.gameObject.GetComponent<Box>().boxUp)
                rigidbody.AddForce(new Vector2(0f, collision.gameObject.GetComponent<Box>().boxForce * -1 * 0.5f), ForceMode2D.Impulse);
            else
                rigidbody.AddForce(new Vector2(0f, collision.gameObject.GetComponent<Box>().boxForce), ForceMode2D.Impulse);
            collision.gameObject.GetComponentInParent<Animator>().SetTrigger("hit");
            collision.gameObject.GetComponent<Box>().boxLife -= 1;
            GameController.instance.totalScore += collision.gameObject.GetComponent<Box>().boxScore;
            GameController.instance.UpdateScoreText();
            if(collision.gameObject.GetComponent<Box>().boxLife <= 0)
            {
                Destroy(collision.transform.parent.gameObject);
            }
        }
        if(collision.gameObject.tag == "Enemy")
        {
            float height = collision.contacts[0].point.y - collision.gameObject.GetComponent<Enemy>().heightPoint.position.y;
            if(height > 0)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x , 0f);
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f , jumpForce/1.5f), ForceMode2D.Impulse);
                collision.gameObject.GetComponent<Enemy>().speed = 0;
                collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Animator>().SetTrigger("die");
                Destroy (collision.gameObject, collision.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); 
            }
            else
            {
                GameController.instance.ShowGameOver();
                Destroy(gameObject);
            }
            Debug.Log(height);
        }
        /*
        if(collision.gameObject.tag == "Enemy")
        {
            float height = gameObject.GetComponent<BoxCollider2D>().collision.contacts[0].point.y - collision.gameObject.GetComponent<Enemy>().heightPoint.position.y;
            if(height > 0)
            {
                rigidbody.AddForce(new Vector2(0f, jumpForce/1.5f), ForceMode2D.Impulse);
                Destroy(collision.gameObject);
            }
            else
            {
                GameController.instance.ShowGameOver();
            }
        }*/
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        /*if(collision.gameObject.layer == 6)
        {
            isJumping = false;
            doubleJump = true;
            anim.SetBool("doubleJump", true);
        }*/
        if(collision.gameObject.tag == "Danger")
        {
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Finish")
        {
            GameController.instance.ChangeScene(GameController.instance.nextStage);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        /*if(collision.gameObject.layer == 6)
        {
            isJumping = true;
        }*/
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag == "Collectible")
        {
            GameController.instance.totalScore += collider.gameObject.GetComponent<Collectible>().score;
            GameController.instance.UpdateScoreText();
            Destroy(collider.gameObject);
        }
    }
}
