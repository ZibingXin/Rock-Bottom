using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
//using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Tilemap tilemap;
    public InputActionReference digDownAction;
    public InputActionReference digRightAction;
    public InputActionReference digLeftAction;

    [Header("Movement")]
    public float moveTime = 0.1f;
    public bool autoEnterMind = true;

    [Header("Hold Repeat")]
    public bool holdToRepeat = true;
    public float initialDelay = 0.25f;
    public float repeatRate = 0.1f;

    [Header("Drill Settings")]
    public DrillCostsConfig costs;
    public DrillWorthConfig worth;
    public int startOil = 100;


    private Vector3Int currentCell;
    private bool isMoving = false;
    private Coroutine repeatCo;
    private AudioSource sfx;
    private PlayerStats playerStats;
    private TilemapDrillInteractor drillInteractor;

    public Camera playerCamera;


    private void Awake()
    {
        // audio source
        // sfx = GetComponent<AudioSource>();
        playerStats = GetComponent<PlayerStats>();
        drillInteractor = GetComponent<TilemapDrillInteractor>();
        costs = drillInteractor.costs;
        worth = drillInteractor.worth;
    }

    private void Update()
    {
        playerCamera.transform.position = new Vector3(0, transform.position.y - 2, -10);
    }

    private void OnEnable()
    {
        digDownAction.action.performed += OnDigDownPerformed;
        digRightAction.action.performed += OnDigRightPerformed;
        digLeftAction.action.performed += OnDigLeftPerformed;

        if (holdToRepeat)
        {
            digDownAction.action.canceled += OnActionCanceled;
            digRightAction.action.canceled += OnActionCanceled;
            digLeftAction.action.canceled += OnActionCanceled;
        }

        digDownAction.action.Enable();
        digRightAction.action.Enable();
        digLeftAction.action.Enable();

        currentCell = tilemap.WorldToCell(transform.position);
        Snap(currentCell);
    }

    private void OnDisable()
    {
        StopRepeat();
        digDownAction.action.performed -= OnDigDownPerformed;
        digRightAction.action.performed -= OnDigRightPerformed;
        digLeftAction.action.performed -= OnDigLeftPerformed;
        digDownAction.action.canceled -= OnActionCanceled;
        digRightAction.action.canceled -= OnActionCanceled;
        digLeftAction.action.canceled -= OnActionCanceled;
    }

    void OnDigDownPerformed(InputAction.CallbackContext context) => Step(Vector3Int.down, true);
    void OnDigRightPerformed(InputAction.CallbackContext context) => Step(Vector3Int.right, true);
    void OnDigLeftPerformed(InputAction.CallbackContext context) => Step(Vector3Int.left, true);
    void OnActionCanceled(InputAction.CallbackContext context) => StopRepeat();

    private void Step(Vector3Int dir, bool fromInput)
    {
        if (isMoving) return;

        Vector3Int target = currentCell + dir;

        var tile = tilemap.GetTile(target);
        if (tile is ResourceTile resourceTile && resourceTile.IsDiggable)
        {
            Debug.Log($"{costs.CostFor(resourceTile.type)}");

            if (playerStats.CurrentOil < costs.CostFor(resourceTile.type) && resourceTile.type != ResourceType.Oil)
            {
                Debug.Log($"Not enough oil to dig! {playerStats.CurrentOil} < {costs.CostFor(resourceTile.type)}");
                return;
            }
            resourceTile.HandleDig(target, tilemap, drillInteractor.GetContext());
            if (autoEnterMind)
            { 
                StartCoroutine(MoveToCell(target)); 
            }
        }
        else if (tile == null)
        {
            StartCoroutine(MoveToCell(target)); 
        }

        if (fromInput && holdToRepeat)
        {
            StopRepeat();
            repeatCo = StartCoroutine(Repeat(dir));
        }
    }

    IEnumerator Repeat(Vector3Int dir)
    {
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            Step(dir, false);
            yield return new WaitForSeconds(repeatRate);
        }
    }

    void StopRepeat()
    {
        if (repeatCo != null)
        {
            StopCoroutine(repeatCo);
            repeatCo = null;
        }
    }

    IEnumerator MoveToCell(Vector3Int targetCell)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell);
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime / Mathf.Max(0.0001f, moveTime);
            transform.position = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0, 1, elapsed));
            yield return null;
        }
        currentCell = targetCell;
        Snap(currentCell);
        isMoving = false;
    }

    void Snap(Vector3Int cell)
    {
        transform.position = tilemap.GetCellCenterWorld(cell);
    }
}
