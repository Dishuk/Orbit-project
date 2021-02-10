using UnityEngine;
using System.Collections;
using UnityEngine.U2D;
using Unity.Mathematics;

public class Circle : MonoBehaviour
{
    [SerializeField] private Material whiteDiffuseMat;

    private int _segments = 100;
    private float radius;
    private float _lineWidth = 0.05f;

    private LineRenderer _line;
    private Orbit _orbit;

    private Camera cam;
    private float baseModifier;
    private float lastCameraSize;

    


    private void Awake()
    {
        _orbit = GetComponent<Orbit>();
        _line = GetComponent<LineRenderer>();
        _line.material = whiteDiffuseMat;
    }
   
    private void Update()
    {
        CalculateOrbit();
        if (cam.orthographicSize != lastCameraSize)
        {
            CalculateLineWidth();
        }
    }

    private void CalculateOrbit() 
    {
        radius = (_orbit.mainOrbitGameObject.transform.position - Vector3.zero).magnitude;
        cam = Camera.main;

        baseModifier = _lineWidth / cam.orthographicSize;

        _line.positionCount = _segments + 1;
        _line.useWorldSpace = false;
        CreatePoints();
    }

    private void CalculateLineWidth()
    {
        _line.startWidth = cam.orthographicSize * baseModifier;
        _line.endWidth = _line.startWidth;
        lastCameraSize = cam.orthographicSize;
    }

    private void CreatePoints()
    {
        _line.SetPositions(new Vector3[0]);

        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (_segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            _line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / _segments);
        }
    }
}
