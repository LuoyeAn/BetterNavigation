using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BetterNavigation;
using BetterNavigation.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationPageRenderer))]
namespace BetterNavigation.Droid
{
    public class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        public CustomNavigationPageRenderer(Context context) : base(context)
        {
        }
        Android.Support.V7.Widget.Toolbar _toolbar;
        LinearLayout _titleViewLayout;
        LinearLayout _leftMenuLayout;
        LinearLayout _rightMenuLayout;
        TextView _titleTextView;
        TextView _subTitleTextView;

        Drawable _originalDrawable;
        Drawable _originalToolbarBackground;
        Drawable _originalWindowContent;
        ColorStateList _originalColorStateList;
        Typeface _originalFont;

        protected override void SetupPageTransition(Android.Support.V4.App.FragmentTransaction transaction, bool isPush)
        {
            SetupToolbarCustomization(Element.CurrentPage);
            base.SetupPageTransition(transaction, isPush);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var lastPage = Element.CurrentPage;
            if (e.PropertyName == CustomNavigationPage.HasShadowProperty.PropertyName)
            {
                UpdateToolbarShadow(_toolbar, CustomNavigationPage.GetHasShadow(lastPage), Context as Activity, _originalWindowContent);
            }
            else if (e.PropertyName == CustomNavigationPage.TitleBackgroundProperty.PropertyName)
            {
                UpdateTitleViewLayoutBackground(_titleViewLayout, CustomNavigationPage.GetTitleBackground(lastPage), _originalDrawable);
            }
            else if (e.PropertyName == CustomNavigationPage.BarBackgroundProperty.PropertyName)
            {
                UpdateToolbarBackground(_toolbar, lastPage, Context as Activity, _originalToolbarBackground);

            }
            else if (e.PropertyName == CustomNavigationPage.GradientColorsProperty.PropertyName)
            {
                UpdateToolbarBackground(_toolbar, lastPage, Context as Activity, _originalToolbarBackground);

            }
            else if (e.PropertyName == CustomNavigationPage.GradientDirectionProperty.PropertyName)
            {
                UpdateToolbarBackground(_toolbar, lastPage, Context as Activity, _originalToolbarBackground);

            }
            else if (e.PropertyName == CustomNavigationPage.BarBackgroundOpacityProperty.PropertyName)
            {
                UpdateToolbarBackground(_toolbar, lastPage, Context as Activity, _originalToolbarBackground);

            }
            else if (e.PropertyName == CustomNavigationPage.TitleBorderCornerRadiusProperty.PropertyName)
            {
                _titleViewLayout?.SetBackground(CreateShape(ShapeType.Rectangle, (int)CustomNavigationPage.GetTitleBorderWidth(lastPage), (int)CustomNavigationPage.GetTitleBorderCornerRadius(lastPage), CustomNavigationPage.GetTitleFillColor(lastPage), CustomNavigationPage.GetTitleBorderColor(lastPage)));
            }
            else if (e.PropertyName == CustomNavigationPage.TitleBorderWidthProperty.PropertyName)
            {

                _titleViewLayout?.SetBackground(CreateShape(ShapeType.Rectangle, (int)CustomNavigationPage.GetTitleBorderWidth(lastPage), (int)CustomNavigationPage.GetTitleBorderCornerRadius(lastPage), CustomNavigationPage.GetTitleFillColor(lastPage), CustomNavigationPage.GetTitleBorderColor(lastPage)));

            }
            else if (e.PropertyName == CustomNavigationPage.TitleBorderColorProperty.PropertyName)
            {

                _titleViewLayout?.SetBackground(CreateShape(ShapeType.Rectangle, (int)CustomNavigationPage.GetTitleBorderWidth(lastPage), (int)CustomNavigationPage.GetTitleBorderCornerRadius(lastPage), CustomNavigationPage.GetTitleFillColor(lastPage), CustomNavigationPage.GetTitleBorderColor(lastPage)));

            }
            else if (e.PropertyName == CustomNavigationPage.TitleFillColorProperty.PropertyName)
            {

                _titleViewLayout?.SetBackground(CreateShape(ShapeType.Rectangle, (int)CustomNavigationPage.GetTitleBorderWidth(lastPage), (int)CustomNavigationPage.GetTitleBorderCornerRadius(lastPage), CustomNavigationPage.GetTitleFillColor(lastPage), CustomNavigationPage.GetTitleBorderColor(lastPage)));

            }
            else if (e.PropertyName == CustomNavigationPage.TitlePositionProperty.PropertyName)
            {
                UpdateTitleViewLayoutAlignment(_titleViewLayout, _titleTextView, _subTitleTextView, CustomNavigationPage.GetTitlePosition(lastPage));
            }
            else if (e.PropertyName == CustomNavigationPage.TitlePaddingProperty.PropertyName)
            {
                UpdateTitleViewLayoutPadding(_titleViewLayout, CustomNavigationPage.GetTitlePadding(lastPage));
            }
            else if (e.PropertyName == CustomNavigationPage.TitleMarginProperty.PropertyName)
            {
                UpdateTitleViewLayoutMargin(_titleViewLayout, CustomNavigationPage.GetTitleMargin(lastPage));
            }
            else if (e.PropertyName == CustomNavigationPage.TitleColorProperty.PropertyName)
            {
                UpdateToolbarTextColor(_titleTextView, CustomNavigationPage.GetTitleColor(lastPage), _originalColorStateList);
            }
            else if (e.PropertyName == CustomNavigationPage.TitleFontProperty.PropertyName)
            {
                UpdateToolbarTextFont(_titleTextView, CustomNavigationPage.GetTitleFont(lastPage), _originalFont);

            }
            else if (e.PropertyName == Page.TitleProperty.PropertyName)
            {
                UpdateTitleText(_titleTextView, lastPage.Title);

            }
            else if (e.PropertyName == CustomNavigationPage.SubtitleColorProperty.PropertyName)
            {
                UpdateToolbarTextColor(_subTitleTextView, CustomNavigationPage.GetSubtitleColor(lastPage), _originalColorStateList);
            }
            else if (e.PropertyName == CustomNavigationPage.SubtitleFontProperty.PropertyName)
            {
                UpdateToolbarTextFont(_subTitleTextView, CustomNavigationPage.GetSubtitleFont(lastPage), _originalFont);
            }
        }

