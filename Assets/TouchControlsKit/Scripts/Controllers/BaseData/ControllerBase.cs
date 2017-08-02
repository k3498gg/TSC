/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 ControllerBase.cs                     *
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

namespace TouchControlsKit
{       
    // ControllerBase
    public abstract class ControllerBase : MonoBehaviour
    {
        public EUpdateType updateType = EUpdateType.Update;

        internal ETouchPhase touchPhase = ETouchPhase.NoTouch;

        public RectTransform baseRect = null;
        public Image baseImage = null;

        [SerializeField]
        protected string myName = "NONAME_Controller";

        protected Color32 baseImageNativeColor;
        
        public bool broadcast = false;

        protected int touchId = -1;
        protected bool touchDown = false;
        protected bool touchDrag = false;

        [SerializeField]
        protected bool enable = true;

        [SerializeField]
        protected bool active = true;

        [SerializeField]
        protected bool visible = true;

        protected bool useDown, usePress, useUp, useClick, useAlways;

        private float touchPosMag, prevTouchPosMag;


        // MyName
        public string MyName
        {
            get { return myName; }
            set
            {
                if( myName == value ) 
                    return;

                if( value == string.Empty )
                {
                    Debug.LogError( "ERROR: Controller name for " + this.name + " cannot be empty" );
                    return;
                }

                myName = value;
                this.name = myName;
            }
        }

        // isEnable
        public bool isEnable
        {
            get { return enable; }
            set
            {
                if( enable == value )
                    return;

                enable = value;
                SetEnable();
            }
        }
        // Set Enable
        protected virtual void SetEnable()
        {
            this.enabled = ( enable && active );
            baseImage.enabled = enable;
        }

        // Active
        public bool isActive
        {
            get { return active; }
            set
            {
                if( active == value )
                    return;

                active = value;
                SetActive();
            }
        }
        // SetActive
        private void SetActive()
        {
            this.enabled = ( enable && active );

            if( !Application.isPlaying )
                return;

            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();

            if( active )
            {
                if( canvasGroup != null )
                    Destroy( canvasGroup );                
            }
            else
            {
                if( canvasGroup != null )
                {
                    canvasGroup.blocksRaycasts = false;
                }
                else
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    canvasGroup.blocksRaycasts = false;
                }
            }
        }

        // Visible
        public bool isVisible
        {
            get { return visible; }
            set
            {
                if( visible == value )
                    return;

                visible = value;
                SetVisible();
            }
        }
        // SetVisible
        protected virtual void SetVisible()
        {
            if( visible )
            {
                baseImage.color = baseImageNativeColor;
            }
            else
            {
                baseImageNativeColor = baseImage.color;
                baseImage.color = ( Color32 )Color.clear;
            }
        }


        // OnDisable
        void OnDisable()
        {
            if( Application.isPlaying && touchDown )
                ControlReset();
        }

        // ControlAwake
        public virtual void ControlAwake()
        {
            baseRect = this.GetComponent<RectTransform>();
            baseImage = this.GetComponent<Image>();

            SetActive();            
        }
        

        // Update
        void Update()
        {           
            if( updateType == EUpdateType.Update )
                EventsUpdate();
        }
        // Late Update
        void LateUpdate()
        {
            if( updateType == EUpdateType.LateUpdate )
                EventsUpdate();
        }
        // Fixed Update
        void FixedUpdate()
        {
            if( updateType == EUpdateType.FixedUpdate )
                EventsUpdate();
        }

        // EventsUpdate
        private void EventsUpdate()
        {
            PressHandler();
            UpdateTouchPhase();
        }
        // Update TouchPhase
        private void UpdateTouchPhase()
        {
            if( touchDown )
            {
                if ( touchPosMag == prevTouchPosMag )
                    touchPhase = ETouchPhase.Stationary;
                else
                    touchPhase = ETouchPhase.Moved;                
            }
            else
            {
                touchPhase = ETouchPhase.NoTouch;
            }

            prevTouchPosMag = touchPosMag;
        }


        // Update Position
        protected virtual void UpdatePosition( Vector2 touchPos )
        {
            touchPosMag = touchPos.magnitude;
        }

        // Control Reset
        protected virtual void ControlReset()
        {
            touchPhase = ETouchPhase.Ended;
            touchId = -1;
            touchDown = false;
            touchDrag = false;
        }


        // Down Handler
        protected abstract void DownHandler();

        // Press Handler
        protected abstract void PressHandler();

        // Up Handler
        protected abstract void UpHandler();

        // Click Handler
        protected abstract void ClickHandler();
    }
}