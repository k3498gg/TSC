/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKInput.cs                           *
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
using UnityEngine.EventSystems;

namespace TouchControlsKit
{
    public sealed class TCKInput : MonoBehaviour
    {
        private static ControllerBase[] controllers = null;
        private static AxesBasedController[] abControllers = null;
        private static TCKButton[] buttons = null;

        private static bool active = false;

        /// <summary>
        /// The local active state of all controllers. (Read Only)
        /// </summary>
        public static bool isActive { get { return active; } }


        private static TCKInput instance = null;


        // Awake
        void Awake()
        {
            if( FindObjectOfType<EventSystem>() == null )            
                new GameObject( "EventSystem", typeof( EventSystem ), typeof( StandaloneInputModule ) );            

            instance = this;
            SetActive( true );            
        }


        /// <summary>
        /// Activates/Deactivates all controllers in scene.
        /// </summary>
        /// <param name="value"></param>
        public static void SetActive( bool value )
        {
            active = value;
            instance.enabled = value;
            instance.gameObject.SetActive( value );

            if( value )
            {
                controllers = instance.gameObject.GetComponentsInChildren<ControllerBase>();
                abControllers = instance.gameObject.GetComponentsInChildren<AxesBasedController>();
                buttons = instance.gameObject.GetComponentsInChildren<TCKButton>();

                foreach( ControllerBase controller in controllers )
                    controller.ControlAwake();
            }
            else
            {
                controllers = null;
                abControllers = null;
                buttons = null;
            }
        }


        /// <summary>
        /// Bind your void to button action, identified by buttonName. Called established event in EActionEvent.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="m_Handler"></param>
        /// <param name="EActionEvent"></param>
        public static void BindAction( string buttonName, EActionEvent m_Event, ActionEventHandler m_Handler )
        {
            foreach( TCKButton button in buttons )
            {
                if( button.MyName == buttonName )
                {
                    button.BindAction( m_Handler, m_Event );
                    return;
                }
            }
            Debug.LogError( "Button: " + buttonName + " Not Found." );
        }
        /// <summary>
        /// Bind your void to button update, identified by buttonName. Always called.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="m_Handler"></param>
        public static void BindAction( string buttonName, ActionAlwaysHandler m_Handler )
        {
            foreach( TCKButton button in buttons )
            {
                if( button.MyName == buttonName )
                {
                    button.BindAction( m_Handler );
                    return;
                }
            }
            Debug.LogError( "Button: " + buttonName + " Not Found." );
        }

        /// <summary>
        /// Unbind your void of button action, identified by buttonName & EActionEvent.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="m_Handler"></param>
        /// <param name="EActionEvent"></param>
        public static void UnbindAction( string buttonName, EActionEvent m_Event, ActionEventHandler m_Handler )
        {
            foreach( TCKButton button in buttons )
            {
                if( button.MyName == buttonName )
                {
                    button.UnBindAction( m_Handler, m_Event );
                    return;
                }
            }
            Debug.LogError( "Button: " + buttonName + " Not Found." );
        }
        /// <summary>
        /// Unbind your void of button update, identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="m_Handler"></param>
        public static void UnbindAction( string buttonName, ActionAlwaysHandler m_Handler )
        {
            foreach( TCKButton button in buttons )
            {
                if( button.MyName == buttonName )
                {
                    button.UnBindAction( m_Handler );
                    return;
                }
            }
            Debug.LogError( "Button: " + buttonName + " Not Found." );
        }

        /// <summary>
        /// Bind your void to controller axis action, identified by controllerName. Called established event in EActionEvent.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="m_Handler"></param>
        /// <param name="EActionEvent"></param>
        public static void BindAxes( string controllerName, EActionEvent m_Event, AxesEventHandler m_Handler )
        {
            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.BindAxes( m_Handler, m_Event );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }
        /// <summary>
        /// Bind your void to controller axis update, identified by controllerName. Always called.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="m_Handler"></param>
        public static void BindAxes( string controllerName, AxesAlwaysHandler m_Handler )
        {
            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.BindAxes( m_Handler );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }

