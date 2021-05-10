using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Transform evenRoomSpawnPoint;
    public Transform oddRoomSpawnPoint;

    public RoomController room;
    public GameObject roomPrefab;
    public Text roomText;

    public int roomNumber = 1;
    public int codeLength = 2;
    public int changeCount = 0;

    private int _changesLeft = 0;
    private GameObject _prevRoom;


    void Start()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        PrepareRoom();
    }

    public void PrepareRoom()
    {
        _changesLeft = changeCount;

        var code = "";

        for (var i = 0; i < codeLength; i++)
        {
            code += Random.Range(0, 9).ToString();
        }

        room.SetCode(code);
        StartCoroutine(ShowRoomNumber());
    }

    public void Advance()
    {
        if (_changesLeft > 0 && Random.Range(0f, 1f) > (0.3f + _changesLeft * 0.05f))
        {
            _changesLeft--;

            var randomIndex = Random.Range(0, room.code.Length);
            var randomChar = room.code[randomIndex];

            switch (randomChar)
            {
                case '0':
                    randomChar = '8';
                    break;
                case '1':
                    randomChar = '7';
                    break;
                case '2':
                    randomChar = '5';
                    break;
                case '3':
                    randomChar = '8';
                    break;
                case '4':
                    randomChar = '9';
                    break;
                case '5':
                    randomChar = '2';
                    break;
                case '6':
                    randomChar = '9';
                    break;
                case '7':
                    randomChar = '1';
                    break;
                case '8':
                    randomChar = '3';
                    break;
                case '9':
                    randomChar = '6';
                    break;
                default:
                    break;
            }

            var sb = new StringBuilder(room.code);
            sb[randomIndex] = randomChar;
            room.SetCode(sb.ToString());
            room.keypad.ShowError();
        }
        else
        {
            room.door.SetActive(false);
            roomNumber++;
            room.enabled = false;
            var spawnPoint = evenRoomSpawnPoint;
            if (roomNumber % 2 == 0)
            {
                codeLength++;
            } 
            else
            {
                spawnPoint = oddRoomSpawnPoint;
                changeCount++;
            }

            if (_prevRoom)
            {
                Destroy(_prevRoom);
            }

            // spawn new room
            _prevRoom = room.gameObject;
            var newRoom = Instantiate(roomPrefab, spawnPoint.position + new Vector3(0, room.doorSide.transform.position.y, 0), spawnPoint.rotation);
            room = newRoom.GetComponent<RoomController>();

            // prepare room
            PrepareRoom();
        }
    }

    IEnumerator ShowRoomNumber()
    {
        yield return new WaitForSeconds(2f);
        roomText.text = "Room " + roomNumber;
        roomText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        roomText.gameObject.SetActive(false);

    }
}
