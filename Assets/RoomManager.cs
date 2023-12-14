using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Room currentRoom = null;
    [SerializeField] Room nextRoom = null;
    [SerializeField] Player player = null;
    [SerializeField] Room[] rooms = null;

    bool changingRoom = false;

    private void Awake()
    {
        SelectRandomRoom();
    }

    private void Update()
    {
        if (changingRoom)
        {
            float dist = Mathf.Abs(nextRoom.transform.position.y);
            dist = Mathf.Max(dist * 15f, 1f);

            Vector3 newPos1 = Vector3.MoveTowards(currentRoom.transform.position,
                new Vector3(0, 15f, 0f), dist * Time.deltaTime);
            currentRoom.transform.position = newPos1;

            Vector3 newPos2 = Vector3.MoveTowards(nextRoom.transform.position,
                new Vector3(0, 0f, 0f), dist * Time.deltaTime);
            nextRoom.transform.position = newPos2;

            Vector3 playerPos = player.transform.position;
            Vector3 newPos3 = Vector3.MoveTowards(new Vector3(playerPos.x, playerPos.y, 0f),
                new Vector3(playerPos.x, 7.5f, 0f), dist * Time.deltaTime);
            player.transform.position = newPos3;

            if (nextRoom.transform.position == Vector3.zero)
            {
                changingRoom = false;
                currentRoom = nextRoom;
                SelectRandomRoom();
            }
        }
    }

    public void NextRoom()
    {
        changingRoom = true;
        nextRoom.StartRoom();
        BulletPool.Instance.KillAll();
    }

    private void SelectRandomRoom()
    {
        do
        {
            nextRoom = rooms[Random.Range(0, rooms.Length)];
        }
        while (nextRoom == currentRoom);
        nextRoom.transform.position = new Vector3(0, -15, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NextRoom();
            player.ResetVSpeed();
        }
    }
}
