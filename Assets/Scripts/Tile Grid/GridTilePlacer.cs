using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GridTilePlacer : MonoBehaviour
{
    // Public Variables

    // Private Variables

    [SerializeField] private List<GameObject> _availableTiles = new();

    private List<ObjectBehaviour> _placedTilesObjectBehaviours = new();
    private GridBuilder _gridBuilder;
    private UIManager _uIManager;

    private Vector3 mouseTilePosition;

    private bool _isMouseOnGridOrTile = false;
    private bool _isLeftClicking = false;

    //private LayerMask layersToHit = LayerMask.GetMask("Grid Tile,Object Unstable")

    // Getters & Setters

    public List<GameObject> AvailableTiles
    {
        get { return _availableTiles; }
    }

    public List<ObjectBehaviour> PlacedTilesObjectBehaviours
    {
        get { return _placedTilesObjectBehaviours; }
    }

    public bool IsMouseOnGridOrTile
    {
        get { return _isMouseOnGridOrTile; }
    }

    void Awake()
    {
        _gridBuilder = FindObjectOfType<GridBuilder>(true);
        _uIManager = FindObjectOfType<UIManager>(true);

        mouseTilePosition = new Vector3(0f, 0f, 0f);
    }

    private void FixedUpdate()
    {
        MouseTileOnGrid();
    }

    private void MouseTileOnGrid()
    {
        if (_uIManager.IsBuildMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!_uIManager.IsTopToBottomBuildMode &&
                Physics.Raycast(ray, out RaycastHit hitInfo, 100f,
                LayerMask.GetMask("Grid Tile"), QueryTriggerInteraction.UseGlobal))
            {
                if (hitInfo.collider.gameObject.layer ==
                    LayerMask.NameToLayer("Grid Tile")) // does ray hit a grid tile collider
                {
                    if (!_isMouseOnGridOrTile) _isMouseOnGridOrTile = true;
                    FreeHandBasedBuildingOnGround(hitInfo);
                }
                //else if (_isMouseOnTile) _isMouseOnTile = false;
            }
            else if (_uIManager.IsTopToBottomBuildMode &&
                Physics.Raycast(ray, out RaycastHit hitInformation, 100f,
                LayerMask.GetMask("Grid Tile", "Object Unstable", "Object Stable"),
                QueryTriggerInteraction.UseGlobal))
            {
                if (hitInformation.collider.gameObject.layer ==
                    LayerMask.NameToLayer("Grid Tile")) // does ray hit a grid tile collider
                {
                    if (!_isMouseOnGridOrTile) _isMouseOnGridOrTile = true;
                    FreeHandBasedBuildingOnGround(hitInformation);
                }
                else if (hitInformation.collider.gameObject.layer ==
                    LayerMask.NameToLayer("Object Unstable") ||
                    hitInformation.collider.gameObject.layer ==
                    LayerMask.NameToLayer("Object Stable")) // does ray hit a grid tile collider
                {
                    if (!_isMouseOnGridOrTile) _isMouseOnGridOrTile = true;
                    //Debug.DrawRay(hitInformation.point, -ray.direction * 50f, Color.red);

                    if (!_uIManager.IsFreeHandBuilding)
                    {
                        mouseTilePosition = new Vector3(
                            Mathf.Clamp(hitInformation.transform.position.x,
                                _gridBuilder.FromPoint.position.x +
                                (_gridBuilder.GridTileSize * 0.5f) +
                                (_gridBuilder.GridTileSize *
                                (_uIManager.TileRescaleXSliderValue - 1f) / 2f),
                                _gridBuilder.ToPoint.position.x -
                                (_gridBuilder.GridTileSize * 0.5f) -
                                (_gridBuilder.GridTileSize *
                                (_uIManager.TileRescaleXSliderValue - 1f) / 2f)),
                            hitInformation.transform.position.y +
                            (hitInformation.transform.localScale.y * 0.5f) +
                            (_gridBuilder.GridTileSize * 0.5f) +
                            (_gridBuilder.GridTileSize *
                            (_uIManager.TileRescaleYSliderValue - 1f) / 2f),
                            Mathf.Clamp(hitInformation.transform.position.z,
                            _gridBuilder.FromPoint.position.z +
                            (_gridBuilder.GridTileSize * 0.5f) +
                            (_gridBuilder.GridTileSize *
                            (_uIManager.TileRescaleZSliderValue - 1f) / 2f),
                                _gridBuilder.ToPoint.position.z -
                                (_gridBuilder.GridTileSize * 0.5f) -
                                (_gridBuilder.GridTileSize *
                                (_uIManager.TileRescaleZSliderValue - 1f) / 2f)));
                    }
                    else if (_uIManager.IsFreeHandBuilding)
                    {
                        mouseTilePosition = new Vector3(
                            Mathf.Clamp(hitInformation.point.x,
                                _gridBuilder.FromPoint.position.x +
                                (_gridBuilder.GridTileSize * 0.5f) +
                                (_gridBuilder.GridTileSize *
                                (_uIManager.TileRescaleXSliderValue - 1f) / 2f),
                                _gridBuilder.ToPoint.position.x -
                                (_gridBuilder.GridTileSize * 0.5f) -
                                (_gridBuilder.GridTileSize *
                                (_uIManager.TileRescaleXSliderValue - 1f) / 2f)),
                            /*hitInformation.point.y*/
                            hitInformation.transform.position.y +
                            (hitInformation.transform.localScale.y * 0.5f) +
                            (_gridBuilder.GridTileSize * 0.5f) +
                            (_gridBuilder.GridTileSize *
                            (_uIManager.TileRescaleYSliderValue - 1f) / 2f),
                            Mathf.Clamp(hitInformation.point.z,
                                _gridBuilder.FromPoint.position.z +
                                (_gridBuilder.GridTileSize * 0.5f) +
                                (_gridBuilder.GridTileSize *
                                (_uIManager.TileRescaleZSliderValue - 1f) / 2f),
                                _gridBuilder.ToPoint.position.z -
                                (_gridBuilder.GridTileSize * 0.5f) -
                                (_gridBuilder.GridTileSize *
                                (_uIManager.TileRescaleZSliderValue - 1f) / 2f)));
                    }
                }
            }
            else
            {
                _isMouseOnGridOrTile = false;
            }
        }
    }

    private void FreeHandBasedBuildingOnGround(RaycastHit hitInfo)
    {
        if (!_uIManager.IsFreeHandBuilding)
        {
            mouseTilePosition = new Vector3(
                Mathf.Clamp(hitInfo.transform.position.x,
                    _gridBuilder.FromPoint.position.x +
                    (_gridBuilder.GridTileSize * 0.5f) +
                    (_gridBuilder.GridTileSize *
                    (_uIManager.TileRescaleXSliderValue - 1f) / 2f),
                    _gridBuilder.ToPoint.position.x -
                    (_gridBuilder.GridTileSize * 0.5f) -
                    (_gridBuilder.GridTileSize *
                    (_uIManager.TileRescaleXSliderValue - 1f) / 2f)),
                _gridBuilder.GridTileSize * 0.5f +
                (_gridBuilder.GridTileSize *
                (_uIManager.TileRescaleYSliderValue - 1f) / 2f),
                Mathf.Clamp(hitInfo.transform.position.z,
                    _gridBuilder.FromPoint.position.z +
                    (_gridBuilder.GridTileSize * 0.5f) +
                    (_gridBuilder.GridTileSize *
                    (_uIManager.TileRescaleZSliderValue - 1f) / 2f),
                    _gridBuilder.ToPoint.position.z -
                    (_gridBuilder.GridTileSize * 0.5f) -
                    (_gridBuilder.GridTileSize *
                    (_uIManager.TileRescaleZSliderValue - 1f) / 2f)));
        }
        else if (_uIManager.IsFreeHandBuilding)
        {
            mouseTilePosition = new Vector3(
                Mathf.Clamp(hitInfo.point.x,
                    _gridBuilder.FromPoint.position.x +
                    (_gridBuilder.GridTileSize * 0.5f) +
                    (_gridBuilder.GridTileSize *
                    (_uIManager.TileRescaleXSliderValue - 1f) / 2f),
                    _gridBuilder.ToPoint.position.x -
                    (_gridBuilder.GridTileSize * 0.5f) -
                    (_gridBuilder.GridTileSize *
                    (_uIManager.TileRescaleXSliderValue - 1f) / 2f)),
                _gridBuilder.GridTileSize * 0.5f +
                (_gridBuilder.GridTileSize *
                (_uIManager.TileRescaleYSliderValue - 1f) / 2f),
                Mathf.Clamp(hitInfo.point.z,
                    _gridBuilder.FromPoint.position.z +
                    (_gridBuilder.GridTileSize * 0.5f) +
                    (_gridBuilder.GridTileSize *
                    (_uIManager.TileRescaleZSliderValue - 1f) / 2f),
                    _gridBuilder.ToPoint.position.z -
                    (_gridBuilder.GridTileSize * 0.5f) -
                    (_gridBuilder.GridTileSize *
                    (_uIManager.TileRescaleZSliderValue - 1f) / 2f)));
        }
    }

    void Update()
    {
        MouseHolographicObjHandler();
        PlaceNewTile();
        EraseObjectOnClick();
    }

    private void PlaceNewTile()
    { 
        if (_uIManager.IsBuildMode &&
            !_uIManager.IsEraserMode &&
            _isMouseOnGridOrTile &&
            isMouseHolographicObjActive() &&
            _uIManager.SelectedTileType != BuildingBlocks.none &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            if (_isLeftClicking)
            {
                if (!_uIManager.IsAutoTilePlacing)
                {
                    InstantiateNewTile();
                    _isLeftClicking = false;
                }
                else if (_uIManager.IsAutoTilePlacing)
                {
                    _uIManager.TimePerTilePlaceCounter -= Time.deltaTime;

                    if (_uIManager.TimePerTilePlaceCounter <= float.Epsilon)
                    {
                        InstantiateNewTile();
                        _uIManager.TimePerTilePlaceCounter = _uIManager.TimePerTilePlace;
                    }
                }
            }
            else if (!_isLeftClicking &&
                _uIManager.TimePerTilePlaceCounter >= float.Epsilon)
            {
                _uIManager.TimePerTilePlaceCounter = 0f;
            }
        }
    }


    private void InstantiateNewTile()
    {
        ObjectBehaviour instantiation = Instantiate(
            _uIManager.TileToPlace,
            mouseTilePosition,
            Quaternion.identity,
            _gridBuilder.GridPlacedTilesParent).
            GetComponentInChildren<ObjectBehaviour>();

        instantiation.GetComponent<ObjectBehaviour>()
            .Construct(_uIManager.PreparationObjectBehaviour);

        //instantiation(_uIManager.PreparationObjectBehaviour);
        SetInstantiationMaterialColor(instantiation);

        instantiation.transform.localScale = new Vector3(
            instantiation.transform.localScale.x *
            (_uIManager.TileRescaleXSliderValue),
            instantiation.transform.localScale.y *
            (_uIManager.TileRescaleYSliderValue),
            instantiation.transform.localScale.z *
            (_uIManager.TileRescaleZSliderValue));

        _placedTilesObjectBehaviours.Add(instantiation);
    }

    
    //private ObjectBehaviour TileSetValues(ObjectBehaviour objectBehaviour)
    //{
    //    if (_uIManager.PreparationObjectBehaviour.ObjectKinematic)
    //    {
    //        objectBehaviour.ObjectKinematic =
    //            _uIManager.PreparationObjectBehaviour.ObjectKinematic;
    //        objectBehaviour.ObjectPhysicMaterial =
    //            _uIManager.PreparationObjectBehaviour.ObjectPhysicMaterial;

    //        if (_uIManager.PreparationObjectBehaviour.ObjectGravity)
    //        {
    //            objectBehaviour.ObjectGravity =
    //                _uIManager.PreparationObjectBehaviour.ObjectGravity;
    //            objectBehaviour.ObjectMass =
    //                _uIManager.PreparationObjectBehaviour.ObjectMass;
    //        }
    //    }
    //    if (_uIManager.PreparationObjectBehaviour.ObjectType == ObjectType.metalic)
    //    {
    //        objectBehaviour.ObjectType =
    //            _uIManager.PreparationObjectBehaviour.ObjectType;

    //        if (_uIManager.PreparationObjectBehaviour.ObjectMagnet)
    //        {
    //            objectBehaviour.ObjectMagnet =
    //                _uIManager.PreparationObjectBehaviour.ObjectMagnet;
    //            objectBehaviour.ObjectMagnetStrength =
    //                _uIManager.PreparationObjectBehaviour.ObjectMagnetStrength;
    //            objectBehaviour.ObjectMagnetRange =
    //                _uIManager.PreparationObjectBehaviour.ObjectMagnetRange;
    //        }
    //    }

    //    return objectBehaviour;
    //}

    private void SetInstantiationMaterialColor(ObjectBehaviour instantiation)
    {
        switch (_uIManager.BlockColor)
        {
            case BlockColors.none:
                break;
            case BlockColors.red:
                instantiation.GetComponent<MeshRenderer>().material =
                    _uIManager.RedMaterial;
                break;
            case BlockColors.orange:
                instantiation.GetComponent<MeshRenderer>().material =
                    _uIManager.OrangeMaterial;
                break;
            case BlockColors.brown:
                instantiation.GetComponent<MeshRenderer>().material =
                    _uIManager.BrownMaterial;
                break;
            case BlockColors.yellow:
                instantiation.GetComponent<MeshRenderer>().material =
                    _uIManager.YellowMaterial;
                break;
            case BlockColors.lightGrey:
                instantiation.GetComponent<MeshRenderer>().material =
                    _uIManager.LightGreyMaterial;
                break;
            case BlockColors.green:
                instantiation.GetComponent<MeshRenderer>().material =
                    _uIManager.GreenMaterial;
                break;
            case BlockColors.blue:
                instantiation.GetComponent<MeshRenderer>().material =
                    _uIManager.BlueMaterial;
                break;
            case BlockColors.darkGrey:
                instantiation.GetComponent<MeshRenderer>().material =
                    _uIManager.DarkGreyMaterial;
                break;
            default:
                break;
        }
    }

    private bool isMouseHolographicObjActive() => _uIManager.SelectedTile.activeSelf;

    private void MouseHolographicObjHandler() // shitty name change later
    {
        if (_uIManager.IsBuildMode)
        {
            _uIManager.SelectedTile.transform.position = mouseTilePosition;

            if (_isMouseOnGridOrTile == true &&
                isMouseHolographicObjActive() == false)
                _uIManager.SelectedTile.SetActive(true);
            else if (_isMouseOnGridOrTile == false &&
                isMouseHolographicObjActive())
                _uIManager.SelectedTile.SetActive(false);
        }
        else if (!_uIManager.IsBuildMode) _uIManager.SelectedTile.SetActive(false);
        // needs to later be expanded to in order to replace..
        // ..the selected object which will be used with the glass material
    }

    public void UpdateMouseHolographicObjScale()
    {
        _uIManager.SelectedTile.transform.localScale = new Vector3(
            _gridBuilder.GridTileSize *
            (_uIManager.TileRescaleXSliderValue),
            _gridBuilder.GridTileSize *
            (_uIManager.TileRescaleYSliderValue),
            _gridBuilder.GridTileSize *
            (_uIManager.TileRescaleZSliderValue));
    }

    private void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            _isLeftClicking = true;
        }
        else if (!value.isPressed)
        {
            _isLeftClicking = false;
        }
    }

    // Update this method to use the new kill function on ObjectBehaviour (maybe?)
    private void EraseObjectOnClick()
    {
        if (_uIManager.IsEraserMode &&
            !_uIManager.IsBuildMode &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            if (_isLeftClicking)
            {
                if (!_uIManager.IsAutoTilePlacing)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // setup ray
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f,
                        LayerMask.GetMask("Object Unstable", "Object Stable"),
                        QueryTriggerInteraction.UseGlobal)) // cast the ray
                    {
                        if (hitInfo.transform.parent != null &&
                            hitInfo.transform.parent != _gridBuilder.GridPlacedTilesParent)
                        {
                            Destroy(hitInfo.transform.parent.gameObject);
                        }
                        else
                        {
                            Destroy(hitInfo.transform.gameObject);
                        }
                    }
                    _isLeftClicking = false;
                }
                else if (_uIManager.IsAutoTilePlacing)
                {
                    _uIManager.TimePerTilePlaceCounter -= Time.deltaTime;

                    if (_uIManager.TimePerTilePlaceCounter <= float.Epsilon)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // setup ray
                        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f,
                            LayerMask.GetMask("Object Unstable", "Object Stable"),
                            QueryTriggerInteraction.UseGlobal)) // cast the ray
                        {
                            if (hitInfo.transform.parent != null &&
                            hitInfo.transform.parent != _gridBuilder.GridPlacedTilesParent)
                            {
                                Destroy(hitInfo.transform.parent.gameObject);
                            }
                            else
                            {
                                Destroy(hitInfo.transform.gameObject);
                            }
                        }

                        _uIManager.TimePerTilePlaceCounter = _uIManager.TimePerTilePlace;
                    }
                }
            }
            else if (!_isLeftClicking &&
                _uIManager.TimePerTilePlaceCounter >= float.Epsilon)
            {
                _uIManager.TimePerTilePlaceCounter = 0f;
            }
        }
    }

    public void removePlacedTileFromList(ObjectBehaviour tileToRemove) =>
        _placedTilesObjectBehaviours.Remove(tileToRemove);
}

//Vector3 distanceToTarget = hitInfo.point - transform.position;
//Vector3 forceDirection = distanceToTarget.normalized;