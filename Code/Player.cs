using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Source: https://www.noveltech.dev/reusable-character-controller-platformer/
//Source: https://github.com/Wally869/TutorialPlatformer2D/blob/master/Assets/Scripts/PlayerController.cs



public enum CharacterState
{
    IDLE,
    RUNNING,
    JUMPING
}

public class Player : MonoBehaviour
{
    public CharacterState mPlayerState = CharacterState.IDLE;

    [Header("Movement Settings")]
    public float mSpeed =5.0f;
    public float mJumpStrength = 10.0f;

/*
    TODO: Animations
    [Header("State Sprites")]
    public RuntimeAnimatorController mIdleController;
    public RuntimeAnimatorController mRunningController;
    public RuntimeAnimatorController mJumpingController;
    

    private Animator _mAnimatorComponent;
*/
    private bool _bIsGoingRight = true;
    //private bool _bPlayerStateChanged = false;


    void Start()
    {
       //_mAnimatorComponent = gameObject.GetComponent<Animator>();
       //_mAnimatorComponent.RuntimeAnimatorController = mIdleController; 
    }

    
    void Update()
    {
        //_bPlayerStateChanged = false;
        
        if (mPlayerState == CharacterState.IDLE)
        {
            if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A)))
            {
                //_bPlayerStateChanged = true;
                mPlayerState = CharacterState.RUNNING;
                if (Input.GetKey(KeyCode.D))
                {
                    _bIsGoingRight = true;
                }
                else
                {
                    _bIsGoingRight = false;
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                //_bPlayerStateChanged = true;
                mPlayerState = CharacterState.JUMPING;
                StartCoroutine("CheckGrounded");
            }
        }
        else if (mPlayerState == CharacterState.RUNNING)
        {
            if (Input.GetKey(KeyCode.W))
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                //_bPlayerStateChanged = true;
                mPlayerState = CharacterState.JUMPING;
                StartCoroutine("CheckGrounded");
            }
            else if (!Input.GetKey(KeyCode.D) && (!Input.GetKey(KeyCode.A)))
            {
                //_bPlayerStateChanged = true;
                mPlayerState = CharacterState.IDLE;
            }
        }



        if (mPlayerState == CharacterState.JUMPING || mPlayerState == CharacterState.RUNNING)
        {
            if (Input.GetKey(KeyCode.D))
            {
                _bIsGoingRight = true;
                if (mPlayerState == CharacterState.JUMPING)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = transform.right *Time.deltaTime * mSpeed;
                }
                transform.Translate(transform.right * Time.deltaTime * mSpeed);
                
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _bIsGoingRight = false;
                if (mPlayerState == CharacterState.JUMPING)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = -transform.right *Time.deltaTime * mSpeed;
                }
                transform.Translate(-transform.right * Time.deltaTime * mSpeed);
                

            }
        }
    }


/*
    public void ChangeAnimator()
    {
        RuntimeAnimatorController newAnimator = mIdleController;

        if (mPlayerState == CharacterState.RUNNING || mPlayerState == CharacterState.JUMPING)
        {
            newAnimator = mRunningController;
            if (_bIsGoingRight)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        gameObject.GetComponent<Animator>().runtimeAnimatorController = newAnimator;
    }
*/
    IEnumerator CheckGrounded()
    {
        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position -Vector3.up * 1f, -Vector2.up, 0.05f);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Terrain")
                {
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                    {
                        mPlayerState = CharacterState.RUNNING;
                    }
                    else
                    {
                        mPlayerState = CharacterState.IDLE;
                    }
                    break;
                }
            }

            yield return new WaitForSeconds(0.05f);

        }

        //ChangeAnimator();
        yield return null;
    }
}
