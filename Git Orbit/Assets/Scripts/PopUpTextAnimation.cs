using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextAnimation : MonoBehaviour
{
    private void Update()
    {
        transform.position += new Vector3(0, 2 * Time.deltaTime, 0);
        if (transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
        transform.localScale -= Vector3.one * Time.deltaTime * 2;
    }
}
