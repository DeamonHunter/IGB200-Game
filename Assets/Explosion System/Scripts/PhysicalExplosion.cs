using UnityEngine;
using System.Collections;
 
public class PhysicalExplosion : MonoBehaviour 
{
    public float Radius;// explosion radius
    public float Force;// explosion forse
    void Update () 
    {
        Destroy(gameObject,0.2f);// destroy explosion
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,Radius);
    }
}