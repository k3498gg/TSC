/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKSteeringWheelEditor.cs             *
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
    [CustomEditor( typeof( TCKSteeringWheel ) )]
    public class TCKSteeringWheelEditor : BaseInspector
    {
        private TCKSteeringWheel myTarget = null;


        // OnEnable
        void OnEnable()
        {
            myTarget = ( TCKSteeringWheel )target;

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

            if( myTarget.isEnable )
            {
                myTarget.isActive = ParametersHelper.ShowBoolField( myTarget.isActive, "Active", StyleHelper.STANDARD_INDENT );
                myTarget.isVisible = ParametersHelper.ShowBoolField( myTarget.isVisible, "Visible", StyleHelper.STANDARD_INDENT );
            }

            ParametersHelper.ShowName( "Wheel Name" );
            ParametersHelper.ShowSensitivity();

            StyleHelper.StandardSpace();
            GUILayout.BeginHorizontal();
            GUILayout.Label( "Max Steering Angle", GUILayout.Width( StyleHelper.STANDARD_SIZE ) );
            myTarget.maxSteeringAngle = EditorGUILayout.Slider( myTarget.maxSteeringAngle, 36f, 360f );
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label( "Released Speed", GUILayout.Width( StyleHelper.STANDARD_SIZE ) );
            myTarget.releasedSpeed = EditorGUILayout.Slider( myTarget.releasedSpeed, 25f, 150f );
            GUILayout.EndHorizontal();

            StyleHelper.StandardSpace();
            ParametersHelper.ShowSpriteAndColor( ref myTarget.baseImage, "Wheel" );

            StyleHelper.StandardSpace();
            GUILayout.EndVertical();

            AxesHelper.ShowAxes( myTarget, true );
            EventsHelper.ShowEvents();
        }
    }
}