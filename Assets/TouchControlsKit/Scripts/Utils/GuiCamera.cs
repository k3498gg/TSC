/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 GuiCamera.cs                          *
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

namespace TouchControlsKit.Utils
{
    [RequireComponent( typeof( Camera ) )]
    public sealed class GuiCamera : MonoBehaviour
    {
        public static Camera m_Camera { get; private set; }
        public static Transform m_Transform { get; private set; }

        // Awake
        void Awake()
        {
            m_Transform = transform;
            m_Camera = this.GetComponent<Camera>();
        }                 

        // ScreenToWorldPoint
        public static Vector2 ScreenToWorldPoint( Vector2 position )
        {
            return m_Camera.ScreenToWorldPoint( position );
        }
    }
}