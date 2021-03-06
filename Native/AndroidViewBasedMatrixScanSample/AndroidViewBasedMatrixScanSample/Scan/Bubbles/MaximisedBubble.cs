﻿using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using AndroidViewBasedMatrixScanSample.Utility;

namespace AndroidViewBasedMatrixScanSample.Scan.Bubbles
{
    public class MaximisedBubble : BaseBubble
    {
        const int WIDTH = 200;
        const int FULL_INDICATOR_HEIGHT = 104;
        const int INDICATOR_WITHOUT_DELIVERY_HEIGHT = 82;

        readonly ViewStates deliveryVisibility;
        readonly Drawable backgroundDrawable;

        BubbleData bubbleData;

        public MaximisedBubble(Context context, BubbleData bubbleData) : base(context, bubbleData)
        {
            this.bubbleData = bubbleData;
            if (bubbleData.DeliveryDate != string.Empty)
            {
                deliveryVisibility = ViewStates.Visible;
                backgroundDrawable = ContextCompat.GetDrawable(context, Resource.Drawable.bg_transparent_black);
            }
            else
            {
                deliveryVisibility = ViewStates.Gone;
                backgroundDrawable = ContextCompat.GetDrawable(context, Resource.Drawable.bg_rounded_bottom_black);
            }
        }

        public override int GetHeight()
        {
            return bubbleData.DeliveryDate != string.Empty ? FULL_INDICATOR_HEIGHT : INDICATOR_WITHOUT_DELIVERY_HEIGHT;
        }

        public override int GetWidth()
        {
            return WIDTH;
        }

        public override View GetView(Context context, LayoutInflater inflater)
        {
            int targetHeight = GetHeight();
            View view = inflater.Inflate(Resource.Layout.BubbleMaximised, null);

            // XXX A vector drawable has to be loaded with the AppCompatResources.GetDrawable method
            // and then added programatically in order to avoid InflateException.
            view.FindViewById(Resource.Id.triangle).Background = ContextCompat.GetDrawable(context, Resource.Drawable.bg_triangle);

            view.FindViewById(Resource.Id.top).Background = backgroundDrawable;

            ((TextView)view.FindViewById(Resource.Id.stock_header)).SetTextColor(ColorStateList.ValueOf(new Color(highlightColor)));
            ((TextView)view.FindViewById(Resource.Id.stock_value)).Text = bubbleData.Stock.ToString();

            ((TextView)view.FindViewById(Resource.Id.online_header)).SetTextColor(ColorStateList.ValueOf(new Color(highlightColor)));
            ((TextView)view.FindViewById(Resource.Id.online_value)).Text = bubbleData.Online.ToString();

            view.FindViewById(Resource.Id.bottom).Visibility = deliveryVisibility;

            ((TextView)view.FindViewById(Resource.Id.delivery)).Text = bubbleData.DeliveryDate;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                view.FindViewById(Resource.Id.header).BackgroundTintList = ColorStateList.ValueOf(new Color(highlightColor));
                view.FindViewById(Resource.Id.bottom).BackgroundTintList = ColorStateList.ValueOf(new Color(highlightColor));
                view.FindViewById(Resource.Id.triangle).BackgroundTintList = ColorStateList.ValueOf(new Color(highlightColor));
            }

            view.LayoutParameters = new FrameLayout.LayoutParams(
                UiUtils.PxFromDp(context, WIDTH), UiUtils.PxFromDp(context, targetHeight));

            return view;
        }
    }
}
