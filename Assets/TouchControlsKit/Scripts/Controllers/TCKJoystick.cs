/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKJoystick.cs                        *
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
using m_IEnumerator = System.Collections.IEnumerator;

namespace TouchControlsKit
{
    /// <summary>
    /// isStatic = true;  - Switches a joystick in a static mode, in which it is only at the specified position.
    /// isStatic = false; - Switches a joystick in the dynamic mode, in this mode, it operates within the touch zone.
    /// </summary>

    [RequireComponent(typeof(Image))]
    public class TCKJoystick : AxesBasedController,
        IPointerUpHandler, IPointerDownHandler, IDragHandler, IPointerClickHandler
    {
        public Image joystickImage, joystickBackgroundImage;
        public RectTransform joystickRT, joystickBackgroundRT;

        [SerializeField]
        private bool isStatic = true;

        public float borderSize = 5.85f;
        private Vector2 borderPosition = Vector2.zero;

        public bool smoothReturn = false;
        public float smoothFactor = 7f;

        private Color joystickNativeColor, backgroundNativeColor;


        // Joystick Mode
        public bool IsStatic
        {
            get { return isStatic; }
            set
            {
                if (isStatic == value) return;
                isStatic = value;
            }
        }


        // Control Awake
        public override void ControlAwake()
        {
            base.ControlAwake();

            if (Application.isPlaying)
                joystickImage.enabled = joystickBackgroundImage.enabled = isStatic;
        }

        void Awake()
        {
            joystickNativeColor = joystickImage.color;
            backgroundNativeColor = joystickBackgroundImage.color;
        }


        // Set Enable
        protected override void SetEnable()
        {
            base.SetEnable();
            joystickImage.enabled = enable;
            joystickBackgroundImage.enabled = enable;
        }

        // Set Visible
        protected override void SetVisible()
        {
            if (visible)
            {
                joystickImage.color = joystickNativeColor;
                joystickBackgroundImage.color = backgroundNativeColor;
            }
            else
            {
                joystickImage.color = Color.clear;
                joystickBackgroundImage.color = Color.clear;
            }
        }

        // Update Position
        protected override void UpdatePosition(Vector2 touchPos)
        {
            if (!axisX.enabled && !axisY.enabled)
                return;

            base.UpdatePosition(touchPos);
            if (touchDown)
            {
                GetCurrentPosition(touchPos);

                currentDirection = currentPosition - defaultPosition;

                float currentDistance = Vector2.Distance(defaultPosition, currentPosition);
                float touchForce = 100f;

                float calculatedBorderSize = (joystickBackgroundRT.sizeDelta.magnitude / 2f) * borderSize / 8f;

                borderPosition = defaultPosition;
                borderPosition += currentDirection.normalized * calculatedBorderSize;

                if (currentDistance > calculatedBorderSize)
                    currentPosition = borderPosition;
                else
                    touchForce = (currentDistance / calculatedBorderSize) * 100f;

                UpdateJoystickPosition();

                float aX = currentDirection.normalized.x * touchForce / 100f * sensitivity;
                float aY = currentDirection.normalized.y * touchForce / 100f * sensitivity;
                SetAxis(aX, aY);
            }
            else
            {
                touchDown = true;
                touchPhase = ETouchPhase.Began;
                if (!isStatic)
                    UpdateTransparencyAndPosition(touchPos);

                GetCurrentPosition(touchPos);
                UpdatePosition(touchPos);
                SetAxis(0f, 0f);
                // Broadcasting
                DownHandler();
            }
        }

        // Get CurrentPosition
        private void GetCurrentPosition(Vector2 touchPos)
        {
            defaultPosition = currentPosition = joystickBackgroundRT.position;
            if (axisX.enabled) currentPosition.x = GuiCamera.ScreenToWorldPoint(touchPos).x;
            if (axisY.enabled) currentPosition.y = GuiCamera.ScreenToWorldPoint(touchPos).y;
        }

        // Update JoystickPosition
        private void UpdateJoystickPosition()
        {
            joystickRT.position = currentPosition;
        }

        // Update Transparency and Position for DynamicJoystick
        private void UpdateTransparencyAndPosition(Vector2 touchPos)
        {
            SetVisible();
            joystickImage.enabled = joystickBackgroundImage.enabled = true;
            joystickRT.position = joystickBackgroundRT.position = GuiCamera.ScreenToWorldPoint(touchPos);
        }

        // SmoothReturn Run
        private m_IEnumerator SmoothReturnRun()
        {
            bool smoothReturnIsRun = true;
            int defPosMagnitude = Mathf.RoundToInt(defaultPosition.sqrMagnitude);

            while (smoothReturnIsRun && !touchDown)
            {
                float smoothTime = smoothFactor * Time.smoothDeltaTime;

                currentPosition = Vector2.Lerp(currentPosition, defaultPosition, smoothTime);

                if (!isStatic)
                {
                    joystickImage.color = Color.Lerp(joystickImage.color, Color.clear, smoothTime);
                    joystickBackgroundImage.color = Color.Lerp(joystickBackgroundImage.color, Color.clear, smoothTime);
                }

                if (Mathf.RoundToInt(currentPosition.sqrMagnitude) == defPosMagnitude)
                {
                    currentPosition = defaultPosition;
                    smoothReturnIsRun = false;

                    if (!isStatic)
                        joystickImage.enabled = joystickBackgroundImage.enabled = false;
                }

                UpdateJoystickPosition();
                yield return null;
            }
        }


        // Contro lReset
        protected override void ControlReset()
        {
            base.ControlReset();

            if (smoothReturn)
            {
                StopCoroutine("SmoothReturnRun");
                StartCoroutine("SmoothReturnRun");
            }
            else
            {
                joystickImage.enabled = joystickBackgroundImage.enabled = isStatic;
                currentPosition = defaultPosition;
                UpdateJoystickPosition();
            }

            // Broadcasting
            UpHandler();
        }


        // OnPointer Down
        public void OnPointerDown(PointerEventData pointerData)
        {
            if (!touchDown)
            {
                touchId = pointerData.pointerId;
                UpdatePosition(pointerData.position);
            }

            if (!touchDrag)
            {
                UpdatePosition(pointerData.position);
            }
        }

        // OnDrag
        public void OnDrag(PointerEventData pointerData)
        {
            if (Input.touchCount >= touchId && touchDown)
            {
                UpdatePosition(pointerData.position);
            }
            touchDrag = true;
        }

        // OnPointer Up
        public void OnPointerUp(PointerEventData pointerData)
        {
            UpdatePosition(pointerData.position);
            ControlReset();
        }

        // OnPointer Click
        public void OnPointerClick(PointerEventData pointerData)
        {
            ClickHandler();
        }
    }
}