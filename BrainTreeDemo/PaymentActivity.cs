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
using Android.Webkit;
using Android.Util;
using Android.Content.Res;
using System.IO;

namespace BrainTreeDemo
{
    [Activity(Label = "PaymentActivity")]
    public class PaymentActivity : Activity
    {
        private WebView wv_webview;
        private string clientToken = "";
        private string html = "";
        //private String html = @"
        //    <head>
        //        <meta charset=""utf-8"">
        //        <script src=""https://js.braintreegateway.com/web/dropin/1.6.1/js/dropin.min.js"" ></script>
        //    </head>
        //    <body>
        //        <div id=""dropin-container""></div>
        //        <button id=""submit-button"">Request payment method</button>
        //        <script>
        //            var button = document.querySelector('#submit-button');
        //            braintree.dropin.create({
        //                authorization: 'CLIENT_TOKEN_FROM_SERVER',
        //                container: '#dropin-container'
        //            }, function(createErr, instance) {
        //                    button.addEventListener('click', function() {
        //                    instance.requestPaymentMethod(function(err, payload) {
        //                    // Submit payload.nonce to your server
        //                    });
        //                });
        //            });
        //        </script>
        //    </body>";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PaymentView);

            Init();

            if (clientToken != "")
            {
                LoadPage();
            }
        }

        private void LoadPage()
        {
            LoadAssetHtml();

            string replacedHtml = ReplaceStringInHtml();
            Log.Debug("PAYMENT", replacedHtml);
            wv_webview.LoadDataWithBaseURL("", replacedHtml, "text/html", "utf-8", "");
        }

        private void LoadAssetHtml()
        {
            AssetManager assetManager = this.Assets;
            using (StreamReader sr = new StreamReader(assetManager.Open("dropin.html")))
            {
                html = sr.ReadToEnd();
            }
        }

        private void Init()
        {
            clientToken = Intent.GetStringExtra("CLIENTTOKEN") ?? "";
            wv_webview = FindViewById<WebView>(Resource.Id.wv_webview);
            wv_webview.Settings.JavaScriptEnabled = true;
            //wv_webview.AddJavascriptInterface(new JSRestartInterface(this), "JSRestart");

        }

        private string ReplaceStringInHtml()
        {
            int imageWidth = Resources.DisplayMetrics.WidthPixels / 4;
            StringBuilder builder = new StringBuilder(html);
            builder.Replace("CLIENT_TOKEN_FROM_SERVER", clientToken);
            return builder.ToString();
        }
    }
}