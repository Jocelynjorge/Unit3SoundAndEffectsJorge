using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float jumpForce;
    public float gravityModifier;
    private Animator playerAnim;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public bool isOnGround = true;
    public ParticleSystem dirtParticle;
    public bool gameOver;
    public ParticleSystem explosionParticle;
    private AudioSource playerAudio;
    public bool doubleJumpUsed = false;
    public float doubleJumpForce;
    public bool doubleSpeed = false;



    // Start is called before the first frame update
    void Start()

    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }




    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            dirtParticle.Stop();

            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            doubleJumpUsed = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && !doubleJumpUsed)
        {
            doubleJumpUsed = true;
            playerRb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            playerAnim.Play("Running_Jump", 3, 0f);
            playerAudio.PlayOneShot(jumpSound,1.0f);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            doubleSpeed = true;
            playerAnim.SetFloat("Speed_Multiplier", 2.0f);
        }
        else if (doubleSpeed)
        {
            doubleSpeed = false;
            playerAnim.SetFloat("Speed_Multipiler", 1.0f);
        }
        
        
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            dirtParticle.Play();
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerAudio.PlayOneShot(crashSound, 1.0f);
            explosionParticle.Play();
            dirtParticle.Stop();
        


            gameOver = true;
            Debug.Log("Game Over");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
        }
    }
     

   


}

