/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKJoystickEditor.cs                  *
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
using UnityEditor;

namespace TouchControlsKit.Inspector
{
    [CustomEditor( typeof( TCKJoystick ) )]
    public class TCKJoystickEditor : BaseInspector
    {
        private TCKJoystick myTarget = null;
        private static string[] modNames = { "Dynamic", "Static" };


        // OnEnable
        void OnEnable()
        {
            myTarget = ( TCKJoystick )target;

            ParametersHelper.HelperSetup( myTarget );
            EventsHelper.HelperSetup( myTarget, serializedObject );

            //myTarget.ControlAwake();
        }


        // Dirty
        protected override void Dirty()
        {
            EditorUtility.SetDirty( myTarget );
        }

        // ShowParameters
        protected override void ShowParameters()
        {
            GUILayout.BeginVertical( "Box" );
            GUILayout.Label( "Parameters", StyleHelper.labelStyle );

            ParametersHelper.ShowEUpdateType();

            StyleHelper.StandardSpace();
            myTarget.isEnable = ParametersHelper.ShowBoolField( myTarget.isEnable, "Enable" );

            GUI.enabled = myTarget.isEnable;
            myTarget.isActive = ParametersHelper.ShowBoolField( myTarget.isActive, "Active", StyleHelper.STANDARD_INDENT );
            myTarget.isVisible = ParametersHelper.ShowBoolField( myTarget.isVisible, "Visible", StyleHelper.STANDARD_INDENT );
            GUI.enabled = true;

            ParametersHelper.ShowName( "Joystick Name" );

            StyleHelper.StandardSpace();
            GUILayout.BeginHorizontal();
            GUILayout.Label( "Mode", GUILayout.Width( StyleHelper.STANDARD_SIZE ) );
            myTarget.IsStatic = System.Convert.ToBoolean( GUILayout.Toolbar( System.Convert.ToInt32( myTarget.IsStatic ), modNames, GUILayout.Height( 20 ) ) );
            GUILayout.EndHorizontal();

            ParametersHelper.ShowSensitivity();

            StyleHelper.StandardSpace();
            GUILayout.BeginHorizontal();
            GUILayout.Label( "Border Size", GUILayout.Width( StyleHelper.STANDARD_SIZE ) );
            myTarget.borderSize = EditorGUILayout.Slider( myTarget.borderSize, 1f, 9f );
            GUILayout.EndHorizontal();

            StyleHelper.StandardSpace();

            GUILayout.BeginHorizontal();
            myTarget.smoothReturn = EditorGUILayout.Toggle( myTarget.smoothReturn, GUILayout.Width( 15f ) );
            GUILayout.Label( ( myTarget.IsStatic ? "Smooth Return" : "Fadeout" ), GUILayout.Width( StyleHelper.STANDARD_SIZE - 20f ) );
            GUI.enabled = myTarget.smoothReturn;
            myTarget.smoothFactor = EditorGUILayout.Slider( myTarget.smoothFactor, 1f, 20f );
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            ParametersHelper.ShowTouchZone();

            StyleHelper.StandardSpace();
            ParametersHelper.ShowSpriteAndColor( ref myTarget.joystickImage, "Joystick" );
            ParametersHelper.ShowSpriteAndColor( ref myTarget.joystickBackgroundImage, "Background" );

            StyleHelper.StandardSpace();
            GUILayout.EndVertical();

            AxesHelper.ShowAxes( myTarget );
            EventsHelper.ShowEvents();
        }
    }
}