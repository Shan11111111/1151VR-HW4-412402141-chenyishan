using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody))]
public class AirplaneController : MonoBehaviour
{
    [Header("Speeds")]
    public float forwardSpeed = 8f;
    public float horizontalSpeed = 6f;
    public float verticalSpeed = 5f;

    [Header("Flight Boundaries (plane CENTER)")]
    public float minX = -1.5f;
    public float maxX = 1.5f;
    public float minY = 0.5f;
    public float maxY = 5f;
    public float maxZ = 71f;

    private Rigidbody rb;
    private float horizontalInput, verticalInput;
    private bool leftPressed, rightPressed, upPressed, downPressed;
    private bool flying;

    void Awake() { rb = GetComponent<Rigidbody>(); rb.isKinematic = true; rb.useGravity = false; }
    public void SetFlying(bool value)
    {
        flying = value;
        if (!value) { horizontalInput = 0; verticalInput = 0; ClearTouch(); }
    }
    public void SetButtonState(int direction, bool pressed)
    {
        switch (direction)
        {
            case 0: leftPressed = pressed; break;
            case 1: rightPressed = pressed; break;
            case 2: upPressed = pressed; break;
            case 3: downPressed = pressed; break;
        }
    }
    void ClearTouch() { leftPressed = rightPressed = upPressed = downPressed = false; }

    void Update()
    {
        if (!flying) return;
        float h = 0f, v = 0f;
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        if (k != null)
        {
            if (k.aKey.isPressed || k.leftArrowKey.isPressed) h -= 1f;
            if (k.dKey.isPressed || k.rightArrowKey.isPressed) h += 1f;
            if (k.wKey.isPressed || k.upArrowKey.isPressed) v += 1f;
            if (k.sKey.isPressed || k.downArrowKey.isPressed) v -= 1f;
        }
#elif ENABLE_LEGACY_INPUT_MANAGER
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
#endif
        horizontalInput = Mathf.Clamp(h + (rightPressed ? 1f : 0f) - (leftPressed ? 1f : 0f), -1f, 1f);
        verticalInput = Mathf.Clamp(v + (upPressed ? 1f : 0f) - (downPressed ? 1f : 0f), -1f, 1f);
    }

    void FixedUpdate()
    {
        if (!flying) return;
        Vector3 next = rb.position + new Vector3(horizontalInput * horizontalSpeed,
            verticalInput * verticalSpeed, forwardSpeed) * Time.fixedDeltaTime;
        next.x = Mathf.Clamp(next.x, minX, maxX);
        next.y = Mathf.Clamp(next.y, minY, maxY);
        next.z = Mathf.Min(next.z, maxZ);
        rb.MovePosition(next);
    }
}
