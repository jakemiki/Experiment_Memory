using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public CharacterController characterController;
    public new Transform camera;
    public float speed = 1;

    private Vector2 _movement;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();

        transform.rotation = Quaternion.Euler(0, camera.rotation.eulerAngles.y, 0);

        characterController.Move(Physics.gravity * Time.deltaTime);
        characterController.Move((transform.forward * _movement.y + transform.right * _movement.x).normalized * speed * Time.deltaTime);
    }

    void ProcessInputs()
    {
        _movement.x = 0;
        _movement.y = 0;

        if (Input.GetKey(KeyCode.W))
        {
            _movement.y = 1;
        } else if (Input.GetKey(KeyCode.S))
        {
            _movement.y = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _movement.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _movement.x = -1;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(camera.position, camera.forward, out var hit, 1))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
}
