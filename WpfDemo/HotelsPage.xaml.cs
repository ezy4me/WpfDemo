using System;
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
using System.Data.Entity;

namespace WpfDemo
{
    /// <summary>
    /// Логика взаимодействия для HotelsPage.xaml
    /// </summary>
    public partial class HotelsPage : Page
    {
        public static ToursBaseEntities _context = new ToursBaseEntities();
        
        private int _currentPage = 1;
        private int _maxPage = 0;
        public HotelsPage()
        {
            InitializeComponent();
            //DGridHotel.ItemsSource = ToursBaseEntities.GetContext().Hotel.ToList();
            RefreshHotels();
        }
        /// <summary>
        /// Обновление списка отелей(постранично)
        /// </summary>
        public void RefreshHotels()
        {
            DGridHotel.ItemsSource = ToursBaseEntities.GetContext().Hotel.OrderBy(h => h.Name).ToList();
            _maxPage = Convert.ToInt32(Math.Ceiling(ToursBaseEntities.GetContext().Hotel.ToList().Count * 1.0 / 10));

            var listHotels = ToursBaseEntities.GetContext().Hotel.ToList().Skip((_currentPage - 1) * 10).Take(10).ToList();

            LabelText.Content = $"Количество страниц: " + _maxPage.ToString();

            CountPage.Text = _currentPage.ToString();

            DGridHotel.ItemsSource = listHotels;
        }
        /// <summary>
        /// Переход к странице реактирования данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Hotel));          
        }
        /// <summary>
        /// Переход к странице добавления данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));       
        }
        /// <summary>
        /// Удаление выбранного отеля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var deletehotel = DGridHotel.SelectedItems.Cast<Hotel>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {deletehotel.Count()} элемента?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                
                try
                {                   
                    ToursBaseEntities.GetContext().Hotel.RemoveRange(deletehotel);
                    ToursBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация удалена успешно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    DGridHotel.ItemsSource = ToursBaseEntities.GetContext().Hotel.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                ToursBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                RefreshHotels();
            }
        }
        /// <summary>
        /// Переход к первой странице с данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoFirstPage_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            RefreshHotels();
        }
        /// <summary>
        /// Переход к предыдущей странице с данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage - 1 < 1)
            {
                return;
            }
            _currentPage = _currentPage - 1;
            RefreshHotels();
        }
        /// <summary>
        /// Переход к выбранной странице с данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CountPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentPage > 0 && _currentPage <= _maxPage && CountPage.Text != "")
            {
                _currentPage = Convert.ToInt32(CountPage.Text);
                RefreshHotels();
            }
        }
        /// <summary>
        /// Переход к последней странице с данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoLastPage_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = _maxPage;
            RefreshHotels();
        }
        /// <summary>
        /// Переход к следующей странице с данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage + 1 > _maxPage)
            {
                return;
            }
            _currentPage = _currentPage + 1;
            RefreshHotels();
        }
    }
}