        public override void OnViewRemoved(Android.Views.View child)
        {
            base.OnViewRemoved(child);
            if (child.GetType() == typeof(Android.Support.V7.Widget.Toolbar))
            {
                if (_toolbar != null)
                {
                    var lastPage = Element?.Navigation?.NavigationStack?.Last();
                    _toolbar.ChildViewAdded -= OnToolbarChildViewAdded;
                }
            }
        }
        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            if (child.GetType() == typeof(Android.Support.V7.Widget.Toolbar))
            {
                _toolbar = (Android.Support.V7.Widget.Toolbar)child;
                _originalToolbarBackground = _toolbar.Background;

                var originalContent = (Context as Activity)?.Window?.DecorView?.FindViewById<FrameLayout>(Window.IdAndroidContent);
                if (originalContent != null)
                {
                    _originalWindowContent = originalContent.Foreground;
                }

                _leftMenuLayout = new Android.Widget.LinearLayout(_toolbar.Context)
                {
                    Orientation = Android.Widget.Orientation.Horizontal,
                    LayoutParameters = new Android.Widget.FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent),
                };

                _rightMenuLayout = new Android.Widget.LinearLayout(_toolbar.Context)
                {
                    Orientation = Android.Widget.Orientation.Horizontal,
                    LayoutParameters = new Android.Widget.FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent),
                };

                //foreach (var item in Element.CurrentPage.ToolbarItems)
                //{
                //    if (item.Priority == 0)
                //    {
                //        _leftMenuLayout.AddView(GetButton(item));
                //    }
                //    else
                //    {
                //        _rightMenuLayout.AddView(GetButton(item));
                //    }
                //}

                //Create custom title view layout
                _titleViewLayout = new Android.Widget.LinearLayout(_toolbar.Context)
                {
                    Orientation = Android.Widget.Orientation.Vertical,
                    LayoutParameters = new Android.Widget.FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent),
                };

                //Create custom title text view
                _titleTextView = new TextView(_titleViewLayout.Context)
                {
                    Gravity = GravityFlags.CenterHorizontal,
                    LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent),
                };

                //Create custom subtitle text view
                _subTitleTextView = new TextView(_titleViewLayout.Context)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
                };

                _titleViewLayout.AddView(_titleTextView);
                _titleViewLayout.AddView(_subTitleTextView);

                _toolbar.AddView(_titleViewLayout);
                _toolbar.AddView(_leftMenuLayout);
                _toolbar.AddView(_rightMenuLayout);

                _toolbar.ChildViewAdded += OnToolbarChildViewAdded;
            }
        }

        private Android.Widget.Button GetButton(ToolbarItem item)
        {
            var button = new Android.Widget.Button(_rightMenuLayout.Context)
            {
                Text = item.Text
            };
            button.Click += (object sender, System.EventArgs e) => {
                item.Activate();
            };
            //button.SetTextColor(Element.BarTextColor.ToAndroid());
            button.SetTextColor(Android.Graphics.Color.Red);
            button.SetBackgroundColor(Android.Graphics.Color.Transparent);
            button.SetMinWidth(10);
            button.SetMinimumWidth(10);
            button.SetPadding(5, 0, 5, 0);
            button.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            return button;
        }

        void SetupToolbarCustomization(Page lastPage)
        {
            if (lastPage != null && _titleViewLayout != null)
            {
                UpdateTitleViewLayout(lastPage, _titleViewLayout, _titleTextView, _subTitleTextView, _originalDrawable);

                UpdateToolbarTitle(lastPage, _titleTextView, _subTitleTextView, _originalFont, _originalColorStateList);

                UpdateToolbarStyle(_toolbar, lastPage, Context as Activity, _originalToolbarBackground, _originalWindowContent);
            }
        }

        #region Title View Layout
        void UpdateTitleViewLayout(Page lastPage, Android.Widget.LinearLayout titleViewLayout, Android.Widget.TextView titleTextView, Android.Widget.TextView subTitleTextView, Android.Graphics.Drawables.Drawable defaultBackground)
        {
            UpdateTitleViewLayoutAlignment(titleViewLayout, titleTextView, subTitleTextView, CustomNavigationPage.GetTitlePosition(lastPage));

            if (!string.IsNullOrEmpty(CustomNavigationPage.GetTitleBackground(lastPage)))
            {
                UpdateTitleViewLayoutBackground(titleViewLayout, CustomNavigationPage.GetTitleBackground(lastPage), defaultBackground);
            }
            else
            {
                _titleViewLayout?.SetBackground(CreateShape(ShapeType.Rectangle, (int)CustomNavigationPage.GetTitleBorderWidth(lastPage), (int)CustomNavigationPage.GetTitleBorderCornerRadius(lastPage), CustomNavigationPage.GetTitleFillColor(lastPage), CustomNavigationPage.GetTitleBorderColor(lastPage)));
            }

            UpdateTitleViewLayoutMargin(titleViewLayout, CustomNavigationPage.GetTitleMargin(lastPage));

            UpdateTitleViewLayoutPadding(titleViewLayout, CustomNavigationPage.GetTitlePadding(lastPage));
        }

        void UpdateTitleViewLayoutAlignment(LinearLayout titleViewLayout, Android.Widget.TextView titleTextView, Android.Widget.TextView subTitleTextView, CustomNavigationPage.TitleAlignment alignment)
        {
            var titleViewParams = titleViewLayout.LayoutParameters as Android.Support.V7.App.ActionBar.LayoutParams;
            var titleTextViewParams = titleTextView.LayoutParameters as LinearLayout.LayoutParams;
            var subTitleTextViewParams = subTitleTextView.LayoutParameters as LinearLayout.LayoutParams;
            var leftMenuParams = _leftMenuLayout.LayoutParameters as Android.Support.V7.App.ActionBar.LayoutParams;
            var rightMenuParams = _rightMenuLayout.LayoutParameters as Android.Support.V7.App.ActionBar.LayoutParams;

            switch (alignment)
            {
                case CustomNavigationPage.TitleAlignment.Start:
                    titleViewParams.Gravity = (int)GravityFlags.Start;
                    titleTextViewParams.Gravity = GravityFlags.Start;
                    subTitleTextViewParams.Gravity = GravityFlags.Start;

                    break;
                case CustomNavigationPage.TitleAlignment.Center:
                    leftMenuParams.Gravity = (int)GravityFlags.Start;
                    rightMenuParams.Gravity = (int)GravityFlags.End;
                    rightMenuParams.MarginEnd = 20;
                    titleViewParams.Gravity = (int)GravityFlags.Center;
                    titleTextViewParams.Gravity = GravityFlags.Center;
                    subTitleTextViewParams.Gravity = GravityFlags.Center;
                    break;
                case CustomNavigationPage.TitleAlignment.End:
                    titleViewParams.Gravity = (int)GravityFlags.End;
                    titleTextViewParams.Gravity = GravityFlags.End;
                    subTitleTextViewParams.Gravity = GravityFlags.End;
                    break;

            }


            titleViewLayout.LayoutParameters = titleViewParams;
        }
        void UpdateTitleViewLayoutBackground(LinearLayout titleViewLayout, string backgroundResource, Android.Graphics.Drawables.Drawable defaultBackground)
        {
            if (!string.IsNullOrEmpty(backgroundResource))
            {
                titleViewLayout?.SetBackgroundResource(this.Context.Resources.GetIdentifier(backgroundResource, "drawable", Android.App.Application.Context.PackageName));
            }
            else
            {
                titleViewLayout?.SetBackground(defaultBackground);
            }
        }
        void UpdateTitleViewLayoutPadding(LinearLayout titleViewLayout, Thickness padding)
        {
            titleViewLayout?.SetPadding((int)padding.Left, (int)padding.Top, (int)padding.Right, (int)padding.Bottom);
        }

        void UpdateTitleViewLayoutMargin(LinearLayout titleViewLayout, Thickness margin)
        {
            var titleViewParams = titleViewLayout.LayoutParameters as Android.Support.V7.App.ActionBar.LayoutParams;

            titleViewParams?.SetMargins((int)margin.Left, (int)margin.Top, (int)margin.Right, (int)margin.Bottom);
            titleViewLayout.LayoutParameters = titleViewParams;
        }
        #endregion
        #region Toolbar 
        void UpdateToolbarStyle(Android.Support.V7.Widget.Toolbar toolbar, Page lastPage, Activity activity, Android.Graphics.Drawables.Drawable defaultBackground, Android.Graphics.Drawables.Drawable windowContent)
        {
            UpdateToolbarBackground(toolbar, lastPage, activity, defaultBackground);
            UpdateToolbarShadow(toolbar, CustomNavigationPage.GetHasShadow(lastPage), activity, windowContent);
        }
        void UpdateToolbarBackground(Android.Support.V7.Widget.Toolbar toolbar, Page lastPage, Activity activity, Android.Graphics.Drawables.Drawable defaultBackground)
        {

            if (string.IsNullOrEmpty(CustomNavigationPage.GetBarBackground(lastPage)) && CustomNavigationPage.GetGradientColors(lastPage) == null)
            {

                toolbar.SetBackground(defaultBackground);
            }
            else
            {
                if (!string.IsNullOrEmpty(CustomNavigationPage.GetBarBackground(lastPage)))
                {
                    toolbar.SetBackgroundResource(this.Context.Resources.GetIdentifier(CustomNavigationPage.GetBarBackground(lastPage), "drawable", Android.App.Application.Context.PackageName));
                }

                if (CustomNavigationPage.GetGradientColors(lastPage) != null)
                {
                    var colors = CustomNavigationPage.GetGradientColors(lastPage);
                    var direction = GradientDrawable.Orientation.TopBottom;
                    switch (CustomNavigationPage.GetGradientDirection(lastPage))
                    {
                        case CustomNavigationPage.GradientDirection.BottomToTop:
                            direction = GradientDrawable.Orientation.BottomTop;
                            break;
                        case CustomNavigationPage.GradientDirection.RightToLeft:
                            direction = GradientDrawable.Orientation.RightLeft;
                            break;
                        case CustomNavigationPage.GradientDirection.LeftToRight:
                            direction = GradientDrawable.Orientation.LeftRight;
                            break;
                    }

                    GradientDrawable gradient = new GradientDrawable(direction, new int[] { colors.Item1.ToAndroid().ToArgb(), colors.Item2.ToAndroid().ToArgb() });
                    gradient.SetCornerRadius(0f);
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
                    {
                        toolbar.SetBackground(gradient);
                    }
                    else
                    {
                        toolbar.SetBackgroundDrawable(gradient);
                    }
                }
            }
            toolbar.Background.SetAlpha((int)(CustomNavigationPage.GetBarBackgroundOpacity(lastPage) * 255));

        }

        void UpdateToolbarShadow(Android.Support.V7.Widget.Toolbar toolbar, bool hasShadow, Activity activity, Android.Graphics.Drawables.Drawable windowContent)
        {
            var androidContent = activity?.Window?.DecorView?.FindViewById<FrameLayout>(Window.IdAndroidContent);
            if (androidContent != null)
            {
                if (hasShadow && activity != null)
                {

                    GradientDrawable shadowGradient = new GradientDrawable(GradientDrawable.Orientation.RightLeft, new int[] { Android.Graphics.Color.Transparent.ToArgb(), Android.Graphics.Color.Gray.ToArgb() });
                    shadowGradient.SetCornerRadius(0f);


                    androidContent.Foreground = shadowGradient;

                    toolbar.Elevation = 4;
                }
                else
                {

                    androidContent.Foreground = windowContent;

                    toolbar.Elevation = 0;
                }
            }

        }
        #endregion
        #region Title TextView
        void UpdateToolbarTitle(Page lastPage, Android.Widget.TextView titleTextView, Android.Widget.TextView subTitleTextView, Typeface originalFont, ColorStateList defaultColorStateList)
        {
            subTitleTextView.TextFormatted = new Java.Lang.String("");
            subTitleTextView.Text = string.Empty;
            subTitleTextView.Visibility = ViewStates.Gone;

            //Update main title text
            UpdateTitleText(titleTextView, lastPage.Title);

            //Update main title color
            UpdateToolbarTextColor(titleTextView, CustomNavigationPage.GetTitleColor(lastPage), defaultColorStateList);

            //Update main title font
            UpdateToolbarTextFont(titleTextView, CustomNavigationPage.GetTitleFont(lastPage), originalFont);

        }
        void UpdateFormattedTitleText(Android.Widget.TextView titleTextView, FormattedString formattedString, string defaulTitle)
        {
            if (formattedString != null && formattedString.Spans.Count > 0)
            {
                titleTextView.TextFormatted = formattedString.ToAttributed(Font.Default, Xamarin.Forms.Color.Default, titleTextView);
            }
            else
            {
                //Update if not formatted text then update with normal title text
                UpdateTitleText(titleTextView, defaulTitle);
            }

        }
        void UpdateTitleText(Android.Widget.TextView titleTextView, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                titleTextView.Text = text;
            }
            else
            {
                titleTextView.Text = string.Empty;
                titleTextView.TextFormatted = new Java.Lang.String("");
            }
        }

        #endregion
        #region General TextView
        void UpdateToolbarTextColor(Android.Widget.TextView textView, Xamarin.Forms.Color? titleColor, ColorStateList defaultColorStateList)
        {
            if (titleColor != null)
            {
                textView.SetTextColor(titleColor?.ToAndroid() ?? Android.Graphics.Color.White);
            }
            else
            {
                //textView.SetTextColor(defaultColorStateList);
            }
        }
        void UpdateToolbarTextFont(Android.Widget.TextView textView, Font customFont, Typeface originalFont)
        {
            if (customFont != null)
            {
                textView.Typeface = customFont.ToTypeface();

                float tValue = customFont.ToScaledPixel();
                textView.SetTextSize(ComplexUnitType.Sp, tValue);
            }
            else
            {
                textView.Typeface = originalFont;
            }
        }
        void ClearTextView(TextView textView, bool hide)
        {
            textView.TextFormatted = new Java.Lang.String("");
            textView.Text = string.Empty;
            if (hide)
            {
                textView.Visibility = ViewStates.Gone;
            }

        }
        #endregion

        Drawable CreateShape(ShapeType type, int strokeWidth, int cornerRadius, Xamarin.Forms.Color? fillColor, Xamarin.Forms.Color? strokeColor)
        {
            GradientDrawable shape = new GradientDrawable();
            shape.SetShape(type);
            if (fillColor != null)
            {
                shape.SetColor(fillColor?.ToAndroid() ?? Xamarin.Forms.Color.Transparent.ToAndroid());
            }

            if (strokeColor != null)
            {
                shape.SetStroke(strokeWidth, strokeColor?.ToAndroid() ?? Xamarin.Forms.Color.Transparent.ToAndroid());
            }
            shape.SetCornerRadius(cornerRadius);

            return shape;
        }

        private void OnToolbarChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            var view = e.Child.GetType();

            if (e.Child.GetType() == typeof(Android.Support.V7.Widget.AppCompatTextView))
            {
                var textView = (Android.Support.V7.Widget.AppCompatTextView)e.Child;
                textView.Visibility = ViewStates.Gone;
                _originalDrawable = textView.Background;
                _originalFont = textView.Typeface;
                _originalColorStateList = textView.TextColors;

                SetupToolbarCustomization(Element.CurrentPage);
            }
            else if (e.Child.GetType() == typeof(Android.Support.V7.Widget.ActionMenuView))
            {
                var menuView = (Android.Support.V7.Widget.ActionMenuView)e.Child;
                menuView.ChildViewAdded += MenuView_ChildViewAdded;

                SetupToolbarCustomization(Element.CurrentPage);
            }
        }

        private void MenuView_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            var menuView = sender as Android.Support.V7.Widget.ActionMenuView;
            menuView?.Menu?.Clear();

            _leftMenuLayout.RemoveAllViews();
            _rightMenuLayout.RemoveAllViews();
            if (Element?.CurrentPage?.ToolbarItems == null)
                return;
            foreach (var item in Element.CurrentPage.ToolbarItems)
            {
                if (item.Priority == 0)
                {
                    _leftMenuLayout.AddView(GetButton(item));
                }
                else
                {
                    _rightMenuLayout.AddView(GetButton(item));
                }
            }
        }
    }
}