/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 AxesHelper.cs                         *
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
    public sealed class AxesHelper
    {
         /// <summary>
        /// Show Axes
        /// </summary>
        /// <param name="size"></param>
        /// <param name="hideVert"></param>
        public static void ShowAxes( AxesBasedController target, bool hideVert = false )
        {
            StyleHelper.StandardSpace();
            GUILayout.BeginVertical( "Box" );
            GUILayout.Label( "Axes", StyleHelper.labelStyle );
            StyleHelper.StandardSpace();

            ShowAxis( ref target.axisX, "Horizontal ( Axis X )" );
            
            if( !hideVert )
                ShowAxis( ref target.axisY, "Vertical ( Axis Y )" );

            GUI.enabled = target.axisX.enabled || target.axisY.enabled;
            GUILayout.Space( 10f );
            GUILayout.BeginHorizontal();
            GUILayout.Space( 10f );
            target.axesLag = EditorGUILayout.Toggle( target.axesLag, GUILayout.Width( 15f ) );
            GUILayout.Label( "Lag", GUILayout.Width( 25f ) );
            GUI.enabled = target.axesLag;
            target.axesLagSpeed = EditorGUILayout.Slider( target.axesLagSpeed, 5f, 25f );
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUI.enabled = true;

            StyleHelper.StandardSpace();
            GUILayout.EndVertical();
        }

        // ShowAxis
        private static void ShowAxis( ref Axis axis, string label )
        {
            GUILayout.BeginHorizontal();
            axis.enabled = EditorGUILayout.Toggle( axis.enabled, GUILayout.Width( 15f ) );
            GUI.enabled = axis.enabled;
            GUILayout.Label( label );            
            axis.inverse = EditorGUILayout.Toggle( axis.inverse, GUILayout.Width( 15f ) );
            GUILayout.Label( "Inverse", GUILayout.Width( 55f ) );
            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }
    }
}