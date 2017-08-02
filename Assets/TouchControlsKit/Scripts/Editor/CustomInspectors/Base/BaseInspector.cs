/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 BaseInspector.cs                      *
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
    public abstract class BaseInspector : Editor
    {
        // OnInspectorGUI
        public override void OnInspectorGUI()
        {
            // BEGIN
            GUILayout.BeginVertical( "Box", GUILayout.Width( 300f ) );
            StyleHelper.StandardSpace();
            //

            ShowParameters();

            if( GUI.changed )
                Dirty();

            // END
            StyleHelper.StandardSpace();
            GUILayout.EndVertical();
            //
        }

        // ShowParameters
        protected abstract void ShowParameters();

        // Dirty
        protected abstract void Dirty();
    }
}