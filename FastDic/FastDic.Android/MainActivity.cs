using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;
using System.IO;

namespace FastDic.Droid
{
    [Activity(Label = "FastDic", Icon = "@drawable/FastDicIcon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode,
        LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { Intent.ActionProcessText }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = "text/plain")]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = "text/plain")]
    [IntentFilter(new[] { Intent.ActionSearch }, Categories = new[] { Intent.CategoryDefault })]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        readonly DateTime lastDBModification = new DateTime(2021, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // make sure realm db is extracted and ready for use
            var filePath = "db.sqlite";
            var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), filePath);
            
            
            if (!File.Exists(databasePath))
            {
                ExtractDB(databasePath, filePath);
            }

            // Update the database in case the lastDBModification date is 
            // greater than the last modified date of the db in the device
            var dbInfo = new FileInfo(databasePath);
            if (dbInfo.LastWriteTimeUtc < lastDBModification)
            {
                File.Delete(databasePath);
                ExtractDB(databasePath, filePath);
            }

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(GetAppByIntent());
        }

        private void ExtractDB(string databasePath, string filePath)
        {
            using (var asset = Assets.Open(filePath))
            using (var destination = File.OpenWrite(databasePath))
            {
                asset.CopyTo(destination);
            }
        }

        public App GetAppByIntent()
        {
            string word;
            App.OnFinishActivity += App_OnFinishActivity;
            switch (this.Intent.Action)
            {
                case Intent.ActionSend:
                    word = this.Intent.GetStringExtra(Intent.ExtraText);
                    break;

                case Intent.ActionSearch:
                    word = this.Intent.GetStringExtra(Intent.ExtraContentQuery);
                    break;

                case Intent.ActionProcessText:
                    word = this.Intent.GetStringExtra(Intent.ExtraProcessText);
                    break;

                default:
                    var a = new App();
                    return a;
            }

            var app = new App(word);
            return app;
        }

        private void App_OnFinishActivity()
        {
            this.Finish();
        }
    }
}