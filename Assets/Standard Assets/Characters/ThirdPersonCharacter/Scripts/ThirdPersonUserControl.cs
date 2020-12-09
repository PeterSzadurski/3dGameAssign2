using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private Transform m_Spine;
        public float _mouseSensitivity = 1;
        private void Start()
        {
            m_Spine = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine);
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }

        private void TurnTowardsMouse()
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            /*  Vector2 positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
              Vector2 mousePosition = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
              float angle = Vector2.Angle(positionOnScreen, mousePosition);
              transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));*/
            //m_anim.GetBoneTransform(HumanBodyBones.Spine).LookAt(mousePoint);
        }

        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }
        private void LateUpdate()
        {
            //Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            /*Vector3 positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 relativePos = mousePosition - transform.position;
            //  Debug.Log("relative: " + relativePos);
            // Quaternion rot = Quaternion.LookRotation(relativePos);
            float angle = Vector2.Angle(positionOnScreen, relativePos);

            Debug.Log("MouseX: " + Input.GetAxisRaw("Mouse X"));
            m_Spine.Rotate(Input.GetAxisRaw("Mouse X") * _mouseSensitivity, 0, 0);

            //m_Spine.rotation = Quaternion.Euler(new Vector3(m_Spine.rotation.eulerAngles.x, angle*5, m_Spine.rotation.eulerAngles.z));

            //  m_Spine.rotation = rot;
            */
            Vector3 torsoBoneLookAtOffset = new Vector3(0f, -90f, -100f);

            Vector3 mousePos = Input.mousePosition;
            //mousePos.y = -mousePos.y;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = -mousePos.z;
            m_Spine.LookAt(mousePos);
            m_Spine.Rotate(torsoBoneLookAtOffset);
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);
            TurnTowardsMouse();
            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
