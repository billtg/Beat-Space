using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float enemySpeed;
    public float startBeat;
    public float endBeat = 8;
    public int enemyType;

    Rigidbody2D rb;

    public GameObject explosionPrefab;

    public Animator animator;
    public AnimatorStateInfo animatorSI;
    public int currentState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animatorSI = animator.GetCurrentAnimatorStateInfo(0);
        currentState = animatorSI.fullPathHash;
        if (enemyType == 1)
            rb.velocity = new Vector2(-enemySpeed, 0);
    }

    public void Initialize(float startBeat)
    {
        this.startBeat = startBeat;
    }

    private void Update()
    {
        switch (enemyType){
            case 1:
                CheckForHitEnd();
                WarpVelocity();
                break;
            case 2:
                PatternMove();
                break;
        }
        
    }

    private void CheckForHitEnd()
    {
        if (this.gameObject.transform.position.x < -9)
        {
            Debug.Log("Hit!");
            Destroy(this.gameObject);
            GameManager.instance.LoseGame();
        }
    }

    private void WarpVelocity()
    {
        float xwarp = Mathf.Abs(Conductor.instance.loopPosInAnalog-0.5f);
        float ywarp = 5*Mathf.Abs(Conductor.instance.loopPosInAnalog - 0.5f)-1.25f;
        rb.velocity = new Vector2(-enemySpeed - xwarp, ywarp);
    }

    private void PatternMove()
    {
        float localLoopPos = (Conductor.instance.songPosInBeats - startBeat)/endBeat;
        if (localLoopPos > 1)
            Destroy(this.gameObject);
        animator.Play(currentState, -1, (localLoopPos));
        animator.speed = 0;
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
        Destroy(explosion, 1f);
        Destroy(this.gameObject);
    }
}
