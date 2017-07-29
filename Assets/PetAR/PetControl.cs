using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class PetControl : MonoBehaviour {
	private float currScale;
	private float scaleMax = 5f;
	private float scaleMin = 1f;

	private Animator anim;
	private Rigidbody rb;
	private int speedHash = Animator.StringToHash("speed");

	void Awake() {
		Joystick.JoystickMoved += UpdateMove;
	}

	void OnDestroy() {
		Joystick.JoystickMoved -= UpdateMove;
	}

	// Use this for initialization
	void Start () {
		currScale = Mathf.Clamp (transform.localScale.x, scaleMin, scaleMax);
		anim = gameObject.GetComponent<Animator> ();
		rb = gameObject.GetComponent<Rigidbody> ();
	}

	public void SetPosition() {
		// Project from the middle of the screen to look for a hit point on the detected surfaces.
		var screenPosition = Camera.main.ScreenToViewportPoint (new Vector2 (Screen.width / 2f, Screen.height / 2f));
		ARPoint pt = new ARPoint {
			x = screenPosition.x,
			y = screenPosition.y
		};

		// Try to hit within the bounds of an existing AR plane.
		List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (
			                                   pt, 
			                                   ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);

		if (hitResults.Count > 0) { // If a hit is found, set the position and reset the rotation.
			transform.rotation = Quaternion.Euler (Vector3.zero);
			transform.position = UnityARMatrixOps.GetPosition (hitResults[0].worldTransform);
		}
	}

	public void Jump() {
		rb.AddForce (Vector3.up * 80f);
	}

	public void UpScale() {
		if (Mathf.Approximately(currScale,scaleMax)) return;
		currScale += 1f;
		transform.localScale = new Vector3 (currScale, currScale, currScale);
	}

	public void DownScale() {
		if (Mathf.Approximately(currScale,scaleMin)) return;
		currScale -= 1f;
		transform.localScale = new Vector3 (currScale, currScale, currScale);
	}

	private void UpdateMove (Vector2 input) {
		if (input.Equals (Vector2.zero)) {
			anim.SetFloat (speedHash, 0f);
			return;
		}

		Vector3 inputAxes = new Vector3 (input.x, 0, input.y);
		anim.SetFloat (speedHash, inputAxes.magnitude); // Update the animator parameter for speed based on the joystick.
		SetLookDirection (inputAxes); // Set the cat to look in the correct direction

		// Move the cat, the animator will handle triggering the correct animations.
		transform.localPosition += (transform.forward * inputAxes.magnitude * Time.deltaTime);
	}

	void SetLookDirection(Vector3 inputAxes) {
		// Get the camera's y rotation, then rotate inputAxes by the rotation to get up/down/left/right according to the camera
		Quaternion yRotation = Quaternion.Euler (0, Camera.main.transform.rotation.eulerAngles.y, 0);
		Vector3 lookDirection = (yRotation * inputAxes).normalized;
		transform.rotation = Quaternion.LookRotation (lookDirection);
	}
}
