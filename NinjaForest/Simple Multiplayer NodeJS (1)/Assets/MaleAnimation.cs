using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleAnimation : MonoBehaviour {
    private Animator animator;
    private const string key_run = "runBool";
    private const string key_backrun = "backrunBool";
    private const string key_wave = "waveBool";
    private const string key_jump = "jumpBool";

    // Use this for initialization
    void Start () {
        this.animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.animator.SetBool(key_run, true);
        }
        else
        {
            this.animator.SetBool(key_run, false);
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            this.animator.SetBool(key_backrun, true);
        }
        else
        {
            this.animator.SetBool(key_backrun, false);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            this.animator.SetBool(key_wave, true);
        }
        else
        {
            this.animator.SetBool(key_wave, false);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            this.animator.SetBool(key_jump, true);
        }
        else
        {
            this.animator.SetBool(key_jump, false);
        }
    }
}
