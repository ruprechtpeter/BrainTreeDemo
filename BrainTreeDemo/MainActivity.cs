using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Util;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Content;
using Braintree;
using Com.Braintreepayments.Api.Models;

namespace BrainTreeDemo
{
    [Activity(Label = "BrainTreeDemo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private static readonly String TAG = typeof(MainActivity).Name;
        private static readonly String PATH_TO_SERVER = "http://192.168.0.111/braintree/";
        private static String clientToken;
        private static readonly int BRAINTREE_REQUEST_CODE = 4949;

        private Button btn_buy;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            getClientTokenFromServer();

            btn_buy = FindViewById<Button>(Resource.Id.buy_now);
            btn_buy.Click += delegate
            {
                onBraintreeSubmit();
            };
        }

        private void getClientTokenFromServer()
        {
            Log.Debug(TAG, "getClientTokenFromServer");
            Task task = new Task(GetClientToken);
            task.Start();
        }

        public void onBraintreeSubmit()
        {
            Intent intent = new Intent(this, typeof(PaymentActivity));
            intent.PutExtra("CLIENTTOKEN", clientToken);
            //intent.PutExtra(BundleCodeConsts.BUNDLE_IMAGE, CapturedImage._file.AbsolutePath.ToString());
            this.StartActivityForResult(intent, 10);

            //var request = new BTDropInRequest();
            //var dropIn = new BTDropInController(clientTokenOrTokenizationKey, request, HandleBTDropInControllerHandler);
            //PresentViewController(dropIn, false, null);


            //DropInRequest dropInRequest = new DropInRequest().clientToken(clientToken);
            //startActivityForResult(dropInRequest.getIntent(this), BRAINTREE_REQUEST_CODE);
        }

        static async void GetClientToken()
        {
            Log.Debug(TAG, "GetClientToken");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(PATH_TO_SERVER))
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = await content.ReadAsStringAsync();

                        if (result != null)
                        {
                            clientToken = result;
                            Log.Debug(TAG, "Client token: " + clientToken);
                        } else
                        {
                            clientToken = "";
                            Log.Debug(TAG, "Client token is null");
                        }
                    }
                } else
                {
                    Log.Debug(TAG, "Unsuccess response: " + response.ToString());
                }
        }
    }
}

