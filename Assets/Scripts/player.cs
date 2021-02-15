using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyColider;
    BoxCollider2D myfeet;
    CameraScript myCamera;
    AudioSource jumpaudio;

    
    

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 0f;
    [SerializeField] AudioClip jumpsound;
    [SerializeField] AudioClip damagesound;
    [SerializeField] AudioClip fuelsound;
    [SerializeField] AudioClip deathsound;



    float gravityScaleAtStart;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyColider = GetComponent<CapsuleCollider2D>();
        myfeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        myCamera = Camera.main.GetComponent<CameraScript>();
        jumpaudio = new AudioSource();
    }

    void Update()
    {
        Run();
        Jump();
        FlipSprite();
        SetJump();
        CheckDead();

        //if (myRigidBody.velocity.magnitude > 0)
        //{
        //    FinishJump();
        //}
    }

    void Run()
    {
        if (myfeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) 
        {
            Vector2 StopplayerVelocity = new Vector2(0f, 0f);
            myRigidBody.velocity = StopplayerVelocity;
            myAnimator.SetBool("Running", false);
            return; 
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float calculatedRunSpeed = 5f;

        if (CrossPlatformInputManager.GetButton("RunFast"))
        {
            calculatedRunSpeed = runSpeed * 2f;
            myAnimator.SetFloat("DoubleAnimationPlay", 1f);
        }
        else
        {
            calculatedRunSpeed = runSpeed;
        }

        Vector2 playerVelocity = new Vector2(controlThrow * calculatedRunSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    void Jump()
    {
        
        if (!myfeet.IsTouchingLayers(LayerMask.GetMask("Ground")) || jumpSpeed == 0 || myRigidBody.velocity.magnitude > 0)
        {
            return; 
        }

        if (!HasFuel())
        {
            return;
        }
        
        if (CrossPlatformInputManager.GetButton("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
            UseFuel(Convert.ToInt32(jumpSpeed * .28));
            AudioSource.PlayClipAtPoint(jumpsound, new Vector3(myRigidBody.transform.position.x, myRigidBody.transform.position.y, myRigidBody.transform.position.z));
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    void SetJump()
    {
        float newJumpSpeed;

        if (!myfeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return; 
        }

        if (CrossPlatformInputManager.GetButton("IncreaseJump"))
        {
            newJumpSpeed = jumpSpeed + 1f;
            if(jumpSpeed >= 100)
            {
                jumpSpeed = 100;
            }
            else
            {
                jumpSpeed = newJumpSpeed;
            }
            
        }
        else if (CrossPlatformInputManager.GetButton("DecreaseJump"))
        {
            newJumpSpeed = jumpSpeed + -1f;

            if(jumpSpeed <= 0)
            {
                jumpSpeed = 0;
            }
            else
            {
                jumpSpeed = newJumpSpeed;
            }
        }
        myCamera.SetLvl(jumpSpeed);
        return;
    }

    void DoDamage(int dam)
    {
        myCamera.SetHp(dam);
        AudioSource.PlayClipAtPoint(damagesound, new Vector3(myRigidBody.transform.position.x, myRigidBody.transform.position.y, myRigidBody.transform.position.z));
        return;
    }

    void CheckDead()
    {
        int hp = Convert.ToInt32(myCamera.GetHp());
        if(hp <= 0)
        {
            AudioSource.PlayClipAtPoint(deathsound, new Vector3(myRigidBody.transform.position.x, myRigidBody.transform.position.y, myRigidBody.transform.position.z));
            SceneManager.LoadScene("Lose");
        }
        else
        {
            return;
        }
    }

    void UseFuel(int fueltouse)
    {
        if (HasFuel())
        {
            myCamera.UseFuel(fueltouse);
        }
    }

    bool HasFuel()
    {
        if(myCamera.GetFuel() > 0)
        {
            return true;
        }
        else
        {
            myCamera.SetFuel(0);
            SceneManager.LoadScene("Lose");
            return false;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 80)
        {
            AudioSource.PlayClipAtPoint(damagesound, new Vector3(myRigidBody.transform.position.x, myRigidBody.transform.position.y, myRigidBody.transform.position.z));
            DoDamage(15);
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fuel")
        {
            AudioSource.PlayClipAtPoint(fuelsound, new Vector3(myRigidBody.transform.position.x, myRigidBody.transform.position.y, myRigidBody.transform.position.z));
            myCamera.SetFuel(100);
            Destroy(collision.gameObject);
        }
    }

    void FinishJump()
    {
        if (!myfeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        jumpSpeed = 0;
    }
}
