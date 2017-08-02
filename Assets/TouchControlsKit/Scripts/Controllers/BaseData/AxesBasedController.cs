/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 AxesBasedController.cs                *
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
using m_IEnumerator = System.Collections.IEnumerator;

namespace TouchControlsKit
{
    [System.Serializable]
    public sealed class Axis
    {
        internal const int DIGITS = 2;

        public bool enabled = true;
        public bool inverse = false;

        internal float value { get; private set; }

        // SetValue
        internal void SetValue(float value)
        {
            if (enabled)
                this.value = (float)System.Math.Round((double)value, 3);
            else
                this.value = 0f;
        }
    }

    public abstract class AxesBasedController : ControllerBase
    {
        public float sensitivity = 1f;

        public bool axesLag = false;
        public float axesLagSpeed = 10f;

        public Axis axisX, axisY;

        private AxesEventHandler downHandler, pressHandler, upHandler, clickHandler;
        private AxesAlwaysHandler alwaysHandler;

        [SerializeField] public AxesEvent downEvent, pressEvent, upEvent, clickEvent;
        [SerializeField] public AlwaysAxesEvent alwaysEvent;

        [SerializeField]
        private bool showBaseImage = true;

        protected Vector2 defaultPosition, currentPosition, currentDirection;

        // Show TouchZone
        public bool ShowTouchZone
        {
            get { return showBaseImage; }
            set
            {
                if (showBaseImage == value)
                    return;

                showBaseImage = value;
                ShowHideTouchZone();
            }
        }
        // ShowHide TouchZone
        private void ShowHideTouchZone()
        {
            if (showBaseImage)
            {
                baseImage.color = baseImageNativeColor;
            }
            else
            {
                baseImageNativeColor = baseImage.color;
                baseImage.color = (Color32)Color.clear;
            }
        }


        // Bind Action
        internal void BindAxes(AxesEventHandler m_Handler, EActionEvent EActionEvent)
        {
            switch (EActionEvent)
            {
                case EActionEvent.Down:
                    useDown = true;
                    if (downHandler != m_Handler)
                        downHandler += m_Handler;
                    break;
                case EActionEvent.Press:
                    usePress = true;
                    if (pressHandler != m_Handler)
                        pressHandler += m_Handler;
                    break;
                case EActionEvent.Up:
                    useUp = true;
                    if (upHandler != m_Handler)
                        upHandler += m_Handler;
                    break;
                case EActionEvent.Click:
                    useClick = true;
                    if (clickHandler != m_Handler)
                        clickHandler += m_Handler;
                    break;
            }
        }
        // Bind Axes
        internal void BindAxes(AxesAlwaysHandler m_Handler)
        {
            useAlways = true;
            if (alwaysHandler != m_Handler)
                alwaysHandler += m_Handler;
        }

        // UnBind Action
        internal void UnBindAxes(AxesEventHandler m_Handler, EActionEvent EActionEvent)
        {
            switch (EActionEvent)
            {
                case EActionEvent.Down:
                    if (downHandler == m_Handler)
                    {
                        downHandler -= m_Handler;
                        useDown = (downHandler != null);
                    }
                    break;
                case EActionEvent.Press:
                    if (pressHandler == m_Handler)
                    {
                        pressHandler -= m_Handler;
                        usePress = (pressHandler != null);
                    }
                    break;
                case EActionEvent.Up:
                    if (upHandler == m_Handler)
                    {
                        upHandler -= m_Handler;
                        useUp = (upHandler != null);
                    }
                    break;
                case EActionEvent.Click:
                    if (clickHandler == m_Handler)
                    {
                        clickHandler -= m_Handler;
                        useClick = (clickHandler != null);
                    }
                    break;
            }
        }
        // UnBind Axes
        internal void UnBindAxes(AxesAlwaysHandler m_Handler)
        {
            if (alwaysHandler == m_Handler)
            {
                alwaysHandler -= m_Handler;
                useAlways = (clickHandler != null);
            }
        }


        // Set Axis
        protected void SetAxis(float x, float y)
        {
            x = axisX.inverse ? -x : x;
            y = axisY.inverse ? -y : y;

            if (axesLag)
            {
                if (axisX.enabled)
                {
                    StopCoroutine("SmoothAxisX");
                    StartCoroutine("SmoothAxisX", x);
                }
                else
                    axisX.SetValue(0f);

                if (axisY.enabled)
                {
                    StopCoroutine("SmoothAxisY");
                    StartCoroutine("SmoothAxisY", y);
                }
                else
                    axisY.SetValue(0f);
            }
            else
            {
                axisX.SetValue(x);
                axisY.SetValue(y);
            }
        }

        // Smooth AxisX
        private m_IEnumerator SmoothAxisX(float targetValue)
        {
            while (System.Math.Round((double)axisX.value, Axis.DIGITS) != System.Math.Round((double)targetValue, Axis.DIGITS))
            {
                axisX.SetValue(Mathf.Lerp(axisX.value, targetValue, Time.smoothDeltaTime * axesLagSpeed));
                yield return null;
            }

            axisX.SetValue(targetValue);
        }
        // Smooth AxisY
        private m_IEnumerator SmoothAxisY(float targetValue)
        {
            while (System.Math.Round((double)axisY.value, Axis.DIGITS) != System.Math.Round((double)targetValue, Axis.DIGITS))
            {
                axisY.SetValue(Mathf.Lerp(axisY.value, targetValue, Time.smoothDeltaTime * axesLagSpeed));
                yield return null;
            }

            axisY.SetValue(targetValue);
        }

        // Control Reset
        protected override void ControlReset()
        {
            base.ControlReset();
            SetAxis(0f, 0f);
        }

        // Down Handler
        protected override void DownHandler()
        {
            if (useDown)
                downHandler.Invoke(axisX.value, axisY.value);

            if (broadcast)
                downEvent.Invoke(axisX.value, axisY.value);
        }
        // Press Handler
        protected override void PressHandler()
        {
            if (useAlways)
                alwaysHandler.Invoke(axisX.value, axisY.value, touchPhase);

            if (broadcast)
                alwaysEvent.Invoke(axisX.value, axisY.value, touchPhase);

            if (touchDown)
            {
                if (usePress)
                    pressHandler.Invoke(axisX.value, axisY.value);

                if (broadcast)
                    pressEvent.Invoke(axisX.value, axisY.value);
            }
        }
        // Up Handler
        protected override void UpHandler()
        {
            if (useUp)
                upHandler.Invoke(axisX.value, axisY.value);

            if (broadcast)
                upEvent.Invoke(axisX.value, axisY.value);
        }
        // Click Handler
        protected override void ClickHandler()
        {
            if (useClick)
                clickHandler.Invoke(axisX.value, axisY.value);

            if (broadcast)
                clickEvent.Invoke(axisX.value, axisY.value);
        }
    }
}