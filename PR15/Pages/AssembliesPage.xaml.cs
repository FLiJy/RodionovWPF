using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PR15;

namespace PR15.Pages
{
    public partial class AssembliesPage : Page
    {
        private assembly_ _selectedAssembly;

        public AssembliesPage()
        {
            InitializeComponent();
            LoadAssemblies();
        }

        private void LoadAssemblies()
        {
            using (var context = Core.Context)
            {
                DgAssemblies.ItemsSource = context.assembly_.ToList();
            }
        }

        private void DgAssemblies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgAssemblies.SelectedItem is assembly_ selectedAssembly)
            {
                _selectedAssembly = selectedAssembly;
                LoadAssemblyParts(selectedAssembly.id);
            }
        }

        private void LoadAssemblyParts(int assemblyId)
        {
            using (var context = Core.Context)
            {
                var partIds = context.partassembly_
                    .Where(pa => pa.assemblyid == assemblyId)
                    .Select(pa => pa.partid)
                    .ToList();

                var parts = context.basepart_
                    .Include("manufacturer_")
                    .Where(p => partIds.Contains(p.id))
                    .ToList();

                LbAssemblyParts.ItemsSource = parts.Select(p => p.DisplayName).ToList();

                var total = parts.Sum(p => p.price);
                TxtAssemblyTotal.Text = $"Общая стоимость: {total:N0} ₽";
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAssembly == null)
            {
                MessageBox.Show("Выберите сборку для удаления!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить сборку \"{_selectedAssembly.name}\"?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = Core.Context)
                {
                    var partAssemblies = context.partassembly_
                        .Where(pa => pa.assemblyid == _selectedAssembly.id)
                        .ToList();
                    context.partassembly_.RemoveRange(partAssemblies);

                    context.assembly_.Remove(_selectedAssembly);
                    context.SaveChanges();
                }

                LoadAssemblies();
                LbAssemblyParts.ItemsSource = null;
                TxtAssemblyTotal.Text = "";
                _selectedAssembly = null;

                MessageBox.Show("Сборка удалена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}