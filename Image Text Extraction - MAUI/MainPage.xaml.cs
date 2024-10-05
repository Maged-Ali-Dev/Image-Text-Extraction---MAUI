using System;
using System.IO;
using Microsoft.Maui.Controls;
using Tesseract;
 

namespace Image_Text_Extraction___MAUI
{
    public partial class MainPage : ContentPage
    {
        private ImageSource selectedImageSource;
        private string selectedLanguage = "eng"; // Default to English

        public MainPage()
        {
            InitializeComponent();

        }




        private async void btnSelectImage_Click(object sender, EventArgs e)
        {
            if (languagePicker.SelectedItem == null)
            {
                await DisplayAlert("Information", "Please select a language first.", "OK");
                return;
            }

            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image file",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    // Load the selected image
                    selectedImageSource = ImageSource.FromFile(result.FullPath);
                    selectedImage.Source = selectedImageSource;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void btnExtractText_Click(object sender, EventArgs e)
        {
            if (languagePicker.SelectedItem == null)
            {
                await DisplayAlert("Information", "Please select a language first.", "OK");
                return;
            }

            if (selectedImageSource == null)
            {
                await DisplayAlert("Information", "Please select an image first.", "OK");
                return;
            }

            loadingLabel.IsVisible = true;
            richTextBox1.Text = ""; // Clear previous text

            try
            {
                byte[] imageBytes = File.ReadAllBytes(((FileImageSource)selectedImageSource).File);

                string tessDataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");

                if (!Directory.Exists(tessDataPath))
                {
                    await DisplayAlert("Error", $"tessdata folder not found at: {tessDataPath}.", "OK");
                    return;
                }

                await Task.Run(() =>
                {
                    using (var ocrEngine = new TesseractEngine(tessDataPath, selectedLanguage, EngineMode.Default))
                    {
                        using (var pixImage = Pix.LoadFromMemory(imageBytes))
                        {
                            using (var page = ocrEngine.Process(pixImage))
                            {
                                string extractedText = page.GetText();

                                Dispatcher.Dispatch(() =>
                                {
                                    if (string.IsNullOrWhiteSpace(extractedText))
                                    {
                                        DisplayAlert("Error", "No text extracted.", "OK");
                                    }
                                    else
                                    {
                                        richTextBox1.Text = extractedText;
                                    }
                                });
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                loadingLabel.IsVisible = false;
            }
        }


        private void languagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            string language = picker.SelectedItem.ToString();

            selectedLanguage = language switch
            {
                "Arabic" => "ara",
                "French" => "fra",
                "Spanish" => "spa",
                _ => "eng" // Default to English
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                Clipboard.SetTextAsync(richTextBox1.Text);
                DisplayAlert("Success", "Text copied to clipboard!", "OK");
            }
            else
            {
                DisplayAlert("Warning", "There is no text to copy.", "OK");
            }
        }
    }
}
