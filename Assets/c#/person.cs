using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class person : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    public bool isGrounded;
    private float currentSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        // 地面检测
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 移动输入
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 根据摄像机方向计算移动方向
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 跳跃
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 重力应用
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 奔跑切换
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    }
}