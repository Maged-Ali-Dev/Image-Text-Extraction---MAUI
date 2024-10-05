# Image Text Extraction - MAUI

![Maui7](https://github.com/user-attachments/assets/1dd092a7-28a5-484e-82a3-c6ae5ffc1745)


This project a MAUI (Multi-platform App UI) application built using .NET 8. The goal of the project is to extract text from images using OCR (Optical Character Recognition), specifically leveraging the Tesseract OCR library. The app allows users to select an image, extract text from it in various languages, and copy the extracted text to the clipboard.

Here’s a breakdown of how the app works:

1. XAML Structure (UI)

The UI is defined in XAML (Extensible Application Markup Language), which is a common way of designing user interfaces in .NET applications. Here's what the UI consists of:

a. Grid Layout
   - The `Grid` has 7 rows and 2 columns. Various UI elements are placed in this grid to organize the layout.
   
b. Instruction Label
   - This label provides guidance to the user, asking them to select an image with clear text. It also displays the title of the app and developer information.

c. Image Box
   - The `<Image>` control (`selectedImage`) is used to display the selected image. The user selects an image, and it is displayed here.

d. Select Image Button
   - The first button (`btnSelectImage`) allows the user to select an image file from their device. When clicked, the `btnSelectImage_Click` event handler is triggered.

e. Extract Text Button
   - The second button (`btnExtractText`) initiates the text extraction process. When clicked, it calls the `btnExtractText_Click` event handler.

f. Rich Text Box
   - The `<Editor>` (`richTextBox1`) is a read-only text area where the extracted text is displayed after processing.

g. Copy Text Button
   - The third button (`button1`) copies the extracted text to the clipboard for the user to paste elsewhere.

h. Language Picker
   - The `<Picker>` (`languagePicker`) allows the user to select the language for text extraction (English, Arabic, French, or Spanish). The selected language is tied to how Tesseract processes the text.

i. Loading Label
   - The `<Label>` (`loadingLabel`) displays a loading message while the text extraction process is running.

---

2. Code-Behind (C#)
The functionality of the app is managed in the MainPage.xaml.cs file, where event handlers and logic are defined.

a. MainPage Constructor
   - The constructor (`MainPage`) initializes the page. It also sets the default language for OCR to English (`"eng"`).

b. Image Selection (btnSelectImage_Click)
   - When the user clicks the "Select Image" button, this method is triggered.
   - The method checks if a language is selected using the `languagePicker`. If not, it shows an alert.
   - If a language is selected, the method opens a file picker, allowing the user to choose an image file. Once an image is selected, it is loaded and displayed in the `<Image>` control.

c. Text Extraction (btnExtractText_Click)
   - This is the core OCR functionality. After the user selects an image and clicks "Extract Text":
     - The method checks if a language is selected and if an image has been chosen. If not, it displays appropriate alerts.
     - It then begins the OCR process using the Tesseract OCR library.
     - The selected image is converted to a byte array, which Tesseract can process.
     - A folder (`tessdata`) must exist in the app’s directory, containing the trained language data required by Tesseract. The path to this folder is set using the `AppContext.BaseDirectory` method.
     - The OCR process runs in a separate thread (using `Task.Run`) to avoid freezing the UI.
     - After OCR is complete, the extracted text is displayed in the `<Editor>` control (`richTextBox1`).

d. Language Selection (languagePicker_SelectedIndexChanged)
   - When the user selects a language from the picker, this method is triggered. It maps the selected language to the appropriate language code for Tesseract:
     - English: `"eng"`
     - Arabic: `"ara"`
     - French: `"fra"`
     - Spanish: `"spa"`

e. Copy Text to Clipboard (button1_Click)
   - This method allows the user to copy the extracted text to the clipboard. If there is text in the `<Editor>`, it is copied using the `Clipboard.SetTextAsync` method. An alert is displayed to notify the user of success.

---

3. Dependencies
   - Tesseract OCR Library: This is the main library responsible for the text extraction process. It requires language data files (stored in the `tessdata` folder) that correspond to the selected language.
   - FilePicker: This is used for selecting image files from the device.
   - Clipboard: This feature allows the user to copy extracted text for further use.

---

4. Key Features

- Image Selection: Users can choose images from their local files.
- Language Support: The app supports multiple languages (English, Arabic, French, Spanish) for text extraction.
- Text Extraction: The app uses the Tesseract engine to extract text from the selected image.
- Copy to Clipboard: Users can easily copy the extracted text for use in other applications.

---

5. Possible Enhancements
- Error Handling: Improve error handling for cases like missing `tessdata`, unsupported image formats, or low-quality images.
- UI Improvements: Add progress indicators or animations to improve user experience during long OCR operations.
- Additional Language Support: Allow the user to add more languages by downloading additional `tessdata` files.

 
