 /// <summary>
/// 
/// </summary>

using UnityEngine;
using System;
using System.Collections;
  
[RequireComponent(typeof(Animator))]  

//Name of class must be name of file as well

public class LocomotionPlayer : MonoBehaviour {

    protected Animator animator;

    private float speed = 0;
    private float direction = 0;
    private Locomotion locomotion = null;
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.
    
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    // Use this for initialization
    void Start () 
	{
        floorMask = LayerMask.GetMask("Defalut");
        animator = GetComponent<Animator>();
        locomotion = new Locomotion(animator);
    }
    
	void Update () 
	{
        if (animator && Camera.main)
		{
            JoystickToEvents.Do(transform,Camera.main.transform, ref speed, ref direction);
            locomotion.Do(this.gameObject, speed*10 , direction * 180);
		}
        
        {
#if !MOBILE_INPUT
            // Create a ray from the mouse cursor on screen in the direction of the camera.0
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - transform.position;
                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                this.transform.rotation = newRotatation;
                //playerRigidbody.MoveRotation(newRotatation);
            }
#else

			Vector3 turnDir = new Vector3(Input.GetAxisRaw("Mouse X") , 0f , Input.GetAxisRaw("Mouse Y"));

            if (turnDir != Vector3.zero)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                this.transform.rotation = newRotatation;
            }
#endif
        }
    }
}
