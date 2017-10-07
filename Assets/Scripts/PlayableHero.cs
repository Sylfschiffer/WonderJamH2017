﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentPlayer
{
    Player1,
    Player2
}

public class PlayableHero : MonoBehaviour {

    public bool isSnared, isSauced, isReversed, isSliding, isBumped;
    int direction = 1;
    public float speed = 5f;
    public float jumpHeight = 50f;
    public float impulseForce;
    public LayerMask ground;
    public Transform spawn;
    public Animator myAnimator;

    public CurrentPlayer currentPlayer;

    public Rigidbody2D rgb;
    bool isJumping;

    public virtual void Awake()
    {

    }

    [HideInInspector] public PlayerState currentState;
    [HideInInspector] public PlayerState previousState;
    [HideInInspector] public Rigidbody2D playerBody;
    //[HideInInspector] public Animator playerAnimator;

    public void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        myAnimator = gameObject.GetComponent<Animator>();
        currentState = new Idle(this); playerBody = gameObject.GetComponent<Rigidbody2D>(); /*playerAnimator = gameObject.GetComponent<Animator>()*/;
    }

    public void Update()
    {
        if(isSnared)
        {
            isSnared = false;
            Snare();
        }
        if (isSauced)
        {
            isSauced = false;
            Sauce();
        }
        if (isBumped)
        {
            isBumped = false;
            Bump();
        }
        if (isReversed)
        {
            isReversed = false;
            Reverse();
        }
        currentState.Execute();
    }

    public void ChangeState(PlayerState next)
    {
        currentState.Exit();
        previousState = currentState;
        currentState = next;
        currentState.Enter();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "KillingMachine")
        {
            SetKill();
        }
        currentState.HandleCollision(collision);
    }
    public void OnCollisionStay2D(Collision2D collision) { currentState.HandleCollision(collision); }

    public virtual void Spell1()
    {

    }

    public void Snare()
    {
        ChangeState(new Snared(this, 2));
    }

    public bool LookGround()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.1f;
        Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, ground);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    public void Bump()
    {
        ChangeState(new Bumped(this, 1));
    }

    public void Sauce()
    {
        ChangeState(new Sauced(this, 2));
    }
    public void Reverse()
    {
        ChangeState(new Reversed(this, 2));
    }

    public void SetWet(bool value)
    {
        if(value)
        {
            ChangeState(new IsWet(this));
        }
        else
        {
            previousState.Execute();
        }
    }


    public void SetKill()
    {
        ChangeState(new Dying(this));
    }

}

// Basic container for player states
public class PlayerState
{
    public float debuffTimer;
    public Animator anim;
    protected PlayableHero myController;
    public PlayerState(PlayableHero master) { myController = master; anim = myController.GetComponent<Animator>(); }
    public virtual void Enter() { } // Called once when entering current state
    public virtual void Execute() { } // Called once every update
    public virtual void Exit() { } // Called once to clean-up before entering the next state
    public virtual void HandleCollision(Collision2D collision) { } // Called by Controller's OnCollisionEnter2D and OnCollisionStay2D
}

public class Idle : PlayerState
{
    public Idle(PlayableHero master) : base(master) { }

    public override void Enter() { } // Called once when entering current state
    public override void Execute()
    {
        if(Input.GetButtonDown(myController.currentPlayer.ToString() + "Fire1"))
        {
            myController.ChangeState(new CastPower1(myController, myController.currentState));
        }
        if (Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") != 0)
        {
            myController.ChangeState(new Move(myController, 1));
        }
        if (Input.GetButtonDown(myController.currentPlayer.ToString() + "Jump"))
        {
            myController.ChangeState(new Jump(myController,false,myController.currentState));
        }

    }
}

public class Jump : PlayerState
{
    static int jumpNumber = 0;
    bool backToPreviousState;
    PlayerState previousState;
    public Jump(PlayableHero master, bool back, PlayerState previousState) : base(master) { backToPreviousState = back; this.previousState = previousState; this.debuffTimer = debuffTimer; }
    public override void Enter()  // Called once when entering current state
    {
        Debug.Log(myController.LookGround());
        if (myController.LookGround())
        {
            myController.rgb.AddForce(new Vector2(0, myController.jumpHeight), ForceMode2D.Force);
        }
    }

