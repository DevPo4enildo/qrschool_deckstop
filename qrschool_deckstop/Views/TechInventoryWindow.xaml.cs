using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using qrschool_deckstop.DataAccess;
using qrschool_deckstop.Models;
using qrschool_deckstop.Services;

namespace qrschool_deckstop.Views
{
    public partial class TechInventoryWindow : Window
    {
        private readonly AuthenticationService _authService;
        private ComputerRepository _computerRepo;
        private MonitorRepository _monitorRepo;
        private PeripheralRepository _peripheralRepo;
        private InventoryLogRepository _logRepo;
        private List<Computer> _computers;
        private List<Monitor> _monitors;
        private List<Peripheral> _peripherals;

        public TechInventoryWindow(AuthenticationService authService)
        {
            InitializeComponent();
            _authService = authService;
            _computerRepo = new ComputerRepository();
            _monitorRepo = new MonitorRepository();
            _peripheralRepo = new PeripheralRepository();
            _logRepo = new InventoryLogRepository();

            LoadData();
        }

        private void LoadData()
        {
            _computers = _computerRepo.GetAll().ToList();
            _monitors = _monitorRepo.GetAll().ToList();
            _peripherals = _peripheralRepo.GetAll().ToList();

            DisplayCurrentData();
        }

        private void DisplayCurrentData()
        {
            if (dgInventory == null || cmbEquipmentType?.SelectedItem == null)
                return;

            var selectedType = (cmbEquipmentType.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (selectedType)
            {
                case "Компьютеры":
                    dgInventory.ItemsSource = _computers;
                    break;
                case "Мониторы":
                    dgInventory.ItemsSource = _monitors;
                    break;
                case "Периферия":
                    dgInventory.ItemsSource = _peripherals;
                    break;
            }
        }

        private void CmbEquipmentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayCurrentData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgInventory.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для редактирования", "Предупреждение", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedType = (cmbEquipmentType.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (selectedType)
            {
                case "Компьютеры":
                    var computer = dgInventory.SelectedItem as Computer;
                    if (computer != null)
                    {
                        var dialog = new EditComputerDialog(computer, false);
                        if (dialog.ShowDialog() == true)
                        {
                            computer.UpdatedAt = DateTime.Now;
                            computer.UpdatedBy = _authService.CurrentUser.Id;
                            computer.SyncStatus = "modified";
                            _computerRepo.Update(computer);
                            _logRepo.AddLog("computer", computer.Id, "update", _authService.CurrentUser.Id, "Обновлен компьютер (тех. специалист)");
                            LoadData();
                        }
                    }
                    break;
                    
                case "Мониторы":
                    var monitor = dgInventory.SelectedItem as Monitor;
                    if (monitor != null)
                    {
                        var dialog = new EditMonitorDialog(monitor, false);
                        if (dialog.ShowDialog() == true)
                        {
                            monitor.UpdatedAt = DateTime.Now;
                            monitor.UpdatedBy = _authService.CurrentUser.Id;
                            monitor.SyncStatus = "modified";
                            _monitorRepo.Update(monitor);
                            _logRepo.AddLog("monitor", monitor.Id, "update", _authService.CurrentUser.Id, "Обновлен монитор (тех. специалист)");
                            LoadData();
                        }
                    }
                    break;
                    
                case "Периферия":
                    var peripheral = dgInventory.SelectedItem as Peripheral;
                    if (peripheral != null)
                    {
                        var dialog = new EditPeripheralDialog(peripheral, false);
                        if (dialog.ShowDialog() == true)
                        {
                            peripheral.UpdatedAt = DateTime.Now;
                            peripheral.UpdatedBy = _authService.CurrentUser.Id;
                            peripheral.SyncStatus = "modified";
                            _peripheralRepo.Update(peripheral);
                            _logRepo.AddLog("peripheral", peripheral.Id, "update", _authService.CurrentUser.Id, "Обновлена периферия (тех. специалист)");
                            LoadData();
                        }
                    }
                    break;
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            MessageBox.Show("Данные обновлены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _authService.Logout();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
