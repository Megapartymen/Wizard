using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCollisionEffect : MonoBehaviour
{
    [SerializeField] private GameObject _collisionEffect;
    
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_collisionEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
        
        if (collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            rigidbody.useGravity = true;
    }
}
