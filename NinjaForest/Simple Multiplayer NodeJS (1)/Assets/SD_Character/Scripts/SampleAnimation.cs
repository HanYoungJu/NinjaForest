using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SampleAnimation : NetworkBehaviour
{
    private Animator animator;
    public bool isLocal = false;

    private const string key_isRun = "IsRun";
    private const string key_isAttack1 = "IsAttack01";
    private const string key_isAttack2 = "IsAttack02";
    private const string key_isJump = "IsJump";
    private const string key_isDamage = "IsDamage";
    private const string key_isDead = "IsDead";

    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isLocal)
        {
            //move
            if (Input.GetKey(KeyCode.UpArrow) || (Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.RightArrow)))
            {
                this.animator.SetBool(key_isRun, true);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandAnimation("run");
            }
            else
            {
                this.animator.SetBool(key_isRun, false);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandAnimation("unrun");
            }
            //attack1
            if (Input.GetKeyUp("z"))
            {
                this.animator.SetBool(key_isAttack1, true);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandAnimation("attack1");
            }
            else
            {
                this.animator.SetBool(key_isAttack1, false);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandAnimation("unattack1");
            }
            //attack2
            if (Input.GetKeyUp("x"))
            {
                this.animator.SetBool(key_isAttack2, true);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandAnimation("attack2");
            }
            else
            {
                this.animator.SetBool(key_isAttack2, false);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandAnimation("unattack2");
            }
            //jump
            if (Input.GetKeyUp("space"))
            {
                this.animator.SetBool(key_isJump, true);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandAnimation("jump");
            }
            else
            {
                this.animator.SetBool(key_isJump, false);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandAnimation("unjump");
            }
            /*
            if (Input.GetKeyUp("d"))
            {
                this.animator.SetBool(key_isDamage, true);
            }
            else
            {
                this.animator.SetBool(key_isDamage, false);
            }

            if (Input.GetKeyUp("f"))
            {
                this.animator.SetBool(key_isDead, true);
            }
            */
        }
    }
    public void setRun()
    {
        this.animator.SetBool(key_isRun, true);
    }
    public void unRun()
    {
        this.animator.SetBool(key_isRun, false);
    }
    public void setAttack1()
    {
        this.animator.SetBool(key_isAttack1, true);
    }
    public void unAttack1()
    {
        this.animator.SetBool(key_isAttack1, false);
    }
    public void setAttack2()
    {
        this.animator.SetBool(key_isAttack2, true);
    }
    public void unAttack2()
    {
        this.animator.SetBool(key_isAttack2, false);
    }
    public void setJump()
    {
        this.animator.SetBool(key_isJump, true);
    }
    public void unJump()
    {
        this.animator.SetBool(key_isJump, false);
    }
}