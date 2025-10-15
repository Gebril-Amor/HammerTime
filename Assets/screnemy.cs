using System.Collections;
using UnityEngine;

public class screnemy : MonoBehaviour
{
    public int health = 20;
    public float attackRange = 1.5f;
    public float attackDelay = 2f;

    private Animator anim;
    private SpriteRenderer sr;
    private Transform player;

    private float timeInFront = 0f;
    public bool isAttacking = false;
    public int damage = 1;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void FixedUpdate()
    {
        if (health <= 0 || player == null) return;

        // ✅ Face the player left/right
        if (player.position.x < transform.position.x)
            sr.flipX = true; // face left
        else
            sr.flipX = false; // face right

        // ✅ Direction check
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float facingDir = sr.flipX ? -1f : 1f;
        Vector2 facingVector = new Vector2(facingDir, 0f);

        bool isPlayerInFront = Vector2.Dot(facingVector, dirToPlayer) > 0.7f;
        float distance = Vector2.Distance(player.position, transform.position);

        // ✅ Wait 2 seconds if player stays in front
        if (distance <= attackRange && isPlayerInFront && !isAttacking)
        {
            timeInFront += Time.deltaTime;
            if (timeInFront >= attackDelay)
                Attack();
        }
        else
        {
            timeInFront = 0f;
        }
    }

    private void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("attack"); // 🎬 play attack animation
     
        Debug.Log("💥 Skeleton attacks!");
 
      
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log($"💀 Skeleton took {dmg} damage! Remaining HP: {health}");

        if (health <= 0)
        {
            anim.SetTrigger("die");
            GetComponent<Collider2D>().enabled = false;
        }
                       AudioSource audio = Camera.main.GetComponent<AudioSource>();
                if (audio == null)
                    audio = Camera.main.gameObject.AddComponent<AudioSource>();

                AudioClip snd = Resources.Load<AudioClip>("sfx/sndhit");
                if (audio != null && snd != null)
                {
                    audio.PlayOneShot(snd, 0.5f);
                }
                else
                {
                    Debug.Log("Audio clip not found");
                }
    }

    // 🧩 Called by animation event at the end of death animation
    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
    
    public void OnAttackAnimationEnd()
    {
        //player.GetComponent<scrhp>()?.TakeDamage(damage);
       player.GetComponent<scrmovement>().GetComponent<Animator>().SetTrigger("hurt");
     
         isAttacking = false;
        timeInFront = 0f; // Reset after attack completes
    }
}