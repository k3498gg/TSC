/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 Enums.cs                              *
 * 													   *
 * Copyright(c): Victor Klepikov					   *
 * Support: 	 http://bit.ly/vk-Support			   *
 * 													   *
 * mySite:       http://vkdemos.ucoz.org			   *
 * myAssets:     http://u3d.as/5Fb                     *
 * myTwitter:	 http://twitter.com/VictorKlepikov	   *
 * 													   *
 *******************************************************/


namespace TouchControlsKit
{
    // Used for identification axis by type X or Y.
    public enum EAxisType
    {
        Horizontal = 0,
        Vertical = 1
    }

    // Used for select update method of delegates and events invoking.
    public enum EUpdateType
    {
        Update = 0,
        LateUpdate = 2,
        FixedUpdate = 3,
        OFF = 4
    }

    // Describes phase of a finger touch.
    public enum ETouchPhase
    {
        Began = 0,
        Moved = 1,
        Stationary = 2,
        Ended = 3,
        NoTouch = 4
    }

    // Used for delegates of BindAction API.
    public enum EActionEvent
    {
        Down = 0,
        Press = 1,
        Up = 2,
        Click = 3
    }

    // Used for DPad arrows.
    public enum EArrowType
    {
        none = 0,
        UP = 1,
        DOWN = 2,
        LEFT = 3,
        RIGHT = 4
    }
}
