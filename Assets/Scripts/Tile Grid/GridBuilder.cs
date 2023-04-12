using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    // Public Variables

    // Private Variables

    [SerializeField] private Transform _fromPoint;
    [SerializeField] private Transform _toPoint;
    [SerializeField] private Transform _gridCollisionTilesParent;
    [SerializeField] private Transform _gridPlacedTilesParent;
    [SerializeField] private GameObject _gridTileCollisionBox;
    private List<GameObject> _gridCollisionTiles = new();

    [Range(0.5f, 6f)]
    [SerializeField] private float _gridTileSize = 1f;

    private UIManager _uIManager;
    private GridTilePlacer _gridTilePlacer;

    // Getters & Setters

    public Transform FromPoint
    {
        get { return _fromPoint; }
    }

    public Transform ToPoint
    {
        get { return _toPoint; }
    }

    public Transform GridCollisionTilesParent
    {
        get { return _gridCollisionTilesParent; }
    }

    public Transform GridPlacedTilesParent
    {
        get { return _gridPlacedTilesParent; }
    }

    public GameObject GridTileCollisionBox
    {
        get { return _gridTileCollisionBox; }
    }

    public float GridTileSize
    {
        get { return _gridTileSize; }
        set { _gridTileSize = Mathf.Clamp(value, 0.5f, 6f); }
    }

    private void Awake()
    {
        InitializeGrid();
        _uIManager = FindObjectOfType<UIManager>();
        _gridTilePlacer = FindObjectOfType<GridTilePlacer>();
    }

    public void InitializeGrid()
    {
        _gridCollisionTiles.ForEach(tile =>
        {
            Destroy(tile);
        });
        _gridCollisionTiles.Clear();

        _gridTileCollisionBox.transform.localScale =
            new Vector3(_gridTileSize, 1f, _gridTileSize);

        float fromMinusToX = _fromPoint.position.x - _toPoint.position.x;
        float fromMinusToZ = _fromPoint.position.z - _toPoint.position.z;
        float tileModuloGridSizeX = Mathf.Abs(fromMinusToX % _gridTileSize);
        float tileModuloGridSizeZ = Mathf.Abs(fromMinusToZ % _gridTileSize);
        float gridXLength = Mathf.Abs(fromMinusToX) +
            (tileModuloGridSizeX == 0f ? 0f : -_gridTileSize);
        float gridZLength = Mathf.Abs(fromMinusToZ) +
            (tileModuloGridSizeZ == 0f ? 0f : -_gridTileSize);

        for (float i = 0; i < gridXLength; i += _gridTileSize)
        {
            for (float j = 0; j < gridZLength; j += _gridTileSize)
            {
                float x = i + (_gridTileSize / 2f);
                float z = j + (_gridTileSize / 2f);
                float offsetX = Mathf.Abs(fromMinusToX % _gridTileSize) / 2;
                float offsetZ = Mathf.Abs(fromMinusToZ % _gridTileSize) / 2;
                float offsetY = -((_gridTileCollisionBox.transform.localScale.y /
                    2f) + 0.05f);

                _gridCollisionTiles.Add(Instantiate(
                    _gridTileCollisionBox,
                    new Vector3(x, 0f, z) +
                    new Vector3(offsetX, /*TRY offsetY here!*/ -0.45f, offsetZ),
                    Quaternion.identity,
                    _gridCollisionTilesParent));
            }
        }
    }

    public void ReInitializeGrid(float newGridTileSize)
    {
        _gridTileSize = newGridTileSize;

        if (_uIManager.TileToPlace)
        {
            _uIManager.SelectedTile.transform.localScale = new Vector3(
          _gridTileSize, _gridTileSize, _gridTileSize);

            _uIManager.TileToPlace.transform.localScale = new Vector3(
                _gridTileSize, _gridTileSize, _gridTileSize);

            InitializeGrid();
            _gridTilePlacer.UpdateMouseHolographicObjScale();
        }

    }

    // GIZMOS STUFF NOT IMPORTANT FOR PRODUCTION
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.321f, 0.541f, 0.682f, 0.5f);
        Gizmos.DrawWireCube((_fromPoint.position + _toPoint.position) /
            2f, _toPoint.transform.position);

        float fromMinusToX = _fromPoint.position.x - _toPoint.position.x;
        float fromMinusToZ = _fromPoint.position.z - _toPoint.position.z;
        float drawTileModuloGridSizeX = Mathf.Abs(fromMinusToX % _gridTileSize);
        float drawTileModuloGridSizeZ = Mathf.Abs(fromMinusToZ % _gridTileSize);
        float gridXLength = Mathf.Abs(fromMinusToX) +
            (drawTileModuloGridSizeX == 0f ? 0f : -_gridTileSize);
        float gridZLength = Mathf.Abs(fromMinusToZ) +
                (drawTileModuloGridSizeZ == 0f ? 0f : -_gridTileSize);


        for (float i = 0; i < gridXLength; i += _gridTileSize)
        {
            for (float j = 0; j < gridZLength; j += _gridTileSize)
            {
                float x = i + (_gridTileSize / 2f);
                float z = j + (_gridTileSize / 2f);
                float offsetX = Mathf.Abs(fromMinusToX % _gridTileSize) / 2;
                float offsetZ = Mathf.Abs(fromMinusToZ % _gridTileSize) / 2;
                float offsetY = -((_gridTileCollisionBox.transform.localScale.y / 2f) *
                   _gridTileSize) + 0.1f;

                Gizmos.DrawWireCube(
                    new Vector3(x, 0f, z) +
                    new Vector3(offsetX, offsetY, offsetZ),
                    new Vector3(_gridTileSize, _gridTileSize, _gridTileSize));
            }
        }
    }
}
