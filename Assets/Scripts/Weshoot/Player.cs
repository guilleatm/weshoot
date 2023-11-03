using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Weshoot.Input;

namespace Weshoot
{

	// First in execution order
	public class Player : MonoBehaviour
	{
		public static DefaultInput input;
		[SerializeField] PlayerControllerSettings controllerSettings;
		Rigidbody rb;

		void Awake()
		{
			Player.input = new DefaultInput();
			Player.input.Action.Enable();

			SwitchCursorMode();
		}

		void OnEnable()
		{
			Player.input.Action.Escape.performed += OnEscape;
		}

		void OnDisable()
		{
			Player.input.Action.Escape.performed -= OnEscape;
		}

		void Start()
		{
			rb = GetComponent<Rigidbody>();
		}

		void Update()
		{
			ManageMovement();
			ManageRotation();
		}

		void ManageMovement()
		{
			Vector2 movementDirection = Player.input.Action.Move.ReadValue<Vector2>().normalized;
			Vector3 movementDirection_v3 = transform.rotation * new Vector3(movementDirection.x, 0, movementDirection.y);
			//rb.MovePosition(transform.position + movementDirection_v3 * controllerSettings.speed * Time.deltaTime);
			transform.position += movementDirection_v3 * controllerSettings.speed * Time.deltaTime;
		}

		void ManageRotation()
		{
			Vector2 deltaRotation = Player.input.Action.Rotate.ReadValue<Vector2>();

			transform.localRotation *=  Quaternion.AngleAxis(deltaRotation.x * controllerSettings.viewSensitivity.x, Vector3.up);
			Camera.main.transform.localRotation *= Quaternion.AngleAxis(deltaRotation.y * controllerSettings.viewSensitivity.y, Vector3.left);
		}

		void OnEscape(InputAction.CallbackContext context) => SwitchCursorMode();
		void SwitchCursorMode()
		{
			switch(Cursor.lockState)
			{
				case CursorLockMode.None:
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
					break;
				case CursorLockMode.Locked:
				default:
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					break;
			}
		}
	}


	[System.Serializable]
	class PlayerControllerSettings
	{
		public float speed;
		public Vector2 viewSensitivity;
	}

}