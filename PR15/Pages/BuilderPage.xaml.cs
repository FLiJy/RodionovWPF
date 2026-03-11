using PR15.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                var query = context.basepart_
                    .Include("manufacturer_")
                    .Include("parttype_")
                    .AsQueryable();

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

        private void LbAssembly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbAssembly.SelectedItem is basepart_ selectedPart)
            {
                DisplaySpecifications(selectedPart);
            }
        }

        private void DisplaySpecifications(basepart_ part)
        {
            using (var context = Core.Context)
            {
                var specs = new List<string>();

                switch (part.parttypeid)
                {
                    case 1: // CPU
                        var cpu = context.cpu_.FirstOrDefault(c => c.id == part.id);
                        if (cpu != null)
                        {
                            var socket = context.socket_.FirstOrDefault(s => s.id == cpu.socketid);
                            var igpu = context.igpu_.FirstOrDefault(i => i.id == cpu.igpuid);

                            specs.Add($"Сокет: {socket?.name ?? "N/A"}");
                            specs.Add($"Ядер: {cpu.numberofcores}");
                            specs.Add($"Базовая частота: {cpu.basecorefrequency} ГГц");
                            specs.Add($"Макс. частота: {cpu.maxcorefrequency} ГГц");
                            specs.Add($"Кэш L3: {cpu.cachel3} МБ");
                            specs.Add($"TDP: {cpu.thermalpower} Вт");
                            if (cpu.hasigpu)
                                specs.Add($"Встроенная графика: {igpu?.name ?? "Есть"}");
                        }
                        break;

                    case 2: // GPU
                        var gpu = context.gpu_.FirstOrDefault(g => g.id == part.id);
                        if (gpu != null)
                        {
                            var gpuInterface = context.gpuinterface_.FirstOrDefault(gi => gi.id == gpu.gpuinterfaceid);
                            specs.Add($"Интерфейс: {gpuInterface?.name ?? "N/A"}");
                            specs.Add($"Частота чипа: {gpu.chipfrequency} МГц");
                            specs.Add($"Видеопамять: {gpu.videomemory} ГБ");
                            specs.Add($"Шина памяти: {gpu.memorybus} бит");
                            if (gpu.recommendpower.HasValue)
                                specs.Add($"Рекомендуемый БП: {gpu.recommendpower} Вт");
                        }
                        break;

                    case 3: // RAM
                        var ram = context.ram_.FirstOrDefault(r => r.id == part.id);
                        if (ram != null)
                        {
                            var memoryType = context.memorytype_.FirstOrDefault(mt => mt.id == ram.memorytypeid);
                            specs.Add($"Тип памяти: {memoryType?.name ?? "N/A"}");
                            specs.Add($"Объём: {ram.capacity} ГБ");
                            specs.Add($"Количество модулей: {ram.count}");
                            specs.Add($"Частота: {ram.ghz} МГц");
                            specs.Add($"Тайминги: {ram.timings}");
                        }
                        break;

                    case 4: // Motherboard
                        var mb = context.motherboard_.FirstOrDefault(m => m.id == part.id);
                        if (mb != null)
                        {
                            var socket = context.socket_.FirstOrDefault(s => s.id == mb.socketid);
                            var formFactor = context.formfactor_.FirstOrDefault(ff => ff.id == mb.formfactorid);
                            var memoryType = context.memorytype_.FirstOrDefault(mt => mt.id == mb.memorytypeid);

                            specs.Add($"Сокет: {socket?.name ?? "N/A"}");
                            specs.Add($"Форм-фактор: {formFactor?.name ?? "N/A"}");
                            specs.Add($"Тип памяти: {memoryType?.name ?? "N/A"}");
                            specs.Add($"Слотов памяти: {mb.memoryslots}");
                            specs.Add($"PCI слотов: {mb.pcislots}");
                            specs.Add($"SATA портов: {mb.sataports}");
                            specs.Add($"USB портов: {mb.usbports}");
                        }
                        break;

                    case 5: // Case
                        var caseItem = context.case_.FirstOrDefault(c => c.id == part.id);
                        if (caseItem != null)
                        {
                            var caseSize = context.casesize_.FirstOrDefault(cs => cs.id == caseItem.sizeid);
                            specs.Add($"Размер: {caseSize?.name ?? "N/A"}");
                            specs.Add($"Слотов расширения: {caseItem.expansionslots}");
                            specs.Add($"Вентиляторов: {caseItem.fans}");
                        }
                        break;

                    case 6: // Power Supply
                        var psu = context.powersupply_.FirstOrDefault(p => p.id == part.id);
                        if (psu != null)
                        {
                            var cert = context.certificate_.FirstOrDefault(c => c.id == psu.certificationid);
                            var fanDim = context.fandimension_.FirstOrDefault(fd => fd.id == psu.fandimensionid);

                            specs.Add($"Мощность: {psu.power} Вт");
                            specs.Add($"Сертификат: {cert?.name ?? "N/A"}");
                            specs.Add($"Размер вентилятора: {fanDim?.name ?? "N/A"}");
                        }
                        break;

                    case 7: // Processor Cooler
                        var cooler = context.processorcooler_.FirstOrDefault(c => c.id == part.id);
                        if (cooler != null)
                        {
                            var fanDim = context.fandimension_.FirstOrDefault(fd => fd.id == cooler.fandimensionid);
                            specs.Add($"Размер вентилятора: {fanDim?.name ?? "N/A"}");
                            specs.Add($"Тепловых трубок: {cooler.heatpipes}");
                            specs.Add($"Скорость: {cooler.minspeed} - {cooler.maxspeed} об/мин");
                            specs.Add($"Уровень шума: {cooler.noiselevel} дБ");
                        }
                        break;

                    case 8: // Storage Device
                        var storage = context.storagedevice_.FirstOrDefault(s => s.id == part.id);
                        if (storage != null)
                        {
                            var storageType = context.storagedevicetype_.FirstOrDefault(st => st.id == storage.storagedevicetypeid);
                            var storageInterface = context.storagedeviceinterface_.FirstOrDefault(si => si.id == storage.storagedeviceinterfaceid);

                            specs.Add($"Тип: {storageType?.name ?? "N/A"}");
                            specs.Add($"Интерфейс: {storageInterface?.name ?? "N/A"}");
                            specs.Add($"Объём: {storage.capacity} ГБ");

                            if (storage.storagedevicetypeid == 1) // SSD
                            {
                                var ssd = context.ssd_.FirstOrDefault(s => s.id == part.id);
                                if (ssd != null)
                                    specs.Add($"TBW: {ssd.tbw} ТБ");
                            }
                            else if (storage.storagedevicetypeid == 2) // HDD
                            {
                                var hdd = context.hdd_.FirstOrDefault(h => h.id == part.id);
                                if (hdd != null)
                                    specs.Add($"Скорость вращения: {hdd.rotationspeed} об/мин");
                            }
                        }
                        break;
                }

                TxtSpecs.Text = specs.Any() ? string.Join("\n", specs) : "Характеристики не найдены";
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
            TxtSpecs.Text = "Выберите компонент";
        }

        private void MenuItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (LbAssembly.SelectedItem is basepart_ selectedPart)
            {
                _currentAssembly.Remove(selectedPart);
                UpdateAssemblyView();
                TxtSpecs.Text = "Выберите компонент";
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