using System;
using System.Windows;
using System.Windows.Controls;
using qrschool_deckstop.Models;

namespace qrschool_deckstop.Views
{
    public partial class EditPeripheralDialog : Window
    {
        private Peripheral _peripheral;
        private bool _isNew;

        public EditPeripheralDialog(Peripheral peripheral, bool isNew)
        {
            InitializeComponent();
            _peripheral = peripheral;
            _isNew = isNew;
            LoadData();
        }

        private void LoadData()
        {
            txtCode.Text = _peripheral.Code;
            txtInventoryNo.Text = _peripheral.InventoryNo;
            txtBrand.Text = _peripheral.Brand;
            txtModel.Text = _peripheral.Model;
            txtSerialNumber.Text = _peripheral.SerialNumber;
            txtCost.Text = _peripheral.Cost?.ToString();

            if (!string.IsNullOrEmpty(_peripheral.Type))
            {
                foreach (ComboBoxItem item in cmbType.Items)
                {
                    if (item.Content.ToString() == _peripheral.Type)
                    {
                        cmbType.SelectedItem = item;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(_peripheral.Status))
            {
                foreach (ComboBoxItem item in cmbStatus.Items)
                {
                    if (item.Content.ToString() == _peripheral.Status)
                    {
                        cmbStatus.SelectedItem = item;
                        break;
                    }
                }
            }

            if (_peripheral.PurchaseDate.HasValue)
                dpPurchaseDate.SelectedDate = _peripheral.PurchaseDate.Value;

            if (_peripheral.WarrantyUntil.HasValue)
                dpWarrantyUntil.SelectedDate = _peripheral.WarrantyUntil.Value;
        }

        private void SaveData()
        {
            _peripheral.Code = txtCode.Text;
            _peripheral.InventoryNo = txtInventoryNo.Text;

            if (cmbType.SelectedItem is ComboBoxItem selectedType)
                _peripheral.Type = selectedType.Content.ToString();

            _peripheral.Brand = txtBrand.Text;
            _peripheral.Model = txtModel.Text;
            _peripheral.SerialNumber = txtSerialNumber.Text;

            if (cmbStatus.SelectedItem is ComboBoxItem selectedStatus)
                _peripheral.Status = selectedStatus.Content.ToString();

            if (decimal.TryParse(txtCost.Text, out decimal cost))
                _peripheral.Cost = cost;

            _peripheral.PurchaseDate = dpPurchaseDate.SelectedDate;
            _peripheral.WarrantyUntil = dpWarrantyUntil.SelectedDate;
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