        /// <summary>
        /// Unbind your void of controller axis  action, identified by controllerName & EActionEvent.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="m_Handler"></param>
        /// <param name="EActionEvent"></param>
        public static void UnbindAxes( string controllerName, EActionEvent m_Event, AxesEventHandler m_Handler )
        {
            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.UnBindAxes( m_Handler, m_Event );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }
        /// <summary>
        /// Unbind your void of controller update, identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="m_Handler"></param>
        public static void UnbindAxes( string controllerName, AxesAlwaysHandler m_Handler )
        {
            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.UnBindAxes( m_Handler );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Checked controller in scene, identified by controllerName
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool CheckController( string controllerName )
        {
            if( !active || controllers.Length == 0 )
                return false;            

            foreach( ControllerBase controller in controllers )
                if( controller.MyName == controllerName )
                    return true;                    

            return false;
        }


        /// <summary>
        /// Returns the controller Enable value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool GetControllerEnable( string controllerName )
        {
            if( !active )
                return false;

            foreach( ControllerBase controller in controllers )            
                if( controller.MyName == controllerName )                
                    return controller.isEnable;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Sets the controller Enable value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="value"></param>
        public static void SetControllerEnable( string controllerName, bool value )
        {
            if( !active )
                return;

            foreach( ControllerBase controller in controllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.isEnable = value;
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns the controller Active state value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool GetControllerActive( string controllerName )
        {
            if( !active )
                return false;

            foreach( ControllerBase controller in controllers )
                if( controller.MyName == controllerName )
                    return controller.isActive;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Sets the controller Active state value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="value"></param>
        public static void SetControllerActive( string controllerName, bool value )
        {
            if( !active )
                return;

            foreach( ControllerBase controller in controllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.isActive = value;
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns the controller Visibility value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool GetControllerVisible( string controllerName )
        {
            if( !active )
                return false;

            foreach( ControllerBase controller in controllers )
                if( controller.MyName == controllerName )
                    return controller.isVisible;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Sets the controller Visibility value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="value"></param>
        public static void SetControllerVisible( string controllerName, bool value )
        {
            if( !active )
                return;

            foreach( ControllerBase controller in controllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.isVisible = value;
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns the Axis value  identified by controllerName & axisType.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisType"></param>
        /// <returns></returns>
        public static float GetAxis( string controllerName, EAxisType axisType )
        {
            if( !active )
                return 0f;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == EAxisType.Horizontal )
                        return controller.axisX.value;
                    else if( axisType == EAxisType.Vertical )
                        return controller.axisY.value;

                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return 0f;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return 0f;
        }


        /// <summary>
        /// Returns the axis Enable value identified by controllerName & axisType.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisType"></param>
        /// <returns></returns>
        public static bool GetAxisEnable( string controllerName, EAxisType axisType )
        {
            if( !active )
                return false;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == EAxisType.Horizontal )
                        return controller.axisX.enabled;
                    else if( axisType == EAxisType.Vertical )
                        return controller.axisY.enabled;

                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return false;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }        

        /// <summary>
        /// Sets the axis Enable value identified by controllerName & EAxisType.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="EAxisType"></param>
        /// <param name="value"></param>
        public static void SetAxisEnable( string controllerName, EAxisType axisType, bool value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == EAxisType.Horizontal )
                    {
                        controller.axisX.enabled = value;
                        return;
                    }
                    else if( axisType == EAxisType.Vertical )
                    {
                        controller.axisY.enabled = value;
                        return;
                    }
                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }

        /// <summary>
        /// Returns the axis Inverse value identified by controllerName & axisType.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisType"></param>
        /// <returns></returns>
        public static bool GetAxisInverse( string controllerName, EAxisType axisType )
        {
            if( !active )
                return false;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == EAxisType.Horizontal )
                        return controller.axisX.inverse;
                    else if( axisType == EAxisType.Vertical )
                        return controller.axisY.inverse;

                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return false;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Sets the axis Inverse value identified by controllerName & axisType.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisType"></param>
        /// <param name="value"></param>
        public static void SetAxisInverse( string controllerName, EAxisType axisType, bool value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == EAxisType.Horizontal )
                    {
                        controller.axisX.inverse = value;
                        return;
                    }
                    else if( axisType == EAxisType.Vertical )
                    {
                        controller.axisY.inverse = value;
                        return;
                    }
                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns the value of the controller Sensitivity identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static float GetSensitivity( string controllerName )
        {
            if( !active )
                return 0f;

            foreach( AxesBasedController controller in abControllers )            
                if( controller.MyName == controllerName )                
                    return controller.sensitivity;                
            
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return 0f;
        }

        /// <summary>
        /// Sets the Sensitivity value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="value"></param>
        public static void SetSensitivity( string controllerName, float value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.sensitivity = value;
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Showing/Hiding touch zone for all controllers in scene.
        /// </summary>
        /// <param name="value"></param>
        public static void ShowingTouchZone( bool value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
                controller.ShowTouchZone = value;            
        }


        /// <summary>
        /// Returns true during the frame the user action event the touch button identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="m_Event"></param>
        /// <returns></returns>
        public static bool GetAction( string buttonName, EActionEvent m_Event )
        {
            if( !active )
                return false;

            switch( m_Event )
            {
                case EActionEvent.Down:
                    foreach( TCKButton button in buttons )
                        if( button.MyName == buttonName )
                            return button.isDOWN;
                    break;

                case EActionEvent.Press:
                    foreach( TCKButton button in buttons )
                        if( button.MyName == buttonName )
                            return button.isPRESSED;
                    break;

                case EActionEvent.Up:
                    foreach( TCKButton button in buttons )
                        if( button.MyName == buttonName )
                            return button.isUP;
                    break;

                case EActionEvent.Click:
                    foreach( TCKButton button in buttons )
                        if( button.MyName == buttonName )
                            return button.isCLICK;
                    break;
            }

            Debug.LogError( "Button: " + buttonName + " not found!" );
            return false;
        }

        /// <summary>
        /// Returns true during the frame the user pressed down the touch button identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        [System.Obsolete( "Please use 'GetAction' instead." )]
        public static bool GetButtonDown( string buttonName )
        {
            if( !active )
                return false;

            foreach( TCKButton button in buttons )            
                if( button.MyName == buttonName )                
                    return button.isDOWN;                
            
            Debug.LogError( "Button: " + buttonName + " not found!" );
            return false;
        }
        /// <summary>
        /// Returns whether the given touch button is held down identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        [System.Obsolete( "Please use 'GetAction' instead." )]
        public static bool GetButton( string buttonName )
        {
            if( !active )
                return false;

            foreach( TCKButton button in buttons )            
                if( button.MyName == buttonName )                
                    return button.isPRESSED;                
            
            Debug.LogError( "Button: " + buttonName + " not found!" );
            return false;
        }
        /// <summary>
        /// Returns true during the frame the user releases the given touch button identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        [System.Obsolete( "Please use 'GetAction' instead." )]
        public static bool GetButtonUp( string buttonName )
        {
            if( !active )
                return false;

            foreach( TCKButton button in buttons )            
                if( button.MyName == buttonName )                
                    return button.isUP;                
            
            Debug.LogError( "Button: " + buttonName + " not found!" );
            return false;
        }
        /// <summary>
        /// Returns true during the frame the user clicked the given touch button identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        [System.Obsolete( "Please use 'GetAction' instead." )]
        public static bool GetButtonClick( string buttonName )
        {
            if( !active )
                return false;

            foreach( TCKButton button in buttons )            
                if( button.MyName == buttonName )                
                    return button.isCLICK;               
            
            Debug.LogError( "Button: " + buttonName + " not found!" );
            return false;
        }


        /// <summary>
        /// Returns the EUpdateType enum value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static EUpdateType GetEUpdateType( string controllerName )
        {
            if( !active )
                return EUpdateType.OFF;

            foreach( ControllerBase controller in controllers )
                if( controller.MyName == controllerName )
                    return controller.updateType;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return EUpdateType.OFF;
        }

        /// <summary>
        /// Sets the EUpdateType enum value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="EUpdateType"></param>
        public static void SetEUpdateType( string controllerName, EUpdateType EUpdateType )
        {
            if( !active )
                return;

            foreach( ControllerBase controller in controllers )
                if( controller.MyName == controllerName )
                    controller.updateType = EUpdateType;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns describes phase of a finger touch identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static ETouchPhase GetTouchPhase( string controllerName )
        {
            if( !active )
                return ETouchPhase.Ended;

            foreach( ControllerBase controller in controllers )            
                if( controller.MyName == controllerName )                
                    return controller.touchPhase;                
            
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return ETouchPhase.Ended;
        }
    }
}