﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDemo
{
    /// <summary>
    /// Логика взаимодействия для ToursPage.xaml
    /// </summary>
    public partial class ToursPage : Page
    {
        public ToursPage()
        {
            
            InitializeComponent();
            var currentTours = ToursBaseEntities.GetContext().Tour.ToList();           
            LViewTours.ItemsSource = currentTours;

            var alltypes = ToursBaseEntities.GetContext().Type.ToList();  
            alltypes.Insert(0, new Type
            {
                Name = "Все типы"
            });
            ComboType.ItemsSource = alltypes;
            CheckActual.IsChecked = true;
            ComboType.SelectedIndex = 0;
            UpdateTours();           

        }
        
        /// <summary>
        /// Метод обновения списка туров(при изменении полей навигации, т.е поиска/выбора типа тура/актуальность)
        /// </summary>
        private void UpdateTours()
        {
            var currentTours = ToursBaseEntities.GetContext().Tour.ToList();
            if (ComboType.SelectedIndex > 0)
                currentTours = currentTours.Where(p => p.Type.Contains(ComboType.SelectedItem as Type)).ToList();

            currentTours = currentTours.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            if (CheckActual.IsChecked.Value)
                currentTours = currentTours.Where(p => p.IsActual).ToList();
            LViewTours.ItemsSource = currentTours.OrderBy(p => p.TicketCount).ToList();
            
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTours();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTours();
        }

        private void CheckActual_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTours();
        }

        private void CheckActual_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateTours();
        }
        
    }
}
