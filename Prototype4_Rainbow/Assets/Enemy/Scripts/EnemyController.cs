using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 dir;
    private Animator anim;

    [SerializeField] public Transform player;
    [SerializeField] public float Espeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;

    [HideInInspector] public int lineToMove = 1;

    public float lineDistance = 4;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        if(SwipeController.swipeRight)
        {
            if(lineToMove < 2)
                lineToMove++;   
        }

        if(SwipeController.swipeLeft)
        {
            if(lineToMove > 0)
                lineToMove--;
        }

        if(SwipeController.swipeUp)
        {
            if(controller.isGrounded)
                Jump();
        }

        if(controller.isGrounded)
            anim.SetBool("IsRunning", true);
        else
            anim.SetBool("IsRunning", false);
            
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;  

        if(lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;  

        if(transform.position == targetPosition)
            return;

        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;

        if(moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

        // Destroy(gameObject, 6f);
    }

    private void Jump()
    {
        dir.y = jumpForce;
        anim.SetTrigger("IsJumping");
    }
    
    public void FixedUpdate()
    {
        float step = Espeed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(5);
        Espeed += 4;
    }
}
