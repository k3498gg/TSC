/*********EXPERIMENTAL*CONTROLLER***********************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 Tilt.cs                               *
 * 													   *
 * Copyright(c): Victor Klepikov					   *
 * Support: 	 http://bit.ly/vk-Support			   *
 * 													   *
 * mySite:       http://vkdemos.ucoz.org			   *
 * myAssets:     http://u3d.as/5Fb                     *
 * myTwitter:	 http://twitter.com/VictorKlepikov	   *
 * 													   *
 *******************************************************/


using UnityEngine;

namespace TouchControlsKit
{
    public class TCKTilt : MonoBehaviour
    {
        public EUpdateType EUpdateType = EUpdateType.Update;

        [ Range( 1f, 10f ) ]
        public float sensitivity = 4f;

        [Range( 0f, 90f )]
        public float fullTiltAngle = 25f;

        [Range( -50f, 50f )]
        public float centreAngleOffset = 0f;
        

        public static float forwardAxis { get; private set; }
        public static float sidewaysAxis { get; private set; }


        
        // Update
        void Update()
        {
            if( EUpdateType == EUpdateType.Update )
                InputsUpdate();
        }
        // Late Update
        void LateUpdate()
        {
            if( EUpdateType == EUpdateType.LateUpdate )
                InputsUpdate();
        }
        // Fixed Update
        void FixedUpdate()
        {
            if( EUpdateType == EUpdateType.FixedUpdate )
                InputsUpdate();
        }

        // InputsUpdate
        private void InputsUpdate()
        {
            if( Input.acceleration != Vector3.zero )
            {
                float forwardAngle = Mathf.Atan2( Input.acceleration.x, -Input.acceleration.y ) * Mathf.Rad2Deg + centreAngleOffset;
                float sidewaysAngle = Mathf.Atan2( Input.acceleration.z, -Input.acceleration.y ) * Mathf.Rad2Deg + centreAngleOffset;
                forwardAxis = ( Mathf.InverseLerp( -fullTiltAngle, fullTiltAngle, forwardAngle ) * 2f - 1f ) * sensitivity;
                sidewaysAxis = ( Mathf.InverseLerp( -fullTiltAngle, fullTiltAngle, sidewaysAngle ) * 2f - 1f ) * sensitivity;
            }
            else
            {
                forwardAxis = 0f;
                sidewaysAxis = 0f;
            }
        }
    }
}