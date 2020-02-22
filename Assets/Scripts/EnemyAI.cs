using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float enemySpeed;
    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-enemySpeed, 0);
    }

    private void Update()
    {
        CheckForHitEnd();
        WarpVelocity();
        
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
}
