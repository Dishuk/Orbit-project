using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class DEBUG : MonoBehaviour
{
    public CharacterMovement character;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            character.ChangeCharacterOrbit(1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            character.ChangeCharacterOrbit(-1);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 5;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Time.timeScale = 1;
        }
    }
}
