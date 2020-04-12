using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace Andek
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button copy_button;
        private EditText input_pass;
        private EditText output_pass;
        private CheckBox small_letters_allowed;
        private CheckBox capital_letters_allowed;
        private CheckBox digits_allowed;
        private CheckBox ambigous_characters_allowed;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // DO NOT TOUCH code
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Copy  Button
            copy_button = FindViewById<Button>(Resource.Id.copy_button);
            copy_button.Click += delegate { copy_output(); };

            // Input Password
            input_pass = FindViewById<EditText>(Resource.Id.input_password);
            input_pass.TextChanged += delegate { sync(); };

            // Output Password
            output_pass = FindViewById<EditText>(Resource.Id.output_password);

            // Checkboxes configuration
            small_letters_allowed = FindViewById<CheckBox>(Resource.Id.small_letters_allowed);
            small_letters_allowed.CheckedChange += delegate { sync(); };

            capital_letters_allowed = FindViewById<CheckBox>(Resource.Id.capital_letters_allowed);
            capital_letters_allowed.CheckedChange += delegate { sync(); };

            digits_allowed = FindViewById<CheckBox>(Resource.Id.digits_allowed);
            digits_allowed.CheckedChange += delegate { sync(); };

            ambigous_characters_allowed = FindViewById<CheckBox>(Resource.Id.ambigous_characters_allowed);
            ambigous_characters_allowed.CheckedChange += delegate { sync(); };

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void copy_output()
        {
            Clipboard.SetTextAsync(output_pass.Text);
            Toast.MakeText(Android.App.Application.Context, "copied", ToastLength.Short).Show();
        }
        
        void sync()
        {
            bool small_letters              = small_letters_allowed.Checked;
            bool capital_letters            = capital_letters_allowed.Checked;
            bool digits                     = digits_allowed.Checked;
            bool ambigous_characters        = ambigous_characters_allowed.Checked;

            if(!small_letters && !capital_letters && !digits && !ambigous_characters)
            {
                Toast.MakeText(Android.App.Application.Context, "No avaiable characters, resetting...", ToastLength.Short).Show();

                small_letters_allowed.Checked = true;
                capital_letters_allowed.Checked = true;
                digits_allowed.Checked = true;
                ambigous_characters_allowed.Checked = true;
                sync();
                return;
            }

            output_pass.Text = new PasswordEnchanter(input_pass.Text, small_letters, capital_letters, digits, ambigous_characters).generate_pass();
        }


    }
}