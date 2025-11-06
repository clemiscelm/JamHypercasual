using System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dummy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject[] _headSkins;
    
    private Collider[] _colliders;
    private Rigidbody[] _rbs;
    private Transform[] _transforms;
    private Quaternion[] _rotations;
    public int IDSkinChose = 0;
    
    private void Awake()
    {
        _animator.enabled = false;
        _colliders = GetComponentsInChildren<Collider>();
        _rbs = GetComponentsInChildren<Rigidbody>();
        _transforms = GetComponentsInChildren<Transform>();
        _rotations = new Quaternion[_transforms.Length];
        for (int i = 0; i < _transforms.Length; i++)
        {
            _rotations[i] = _transforms[i].rotation;
        }
    }

    private void Start()
    {
        SetSkin();
    }

    public void SetSkin()
    {
        if(GameManager.Instance.AvailableSkins.Length <= 0 || GameManager.Instance.IDskinChose <= 0)
            return;
        
        foreach (GameObject head in _headSkins)
        {
            head.SetActive(false);
        }
        
       
        _headSkins[GameManager.Instance.IDskinChose - 1].SetActive(true);
    }

    [Button]
    public void ResetDummy()
    {
        _animator.enabled = true;
        foreach (Collider col in _colliders)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rb in _rbs)
        {
            rb.isKinematic = true;
        }
        
        for (int i = 0; i < _transforms.Length; i++)
        {
            _transforms[i].rotation = _rotations[i];
        }
        SetSkin();
    }

    [Button]
    public void Ragdoll(float force)
    {
        
        _animator.enabled = false;
        foreach (Collider col in _colliders)
        {
            col.enabled = true;
        }

        foreach (Rigidbody rb in _rbs)
        {
            rb.isKinematic = false;
        }

        float direction = Random.Range(0, 2) == 0 ? 1 : -1;
        Vector3 addForce = new Vector3(.2f * direction,1,-1) * force;
        _rb.AddForce(addForce);
    }
}