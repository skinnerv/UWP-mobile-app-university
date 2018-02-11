using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Graphics.Display;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdoptujZwierzaczka
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class AddPetsPage : Page
    {
        public AddPetsPage()
        {
            this.InitializeComponent();
        }
        List<Pets> LPets = new List<Pets>();

        public string TempImgPathSave = null;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LPets = (List<Pets>)e.Parameter;
            base.OnNavigatedTo(e);
        }

        private void Powrót_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), LPets);
        }

        private void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "Copy";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private async void StackPanel_Drop(object sender, DragEventArgs e)
        {
            //img1.Source = sourceImg.Source;
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var images = await e.DataView.GetStorageItemsAsync();
                if (images.Any())
                {
                    var storageFile = images[0] as StorageFile;
                    var bitMapImage = new BitmapImage();
                    bitMapImage.SetSource(await storageFile.OpenAsync(FileAccessMode.Read));
                    img1.Source = bitMapImage;
                }
            }
        }

        private async void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DisplayInformation display = DisplayInformation.GetForCurrentView();
                var renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(img1, (int)img1.Width, (int)img1.Height);
                IBuffer pixels = await renderTargetBitmap.GetPixelsAsync();
                byte[] bytes = pixels.ToArray();
                var Picker = new FileSavePicker();
                //Picker.FileTypeChoices.Add("Image", new List<string>() { ".png" });
                string name = txtbox_Name.ToString() + txtbox_Type.ToString() + txtbox_Umaszczenie.ToString() + txtbox_Age.ToString();
                var file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(name, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                //file.Path = "";
                //StorageFile file = await Picker.PickSaveFileAsync();
                //StorageFile file = new StorageFile();
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)img1.ActualWidth, (uint)img1.ActualHeight, display.LogicalDpi, display.LogicalDpi, bytes);
                    // If your Image control have a fixed size use 
                    // encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)ImageControl.Width, (uint)ImageControl.Height, display.LogicalDpi, display.LogicalDpi, bytes);
                    await encoder.FlushAsync();

                    TempImgPathSave = file.Path;
                    // TODO: Zmienić ścieżkę zapisu na jakiś bardziej normalny folder
                }
            }
            catch (Exception ex)
            {

            }
            //AddToBase();
            AddToList();

        }
        private void AddToBase()
        {
            using(var context = new Models.PetContext())
            {
                string name = txtbox_Name.ToString();
                string type = txtbox_Type.ToString();
                string rase = txtbox_Rase.ToString();
                int age = int.Parse(txtbox_Age.ToString());
                string colour = txtbox_Umaszczenie.ToString();
                string path = TempImgPathSave;

                context.Pets.Add(new Models.Pet() { Name = name, PathToImg = path, Rasa = rase, Type = type, Umaszczenie = colour, Wiek = age });
                context.SaveChanges();
            };
        }
        private void AddToList()
        {
            int id = LPets.Count() + 2;
            string name = txtbox_Name.Text.ToString();
            string type = txtbox_Type.Text.ToString();
            string rase = txtbox_Rase.Text.ToString();
            bool bage = int.TryParse(txtbox_Age.Text.ToString(), out int age);
            string colour = txtbox_Umaszczenie.ToString();

            LPets.Add(new Pets()
            {
                //PetId = id,
                //Name = txtbox_Name.ToString(),
                //Rasa = txtbox_Rase.ToString(),
                //Type = txtbox_Type.ToString(),
                //Umaszczenie = txtbox_Umaszczenie.ToString(),
                //Wiek = int.Parse(txtbox_Age.ToString())
                PetId = id,
                Name = name,
                Type = type,
                Rasa = rase,
                Wiek = age,
                Umaszczenie = colour
            });

            ClearInputField();

            SendMsgBox();
        }
        private void ClearInputField()
        {
            txtbox_Age.Text = "";
            txtbox_Age.Text = "";
            txtbox_Name.Text = "";
            txtbox_Rase.Text = "";
            txtbox_Type.Text = "";
            txtbox_Umaszczenie.Text = "";
            img1.Source = new BitmapImage(new Uri("ms-appx:///Assets/dropPictureHere.png"));
        }

        private async void SendMsgBox()
        {
            MessageDialog msgbox = new MessageDialog("Dodano pomyślnie do bazy", "Dodano do bazy");

            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
            //msgbox.Commands.Add(new UICommand { Label = "No", Id = 1 });
            var res = await msgbox.ShowAsync();
            //if ((int)res.Id == 0)
            //    this.Frame.Navigate(typeof(MainPage));
            //else if ((int)res.Id == 1)
            //    CoreApplication.Exit();
        }
    }
}
