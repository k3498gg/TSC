/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKDPadEditor.cs                      *
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
    [CustomEditor( typeof( TCKDPad ) )]
    public class TCKDPadEditor : BaseInspector
    {
        private TCKDPad myTarget = null;


        // OnEnable
        void OnEnable()
        {
            myTarget = ( TCKDPad )target;

            ParametersHelper.HelperSetup( myTarget );
            EventsHelper.HelperSetup( myTarget, serializedObject );

            myTarget.ControlAwake();
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

            ParametersHelper.ShowName( "DPad Name" );
            ParametersHelper.ShowSensitivity();
            ParametersHelper.ShowTouchZone();

            StyleHelper.StandardSpace();

            ParametersHelper.ShowSpriteAndColor( ref myTarget.normalSprite, ref myTarget.normalColor, "Norm Arrow" );

            if( GUI.changed )
                myTarget.ControlAwake();

            ParametersHelper.ShowSpriteAndColor( ref myTarget.pressedSprite, ref myTarget.pressedColor, "Press Arrow" );

            StyleHelper.StandardSpace();
            GUILayout.EndVertical();

            AxesHelper.ShowAxes( myTarget );
            EventsHelper.ShowEvents();
        }
    }
}