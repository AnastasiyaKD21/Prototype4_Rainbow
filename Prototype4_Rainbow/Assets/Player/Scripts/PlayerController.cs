using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 dir;
    private Animator anim;

    [SerializeField] public float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private int iceCream;
    [SerializeField] private Text iceCreamText;

    [HideInInspector] public int lineToMove = 1;

    private float maxSpeed = 110;

    public float lineDistance = 4;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(SpeedIncrease());
        Time.timeScale = 1;
        iceCreamText.text = ": " + iceCream.ToString();
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
            anim.SetBool("IsRun", true);
        else
            anim.SetBool("IsRun", false);
            

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
    }

    private void Jump()
    {
        dir.y = jumpForce;
        anim.SetTrigger("IsJump");
    }

    void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(5);
        if(speed < maxSpeed)
        {
            speed += 4;
            StartCoroutine(SpeedIncrease()); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "IceCream")
        {
            iceCream++;
            iceCreamText.text = ": " + iceCream.ToString();
            Destroy(other.gameObject);
        }
    }      
}
