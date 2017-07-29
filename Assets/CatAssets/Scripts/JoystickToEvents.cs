using UnityEngine;
using System.Collections;

public class JoystickToEvents : MonoBehaviour 
{
    
    public static void Do(Transform root, Transform camera, ref float speed, ref float direction)
    {
        CharacterController controller = root.GetComponent<CharacterController>();

        Vector3 rootDirection = root.forward;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
				
        Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

        // Get camera rotation.    

        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f; // kill Y
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

        // Convert joystick input in Worldspace coordinates
        Vector3 moveDirection = referentialShift * stickDirection;
				
		Vector2 speedVec =  new Vector2(horizontal, vertical);
		speed = Mathf.Clamp(speedVec.magnitude, 0, 100);



        if (speed > 0.01f) // dead zone
        {
            Vector3 axis = Vector3.Cross(rootDirection, moveDirection);
            direction = Vector3.Angle(rootDirection, moveDirection) / 180.0f * (axis.y < 0 ? -1 : 1);

            moveDirection = root.TransformDirection(moveDirection);
            moveDirection *= speed;
            
            
            
        }
        else
		{
            direction = 0.0f;

        }
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            speed = 0;
            direction = 0;
            moveDirection = Vector3.zero;
        }
        if((Input.GetKey(KeyCode.A) ))
        {
            root.transform.Rotate(Vector3.down * 100.0f* Time.deltaTime);
        }
        if ((Input.GetKey(KeyCode.D)))
        {
            root.transform.Rotate(Vector3.up * 100.0f * Time.deltaTime);
        }
        float gravity = 60.0F;
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);


    }
	
}
