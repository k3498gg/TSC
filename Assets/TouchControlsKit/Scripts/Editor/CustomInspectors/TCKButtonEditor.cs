/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKButtonEditor.cs                    *
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
    [CustomEditor( typeof( TCKButton ) )]
    public class TCKButtonEditor : BaseInspector
    {
        private TCKButton myTarget = null;


        // OnEnable
        void OnEnable()
        {
            myTarget = ( TCKButton )target;

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

            ParametersHelper.ShowName( "Button Name" );

            StyleHelper.StandardSpace();
            myTarget.swipeOut = ParametersHelper.ShowBoolField( myTarget.swipeOut, "Swipe Out" );            

            StyleHelper.StandardSpace();

            Sprite sprite = myTarget.normalSprite;
            ParametersHelper.ShowSpriteAndColor( ref sprite, ref myTarget.normalColor, "Normal" );
            myTarget.normalSprite = sprite;
            myTarget.baseImage.color = myTarget.isVisible ? myTarget.normalColor : ( Color32 )Color.clear;

            ParametersHelper.ShowSpriteAndColor( ref myTarget.pressedSprite, ref myTarget.pressedColor, "Pressed" );
            
            StyleHelper.StandardSpace();
            GUILayout.EndVertical();

            EventsHelper.ShowEvents();
        }
    }
}