/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKButton.cs                          *
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

namespace TouchControlsKit
{
    [RequireComponent( typeof( Image ) )]
    public class TCKButton : ControllerBase,
        IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
    {        
        private ActionEventHandler downHandler, pressHandler, upHandler, clickHandler;
        private ActionAlwaysHandler alwaysHandler;
        
        [SerializeField] public ActionEvent downEvent, pressEvent, upEvent, clickEvent;
        [SerializeField] public AlwaysActionEvent alwaysEvent;

        public bool swipeOut = false;

        [SerializeField]
        private Sprite normalsprite = null;
        public Sprite pressedSprite = null;

        public Color32 normalColor, pressedColor;

        public Sprite normalSprite
        {
            get { return normalsprite; }
            set
            {
                if( normalsprite == value )
                    return;

                normalsprite = value;
                baseImage.sprite = normalsprite;
            }
        }        
        
        private int 
            pressedFrame = -1, releasedFrame = -1, clickedFrame = -1;


        // isPRESSED
        internal bool isPRESSED {  get { return touchDown; } }
        // isDOWN
        internal bool isDOWN { get { return ( pressedFrame == Time.frameCount - 1 ); } }
        // isUP
        internal bool isUP { get { return ( releasedFrame == Time.frameCount - 1 ); } }
        // isCLICK
        internal bool isCLICK { get { return ( clickedFrame == Time.frameCount - 1 ); } }


        // Bind Action
        internal void BindAction( ActionEventHandler m_Handler, EActionEvent EActionEvent )
        {
            switch( EActionEvent )
            {
                case EActionEvent.Down:
                    useDown = true;
                    if( downHandler != m_Handler )
                        downHandler += m_Handler;
                    break;
                case EActionEvent.Press:
                    usePress = true;
                    if( pressHandler != m_Handler )
                        pressHandler += m_Handler;
                    break;
                case EActionEvent.Up:
                    useUp = true;
                    if( upHandler != m_Handler )
                        upHandler += m_Handler;
                    break;
                case EActionEvent.Click:
                    useClick = true;
                    if( clickHandler != m_Handler )
                        clickHandler += m_Handler;
                    break;
            }
        }
        // Bind Action
        internal void BindAction( ActionAlwaysHandler m_Handler )
        {
            useAlways = true;
            if( alwaysHandler != m_Handler )
                alwaysHandler += m_Handler;
        }
        
        // UnBind Action
        internal void UnBindAction( ActionEventHandler m_Handler, EActionEvent EActionEvent )
        {
            switch( EActionEvent )
            {
                case EActionEvent.Down:
                    if( downHandler == m_Handler )
                    {
                        downHandler -= m_Handler;
                        useDown = ( downHandler != null );
                    }
                    break;
                case EActionEvent.Press:
                    if( pressHandler == m_Handler )
                    {
                        pressHandler -= m_Handler;
                        usePress = ( pressHandler != null );
                    }
                    break;
                case EActionEvent.Up:
                    if( upHandler == m_Handler )
                    {
                        upHandler -= m_Handler;
                        useUp = ( upHandler != null );
                    }
                    break;
                case EActionEvent.Click:
                    if( clickHandler == m_Handler )
                    {
                        clickHandler -= m_Handler;
                        useClick = ( clickHandler != null );
                    }
                    break;
            }
        }
        // UnBind Action
        internal void UnBindAction( ActionAlwaysHandler m_Handler )
        {
            if( alwaysHandler == m_Handler )
            {
                alwaysHandler -= m_Handler;
                useAlways = ( clickHandler != null );
            }
        }

        
        // Update Position
        protected override void UpdatePosition( Vector2 touchPos )
        {
            base.UpdatePosition( touchPos );

            if( !touchDown )
            {
                touchDown = true;
                touchPhase = ETouchPhase.Began;
                pressedFrame = Time.frameCount;

                ButtonDown();

                // Broadcasting
                DownHandler();
            }            
        }
                

        // Button Down
        protected void ButtonDown()
        {
            baseImage.sprite = pressedSprite;
            baseImage.color = visible ? pressedColor : ( Color32 )Color.clear;
        }

        // Button Up
        protected void ButtonUp()
        {
            baseImage.sprite = normalSprite;
            baseImage.color = visible ? normalColor : ( Color32 )Color.clear;
        }

        // Control Reset
        protected override void ControlReset()
        {
            base.ControlReset();

            releasedFrame = Time.frameCount;
            ButtonUp();            

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
            {
                UpdatePosition( pointerData.position );
            }
        }

        // OnPointer Exit
        public void OnPointerExit( PointerEventData pointerData )
        {
            if( !swipeOut )            
                OnPointerUp( pointerData );
        }

        // OnPointer Up
        public void OnPointerUp( PointerEventData pointerData )
        {
            ControlReset();
        }

        // OnPointer Click
        public void OnPointerClick( PointerEventData pointerData )
        {
            clickedFrame = Time.frameCount;
            ClickHandler();
        }


        // Down Handler
        protected override void DownHandler()
        {
            if( useDown )
                downHandler.Invoke();

            if( broadcast )
                downEvent.Invoke();
        }

        // Press Handler
        protected override void PressHandler()
        {
            if( useAlways )
                alwaysHandler.Invoke( touchPhase );

            if( broadcast )
                alwaysEvent.Invoke( touchPhase );

            if( touchDown )
            {
                if( usePress )
                    pressHandler.Invoke();

                if( broadcast )
                    pressEvent.Invoke();
            }
        }

        // Up Handler
        protected override void UpHandler()
        {
            if( useUp )
                upHandler.Invoke();

            if( broadcast )
                upEvent.Invoke();
        }

        // Click Handler
        protected override void ClickHandler()
        {
            if( useClick )
                clickHandler.Invoke();

            if( broadcast )
                clickEvent.Invoke();
        }
    }
}