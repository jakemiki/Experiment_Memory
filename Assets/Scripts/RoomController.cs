using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomPosition
{
    CODE,
    DOOR,
}

public class RoomController : MonoBehaviour
{
    public Transform doorSide;
    public Transform codeSide;
    public Transform stairs;
    public Transform walls;
    public RoomPosition roomPosition;
    public Keypad keypad;
    public GameObject door;

    public string code;
    public TMPro.TextMeshPro codeMesh;

    private float _moveStep;
    private Coroutine _coroutine;

    // Start is called before the first frame update
    void Start()
    {
        _moveStep = Mathf.Abs(doorSide.position.y - codeSide.position.y);
    }

    public void MoveToCodeSide()
    {
        if (roomPosition == RoomPosition.CODE || !enabled)
        {
            return;
        }
        roomPosition = RoomPosition.CODE;
        Debug.Log(roomPosition);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
            return;
        }

        _coroutine = StartCoroutine(DelayedAction(() =>
        {
            RotateStairs();

            var pos = doorSide.position;
            pos.y += _moveStep * 2;
            doorSide.position = pos;

            AlignWalls();
        }));
    }

    public void MoveToDoorSide()
    {
        if (roomPosition == RoomPosition.DOOR || !enabled)
        {
            return;
        }
        roomPosition = RoomPosition.DOOR;
        Debug.Log(roomPosition);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
            return;
        }

        _coroutine = StartCoroutine(DelayedAction(() =>
        {
            RotateStairs();

            var pos = codeSide.position;
            pos.y += _moveStep * 2;
            codeSide.position = pos;

            AlignWalls();
        }));
    }

    public void SetCode(string newCode)
    {
        code = newCode;
        codeMesh.text = code;
        keypad.charLimit = code.Length;

    }

    public void SubmitCode(string submitted)
    {
        if (!enabled)
        {
            return;
        }

        if (submitted != code)
        {
            keypad.ShowError();
            return;
        }

        GameController.instance.Advance();

    }

    void RotateStairs()
    {
        stairs.Rotate(0, 180, 0);
        var pos = stairs.position;
        pos.y += _moveStep;
        stairs.position = pos;
    }

    void AlignWalls()
    {
        var pos = walls.position;
        pos.y = Mathf.Min(doorSide.position.y, codeSide.position.y);
        walls.position = pos;
    }

    IEnumerator DelayedAction(System.Action action)
    {
        var done = false;
        while (!done)
        {
            if (!enabled)
            {
                break;
            }

            yield return new WaitForFixedUpdate();

            if (!stairs.GetComponent<Renderer>().isVisible)
            {
                action.Invoke();
                done = true;
                _coroutine = null;
            }
        }
    }
}
