using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PR15.Models;
using PR15.Services;

namespace PR15.Pages
{
    public partial class BuilderPage : Page
    {
        private List<basepart_> _currentAssembly = new List<basepart_>();

        public BuilderPage()
        {
            InitializeComponent();
            LoadFilters();
            LoadParts();
        }

        private void LoadFilters()
        {
            using (var context = Core.Context)
            {
                CboType.ItemsSource = context.parttype_.ToList();
                CboManufacturer.ItemsSource = context.manufacturer_.ToList();
            }
        }

        private void LoadParts()
        {
            using (var context = Core.Context)
            {
                var query = context.basepart_.Include("manufacturer_").Include("parttype_").AsQueryable();

                if (!string.IsNullOrEmpty(TxtSearch.Text))
                    query = query.Where(p => p.name.Contains(TxtSearch.Text));

                if (CboType.SelectedItem != null)
                    query = query.Where(p => p.parttypeid == ((parttype_)CboType.SelectedItem).id);

                if (CboManufacturer.SelectedItem != null)
                    query = query.Where(p => p.manufacturerid == ((manufacturer_)CboManufacturer.SelectedItem).id);

                DgParts.ItemsSource = query.ToList();
            }
        }

        private void DgParts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgParts.SelectedItem is basepart_ selectedPart)
            {
                var existingType = _currentAssembly.FirstOrDefault(p => p.parttypeid == selectedPart.parttypeid);
                if (existingType != null)
                    _currentAssembly.Remove(existingType);

                _currentAssembly.Add(selectedPart);
                UpdateAssemblyView();
                DgParts.SelectedItem = null;
            }
        }

        private void UpdateAssemblyView()
        {
            LbAssembly.ItemsSource = null;
            LbAssembly.ItemsSource = _currentAssembly.ToList();

            var total = _currentAssembly.Sum(p => p.price);
            TxtTotalPrice.Text = $"Итого: {total:N0} ₽";

            var errors = CompatibilityChecker.CheckCompatibility(_currentAssembly);
            TxtErrors.Text = errors.Any() ? string.Join("\n", errors) : "✅ Все компоненты совместимы";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentAssembly.Any())
            {
                MessageBox.Show("Добавьте хотя бы один компонент!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var errors = CompatibilityChecker.CheckCompatibility(_currentAssembly);
            if (errors.Any())
            {
                MessageBox.Show("Нельзя сохранить сборку с ошибками совместимости!\n\n" +
                    string.Join("\n", errors), "Ошибка совместимости",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var saveWindow = new SaveAssemblyWindow();
            if (saveWindow.ShowDialog() == true)
            {
                using (var context = Core.Context)
                {
                    var newAssembly = new assembly_
                    {
                        name = saveWindow.AssemblyName,
                        author = saveWindow.AuthorName
                    };
                    context.assembly_.Add(newAssembly);
                    context.SaveChanges();

                    foreach (var part in _currentAssembly)
                    {
                        context.partassembly_.Add(new partassembly_
                        {
                            partid = part.id,
                            assemblyid = newAssembly.id
                        });
                    }
                    context.SaveChanges();
                }

                MessageBox.Show("Сборка успешно сохранена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                _currentAssembly.Clear();
                UpdateAssemblyView();
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            _currentAssembly.Clear();
            UpdateAssemblyView();
        }

        private void MenuItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (LbAssembly.SelectedItem is basepart_ selectedPart)
            {
                _currentAssembly.Remove(selectedPart);
                UpdateAssemblyView();
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadParts();
        }

        private void CboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadParts();
        }

        private void CboManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadParts();
        }
    }
}