using System;
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
using System.Data.Entity;
using AdoptujZwierzaczka.Models;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace AdoptujZwierzaczka
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Pets> Lpets = new List<Pets>();
        List<Pets> LSetPets = new List<Pets>();

        public MainPage()
        {
            this.InitializeComponent();
            InitializePets();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is List<Pets>)
                SetLPets((List<Pets>)e.Parameter);
            base.OnNavigatedTo(e);
        }
        enum Petsy
        {
            defoult, Pies, Kot, Chomik, Settings
        }
        private int pets = (int)Petsy.defoult;

        private void btnPies_Click(object sender, RoutedEventArgs e)
        {
            pets = (int)Petsy.Pies;
            OpenPetListPage();
        }

        private void btnKotek_Click(object sender, RoutedEventArgs e)
        {
            pets = (int)Petsy.Kot;
            OpenPetListPage();
        }
        private void btnChomik_Click(object sender, RoutedEventArgs e)
        {
            pets = (int)Petsy.Chomik;
            OpenPetListPage();
        }

        private void OpenPetListPage()
        {
            //PetListPage petListPage = new PetListPage();
            if (pets == (int)Petsy.Settings)
                this.Frame.Navigate(typeof(AddPetsPage), Lpets);
            else
                this.Frame.Navigate(typeof(PetListPage), LSetPets);

        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void nvPets_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                pets = (int)Petsy.Settings;
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;
                string selectedPet = ((string)selectedItem.Tag);
                //Type pageType = Type.GetType(pageName);
                //contentFrame.Navigate(pageType);
                if (selectedPet == Petsy.Chomik.ToString())
                    pets = (int)Petsy.Chomik;
                else if (selectedPet == Petsy.Kot.ToString())
                    pets = (int)Petsy.Kot;
                else if (selectedPet == Petsy.Pies.ToString())
                    pets = (int)Petsy.Pies;
            }
            //else
            //{
            //    var selectedItem = (NavigationViewItem)args.SelectedItem;
            //    string pageName = "AppUIBasics.SamplePages." + ((string)selectedItem.Tag);
            //    Type pageType = Type.GetType(pageName);
            //    contentFrame.Navigate(pageType);
            //}
            SetLSetPets();
            OpenPetListPage();

        }
        private void InitializePets()
        {
            Lpets.Add(new Pets() { PetId = 1, Name = "Kitka", Type = "kot", Rasa = "dachowiec europejski", Umaszczenie = "szylkretowate", Wiek = 1 });
            Lpets.Add(new Pets() { PetId = 2, Name = "Węgiel", Type = "kot", Rasa = "dachowiec europejski", Umaszczenie = "szylkretowate", Wiek = 1 });
            Lpets.Add(new Pets() { PetId = 3, Name = "Futro", Type = "kot", Rasa = "szkocki zwisłouchy", Umaszczenie = "szylkretowate", Wiek = 1 });
            Lpets.Add(new Pets() { PetId = 4, Name = "Nosek", Type = "kot", Rasa = "brytyjski", Umaszczenie = "szylkretowate", Wiek = 1 });
            Lpets.Add(new Pets() { PetId = 5, Name = "Franek", Type = "chomik", Rasa = "brak", Umaszczenie = "szylkretowate", Wiek = 1 });
            Lpets.Add(new Pets() { PetId = 6, Name = "Zenek", Type = "chomik", Rasa = "dżungarski", Umaszczenie = "szylkretowate", Wiek = 1 });
            Lpets.Add(new Pets() { PetId = 7, Name = "Włochacz", Type = "pies", Rasa = "pudel", Umaszczenie = "szylkretowate", Wiek = 1 });
            Lpets.Add(new Pets() { PetId = 8, Name = "Reksio", Type = "pies", Rasa = "dkreskówkowy", Umaszczenie = "szylkretowate", Wiek = 1 });
        }
        
        private void SetLPets(List<Pets> lpets)
        {
            Lpets = lpets;
        }
        private void SetLSetPets()
        {
            string pet = "";
            if (pets == (int)Petsy.Chomik)
                pet = "chomik";
            else if (pets == (int)Petsy.Kot)
                pet = "kot";
            else if (pets == (int)Petsy.Pies)
                pet = "pies";

            foreach (var item in Lpets)
            {
                if (item.Type == pet)
                    LSetPets.Add(item);
            }
        }
    }
    
}
