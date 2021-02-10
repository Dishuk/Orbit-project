using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GraphicAnimation : MonoBehaviour
{
    [Range(-600,600)]
    public int rotationSpeed;
    void Update()
    {
        transform.RotateAround(transform.parent.position, transform.forward, Time.deltaTime * rotationSpeed);
    }
}
