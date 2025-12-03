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

    public TextMeshProUGUI buttonText;

    Vector3Int? hoveredCell = null;

    void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    private void Start()
    {
        bombCount = 3;
        buttonText.text = "Use Bomb";
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
        if (t is ResourceTile rT)
        {
            if (rT.type == ResourceType.Bedrock) return;
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
                SwitchHover(false);
            }
        }
        else
        {
            ClearHighlight();
        }

        if (Input.GetMouseButtonDown(1) && enableHover)
        {
            SwitchHover(false);
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


    public void SwitchHover(bool v)
    {
        if (v && bombCount <= 0) 
        {
            return; 
        }
        if (!v || (v && enableHover))
        {
            ClearHighlight();
            buttonText.text = "Use Bomb";
            enableHover = false;
            return;
        }

        if (v)
        {
            enableHover = v;
            buttonText.text = "Cancel"; 
        }
    }
}
