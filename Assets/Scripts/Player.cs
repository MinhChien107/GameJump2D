using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Lực nhảy của người chơi
    public Vector2 jumpForce;
    //Lực tăng lên khi dí chuột
    public Vector2 jumpForceUp;

    public float maxForceX;
    public float minForceX;

    public float maxForceY;
    public float minForceY;

    [HideInInspector]
    public int lastPlastFormId;

    private bool isJump; 
    //Kiểm tra xem lực nhảy của người chơi được bật hay chưa
    private bool powerSetted;

    Rigidbody2D rb;
    Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void SetPower()
    {
        if (powerSetted && !isJump)
        {
            jumpForce.x += jumpForceUp.x * Time.deltaTime;
            jumpForce.y += jumpForceUp.y * Time.deltaTime;

            jumpForce.x = Mathf.Clamp(jumpForce.x, minForceX, maxForceX);
            jumpForce.y = Mathf.Clamp(jumpForce.y, minForceY, maxForceY);
        }
    }

    public void SetPower(bool isHoldingMouse) {
        powerSetted = isHoldingMouse;
        if (!powerSetted && !isJump)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (!rb || jumpForce.x < 0 || jumpForce.y < 0) return;

        rb.velocity = jumpForce;
        isJump = true;
        if (anim)
        {
            anim.SetBool("isJump", false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetPower();
        if (Input.GetMouseButtonDown(1))
        {
            SetPower(true);
        }
         if(Input.GetMouseButtonUp(1))
        {
            SetPower(false);   
        }
    }

    //Kiểm tra va chạm với ground

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConstans.GROUND))
        {
            PlatForm p = collision.transform.root.GetComponent<PlatForm>();
            if (isJump)
            {
                isJump = false;
                if (anim)
                {
                    anim.SetBool("isJump", true);
                }
                if (rb)
                {
                    rb.velocity = Vector2.zero;
                }
                jumpForce = Vector2.zero;
            }

            if (p && p.id != lastPlastFormId)
            {
                GameManager.Ins.CreatePlatformAndLerp(transform.position.x);
                lastPlastFormId = p.id;
                GameManager.Ins.AddScore(); 
            }
        }

        if (collision.CompareTag(TagConstans.DEAD_ZONE))
        {
            GameGUIManager.Ins.ShowGameOverDialog();
            Destroy(gameObject);
        }
    }
}
