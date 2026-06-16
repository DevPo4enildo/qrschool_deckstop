using System;
using System.Windows;
using System.Windows.Controls;
using qrschool_deckstop.Models;

namespace qrschool_deckstop.Views
{
    public partial class EditMonitorDialog : Window
    {
        private Monitor _monitor;
        private bool _isNew;

        public EditMonitorDialog(Monitor monitor, bool isNew)
        {
            InitializeComponent();
            _monitor = monitor;
            _isNew = isNew;
            LoadData();
        }

        private void LoadData()
        {
            txtCode.Text = _monitor.Code;
            txtInventoryNo.Text = _monitor.InventoryNo;
            txtBrand.Text = _monitor.Brand;
            txtModel.Text = _monitor.Model;
            txtDiagonal.Text = _monitor.DiagonalInch?.ToString();
            txtSerialNumber.Text = _monitor.SerialNumber;
            txtCost.Text = _monitor.Cost?.ToString();

            if (!string.IsNullOrEmpty(_monitor.Status))
            {
                foreach (ComboBoxItem item in cmbStatus.Items)
                {
                    if (item.Content.ToString() == _monitor.Status)
                    {
                        cmbStatus.SelectedItem = item;
                        break;
                    }
                }
            }

            if (_monitor.PurchaseDate.HasValue)
                dpPurchaseDate.SelectedDate = _monitor.PurchaseDate.Value;

            if (_monitor.WarrantyUntil.HasValue)
                dpWarrantyUntil.SelectedDate = _monitor.WarrantyUntil.Value;
        }

        private void SaveData()
        {
            _monitor.Code = txtCode.Text;
            _monitor.InventoryNo = txtInventoryNo.Text;
            _monitor.Brand = txtBrand.Text;
            _monitor.Model = txtModel.Text;

            if (decimal.TryParse(txtDiagonal.Text, out decimal diagonal))
                _monitor.DiagonalInch = diagonal;

            _monitor.SerialNumber = txtSerialNumber.Text;

            if (cmbStatus.SelectedItem is ComboBoxItem selectedStatus)
                _monitor.Status = selectedStatus.Content.ToString();

            if (decimal.TryParse(txtCost.Text, out decimal cost))
                _monitor.Cost = cost;

            _monitor.PurchaseDate = dpPurchaseDate.SelectedDate;
            _monitor.WarrantyUntil = dpWarrantyUntil.SelectedDate;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text) || string.IsNullOrEmpty(txtInventoryNo.Text))
            {
                MessageBox.Show("Заполните обязательные поля (Код и Инв. номер)", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveData();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
