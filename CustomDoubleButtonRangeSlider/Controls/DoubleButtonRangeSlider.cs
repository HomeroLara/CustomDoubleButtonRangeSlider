// ***********************************************************************
// Assembly         : nubis
// Author           : homerolara
// Created          : 09-01-2014
//
// Last Modified By : homerolara
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="DoublebuttonRangeSlider.cs" company="Nubis Uno">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
using Context = Android.Content.Context;
using Bitmap = Android.Graphics.Bitmap;
using BitmapFactory = Android.Graphics.BitmapFactory;
using Canvas = Android.Graphics.Canvas;
using Paint = Android.Graphics.Paint;
using AttributeSet = Android.Util.IAttributeSet;
using Log = Android.Util.Log;
using MotionEvent = Android.Views.MotionEvent;
using ImageView = Android.Widget.ImageView;

namespace CustomDoubleButtonRangeSlider.Controls
{
    public class DoubleButtonRangeSlider : View
    {
        #region PRIVATE MEMBERS
        private bool InstanceFieldsInitialized = false;

        private string _tag;
        private Bitmap _leftButtonImage = null;
        private Bitmap _rightButtonImage = null;
        private Bitmap _leftButtonImageScaled = null;
        private Bitmap _rightButtonImageScaled = null;
        private Bitmap _sliderBarActive = null;
        private Bitmap _sliderBarActiveScaled = null;
        private bool _enabledSlider = true;

        private int _id = 0;
        private int _sliderMinValue = 0;
        private int _sliderMaxValue = 100;
        private int _scaleStartValue;
        private int _scaleEndValue;
        private int _leftButtonStartValue;
        private int _rightButtonStartValue;
        private int _minLeftButtonValue;
        private int _minRightButtonValue;
        private int _maxleftButtonValue;
        private int _maxRightButtonValue;
        private int _periodId = 0;
        private int _leftButtonX;
        private int _rightButtonX;
        private decimal _leftButtonValue;
        private decimal _rightButtonValue;
        private Paint _paint = new Paint();
        private SelectedButton _selectedButton;
        private DoubleButtonRangeSliderChangeListener _rangeBarChangeListener;
        private RangeSliderMode _sliderMode = RangeSliderMode.DUALMODE;
        private int _offset;
        private int _minwindow = 10;
        private Paint _textPaint = null;
        private Canvas _canvas;
        private bool _fullStep = true;
        private decimal? _step;
        private int _deadBand = 5;
        private bool _isLeftButtonBeingPressed = false;
        private bool _isRightButtonBeingPressed = false;
        #endregion

        #region PUBLIC MEMBERS
        public int RangeSliderId
        {
            get { return _id; }
            set { _id = value; }
        }
        public bool FullStep
        {
            get { return _fullStep; }
            set { _fullStep = value; }
        }
        public bool EnabledSlider
        {
            get { return _enabledSlider; }
            set { _enabledSlider = value; }
        }

        public RangeSliderMode SliderMode
        {
            get { return _sliderMode; }
            set { _sliderMode = value; }
        }
        public Bitmap LeftButtonImage
        {
            get { return _leftButtonImage; }
            set { _leftButtonImage = value; }
        }

        public Bitmap RightButtonImage
        {
            get { return _rightButtonImage; }
            set { _rightButtonImage = value; }
        }

        public int SliderMinValue
        {
            get { return _sliderMinValue; }
            set { _sliderMinValue = value; }
        }

        public int SliderMaxValue
        {
            get { return _sliderMaxValue; }
            set { _sliderMaxValue = value; }
        }

        public int ScaleStartValue
        {
            get { return _scaleStartValue; }
            set { _scaleStartValue = value; }
        }

        public int ScaleEndValue
        {
            get { return _scaleEndValue; }
            set { _scaleEndValue = value; }
        }

        public int LeftButtonStartValue
        {
            get { return _leftButtonStartValue; }
            set { _leftButtonStartValue = value; }
        }

        public int RightButtonStartValue
        {
            get { return _rightButtonStartValue; }
            set { _rightButtonStartValue = value; }
        }
        public int MinLeftButtonValue
        {
            get { return _minLeftButtonValue; }
            set { _minLeftButtonValue = value; }
        }

        public int MaxLeftButtonValue
        {
            get { return _maxleftButtonValue; }
            set { _maxleftButtonValue = value; }
        }

        public int MinRightButtonValue
        {
            get { return _minRightButtonValue; }
            set { _minRightButtonValue = value; }
        }

