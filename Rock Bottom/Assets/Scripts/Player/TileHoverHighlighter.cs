using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHoverHighlighter : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap tilemap;
    public Tilemap highlightTilemap;
    public TileBase highlightTile;

    [Header("Other")]
    public Camera cam;
    public PlayerController player;
    public int bombCount = 3;
    public TextMeshProUGUI bombCountText;
    public bool enableHover = false;

    Vector3Int? hoveredCell = null;

    void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    private void Start()
    {
        bombCount = 3;
    }
    void Update()
    {
        bombCountText.text = bombCount.ToString() + "/3";
        if (!enableHover)
        {
            ClearHighlight();
            return;
        }

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = tilemap.WorldToCell(mouseWorld);

        TileBase t = tilemap.GetTile(cell);
        if (t is ResourceTile)
        {
            if (hoveredCell == null || hoveredCell.Value != cell)
            {
                ClearHighlight();
                highlightTilemap.SetTile(cell, highlightTile);
                hoveredCell = cell;
            }

            if (Input.GetMouseButtonDown(0) && player != null)
            {
                tilemap.SetTile(cell, null);
                bombCount--;
                SetHoverEnabled(false);
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    void ClearHighlight()
    {
        if (hoveredCell != null)
        {
            highlightTilemap.SetTile(hoveredCell.Value, null);
            hoveredCell = null;
        }
    }


    public void SetHoverEnabled(bool v)
    {
        if (v == true && bombCount <= 0) return;
        enableHover = v;
        if (!v) ClearHighlight();
    }
}
