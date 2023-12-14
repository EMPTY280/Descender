using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLock : MonoBehaviour
{
    public void Lock()
    {
        gameObject.SetActive(true);
    }

    public void Unlock()
    {
        gameObject.SetActive(false);
    }
}
