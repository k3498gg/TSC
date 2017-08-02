/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKDPadArrow.cs                       *
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
using UnityEngine.UI;
using TouchControlsKit.Utils;

namespace TouchControlsKit
{
    [RequireComponent( typeof( Image ) )]
    public class TCKDPadArrow : MonoBehaviour
    {
        public EArrowType arrowType = EArrowType.none;

        [SerializeField]
        private RectTransform baseRect = null;
        [SerializeField]
        private Image baseImage = null;

        internal bool isPressed { get; private set; }

        internal float INDEX { get; private set; }


        // DPadArrowAwake
        internal void DPadArrowAwake( Sprite sprite, Color color )
        {
            baseRect = this.GetComponent<RectTransform>();
            baseImage = this.GetComponent<Image>();
            baseImage.sprite = sprite;
            baseImage.color = color;
        }


        // SetArrowColor
        internal void SetArrowEnable( bool value )
        {
            baseImage.enabled = value;
        }

        // SetArrowActive
        internal void SetArrowPhase( Sprite sprite, Color color, bool pressed )
        {
            baseImage.sprite = sprite;
            baseImage.color = color;
            isPressed = pressed;
        }
        
        // CheckBoolPosition
        private bool CheckBoolPosition( Vector2 touchPos, float sizeX, float sizeY )
        {
            float halfSizeX = baseRect.sizeDelta.x / 2f;
            float halfSizeY = baseRect.sizeDelta.x / 2f;

            switch( arrowType )
            {
                case EArrowType.UP:
                case EArrowType.DOWN:
                    if( touchPos.x < baseRect.position.x + sizeX / 2f
                    && touchPos.y < baseRect.position.y + halfSizeY / 2f //maxY
                    && touchPos.x > baseRect.position.x - sizeX / 2f
                    && touchPos.y > baseRect.position.y - halfSizeY / 2f ) // minY
                    {
                        return true;
                    }
                    break;

                case EArrowType.RIGHT:
                case EArrowType.LEFT:
                    if( touchPos.x < baseRect.position.x + halfSizeX / 2f //maxX
                    && touchPos.y < baseRect.position.y + sizeY / 2f
                    && touchPos.x > baseRect.position.x - halfSizeX / 2f //minX
                    && touchPos.y > baseRect.position.y - sizeY / 2f )
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        // CheckBoolPosition
        internal bool CheckTouchPosition( Vector2 touchPos, float sizeX, float sizeY )
        {
            if( CheckBoolPosition( touchPos, sizeX, sizeY ) )
            {
                switch( arrowType )
                {
                    case EArrowType.UP:
                    case EArrowType.RIGHT:
                        INDEX = 1f;
                        break;

                    case EArrowType.DOWN:
                    case EArrowType.LEFT:
                        INDEX = -1f;
                        break;

                    case EArrowType.none:
                        Debug.LogError( "ERROR: Arrow type " + gameObject.name + " is not assigned!" );
                        INDEX = 0f;
                        return false;
                }
                return true;
            }
            else
            {
                INDEX = 0f;
                return false;
            }
        }
    }
}



/*
Debug.DrawLine( new Vector2( myData.baseRect.position.x + sizeX / 2f, myData.baseRect.position.y + halfSizeY / 2f ),
                new Vector2( myData.baseRect.position.x - sizeX / 2f, myData.baseRect.position.y - halfSizeY / 2f ), Color.red );
 
Debug.DrawLine( new Vector2( myData.baseRect.position.x + halfSizeX / 2f, myData.baseRect.position.y + sizeY / 2f ),
                new Vector2( myData.baseRect.position.x - halfSizeX / 2f, myData.baseRect.position.y - sizeY / 2f ), Color.green );
*/