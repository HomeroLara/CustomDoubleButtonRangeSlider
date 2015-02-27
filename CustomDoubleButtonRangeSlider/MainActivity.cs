using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using CustomDoubleButtonRangeSlider.Controls;
using Android.Graphics;

namespace CustomDoubleButtonRangeSlider
{
    [Activity(Label = "CustomDoubleButtonRangeSlider", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, DoubleButtonRangeSliderChangeListener
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var rangeSliderLayout = FindViewById<RelativeLayout>(Resource.Id.rangeSliderLayout);
            AddRangeSlidersToLayout(rangeSliderLayout);
        }

        private void AddRangeSlidersToLayout(RelativeLayout layout)
        {
            layout.AddView(CreateRangeSliderTable());
        }

        public TableLayout CreateRangeSliderTable()
        {
            var rangeSliderTableLayout = new TableLayout(this);
            var tableRowRangeSliders = new TableRow(this);

            rangeSliderTableLayout.LayoutParameters = new ViewGroup.LayoutParams(this.Resources.DisplayMetrics.WidthPixels, this.Resources.DisplayMetrics.HeightPixels);

            var leftButtonImage = BitmapFactory.DecodeResource(Resources, Resource.Drawable.circle_red);
            var leftButtonImageScalesd = Bitmap.CreateScaledBitmap(leftButtonImage, leftButtonImage.Width / 2, leftButtonImage.Height / 3, false);

            var rightButtonImage = BitmapFactory.DecodeResource(Resources, Resource.Drawable.circle_blue);
            var rightButtonImageScalesd = Bitmap.CreateScaledBitmap(rightButtonImage, rightButtonImage.Width / 2, rightButtonImage.Height / 3, false);

            tableRowRangeSliders.AddView(CreateRangeSlider(0, 40, 70, 85, 80, 40, 81, 90, 60, true, RangeSliderMode.DUALMODE, leftButtonImageScalesd, rightButtonImageScalesd));
            rangeSliderTableLayout.AddView(tableRowRangeSliders);

            tableRowRangeSliders = new TableRow(this);
            tableRowRangeSliders.AddView(CreateRangeSlider(1, 0, 0, 100, 100, 15, 61, 100, 0, true, RangeSliderMode.DUALMODE, leftButtonImageScalesd, rightButtonImageScalesd));
            rangeSliderTableLayout.AddView(tableRowRangeSliders);

            tableRowRangeSliders = new TableRow(this);
            tableRowRangeSliders.AddView(CreateRangeSlider(3, 0, 27, 100, 0, 0, 31, 100, 0, false, RangeSliderMode.RIGHTMODEONLY, leftButtonImageScalesd, rightButtonImageScalesd));
            rangeSliderTableLayout.AddView(tableRowRangeSliders);

            return rangeSliderTableLayout;
        }

        private DoubleButtonRangeSlider CreateRangeSlider(int id, int minLeftButtonValue, int minRightButtonValue
            , int maxRightButtonValue, int maxLeftButtonValue, int leftButtonStartValue, int rightButtonStartValue, int sliderMaxValue
            , int sliderMinValue, bool fullStep, RangeSliderMode mode, Bitmap leftButtonImage, Bitmap rightButtonImage)
        {
            var rangeSlider = new DoubleButtonRangeSlider(this);

            var rangeSliderLayoutParameters = new TableRow.LayoutParams();
            rangeSliderLayoutParameters.Width = this.Resources.DisplayMetrics.WidthPixels;
            rangeSliderLayoutParameters.Height = 100;

            rangeSlider.Id = id;
            rangeSlider.MinLeftButtonValue = minLeftButtonValue;
            rangeSlider.MinRightButtonValue = minRightButtonValue;
            rangeSlider.MaxRightButtonValue = maxRightButtonValue;
            rangeSlider.MaxLeftButtonValue = maxLeftButtonValue;
            rangeSlider.LeftButtonStartValue = leftButtonStartValue;
            rangeSlider.RightButtonStartValue = rightButtonStartValue;
            rangeSlider.SliderMaxValue = sliderMaxValue;
            rangeSlider.SliderMinValue = sliderMinValue;
            rangeSlider.FullStep = fullStep;
            rangeSlider.LeftButtonImage = leftButtonImage;
            rangeSlider.RightButtonImage = rightButtonImage;
            rangeSlider.SliderMode = mode;

            rangeSlider.LayoutParameters = rangeSliderLayoutParameters;
            rangeSlider.SetSeekBarChangeListener(this);

            return rangeSlider;
        }

        public void SeekBarValueChanged(decimal leftButtonValue, int leftButtonX, decimal rightButtonValue, int rightButtontX)
        {
            //do something
        }
        #region HELPERS
        private float Scale(float value, Android.Util.ComplexUnitType unit)
        {
            return Android.Util.TypedValue.ApplyDimension(unit, value, this.Resources.DisplayMetrics);
        }
        #endregion
    }
}