    public override void Execute()
    {
        if (Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") != 0)
            myController.rgb.velocity = new Vector2(Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") * myController.speed, myController.rgb.velocity.y); 
        else
        {
            myController.rgb.velocity = new Vector2(0, myController.rgb.velocity.y);
        }

        if (backToPreviousState)
       {
            debuffTimer -= Time.deltaTime;
       }
    }
    public override void HandleCollision(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!backToPreviousState || previousState.ToString() == "IsWet")
            {
                myController.ChangeState(new Idle(myController));
            }
            else
            {
                myController.ChangeState(previousState);
            }

            
            //myController.playerAnimator.SetBool("Airborn", false);
        }
    }

    private void AddJumpForce() { myController.playerBody.AddForce(new Vector2(0, myController.jumpHeight), ForceMode2D.Impulse); }
}

public class Move : PlayerState
{
    int direction;
    public Move(PlayableHero master, int direction) : base(master) { this.direction = direction; }
    public override void Enter()
    {
        Debug.Log("je marche");
        myController.myAnimator.SetBool("isMoving", true);
        myController.rgb.velocity = new Vector2(Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") * myController.speed*direction, myController.rgb.velocity.y);
    }
    public override void Execute()
    {
        if (Input.GetButtonDown(myController.currentPlayer.ToString() + "Fire1"))
        {
            myController.ChangeState(new CastPower1(myController, myController.previousState));
        }
        if (Input.GetButtonDown(myController.currentPlayer.ToString() + "Jump"))
        {
            myController.ChangeState(new Jump(myController, false, myController.currentState));
        }
        if (Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") != 0)
        {
            myController.rgb.velocity = new Vector2(Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") * myController.speed * direction, myController.rgb.velocity.y);
            if(myController.rgb.velocity.x < 0 && myController.transform.localScale.x > 0)
            {
                myController.transform.localScale = new Vector3(-myController.transform.localScale.x, myController.transform.localScale.y, myController.transform.localScale.z);
            }
            else if(myController.rgb.velocity.x > 0 && myController.transform.localScale.x < 0)
            {
                myController.transform.localScale = new Vector3(-myController.transform.localScale.x, myController.transform.localScale.y, myController.transform.localScale.z);
            }
        }
        else
        {
            myController.rgb.velocity = new Vector2(0, myController.rgb.velocity.y);
            myController.ChangeState(new Idle(myController));
        }

    }
    public override void Exit()
    {
        myController.myAnimator.SetBool("isMoving", false);
    }

}

public class Snared : PlayerState
{
    public Snared(PlayableHero master, float debuffTimer) : base(master) { this.debuffTimer = debuffTimer; }
    public override void Enter()
    {
        myController.rgb.velocity = new Vector2(0, 0);
    }
    public override void Execute()
    {
        if (debuffTimer > 0)
        {
            myController.rgb.velocity = new Vector2(0, myController.rgb.velocity.y);
            debuffTimer -= Time.deltaTime;
        }
        else
        {
            myController.ChangeState(new Idle(myController));
        }
    }
    public override void Exit()
    {

    }

}

public class Sauced : PlayerState
{
    public Sauced(PlayableHero master, float debuffTimer) : base(master) { this.debuffTimer = debuffTimer; }
    public override void Enter()
    {

    }
    public override void Execute()
    {
        if(debuffTimer > 0)
        {
            if (Input.GetButtonDown(myController.currentPlayer.ToString() + "Fire1"))
            {
                myController.ChangeState(new CastPower1(myController, myController.previousState));
            }
            if (Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") != 0)
            {
                myController.rgb.AddForce(new Vector2(Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") * 10, myController.rgb.velocity.y));
                if (myController.rgb.velocity.x > myController.speed)
                {
                    myController.rgb.velocity = new Vector2(myController.speed, myController.rgb.velocity.y);
                }
                else if (myController.rgb.velocity.x < -myController.speed)
                {
                    myController.rgb.velocity = new Vector2(-myController.speed, myController.rgb.velocity.y);
                }
            }
            if (Input.GetButtonDown(myController.currentPlayer.ToString() + "Jump"))
            {
                myController.ChangeState(new Jump(myController, true, myController.currentState));
            }
            debuffTimer -= Time.deltaTime;
        }
        else
        {
            myController.ChangeState(new Idle(myController));
        }
    }
    public override void Exit()
    {

    }

}

public class Bumped : PlayerState
{
    public Bumped(PlayableHero master, float debuffTimer) : base(master) { this.debuffTimer = debuffTimer; }
    public override void Enter()
    {
        myController.rgb.AddForce(new Vector2(myController.impulseForce, 0), ForceMode2D.Impulse);
    }
    public override void Execute()
    {
        if(debuffTimer > 0)
        {
            debuffTimer -= Time.deltaTime;
        }
        else
        {
            myController.ChangeState(new Idle(myController));
        }
    }
    public override void Exit()
    {

    }

}

public class Reversed : PlayerState
{
    public Reversed(PlayableHero master, float debuffTimer) : base(master) { this.debuffTimer = debuffTimer; }
    public override void Enter()
    {

    }
    public override void Execute()
    {
        
        if (debuffTimer > 0)
        {
            if (Input.GetButtonDown(myController.currentPlayer.ToString() + "Fire1"))
            {
                myController.ChangeState(new CastPower1(myController, myController.previousState));
            }
            if (Input.GetButtonDown(myController.currentPlayer.ToString() + "Jump"))
            {
                myController.ChangeState(new Jump(myController, false, myController.currentState));
            }
            if (Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") != 0)
            {
                myController.rgb.velocity = new Vector2(Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") * myController.speed * -1, myController.rgb.velocity.y);
            }
            else
            {
                myController.rgb.velocity = new Vector2(0, myController.rgb.velocity.y);
            }
            debuffTimer -= Time.deltaTime;
        }
        else
        {
            myController.ChangeState(new Idle(myController));
        }

    }
    public override void Exit()
    {

    }

}

public class IsWet : PlayerState
{
    public IsWet(PlayableHero master) : base(master) {}
    public override void Enter()
    {
    }
    public override void Execute()
    {
        if (Input.GetButtonDown(myController.currentPlayer.ToString() + "Fire1"))
        {
            myController.ChangeState(new CastPower1(myController, myController.currentState));
        }
        if (Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") != 0)
        {
            myController.rgb.AddForce(new Vector2(Input.GetAxis(myController.currentPlayer.ToString() + "Horizontal") * 10, myController.rgb.velocity.y));
            if (myController.rgb.velocity.x > myController.speed)
            {
                myController.rgb.velocity = new Vector2(myController.speed, myController.rgb.velocity.y);
            }
            else if (myController.rgb.velocity.x < -myController.speed)
            {
                myController.rgb.velocity = new Vector2(-myController.speed, myController.rgb.velocity.y);
            }
        }
        if (Input.GetButtonDown(myController.currentPlayer.ToString() + "Jump"))
        {
            myController.ChangeState(new Jump(myController, false, myController.currentState));
        }
    }
    public override void Exit()
    {

    }

}

public class CastPower1 : PlayerState
{
    bool backToPreviousState;
    PlayerState previousState;
    float animTimer = 0.25f;

    public CastPower1(PlayableHero master, PlayerState previousState) : base(master) { this.previousState = previousState;}
    public override void Enter()  // Called once when entering current state
    {
        //anim.SetTrigger("castSpell");
        //animTimer = anim.GetCurrentAnimatorStateInfo(0).length;
        myController.Spell1();
    }

    public override void Execute()
    {
        if (animTimer > 0)
        {
            animTimer -= Time.deltaTime;
        }
        else
        {
            myController.ChangeState(previousState);
        }
    }
}

public class Dying : PlayerState
{
    public Dying(PlayableHero master) : base(master) {}
    public override void Enter()
    {
        //start death animation
    }
    public override void Execute()
    {
        //At the end of animation
        myController.ChangeState(new Idle(myController));
    }
    public override void Exit()
    {
        myController.transform.position = myController.spawn.position;
    }

}
//TODO isWetReversed