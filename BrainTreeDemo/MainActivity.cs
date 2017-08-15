﻿using Android.App;
using Android.Widget;
using Android.OS;

namespace BrainTreeDemo
{
    [Activity(Label = "BrainTreeDemo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

             SetContentView (Resource.Layout.Main);
        }
    }
}

