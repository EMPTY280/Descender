using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Vector2 roomSize = new Vector2(20f, 15f);
    [SerializeField] Enemy[] enemyList;
    [SerializeField] BlockLock[] BlockLockList;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, roomSize);
    }
#endif

    private void Start()
    {
        enemyList = GetComponentsInChildren<Enemy>();
        BlockLockList = GetComponentsInChildren<BlockLock>();

        foreach (Enemy enemy in enemyList) { enemy.gameObject.SetActive(false); }
        foreach (BlockLock block in BlockLockList) { block.Unlock(); }
    }

    private void Update()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (enemy.gameObject.activeSelf)
                return;
        }

        EndRoom();
    }
    public void StartRoom()
    {
        foreach (Enemy enemy in enemyList)
            enemy.Revive();
        foreach (BlockLock block in BlockLockList)
            block.Lock();
    }

    public void EndRoom()
    {
        foreach (BlockLock block in BlockLockList)
            block.Unlock();
    }
}
