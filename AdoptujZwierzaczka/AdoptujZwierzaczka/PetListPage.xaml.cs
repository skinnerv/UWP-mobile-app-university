using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdoptujZwierzaczka
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class PetListPage : Page
    {
        enum Petsy
        {
            defoult, Pies, Kot, Chomik
        }

        private int IdPet = 0;

        private int SetPet = 0;

        private int CountPet = 0;

        private int pets = (int)Petsy.defoult;

        List<Pets> Lpets = new List<Pets>();


        public PetListPage()
        {
            this.InitializeComponent();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is List<Pets>)
                SetLPets((List<Pets>)e.Parameter);
            base.OnNavigatedTo(e);
            Set_txtblockZwierze();
            SetNextViewPet();

        }
        private void Powrót_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void Set_txtblockZwierze()
        {
            var pet = Lpets[0].Type;
            IdPet = Lpets[0].PetId;
            CountPet = Lpets.Count;
            var komit = "Oglądasz wszystkie dostępne ";
            if (pet == "chomik")
                txtblockZwierze.Text = komit + "chomiczki";
            else if (pet == "kot")
                txtblockZwierze.Text = komit + "koteczki";
            else if (pet == "pies")
                txtblockZwierze.Text = komit + "pieski";
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void SetLPets(List<Pets> lpets)
        {
            Lpets = lpets;
        }

        private void btn_SetPet_Click(object sender, RoutedEventArgs e)
        {
            AddToFavoritePets();

        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            SetPet++;
            if (SetPet >= CountPet)
                SetPet = 0;

            IdPet = Lpets[SetPet].PetId;

            SetNextViewPet();
        }

        private void AddToFavoritePets()
        {
            SendMsgBox();
        }

        private void SetNextViewPet()
        {
            imgPet.Source = new BitmapImage(new Uri("ms-appx:///Assets/TempImg/"+IdPet.ToString()+".jpg"));
            txtblock_Name.Text = "Imię: "+Lpets[SetPet].Name;
            txtblock_Colour.Text ="Umaszczenie: "+ Lpets[SetPet].Umaszczenie;
            txtblock_Rase.Text = "Rasa: "+Lpets[SetPet].Rasa;
        }
        private async void SendMsgBox()
        {
            MessageDialog msgbox = new MessageDialog("Zaopiekowałeś się zwierzątkiem, "+ Lpets[SetPet].Name + " jest Tobie wdzięczny!", "Dziękujemy");

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
