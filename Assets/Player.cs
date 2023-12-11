using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum HDirec { LEFT, RIGHT }
    enum VDirec { DOWN, MID, UP }


    HDirec hDirection = HDirec.RIGHT;
    [SerializeField] VDirec vDirection = VDirec.MID;

    [SerializeField] Transform gun;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            vDirection = VDirec.UP;
        else if (Input.GetKey(KeyCode.DownArrow))
            vDirection = VDirec.DOWN;
        else
            vDirection = VDirec.MID;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            hDirection = HDirec.RIGHT;
            Vector3 newScale = transform.localScale;
            newScale.x = 1;
            transform.localScale = newScale;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            hDirection = HDirec.LEFT;
            Vector3 newScale = transform.localScale;
            newScale.x = -1;
            transform.localScale = newScale;
        }

            gun.rotation = Quaternion.Euler(0, 0, ((int)vDirection - 1) * 90 * transform.localScale.x);
    }
}
