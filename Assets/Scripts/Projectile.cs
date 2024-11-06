using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.CompareTag("Enemy")) return;
        
        Destroy(other.gameObject);
        Destroy(this.gameObject);
    }
}