        public int MaxRightButtonValue
        {
            get { return _maxRightButtonValue; }
            set { _maxRightButtonValue = value; }
        }

        #endregion

        #region Methods
        public DoubleButtonRangeSlider(Context context, AttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            if (!InstanceFieldsInitialized)
                InstanceFieldsInitialized = true;
        }

        public DoubleButtonRangeSlider(Context context, AttributeSet attrs)
            : base(context, attrs)
        {
            if (!InstanceFieldsInitialized)
                InstanceFieldsInitialized = true;
        }

        public DoubleButtonRangeSlider(Context context)
            : base(context)
        {
            if (!InstanceFieldsInitialized)
                InstanceFieldsInitialized = true;
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            //make sure the the view has been drawn..no need to Init() if the view is not ready
            //just a check to convser memory\cpu resources
            if (Height > 0)
                Init();
        }

        private void Init()
        {
            Invalidate();
        }

        private void InitTextPaint()
        {
            if (this._textPaint == null)
            {
                this._textPaint = new Paint();
                this._textPaint.Color = Android.Graphics.Color.White;
                this._textPaint.AntiAlias = (true);
                this._textPaint.TextSize = Scale(15f, Android.Util.ComplexUnitType.Dip);
            }
        }

        public virtual void SetSeekBarChangeListener(DoubleButtonRangeSliderChangeListener rangeBarChangeListener)
        {
            this._rangeBarChangeListener = rangeBarChangeListener;
        }

        public void InitBitMaps()
        {

            //get the slider bar image
            if (this._sliderBarActive == null)
                this._sliderBarActive = BitmapFactory.DecodeResource(Resources, Resource.Drawable.sliderbar);

            //scale the slider bar image to the specified width
            if (this._sliderBarActiveScaled == null)
                this._sliderBarActiveScaled = Bitmap.CreateScaledBitmap(this._sliderBarActive, this.Width, 10, false);

            //if (this.LeftButtonImage == null)
            //    this._leftButtonImage = BitmapFactory.DecodeResource(Resources, Resource.Drawable.circle_red);

            //if (this.RightButtonImage == null)
            //    this._rightButtonImage = BitmapFactory.DecodeResource(Resources, Resource.Drawable.circle_blue);

            if (!this._step.HasValue)
            {
                var width = Width - (this._leftButtonImage.Width + this._rightButtonImage.Width);
                this._step = Math.Round((decimal)width / (this._sliderMaxValue - this._sliderMinValue));
            }
        }

