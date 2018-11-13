using System;
using Foundation;
using System.Collections.Generic;
using UIKit;

namespace NumberTheoryApp
{
    public partial class RSAViewController : UIViewController
    {
        public RSAViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            RSACode obj = new RSACode();

            List<int> base_10_blocks = new List<int>();
            List<int> encrypt_blocks_10 = new List<int>();
            List<string> encrypt_blocks_27 = new List<string>();

            bool encrypting = false;

            encButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                encrypting = true;
            };

            decryptButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                encrypting = false;
            };

            submit1.TouchUpInside += (object sender, EventArgs e) =>
            {
                string message = MessageText.Text;
                string temp = obj.Sanitize_int(block_size.Text);
                if (temp.Length == 0)
                {
                    error_field_1.Text = "Invalid block size";
                }
                else
                {
                    int block_size_2 = int.Parse(temp);
                    // sanitize and pad the message
                    message = obj.Pad(obj.Sanitize(message), block_size_2);
                    // convert message to base 10 block list
                    base_10_blocks.Clear();
                    int num_blocks = message.Length / block_size_2;
                    for (int i = 0; i < num_blocks; i++)
                    {
                        base_10_blocks.Add(obj.Base_27_10(obj.Get_block(message, i, block_size_2), block_size_2));
                    }
                    string pub_temp = obj.Sanitize_int(pub_key.Text);
                    if (pub_temp.Length == 0)
                    {
                        error_field_1.Text = "Bad public key";
                    }
                    else
                    {
                        if (encrypting == true)
                        {
                            if (obj.Max_value(block_size_2, 27) < int.Parse(pub_temp))
                            {
                                // display block to convert
                                error_field_1.Text = obj.Get_block(message, 0, block_size_2);
                                // activate second submit button (NEED TO ADD THIS)
                                //error_field_1.Text = num_blocks.ToString();
                            }

                            else
                            {
                                // show some message explaining what they did wrong
                                error_field_1.Text = "Block size and public key incompatible.";
                            }
                        }
                        else
                        {
                            error_field_1.Text = obj.Get_block(message, 0, block_size_2);
                        }
                    }
                }
            };

            submit2.TouchUpInside += (object sender, EventArgs e) =>
            {
                string message = MessageText.Text;
                int block_size_2 = int.Parse(obj.Sanitize_int(block_size.Text));
                message = obj.Pad(obj.Sanitize(message), block_size_2);
                string user_temp = obj.Sanitize_int(base_10.Text);
                if (user_temp.Length == 0)
                {
                    error_field_2.Text = "Bad user input";
                }
                else
                {
                    int user_27_10 = int.Parse(user_temp);
                    int num_blocks = message.Length / block_size_2;
                    if (user_27_10 == base_10_blocks[0])
                    {
                        // display all blocks in base 10
                        string block_str_10 = obj.Pad_number(base_10_blocks[0], block_size_2);
                        for (int i = 1; i < num_blocks; i++)
                        {
                            block_str_10 += " " + obj.Pad_number(base_10_blocks[i], block_size_2);
                        }
                        error_field_2.Text = block_str_10;
                        // activate second submit button (NEED TO ADD THIS)
                    }
                    else

                    {
                        // show some message explaining what they did wrong
                        error_field_2.Text = "Incorrect conversion";
                    }
                }
            };

            submit3.TouchUpInside += (object sender, EventArgs e) =>
            {

                string message = MessageText.Text;
                int block_size_2 = int.Parse(obj.Sanitize_int(block_size.Text));
                message = obj.Pad(obj.Sanitize(message), block_size_2);
                string pri_temp = obj.Sanitize_int(pri_key.Text);
                if (pri_temp.Length == 0)
                {
                    error_field_3.Text = "Bad private key";
                }
                else
                {
                    int pri_key_2 = int.Parse(pri_temp);
                    int pub_key_2 = int.Parse(obj.Sanitize_int(pub_key.Text));
                    string user_enc_temp = obj.Sanitize_int(enc_block.Text);
                    if (user_enc_temp.Length == 0)
                    {
                        error_field_3.Text = "Invalid user encryption";
                    }
                    else
                    {
                        int user_encrypt_10 = int.Parse(user_enc_temp);
                        int num_blocks = message.Length / block_size_2;
                        // create list storing all blocks encrypted in base 10
                        encrypt_blocks_10.Clear();
                        for (int i = 0; i < num_blocks; i++)
                        {
                            encrypt_blocks_10.Add(obj.Fast_exp(base_10_blocks[i], pri_key_2, pub_key_2));
                        }
                        if (user_encrypt_10 == encrypt_blocks_10[0])
                        {
                            // output all encrypted blocks in base 10

                            string block_str_encrypted_10 = obj.Pad_number(encrypt_blocks_10[0], block_size_2); //.ToString();
                            for (int i = 1; i < num_blocks; i++)
                            {
                                block_str_encrypted_10 += " " + obj.Pad_number(encrypt_blocks_10[i], block_size_2); //.ToString();
                            }
                            error_field_3.Text = block_str_encrypted_10;
                            // activate third submit button (NEED TO ADD THIS)
                            //error_field_3.Text = base_10_blocks[2].ToString();
                        }
                        else
                        {
                            // show some message explaining what they did wrong
                            error_field_3.Text = "Incorrect encryption. Try again.";
                        }
                    }
                }
            };

            submit4.TouchUpInside += (object sender, EventArgs e) =>
            {
                int pub_key_2 = int.Parse(obj.Sanitize_int(pub_key.Text));
                string output_temp = obj.Sanitize_int(out_block_size.Text);
                if (output_temp.Length == 0)
                {
                    error_field_4.Text = "Bad output block size";
                }
                else
                {
                    int output_block_size = int.Parse(output_temp);
                    if (encrypting == true)
                    {
                        if (obj.Max_value(output_block_size, 27) >= pub_key_2 - 1)
                        {
                            error_field_4.Text = encrypt_blocks_10[0].ToString();
                        }
                        else
                        {
                            error_field_4.Text = "Output block size incompatible with public key";
                        }
                    }
                    else
                    {
                        error_field_4.Text = encrypt_blocks_10[0].ToString();
                    }
                }
            };

            submit5.TouchUpInside += (object sender, EventArgs e) =>
            {
                string message = obj.Sanitize(MessageText.Text);
                int block_size_2 = int.Parse(obj.Sanitize_int(block_size.Text));
                message = obj.Pad(obj.Sanitize(message), block_size_2);
                int pri_key_2 = int.Parse(obj.Sanitize_int(pri_key.Text));
                int pub_key_2 = int.Parse(obj.Sanitize_int(pub_key.Text));
                int user_encrypt_10 = int.Parse(obj.Sanitize_int(enc_block.Text));
                int output_block_size = int.Parse(obj.Sanitize_int(out_block_size.Text));
                string user_10_27 = base_27.Text;
                int num_blocks = message.Length / block_size_2;
                // create list storing all blocks encrypted in base 10
                encrypt_blocks_27.Clear();
                for (int i = 0; i < num_blocks; i++)
                {
                    encrypt_blocks_27.Add(obj.Base_10_27(encrypt_blocks_10[i], output_block_size));
                }
                if (user_10_27 == encrypt_blocks_27[0])
                {
                    // output all encrypted blocks in base 27
                    string block_str_27 = encrypt_blocks_27[0].ToString();
                    for (int i = 1; i < num_blocks; i++)
                    {
                        block_str_27 += encrypt_blocks_27[i].ToString();
                    }
                    error_field_5.Text = block_str_27;
                }
                else
                {

                    // show some message explaining what they did wrong
                    error_field_5.Text = "Incorrect conversion. Try again.";
                }
            };
        }

        /*submit1.TouchUpInside += (object sender, EventArgs e) => {
                // Convert the phone number with text to a number
                // using PhoneTranslator.cs
                translatedNumber = PhoneTranslator.ToNumber(
                    PhoneNumberText.Text);

                // Dismiss the keyboard if text field was tapped
                PhoneNumberText.ResignFirstResponder();

                if (translatedNumber == "")
                {
                    CallButton.SetTitle("Call ", UIControlState.Normal);
                    CallButton.Enabled = false;
                }
                else
                {
                    CallButton.SetTitle("Call " + translatedNumber,
                        UIControlState.Normal);
                    CallButton.Enabled = true;
                }
            };

            submit2.TouchUpInside += (object sender, EventArgs e) => {
                // Use URL handler with tel: prefix to invoke Apple's Phone app...
                var url = new NSUrl("tel:" + translatedNumber);

                // ...otherwise show an alert dialog
                if (!UIApplication.SharedApplication.OpenUrl(url))
                {
                    var alert = UIAlertController.Create("Not supported", "Scheme 'tel:' is not supported on this device", UIAlertControllerStyle.Alert);
alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
            };
        }*/

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}