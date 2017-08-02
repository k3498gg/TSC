/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKDPad.cs                            *
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
using UnityEngine.EventSystems;
using TouchControlsKit.Utils;

namespace TouchControlsKit
{
    [RequireComponent( typeof( Image ) )]
    public class TCKDPad : AxesBasedController,
        IPointerUpHandler, IPointerDownHandler, IDragHandler, IPointerClickHandler
    {
        public Sprite normalSprite, pressedSprite;
        public Color32 normalColor, pressedColor;

        private Color32 nvColor { get { return visible ? normalColor : ( Color32 )Color.clear; } }
        private Color32 pvColor { get { return visible ? pressedColor : ( Color32 )Color.clear; } }

        [SerializeField]
        private TCKDPadArrow[] myArrows = null;
        private Vector2 borderPosition = Vector2.zero;
        private float sizeX, sizeY;


        // Control Awake
        public override void ControlAwake()
        {
            base.ControlAwake();

            myArrows = this.GetComponentsInChildren<TCKDPadArrow>();

            foreach( TCKDPadArrow arrow in myArrows )
                arrow.DPadArrowAwake( normalSprite, nvColor );
        }

        // Set Enable
        protected override void SetEnable()
        {
            base.SetEnable();

            foreach( TCKDPadArrow arrow in myArrows )
                arrow.SetArrowEnable( enable );
        }

        // Set Visible
        protected override void SetVisible()
        { }


        // Update Position
        protected override void UpdatePosition( Vector2 touchPos )
        {
            if( !axisX.enabled && !axisY.enabled )
                return;

            base.UpdatePosition( touchPos );

            if( touchDown )
            {
                GetCurrentPosition( touchPos );

                currentDirection = currentPosition - defaultPosition;

                float borderSizeX = 0f;
                float borderSizeY = 0f;

                CalculateBorderSize( out borderSizeX, out borderSizeY );

                borderPosition = defaultPosition;
                borderPosition.x += currentDirection.normalized.x * borderSizeX;
                borderPosition.y += currentDirection.normalized.y * borderSizeY;

                float currentDistance = Vector2.Distance( defaultPosition, currentPosition );

                if( currentDistance > borderSizeX || currentDistance > borderSizeY )
                    currentPosition = borderPosition; //Debug.DrawLine( defaultPosition, currentPosition );

                float aX = 0f;
                float aY = 0f;

                foreach( TCKDPadArrow arrow in myArrows )
                {
                    if( arrow.CheckTouchPosition( currentPosition, sizeX, sizeY ) )
                    {
                        //ArrowDown
                        if( !arrow.isPressed )
                            arrow.SetArrowPhase( pressedSprite, pvColor, true );

                        if( arrow.arrowType == EArrowType.UP
                            || arrow.arrowType == EArrowType.DOWN )
                            aY = arrow.INDEX * sensitivity;

                        if( arrow.arrowType == EArrowType.RIGHT
                            || arrow.arrowType == EArrowType.LEFT )
                            aX = arrow.INDEX * sensitivity;
                    }
                    else
                    {
                        //ArrowUp
                        if( arrow.isPressed )
                            arrow.SetArrowPhase( normalSprite, nvColor, false );

                        if( arrow.arrowType == EArrowType.UP && arrow.INDEX == 0f )
                        {
                            foreach( TCKDPadArrow mArrow in myArrows )
                                if( mArrow.arrowType == EArrowType.DOWN && mArrow.INDEX == 0f )
                                    aY = mArrow.INDEX * sensitivity;
                        }

                        if( arrow.arrowType == EArrowType.RIGHT && arrow.INDEX == 0f )
                        {
                            foreach( TCKDPadArrow mArrow in myArrows )
                                if( mArrow.arrowType == EArrowType.LEFT && mArrow.INDEX == 0f )
                                    aX = mArrow.INDEX * sensitivity;
                        }
                    }
                }

                SetAxis( aX, aY );
            }
            else
            {
                touchDown = true;
                touchPhase = ETouchPhase.Began;

                UpdatePosition( touchPos );

                // Broadcasting
                DownHandler();
            }            
        }

        // GetCurrent Position
        protected void GetCurrentPosition( Vector2 touchPos )
        {
            if( axisX.enabled )
                currentPosition.x = GuiCamera.ScreenToWorldPoint( touchPos ).x;
            if( axisY.enabled )
                currentPosition.y = GuiCamera.ScreenToWorldPoint( touchPos ).y;

            sizeX = baseRect.sizeDelta.x / 2f;
            sizeY = baseRect.sizeDelta.y / 2f;
            defaultPosition = baseRect.position;
        }

        // Calculate BorderSize
        protected void CalculateBorderSize( out float calcX, out float calcY )
        {
            calcX = baseRect.sizeDelta.x / 6f;
            calcY = baseRect.sizeDelta.y / 6f;
        }


        // Control Reset
        protected override void ControlReset()
        {
            base.ControlReset();

            foreach( TCKDPadArrow arrow in myArrows )
                if( arrow.isPressed )
                    arrow.SetArrowPhase( normalSprite, nvColor, false );

            // Broadcasting
            UpHandler();
        }


        // OnPointer Down
        public void OnPointerDown( PointerEventData pointerData )
        {
            if( !touchDown )
            {
                touchId = pointerData.pointerId;
                UpdatePosition( pointerData.position );
            }
        }

        // OnDrag
        public void OnDrag( PointerEventData pointerData )
        {
            if( Input.touchCount >= touchId && touchDown )
                UpdatePosition( pointerData.position );
        }

        // OnPointer Up
        public void OnPointerUp( PointerEventData pointerData )
        {
            UpdatePosition( pointerData.position );
            ControlReset();
        }

        // OnPointer Click
        public void OnPointerClick( PointerEventData pointerData )
        {
            ClickHandler();
        }
    }
}