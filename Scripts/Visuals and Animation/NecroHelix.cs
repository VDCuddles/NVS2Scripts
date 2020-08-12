using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroHelix : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, -120.0f * Time.deltaTime, 0, Space.Self);
    }
}
