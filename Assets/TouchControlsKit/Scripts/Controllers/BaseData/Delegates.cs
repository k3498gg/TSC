/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 Delegates.cs                          *
 * 													   *
 * Copyright(c): Victor Klepikov					   *
 * Support: 	 http://bit.ly/vk-Support			   *
 * 													   *
 * mySite:       http://vkdemos.ucoz.org			   *
 * myAssets:     http://u3d.as/5Fb                     *
 * myTwitter:	 http://twitter.com/VictorKlepikov	   *
 * 													   *
 *******************************************************/


using UnityEngine.Events;

namespace TouchControlsKit
{
    public delegate void ActionEventHandler();
    public delegate void ActionAlwaysHandler( ETouchPhase touchPhase );

    public delegate void AxesEventHandler( float axisX, float axisY );
    public delegate void AxesAlwaysHandler( float axisX, float axisY, ETouchPhase touchPhase );

    
    [System.Serializable] public class ActionEvent : UnityEvent { }
    [System.Serializable] public class AlwaysActionEvent : UnityEvent<ETouchPhase> { }

    [System.Serializable] public class AxesEvent : UnityEvent<float, float> { }
    [System.Serializable] public class AlwaysAxesEvent : UnityEvent<float, float, ETouchPhase> { }
}