        public void DrawButtons()
        {
            int width = 0;

            switch (this._sliderMode)
            {
                case RangeSliderMode.RIGHTMODEONLY:
                    width = (Width - this._rightButtonImage.Width);
                    break;
                case RangeSliderMode.LEFTMODEONLY:
                    width = (Width - this._leftButtonImage.Width);
                    break;
                case RangeSliderMode.DUALMODE:
                default:
                    width = Width - (this._leftButtonImage.Width + this._rightButtonImage.Width);
                    break;
            }

            //if no starting point is provided
            if (this._leftButtonX <= 0)
            {
                var leftButtonStartValue = Decimal.Divide(this._leftButtonStartValue, 100);
                this._leftButtonX = (int)(leftButtonStartValue * width);
            }

            //if no starting point is provided
            if (this._rightButtonX <= 0)
            {
                var rightButtonStartValue = Decimal.Divide(this._rightButtonStartValue, 100);
                this._rightButtonX = (int)(rightButtonStartValue * width);
            }

            this._canvas.DrawBitmap(this._sliderBarActiveScaled, 0, 55, new Paint() );

            var leftButtonValue = string.Empty;
            var rightButtonValue = string.Empty;
            float startingRightButtonTextX;
            float startingLeftButtonTextX;

            GetButtonValues();

            switch (this._sliderMode)
            {
                case RangeSliderMode.RIGHTMODEONLY:
                    this._canvas.DrawBitmap(this._rightButtonImage, this._rightButtonX, 0, this._paint);

                    //draw the right textView @ center of right button
                    startingRightButtonTextX = XRightButtonPosition(this._textPaint, this._rightButtonImage, this._rightButtonX, rightButtonValue);

                    rightButtonValue = string.Format("{0}{1}", this._rightButtonValue.ToString(), "\u00B0");
                    this._canvas.DrawText(rightButtonValue, startingRightButtonTextX, Scale(35f, Android.Util.ComplexUnitType.Dip), this._textPaint);
                    break;

                case RangeSliderMode.LEFTMODEONLY:
                    this._canvas.DrawBitmap(this._leftButtonImage, this._leftButtonX - this._leftButtonImage.Width, 0, this._paint);

                    //draw the left textView @ center of left button
                    startingLeftButtonTextX = XLeftButtonPosition(this._textPaint, this._leftButtonImage, this._leftButtonX, leftButtonValue);

                    leftButtonValue = string.Format("{0}{1}", this._leftButtonValue.ToString(), "\u00B0");
                    this._canvas.DrawText(leftButtonValue, startingLeftButtonTextX, Scale(35f, Android.Util.ComplexUnitType.Dip), this._textPaint);
                    break;

                case RangeSliderMode.DUALMODE:
                default:
                    this._canvas.DrawBitmap(this._leftButtonImage, this._leftButtonX - this._leftButtonImage.Width, 0, this._paint);
                    this._canvas.DrawBitmap(this._rightButtonImage, this._rightButtonX, 0, this._paint);

                    leftButtonValue = string.Format("{0}{1}", this._leftButtonValue.ToString(), "\u00B0");
                    rightButtonValue = string.Format("{0}{1}", this._rightButtonValue.ToString(), "\u00B0");

                    //draw the left textView @ center of left button
                    startingLeftButtonTextX = XLeftButtonPosition(this._textPaint, this._leftButtonImage, this._leftButtonX, leftButtonValue);
                    //this._canvas.DrawText(leftButtonValue, startingLeftButtonTextX, Scale(this._isLeftButtonBeingPressed ? 10f : 28f, Android.Util.ComplexUnitType.Dip), this._textPaint);
                    this._canvas.DrawText(leftButtonValue, startingLeftButtonTextX, Scale(35f, Android.Util.ComplexUnitType.Dip), this._textPaint);

                    //draw the right textView @ center of right button
                    startingRightButtonTextX = XRightButtonPosition(this._textPaint, this._rightButtonImage, this._rightButtonX, rightButtonValue);
                    this._canvas.DrawText(rightButtonValue, startingRightButtonTextX, Scale(35f, Android.Util.ComplexUnitType.Dip), this._textPaint);
                    break;
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            InitBitMaps();
            InitTextPaint();
            base.OnDraw(canvas);

            this._canvas = canvas;
            DrawButtons();
        }

        /// <summary>
        /// calculates the starting x position to display the value for the right button
        /// </summary>
        /// <param name="paint"></param>
        /// <param name="bitMap"></param>
        /// <param name="currentPosition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private float XRightButtonPosition(Paint paint, Bitmap bitMap, int currentPosition, string value)
        {
            var textWidthMedian = paint.MeasureText(this._rightButtonValue.ToString()) / 2;
            var bitMapMedian = bitMap.Width / 2;
            return currentPosition + (bitMapMedian - textWidthMedian);
        }

        /// <summary>
        /// calculates the starting x poition to display the value for the left button
        /// </summary>
        /// <param name="paint"></param>
        /// <param name="bitMap"></param>
        /// <param name="currentPosition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private float XLeftButtonPosition(Paint paint, Bitmap bitMap, int currentPosition, string value)
        {
            var textWidthMedian = paint.MeasureText(this._leftButtonValue.ToString()) / 2;
            var bitMapMedian = bitMap.Width / 2;
            return currentPosition - (bitMapMedian + textWidthMedian);
        }

        public override bool OnTouchEvent(MotionEvent me)
        {
            int mx = (int)me.GetX();

            switch (me.Action)
            {
                case MotionEventActions.Down:
                    if (mx >= this._leftButtonX - this._leftButtonImage.Width && mx <= this._leftButtonX)
                    {
                        this._isLeftButtonBeingPressed = true;
                        this._selectedButton = SelectedButton.LEFT;
                        this._offset = mx - this._leftButtonX;
                    }
                    else if (mx >= this._rightButtonX && mx <= this._rightButtonX + this._rightButtonImage.Width)
                    {
                        this._isRightButtonBeingPressed = true;
                        this._selectedButton = SelectedButton.RIGHT;
                        this._offset = this._rightButtonX - mx;
                    }
                    break;

                case MotionEventActions.Move:
                    if (this._selectedButton == SelectedButton.LEFT)
                    {
                        this._isLeftButtonBeingPressed = true;
                        _leftButtonX = mx - _offset;
                        if (this._leftButtonX < this._leftButtonImage.Width)
                            this._leftButtonX = this._leftButtonImage.Width;
                    }
                    else if (this._selectedButton == SelectedButton.RIGHT)
                    {
                        this._isRightButtonBeingPressed = true;
                        this._rightButtonX = mx + this._offset;
                    }

                    break;

                case MotionEventActions.Up:
                    this._isRightButtonBeingPressed = false;
                    this._isLeftButtonBeingPressed = false;
                    this._selectedButton = SelectedButton.NONE;
                    break;
            }

            //TODO: this is hacky. refactor will need to be done here
            //maybe assign handlers to the buttons individually?
            //if the user is dragging a button and runs into the other, 
            //then both buttons move in the direction of the user's swipe 
            if (this._selectedButton == SelectedButton.RIGHT)
            {
                if (this._rightButtonX > Width - this._rightButtonImage.Width)
                    this._rightButtonX = (Width - this._rightButtonImage.Width);

                if (this._rightButtonX <= this._leftButtonImage.Width + this._deadBand + this._minwindow)
                    this._rightButtonX = (this._leftButtonImage.Width + this._deadBand + this._minwindow);

                if (this._rightButtonX <= this._leftButtonX + this._minwindow)
                    this._leftButtonX = (this._rightButtonX - this._deadBand - this._minwindow);
            }
            else if (this._selectedButton == SelectedButton.LEFT)
            {
                if (this._leftButtonX < this._leftButtonImage.Width)
                    this._leftButtonX = this._leftButtonImage.Width;

                if (this._leftButtonX >= Width - (this._rightButtonImage.Width + this._minwindow))
                    this._leftButtonX = Width - (this._rightButtonImage.Width + this._minwindow);

                if (this._leftButtonX > this._rightButtonX - this._minwindow)
                    this._rightButtonX = this._leftButtonX + this._deadBand + this._minwindow;
            }

            Invalidate();

            if (this._rangeBarChangeListener != null)
            {
                GetButtonValues();
                //set listener at these particular points
                this._rangeBarChangeListener.SeekBarValueChanged(this._id, this._leftButtonValue, this._leftButtonX, this._rightButtonValue, this._rightButtonX);
            }
            return true;
        }

        /// <summary>
        /// determines, in percentage, the value of left\right thumb positions compared to the width of the bar
        /// </summary>
        private void GetButtonValues()
        {
            var currentLeftButtonPosition = (this._leftButtonX - this._leftButtonImage.Width);
            var currentRightButtonPosition = (this._rightButtonX -  this._rightButtonImage.Width);

            var numberOfTicksTakenLeftButton = currentLeftButtonPosition / this._step;
            this._leftButtonValue = this._sliderMinValue + numberOfTicksTakenLeftButton.Value;
            this._leftButtonValue = Math.Round(this._leftButtonValue, this._fullStep ? 0 : 1);

            var numberOfTicksTakenRightButton = currentRightButtonPosition / this._step;
            this._rightButtonValue = this._sliderMinValue + numberOfTicksTakenRightButton.Value;
            this._rightButtonValue = Math.Round(this._rightButtonValue, this._fullStep ? 0 : 1);
        }
        #endregion

        #region HELPERS
        /// <summary>
        /// this is more visual test purposes
        /// writes output the the Debug Output Window when ever a user touches the one of the buttons
        /// </summary>
        /// <param name="log"></param>
        private void _DebugInfo_old(string log)
        {
            try
            {
                switch (this._selectedButton)
                {
                    case SelectedButton.LEFT:
                        Log.Info(this._tag, string.Format("Tag = {0}. Log = {1}. Position Value = {2}", this._tag, log, this._leftButtonValue));
                        break;
                    case SelectedButton.RIGHT:
                        Log.Info(this._tag, string.Format("Tag = {0}. Log = {1}. Position Value = {2}", this._tag, log, this._rightButtonValue));
                        break;
                    case SelectedButton.NONE:
                        Log.Info(this._tag, string.Format("Tag = {0}. Log = {1}", this._tag, log));
                        break;
                }
            }
            catch
            {
                //DO NOTHING: This is just for debugging information purposes only
            }
        }

        private float Scale(float value, Android.Util.ComplexUnitType unit)
        {
            return Android.Util.TypedValue.ApplyDimension(unit, value, this.Resources.DisplayMetrics);
        }
        #endregion
    }

    //this interface will need to be implemented in the activity that is calling the DoublebuttonRangeSlider
    public interface DoubleButtonRangeSliderChangeListener
    {
        void SeekBarValueChanged(int id,  decimal leftButtonValue, int leftButtonX, decimal rightButtonValue, int rightButtontX);
    }
}