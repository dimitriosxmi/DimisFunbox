using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalicBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _objectsInRange = new();
    [SerializeField]
    private Transform _closestMetalicObject;
    //[SerializeField]
    //private float _distanceToClosest = float.MaxValue;
    [SerializeField]
    private SphereCollider _rangeCollider;
    public SphereCollider RangeCollider
    {
        get { return _rangeCollider; }
        set { _rangeCollider = value; }
    }

    ObjectBehaviour _myObjectBehaviour;

    private GridBuilder _gridBuilder;
    private GridTilePlacer _gridTilePlacer;

    public void Construct(ObjectBehaviour objectBehaviour)
    {
        _myObjectBehaviour = objectBehaviour;
        _myObjectBehaviour.MyRigidbody.WakeUp();
    }

    void FixedUpdate()
    {
        if (!_myObjectBehaviour.ObjectKinematic &&
            _myObjectBehaviour.ObjectMagnet)
            MagneticBehaviour();
    }

    private void MagneticBehaviour()
    {
        GetClosestMetalicObject();

        if (_closestMetalicObject && 
            _objectsInRange.Count > 0)
        {
            float thisToClosestDistance = Vector3.Distance(transform.position,
                _closestMetalicObject.position);
            float distanceBasedCalculation = Mathf.Clamp01(
                ((_rangeCollider.radius - (_rangeCollider.radius * 0.5f)) / 
                thisToClosestDistance) - 0.25f);

            
            //Debug.Log((_rangeCollider.radius / _rangeCollider.radius) -
            //    (thisToClosestDistance / thisToClosestDistance));

            Vector3 magnetToMetallicObjectDirection =
                (_closestMetalicObject.position - transform.position).normalized;
            //print(_myObjectBehaviour.MyRigidbody);
            _myObjectBehaviour.MyRigidbody.AddForce(magnetToMetallicObjectDirection *
                _myObjectBehaviour.ObjectMagnetStrength * _myObjectBehaviour.ObjectMass *
                distanceBasedCalculation);
            Vector3.ClampMagnitude(_myObjectBehaviour.MyRigidbody.velocity, 30f);
        }
    }

    private void GetClosestMetalicObject()
    {
        if (_closestMetalicObject)
        {
            foreach (Transform metalicObject in _objectsInRange)
            {
                if (!metalicObject)
                {
                    _objectsInRange.Remove(metalicObject);
                    return;
                }
                if (Vector3.Distance(transform.position, metalicObject.position) <
                    Vector3.Distance(transform.position, _closestMetalicObject.position))
                {
                    _closestMetalicObject = metalicObject;
                }
            }
        }
        else if (!_closestMetalicObject)
        {
            if (_objectsInRange.Count > 0)
            {
                if (_objectsInRange[0])
                {
                    _closestMetalicObject = _objectsInRange[0];
                }
                else if (!_objectsInRange[0])
                {
                    _objectsInRange.RemoveAt(0);
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_objectsInRange.Count < 3 &&
            other.GetComponent<ObjectBehaviour>() &&
            other.GetComponent<ObjectBehaviour>().ObjectType == ObjectType.metallic &&
                other.gameObject != gameObject &&
                !_objectsInRange.Contains(other.transform))
        {
            _objectsInRange.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ObjectBehaviour>() &&
            other.GetComponent<ObjectBehaviour>().ObjectType == ObjectType.metallic &&
            other.gameObject != gameObject)
        {
            _objectsInRange.Remove(other.transform);
        }
    }
}
