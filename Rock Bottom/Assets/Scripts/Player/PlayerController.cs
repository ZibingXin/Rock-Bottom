using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private GameObject drill;
    [SerializeField] private float offset = 1f;

    [SerializeField] private Vector3Int downDrillDirection;
    [SerializeField] private Vector3Int rightDrillDirection;
    [SerializeField] private Vector3Int leftDrillDirection;

    [SerializeField]private TilemapDrillInteractor tdi;
    //private InputSystem_Actions inputActions;
    //private Vector2 moveInput;

    //private void Awake()
    //{
    //    inputActions = new InputSystem_Actions();
    //    inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
    //    inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    //}

    //private void OnEnable()
    //{
    //    inputActions.Enable();
    //}

    //private void OnDisable()
    //{
    //    inputActions.Disable();
    //}

    private void Update()
    {
        // find the tile in the direction of the drill
        downDrillDirection = Vector3Int.FloorToInt(transform.position + Vector3.down * offset);
        rightDrillDirection = Vector3Int.FloorToInt(transform.position + Vector3.right * offset);
        leftDrillDirection = Vector3Int.FloorToInt(transform.position + Vector3.left * offset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(downDrillDirection, 0.2f);
        Gizmos.DrawSphere(rightDrillDirection, 0.2f);
        Gizmos.DrawSphere(leftDrillDirection, 0.2f);
    }

    //public void Move()
    //{
    //    float moveSpeed = PlayerStats.Instance.MoveSpeed;
    //    Vector3 movement = Vector3.zero;

    //    // Only allow horizontal or downward movement, one direction at a time
    //    if (moveInput.x > 0.1f)
    //    {
    //        movement = Vector3.right;
    //        ActivateDrill(Vector3.right);
    //    }
    //    else if (moveInput.x < -0.1f)
    //    {
    //        movement = Vector3.left;
    //        ActivateDrill(Vector3.left);
    //    }
    //    else if (moveInput.y < -0.1f)
    //    {
    //        movement = Vector3.down;
    //        ActivateDrill(Vector3.down);
    //    }
    //    else
    //    {
    //        DeactivateDrill();
    //    }

    //    transform.position += movement * moveSpeed * Time.deltaTime;
    //}

    public void OnDigDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            tdi.GetTile();
        }

    }

    //private void ActivateDrill(Vector3 direction)
    //{
    //    drill.SetActive(true);
    //    drill.transform.position = transform.position + direction.normalized * offset;
    //}

    //private void DeactivateDrill()
    //{
    //    drill.SetActive(false);
    //}
}
