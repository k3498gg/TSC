/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 ParametersHelper.cs                   *
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
    public sealed class ParametersHelper
    {
        private static ControllerBase controller = null;
        private static AxesBasedController abController = null;


        // HelperSetup
        public static void HelperSetup( ControllerBase target01 )
        {
            controller = target01;
            abController = target01 as AxesBasedController;
        }


        // ShowSensitivity
        public static void ShowEUpdateType()
        {
            GUI.enabled = ( controller.isEnable && controller.isActive );
            StyleHelper.StandardSpace();
            GUILayout.BeginHorizontal();
            GUILayout.Label( "Update Type", GUILayout.Width( StyleHelper.STANDARD_SIZE ) );
            controller.updateType = ( EUpdateType )EditorGUILayout.EnumPopup( controller.updateType );
            GUILayout.EndHorizontal();
            GUI.enabled = true;
        }

        // ShowBoolField
        public static bool ShowBoolField( bool value, string label, float space = 0f )
        {
            GUILayout.BeginHorizontal();
            if( space != 0f ) GUILayout.Space( space );
            GUILayout.Label( label, GUILayout.Width( StyleHelper.STANDARD_SIZE - space ) );
            value = EditorGUILayout.Toggle( value );
            GUILayout.EndHorizontal();
            return value;
        }

        // ShowName
        public static void ShowName( string name )
        {
            StyleHelper.StandardSpace();
            GUILayout.BeginHorizontal();
            GUILayout.Label( name, GUILayout.Width( StyleHelper.STANDARD_SIZE ) );
            controller.MyName = EditorGUILayout.TextField( controller.MyName );
            GUILayout.EndHorizontal();
        }

        // ShowSensitivity
        public static void ShowSensitivity()
        {
            StyleHelper.StandardSpace();            
            GUILayout.BeginHorizontal();
            GUILayout.Label( "Sensitivity", GUILayout.Width( StyleHelper.STANDARD_SIZE ) );
            abController.sensitivity = EditorGUILayout.Slider( abController.sensitivity, 1f, 10f );
            GUILayout.EndHorizontal();
        }

        // ShowTouchZone
        public static void ShowTouchZone()
        {
            StyleHelper.StandardSpace();
            GUILayout.BeginHorizontal();
            abController.ShowTouchZone = EditorGUILayout.Toggle( abController.ShowTouchZone, GUILayout.Width( 15f ) );
            GUILayout.Label( "TZone Sprite", GUILayout.Width( StyleHelper.STANDARD_SIZE - 20f ) );
            GUI.enabled = abController.ShowTouchZone;
            abController.baseImage.color = EditorGUILayout.ColorField( abController.baseImage.color, GUILayout.Width( StyleHelper.STANDARD_SIZE / 2f ) );
            abController.baseImage.sprite = EditorGUILayout.ObjectField( abController.baseImage.sprite, typeof( Sprite ), false ) as Sprite;
            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }


        // ShowSpriteAndColor
        public static void ShowSpriteAndColor( ref Sprite sprite, ref Color32 color, string label )
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label( label + " Sprite", GUILayout.Width( StyleHelper.STANDARD_SIZE ) );
            color = EditorGUILayout.ColorField( color, GUILayout.Width( StyleHelper.STANDARD_SIZE / 2f ) );
            sprite = EditorGUILayout.ObjectField( sprite, typeof( Sprite ), false ) as Sprite;
            GUILayout.EndHorizontal();
        }

        // ShowSpriteAndColor
        public static void ShowSpriteAndColor( ref UnityEngine.UI.Image image, string label )
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label( label + " Sprite", GUILayout.Width( StyleHelper.STANDARD_SIZE ) );
            image.color = EditorGUILayout.ColorField( image.color, GUILayout.Width( StyleHelper.STANDARD_SIZE / 2f ) );
            image.sprite = EditorGUILayout.ObjectField( image.sprite, typeof( Sprite ), false ) as Sprite;
            GUILayout.EndHorizontal();
        }
    }
}