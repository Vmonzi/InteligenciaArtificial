using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPJ : MonoBehaviour
{
    [SerializeField] GameObject _pj;
    [SerializeField] float      _velocity;


    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v != 0)
        {
            var dir = new Vector3(0, 0, v).normalized;
            transform.position += (dir * _velocity) * Time.deltaTime;
            transform.forward = dir;
        }
        if (h != 0)
        {
            var dir = new Vector3(h, 0, 0).normalized;

            transform.position += (dir * _velocity) * Time.deltaTime;
            transform.forward = dir;
        }

    }



}
