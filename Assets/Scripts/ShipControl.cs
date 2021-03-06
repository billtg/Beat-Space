﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speedx;
    public float speedy;
    public bool turbo = false;
    public float turbox;
    public float turboy;
    public float turboMax = 100;
    public float turboAmount;
    public float turboDrain;
    public float turboRestore;
    public bool turboBurnt;
    public GameObject turboScale;

    private Vector2 speed = new Vector2(0,0);
    public float shotSpeed;

    public GameObject shot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = new Vector2(speedx, speedy);
        turboAmount = turboMax;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //Check if turbo is changed
        CheckTurbo();
        //CheckWall(x,y);
        Vector2 dir = new Vector2(x, y);

        rb.velocity = new Vector2(dir.x*speed.x, dir.y*speed.y);

        //Firing
        if(Input.GetButtonDown("Fire1"))
        {
            if (Conductor.instance.InThreshold(1) || Conductor.instance.InThreshold(2))
                Shoot();
            else
            {
                //Debug.Log("Nope" + Conductor.instance.loopPosInBeats.ToString());
                GameManager.instance.ClearMultiplier();
            }
        }

        ModifyTurbo();
     
    }

    private void CheckTurbo()
    {
        //Check for burnt turbo
        if (Input.GetButtonDown("Fire2") && !turboBurnt)
        {
            //Debug.Log("Turbo!");
            turbo = true;
            speed = new Vector2(turbox, turboy);
        }
        if (Input.GetButtonUp("Fire2") || turboBurnt)
        {
            //Debug.Log("No Turbo");
            turbo = false;
            speed = new Vector2(speedx, speedy);
        }

    }

    private void ModifyTurbo()
    {
        //Either turbo is on and draining, or off and restoring
        if (turbo)
        {
            //Case 1: Draining
            turboAmount -= turboDrain;
            if (turboAmount <= 0)
                turboBurnt = true;
        }
        else
        {
            //Case 2: Turbo restoring or full
            if (turboAmount == turboMax)
                return;
            if (turboAmount > turboMax)
                turboAmount = turboMax;
            else
            {
                turboAmount += turboRestore;
                if (turboAmount >= turboMax)
                    turboBurnt = false;
            }
        }

        turboScale.transform.localScale = new Vector3(1-(turboAmount / turboMax), 1, 1);
    }

    private void CheckWall(float x, float y)
    {
        Debug.Log(x.ToString() + "," + y.ToString());
        if (this.gameObject.transform.position.x >= 4.5 && x > 0)
        {
            Debug.Log("1");
            speed.x = 0;
        }
        else
            speed.x = speedx;
        if (this.gameObject.transform.position.x <= -4.5 && x < 0)
        {
            Debug.Log("2");
            speed.x = 0;
        }
        else
            speed.x = speedx;
        if (this.gameObject.transform.position.y >= 4.5 && y > 0)
            speed.y = 0;
        else
            speed.y = speedy;
        if (this.gameObject.transform.position.y <= -4.5 && y < 0)
            speed.y = 0;
    }

    private void Shoot()
    {
        //Debug.Log("Pew pew" + Conductor.instance.loopPosInBeats.ToString());
        GameObject firedShot;
        firedShot = Instantiate(shot,this.transform.position,this.transform.rotation);
        firedShot.GetComponent<Rigidbody2D>().velocity = new Vector2(shotSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<shotcollision>())
            return;

        Destroy(this.gameObject);
        GameManager.instance.LoseGame();

    }
}
