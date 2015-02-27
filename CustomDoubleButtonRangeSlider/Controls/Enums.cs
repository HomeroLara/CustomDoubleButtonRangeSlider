using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CustomDoubleButtonRangeSlider.Controls
{
    public enum SelectedButton
    {
        LEFT = 1,
        RIGHT = 2,
        NONE
    }

    public enum RangeSliderMode
    {
        RIGHTMODEONLY = 1,
        LEFTMODEONLY = 2,
        DUALMODE
    }
}