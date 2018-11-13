
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



namespace frankensteinsmonster
{
    [Activity(Label = "Enigma_Activity")]
    public class Enigma_Activity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Enigma_Layout);
            // Get our UI controls from the loaded layout
            Button reset_button = FindViewById<Button>(Resource.Id.resetBttn);
            EditText message_txt = FindViewById<EditText>(Resource.Id.messageText);
            EditText rotor_order = FindViewById<EditText>(Resource.Id.rotor_order);
            EditText top_letter = FindViewById<EditText>(Resource.Id.top_letter);
            EditText reflect_order = FindViewById<EditText>(Resource.Id.reflect_pairs);
            Button submit_button = FindViewById<Button>(Resource.Id.submitButton);


            Enigma.Enigma_Methods obj = new Enigma.Enigma_Methods();

            /* List<int> reflector_order = new List<int>();
             List<int> encrypt_blocks_10 = new List<int>();
             List<string> encrypt_blocks_27 = new List<string>();

             //EditText  message = FindViewById<EditText>(Resource.Id.messageText);
             // this is how it originally looked and may need to return to this if
             // i'm not getting updated user entries.
             // variables to track whether we are on the first click of a given text box
             int message_clicks = 0;
             int block_size_clicks = 0;
             int user_27_10_clicks = 0;
             int pri_key_clicks = 0;
             int pub_key_clicks = 0;
             int user_enc_clicks = 0;
             int out_block_clicks = 0;
             int user_10_27_clicks = 0;

             // if there is a way to standardize the code below, please let us do that.
             // rn this is kinda crap.
             message_txt.Click += (sender, e) =>
             {
                 if (message_clicks == 0)
                 {
                     message_txt.Text = "";
                 }
                 message_clicks++;
             };

             block_size_txt.Click += (sender, e) =>
             {
                 if (block_size_clicks == 0)
                 {
                     block_size_txt.Text = "";
                 }
                 block_size_clicks++;
             };

             user_27_txt.Click += (sender, e) =>
             {
                 if (user_27_10_clicks == 0)
                 {
                     user_27_txt.Text = "";
                 }
                 user_27_10_clicks++;
             };

             pri_key_text.Click += (sender, e) =>
             {
                 if (pri_key_clicks == 0)
                 {
                     pri_key_text.Text = "";
                 }
                 pri_key_clicks++;
             };

             pub_key_text.Click += (sender, e) =>
             {
                 if (pub_key_clicks == 0)
                 {
                     pub_key_text.Text = "";
                 }
                 pub_key_clicks++;
             };

             user_enc_text.Click += (sender, e) =>
             {
                 if (user_enc_clicks == 0)
                 {
                     user_enc_text.Text = "";
                 }
                 user_enc_clicks++;
             };

             output_b_txt.Click += (sender, e) =>
             {
                 if (out_block_clicks == 0)
                 {
                     output_b_txt.Text = "";
                 }
                 out_block_clicks++;
             };

             user_10_27_txt.Click += (sender, e) =>
             {
                 if (user_10_27_clicks == 0)
                 {
                     user_10_27_txt.Text = "";
                 }
                 user_10_27_clicks++;
             };*/

            //EditText  message = FindViewById<EditText>(Resource.Id.messageText);
            // this is how it originally looked and may need to return to this if
            // i'm not getting updated user entries.

            submit_button.Click += (sender, e) =>
            {
                string message = message_txt.Text;
                string rotor = rotor_order.Text;
                string top_let = top_letter.Text;
                string reflector = reflect_order.Text;

                // sanitize and pad the message
                message = obj.Sanitize(message);
                rotor = obj.Sanitize(rotor);
                top_let = obj.Sanitize(top_let);
                reflector = obj.Sanitize(reflector);
                obj.Order_chars(rotor);
                obj.Order_reflect(reflector);

                // convert message to base 10 block list
                while (obj.Get_order()[0] != top_let[0])
                {
                    obj.Rotate_rotor(1);
                }

                FindViewById<TextView>(Resource.Id.message_restate).Text = message;
                // show some message explaining what they did wrong
                FindViewById<TextView>(Resource.Id.new_message).Text = obj.Scramble(message);
            };
        }
    }
}