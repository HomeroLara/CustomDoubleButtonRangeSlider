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
        private TextView _leftButtonValue;
        private TextView _rightButtonValue;
        private TextView _rangeSliderId;
        private TextView _textViewLeftXPosition;
        private TextView _textViewRightXPosition;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var rangeSliderLayout = FindViewById<RelativeLayout>(Resource.Id.rangeSliderLayout);
            this._leftButtonValue = FindViewById<TextView>(Resource.Id.textViewleftButtonValue);
            this._rightButtonValue = FindViewById<TextView>(Resource.Id.textViewRightButtonValue);
            this._rangeSliderId = FindViewById<TextView>(Resource.Id.textViewRangeSliderId);
            this._textViewLeftXPosition = FindViewById<TextView>(Resource.Id.textViewLeftXPosition);
            this._textViewRightXPosition = FindViewById<TextView>(Resource.Id.textViewRightXPosition);

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
            var leftButtonImageScalesd = Bitmap.CreateScaledBitmap(leftButtonImage, leftButtonImage.Width / 4, leftButtonImage.Height / 4, false);

            var rightButtonImage = BitmapFactory.DecodeResource(Resources, Resource.Drawable.circle_blue);
            var rightButtonImageScalesd = Bitmap.CreateScaledBitmap(rightButtonImage, rightButtonImage.Width / 4, rightButtonImage.Height / 4, false);

            tableRowRangeSliders.AddView(CreateRangeSlider(0, 40, 70, 85, 80, 40, 81, 90, 60, true, RangeSliderMode.DUALMODE, leftButtonImageScalesd, rightButtonImageScalesd));
            rangeSliderTableLayout.AddView(tableRowRangeSliders);

            tableRowRangeSliders = new TableRow(this);
            tableRowRangeSliders.AddView(CreateRangeSlider(1, 0, 0, 100, 100, 15, 61, 100, 0, true, RangeSliderMode.DUALMODE, leftButtonImageScalesd, rightButtonImageScalesd));
            rangeSliderTableLayout.AddView(tableRowRangeSliders);

            tableRowRangeSliders = new TableRow(this);
            tableRowRangeSliders.AddView(CreateRangeSlider(2, 0, 27, 100, 0, 0, 31, 100, 0, true, RangeSliderMode.RIGHTMODEONLY, leftButtonImageScalesd, rightButtonImageScalesd));
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
            rangeSliderLayoutParameters.Height = 150;

            rangeSlider.RangeSliderId = id;
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

        public void SeekBarValueChanged( int id, decimal leftButtonValue, int leftButtonX, decimal rightButtonValue, int rightButtontX)
        {
            this._rangeSliderId.Text = string.Format(" Range Slider Id = {0}", id);
            this._leftButtonValue.Text = string.Format(" Left Button Value = {0}",  leftButtonValue > 0 ? leftButtonValue: 0);
            this._rightButtonValue.Text = string.Format(" Right Button Value = {0}",  rightButtonValue > 0 ? rightButtonValue:0);
            this._textViewLeftXPosition.Text = string.Format(" Left Button X Position Value = {0}", leftButtonX > 0 ? leftButtonX : 0);
            this._textViewRightXPosition.Text = string.Format(" Right Button X Position Value = {0}", rightButtontX > 0 ? rightButtontX : 0);
        }
        #region HELPERS
        private float Scale(float value, Android.Util.ComplexUnitType unit)
        {
            return Android.Util.TypedValue.ApplyDimension(unit, value, this.Resources.DisplayMetrics);
        }
        #endregion
    }
}

