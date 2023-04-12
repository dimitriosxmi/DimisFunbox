using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    none = 0,
    metallic = 1
}

[RequireComponent(typeof(Rigidbody))]
public class ObjectBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _deathParticles;
    public GameObject DeathParticles
    {
        get { return _deathParticles; }
        set { _deathParticles = value; }
    }

    [SerializeField]
    private ObjectType _objectType = ObjectType.none;
    public ObjectType ObjectType
    {
        get { return _objectType; }
        set { _objectType = value; }
    }

    [SerializeField]
    private PhysicMaterial _objectPhysicMaterial;
    public PhysicMaterial ObjectPhysicMaterial
    {
        get { return _objectPhysicMaterial; }
        set { _objectPhysicMaterial = value; }
    }

    [SerializeField]
    private float _objectMass;
    public float ObjectMass
    {
        get { return _objectMass; }
        set { _objectMass = value; }
    }

    [SerializeField]
    private float _objectMagnetStrength;
    public float ObjectMagnetStrength
    {
        get { return _objectMagnetStrength; }
        set { _objectMagnetStrength = value; }
    }

    [SerializeField]
    private float _objectMagnetRange;
    public float ObjectMagnetRange
    {
        get { return _objectMagnetRange; }
        set { _objectMagnetRange = value; }
    }

    [SerializeField]
    private bool _objectKinematic;
    public bool ObjectKinematic
    {
        get { return _objectKinematic; }
        set { _objectKinematic = value; }
    }

    [SerializeField]
    private bool _objectGravity;
    public bool ObjectGravity
    {
        get { return _objectGravity; }
        set { _objectGravity = value; }
    }

    [SerializeField]
    private bool _objectMagnetic;
    public bool ObjectMagnet
    {
        get { return _objectMagnetic; }
        set { _objectMagnetic = value; }
    }

    [SerializeField]
    private Rigidbody _myRigidbody;
    public Rigidbody MyRigidbody
    {
        get { return _myRigidbody; }
        set { _myRigidbody = value; }
    }

    private GridBuilder _gridBuilder;
    public GridBuilder GridBuilderRef
    {
        get { return _gridBuilder; }
        set { _gridBuilder = value; }
    }

    private GridTilePlacer _gridTilePlacer;
    public GridTilePlacer GridTilePlacerRef
    {
        get { return _gridTilePlacer; }
        set { _gridTilePlacer = value; }
    }

    public virtual void Construct(ObjectBehaviour objectBehaviour)
    {        
        // Cache Values
        ObjectKinematic = objectBehaviour.ObjectKinematic;
        ObjectPhysicMaterial = objectBehaviour.ObjectPhysicMaterial;
        ObjectGravity = objectBehaviour.ObjectGravity;
        ObjectMass = objectBehaviour.ObjectMass;
        ObjectType = objectBehaviour.ObjectType;
        ObjectMagnet = objectBehaviour.ObjectMagnet;
        ObjectMagnetStrength = objectBehaviour.ObjectMagnetStrength;
        ObjectMagnetRange = objectBehaviour.ObjectMagnetRange;
        DeathParticles = objectBehaviour.DeathParticles;
        ///*MyRigidbody*/ = objectBehaviour.MyRigidbody;
        GridBuilderRef = objectBehaviour.GridBuilderRef;
        GridTilePlacerRef = objectBehaviour.GridTilePlacerRef;

        if (objectBehaviour.gameObject.layer == LayerMask.NameToLayer("Object Unstable"))
            gameObject.layer = LayerMask.NameToLayer("Object Unstable");
        else if (objectBehaviour.gameObject.layer == LayerMask.NameToLayer("Object Stable"))
            gameObject.layer = LayerMask.NameToLayer("Object Stable");

        _myRigidbody.isKinematic = ObjectKinematic;

        if (objectBehaviour.gameObject.GetComponent<BoxCollider>())
            objectBehaviour.gameObject.GetComponent<BoxCollider>().material =
                ObjectPhysicMaterial;
        else if (objectBehaviour.gameObject.GetComponent<CapsuleCollider>())
            objectBehaviour.gameObject.GetComponent<CapsuleCollider>().material =
                ObjectPhysicMaterial;
        else if (objectBehaviour.gameObject.GetComponent<SphereCollider>())
            objectBehaviour.gameObject.GetComponent<SphereCollider>().material =
                ObjectPhysicMaterial;
        else if (objectBehaviour.gameObject.GetComponent<MeshCollider>())
            objectBehaviour.gameObject.GetComponent<MeshCollider>().material =
                ObjectPhysicMaterial;

        _myRigidbody.useGravity = ObjectGravity;
        _myRigidbody.mass = ObjectMass;
        if (ObjectMass > 0) _myRigidbody.drag = 0.2f;
        if (ObjectMass > 0) _myRigidbody.drag = 0f;
        _myRigidbody.angularDrag = 0f;
        _myRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;

        //Apply cahed Values
        switch (ObjectType)
        {
            case ObjectType.none:
                if (GetComponent<MetalicBehaviour>())
                    Destroy(GetComponent<MetalicBehaviour>());
                break;
            case ObjectType.metallic:
                if (!GetComponent<MetalicBehaviour>())
                {
                    MetalicBehaviour script = gameObject.AddComponent<MetalicBehaviour>();
                    script.Construct(this);
                    SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
                    sphereCollider.isTrigger = true;
                    sphereCollider.radius = ObjectMagnetRange;
                    script.RangeCollider = sphereCollider;
                }
                break;
            default:
                break;
        }
    }

    void Awake()
    {
        _gridBuilder = FindObjectOfType<GridBuilder>(true);
        _gridTilePlacer = FindObjectOfType<GridTilePlacer>(true);
        _myRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if (_objectType == ObjectType.metalic)
        //{
        //    MetalicBehaviour script = gameObject.AddComponent<MetalicBehaviour>();
        //    script.Construct(this);
        //}
    }

    public void KillMe()
    {
        _gridTilePlacer.removePlacedTileFromList(this);

        if (transform.parent != _gridBuilder.GridPlacedTilesParent)
        {
            Destroy(transform.parent.gameObject, UnityEngine.Random.Range(0.2f, 5f));
        }
        else
        {
            Destroy(gameObject, UnityEngine.Random.Range(0.2f, 5f));
        }
    }

    private void OnDestroy()
    {
        if (DeathParticles)
        {
            GameObject particle = Instantiate(
            DeathParticles,
            transform.position,
            Quaternion.identity);
            particle.transform.localScale = new Vector3(
                Mathf.Clamp(transform.localScale.x * 0.5f, 1f, 2.5f),
                Mathf.Clamp(transform.localScale.y * 0.5f, 1f, 2.5f),
                Mathf.Clamp(transform.localScale.z * 0.5f, 1f, 2.5f));
        }
    }
}
