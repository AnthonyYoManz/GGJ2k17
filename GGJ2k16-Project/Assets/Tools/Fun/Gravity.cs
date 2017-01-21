using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Use a SphereCollider as a trigger to map 
/// out the influence range of this body.
/// Increase radius to increase range!
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Gravity : MonoBehaviour
{ 
    /// <summary>
    /// Mass of body. More mass = bigger pull.
    /// </summary>
    public float    m_mass               = 1000;
    /// <summary>
    /// For real physics this should be 6.67408 * Mathf.Pow(10, -11) 
    /// but that doesn't work so well with such small planets. 
    /// A value of 1 works fine!
    /// </summary>
    public float    m_gravitationalConstant = 1;


    void OnTriggerStay(Collider other)
    {
  
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        { 
            /*these are mostly here for helping clarity*/
            double m1 = m_mass;
            double m2 = rb.mass;
            double G = m_gravitationalConstant;
            Vector3 vec = (transform.position - rb.transform.position);
            /*calc force of gravitational pull*/
            Vector3 force = vec.normalized * (float)(m1 * m2 * G / vec.sqrMagnitude);
            rb.AddForce(force);
        }
    }
  
}