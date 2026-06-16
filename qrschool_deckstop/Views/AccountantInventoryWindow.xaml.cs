using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using qrschool_deckstop.DataAccess;
using qrschool_deckstop.Models;
using qrschool_deckstop.Services;
using Microsoft.Win32;

namespace qrschool_deckstop.Views
{
    public partial class AccountantInventoryWindow : Window
    {
        private readonly AuthenticationService _authService;
        private ComputerRepository _computerRepo;
        private MonitorRepository _monitorRepo;
        private PeripheralRepository _peripheralRepo;
        private InventoryLogRepository _logRepo;

        private List<Computer> _computers;
        private List<Monitor> _monitors;
        private List<Peripheral> _peripherals;

        public AccountantInventoryWindow(AuthenticationService authService)
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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var selectedType = (cmbEquipmentType.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (selectedType)
            {
                case "Компьютеры":
                    var computer = new Computer
                    {
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = _authService.CurrentUser.Id,
                        UpdatedBy = _authService.CurrentUser.Id,
                        SyncStatus = "new"
                    };
                    var computerDialog = new EditComputerDialog(computer, true);
                    if (computerDialog.ShowDialog() == true)
                    {
                        _computerRepo.Add(computer);
                        _logRepo.AddLog("computer", computer.Id, "create", _authService.CurrentUser.Id, "Создан компьютер");
                        LoadData();
                    }
                    break;

                case "Мониторы":
                    var monitor = new Monitor
                    {
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = _authService.CurrentUser.Id,
                        UpdatedBy = _authService.CurrentUser.Id,
                        SyncStatus = "new"
                    };
                    var monitorDialog = new EditMonitorDialog(monitor, true);
                    if (monitorDialog.ShowDialog() == true)
                    {
                        _monitorRepo.Add(monitor);
                        _logRepo.AddLog("monitor", monitor.Id, "create", _authService.CurrentUser.Id, "Создан монитор");
                        LoadData();
                    }
                    break;

                case "Периферия":
                    var peripheral = new Peripheral
                    {
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = _authService.CurrentUser.Id,
                        UpdatedBy = _authService.CurrentUser.Id,
                        SyncStatus = "new"
                    };
                    var peripheralDialog = new EditPeripheralDialog(peripheral, true);
                    if (peripheralDialog.ShowDialog() == true)
                    {
                        _peripheralRepo.Add(peripheral);
                        _logRepo.AddLog("peripheral", peripheral.Id, "create", _authService.CurrentUser.Id, "Создана периферия");
                        LoadData();
                    }
                    break;
            }

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
                            _logRepo.AddLog("computer", computer.Id, "update", _authService.CurrentUser.Id, "Обновлен компьютер");
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
                            _logRepo.AddLog("monitor", monitor.Id, "update", _authService.CurrentUser.Id, "Обновлен монитор");
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
                            _logRepo.AddLog("peripheral", peripheral.Id, "update", _authService.CurrentUser.Id, "Обновлена периферия");
                            LoadData();
                        }
                    }
                    break;
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgInventory.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления", "Предупреждение", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            var selectedType = (cmbEquipmentType.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (selectedType)
            {
                case "Компьютеры":
                    var computer = dgInventory.SelectedItem as Computer;
                    if (computer != null)
                    {
                        _computerRepo.Delete(computer.Id);
                        _logRepo.AddLog("computer", computer.Id, "delete", _authService.CurrentUser.Id, "Удален компьютер");
                        LoadData();
                    }
                    break;

                case "Мониторы":
                    var monitor = dgInventory.SelectedItem as Monitor;
                    if (monitor != null)
                    {
                        _monitorRepo.Delete(monitor.Id);
                        _logRepo.AddLog("monitor", monitor.Id, "delete", _authService.CurrentUser.Id, "Удален монитор");
                        LoadData();
                    }
                    break;

                case "Периферия":
                    var peripheral = dgInventory.SelectedItem as Peripheral;
                    if (peripheral != null)
                    {
                        _peripheralRepo.Delete(peripheral.Id);
                        _logRepo.AddLog("peripheral", peripheral.Id, "delete", _authService.CurrentUser.Id, "Удалена периферия");
                        LoadData();
                    }
                    break;
            }

        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            MessageBox.Show("Данные обновлены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnGenerateQr_Click(object sender, RoutedEventArgs e)
        {
            if (dgInventory.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для генерации QR-кода", "Предупреждение", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                Title = "Сохранить QR-код",
                FileName = $"qr_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            };

            if (saveDialog.ShowDialog() == true)
            {
                string qrData = "";
                var selectedType = (cmbEquipmentType.SelectedItem as ComboBoxItem)?.Content.ToString();

                switch (selectedType)
                {
                    case "Компьютеры":
                        var computer = dgInventory.SelectedItem as Computer;
                        qrData = $"Computer: {computer.InventoryNo}\nModel: {computer.Brand} {computer.Model}\nSerial: {computer.SerialNumber}";
                        break;
                    case "Мониторы":
                        var monitor = dgInventory.SelectedItem as Monitor;
                        qrData = $"Monitor: {monitor.InventoryNo}\nModel: {monitor.Brand} {monitor.Model}\nSerial: {monitor.SerialNumber}";
                        break;
                    case "Периферия":
                        var peripheral = dgInventory.SelectedItem as Peripheral;
                        qrData = $"Peripheral: {peripheral.InventoryNo}\nType: {peripheral.Type}\nModel: {peripheral.Brand} {peripheral.Model}";
                        break;
                }

                QrCodeService.SaveQrToFile(qrData, saveDialog.FileName);
                MessageBox.Show($"QR-код сохранен: {saveDialog.FileName}", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

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
