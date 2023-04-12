using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickNDrag : MonoBehaviour
{
    // Public Variables

    // Private Variables

    private GridBuilder _gridBuilder;
    private GridTilePlacer _gridTilePlacer;
    private UIManager _uIManager;

    [SerializeField] private GameObject _mouseFollower;
    private Rigidbody _mouseFollowerRb;

    [SerializeField] private GameObject _objectGrabbed;
    private Rigidbody _objectGrabbedRb;

    private Vector3 _lastMousePosition;

    [SerializeField] private float _dragStrength;
    [SerializeField] private float _minFollowerVelocity = 10f;

    void Awake()
    {
        _gridBuilder = FindObjectOfType<GridBuilder>(true);
        _gridTilePlacer = FindObjectOfType<GridTilePlacer>(true);
        _uIManager = FindObjectOfType<UIManager>(true);

        _mouseFollowerRb = _mouseFollower.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!_uIManager.IsBuildMode &&
            !_uIManager.IsEraserMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo/*, 100f,
                LayerMask.GetMask("Object Unstable"), QueryTriggerInteraction.UseGlobal*/))
            {
                Vector3 mouseFollowerPos = _mouseFollower.transform.position;
                Vector3 mousePos = new Vector3(
                    hitInfo.point.x, mouseFollowerPos.y, hitInfo.point.z);

                float distance = Vector3.Distance(mouseFollowerPos, mousePos);
                Vector3 mouseFollowerToMouseDirection =
                    (mousePos - mouseFollowerPos).normalized;

                if (distance > 2f)
                {
                    _mouseFollowerRb.velocity =
                    mouseFollowerToMouseDirection * _dragStrength * distance;
                }
                else if (distance <= 2f)
                {
                    _mouseFollowerRb.velocity = Vector3.zero;
                }

                if (_objectGrabbed)
                {

                    if (_mouseFollowerRb.velocity.magnitude > _minFollowerVelocity &&
                        Vector3.Distance(_objectGrabbed.transform.position,
                            new Vector3(
                                mousePos.x,
                                _objectGrabbed.transform.position.y,
                                mousePos.z)) > 1f)
                    {
                        Vector3 objToMouseDirection = (
                            new Vector3(
                                mousePos.x,
                                _objectGrabbed.transform.position.y,
                                mousePos.z) -
                            _objectGrabbed.transform.position).normalized;

                        _objectGrabbedRb.velocity =
                            objToMouseDirection *
                            _dragStrength *
                            (_mouseFollowerRb.velocity.magnitude + 1);
                    }
                    else if (_mouseFollowerRb.velocity.magnitude < _minFollowerVelocity &&
                        Vector3.Distance(_objectGrabbed.transform.position,
                            new Vector3(
                                mousePos.x,
                                _objectGrabbed.transform.position.y,
                                mousePos.z)) < 1f &&
                        _objectGrabbedRb.velocity.magnitude < 1f)
                    {
                        _objectGrabbed.transform.position = new Vector3(
                            mousePos.x,
                            _objectGrabbed.transform.position.y,
                            mousePos.z);
                    }
                }
            }
            // On object click and drag,
            // apply force on the clicked obj.
        }
        else if(_uIManager.IsBuildMode || _uIManager.IsEraserMode)
        {
            _mouseFollowerRb.velocity = Vector3.zero;
        }
    }

    void OnClick(InputValue value)
    {
        if (!_uIManager.IsBuildMode &&
            !_uIManager.IsEraserMode)
        {
            if (value.isPressed)
            {
                if (!_objectGrabbed)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f,
                        LayerMask.GetMask("Object Unstable"), QueryTriggerInteraction.UseGlobal))
                    {
                        _mouseFollowerRb.velocity = Vector3.zero;
                        _mouseFollower.transform.position = new Vector3(
                            hitInfo.transform.position.x,
                            _mouseFollower.transform.position.y,
                            hitInfo.transform.position.z);
                        _objectGrabbed = hitInfo.transform.gameObject;
                        _objectGrabbedRb = _objectGrabbed.GetComponent<Rigidbody>();
                        _objectGrabbedRb.velocity = Vector3.zero;

                        //_hitPoint = hitInfo.point;
                        // Think about whether you want to make it a tad more advanced
                        // by grab the objects from a custom pivot point every time.
                    }
                }
            }
            else if (!value.isPressed)
            {
                _objectGrabbed = null;
            }
        }
    }
}

//_mouseFollower.transform.rotation = Quaternion.LookRotation(
//    Vector3.RotateTowards(mouseFollowerPos, direction, _dragStrength, 0f));
