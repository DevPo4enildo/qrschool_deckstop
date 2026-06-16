using System;
using System.Windows;
using System.Windows.Controls;
using qrschool_deckstop.Models;

namespace qrschool_deckstop.Views
{
    public partial class EditComputerDialog : Window
    {
        private Computer _computer;
        private bool _isNew;

        public EditComputerDialog(Computer computer, bool isNew)
        {
            InitializeComponent();
            _computer = computer;
            _isNew = isNew;
            LoadData();
        }

        private void LoadData()
        {
            txtCode.Text = _computer.Code;
            txtInventoryNo.Text = _computer.InventoryNo;
            txtBrand.Text = _computer.Brand;
            txtModel.Text = _computer.Model;
            txtDiagonal.Text = _computer.DiagonalInch?.ToString();
            txtSerialNumber.Text = _computer.SerialNumber;
            txtCost.Text = _computer.Cost?.ToString();
            txtComment.Text = _computer.Comment;

            if (!string.IsNullOrEmpty(_computer.Status))
            {
                foreach (ComboBoxItem item in cmbStatus.Items)
                {
                    if (item.Content.ToString() == _computer.Status)
                    {
                        cmbStatus.SelectedItem = item;
                        break;
                    }
                }
            }

            if (_computer.PurchaseDate.HasValue)
                dpPurchaseDate.SelectedDate = _computer.PurchaseDate.Value;

            if (_computer.WarrantyUntil.HasValue)
                dpWarrantyUntil.SelectedDate = _computer.WarrantyUntil.Value;
        }

        private void SaveData()
        {
            _computer.Code = txtCode.Text;
            _computer.InventoryNo = txtInventoryNo.Text;
            _computer.Brand = txtBrand.Text;
            _computer.Model = txtModel.Text;

            if (decimal.TryParse(txtDiagonal.Text, out decimal diagonal))
                _computer.DiagonalInch = diagonal;

            _computer.SerialNumber = txtSerialNumber.Text;

            if (cmbStatus.SelectedItem is ComboBoxItem selectedStatus)
                _computer.Status = selectedStatus.Content.ToString();

            if (decimal.TryParse(txtCost.Text, out decimal cost))
                _computer.Cost = cost;

            _computer.PurchaseDate = dpPurchaseDate.SelectedDate;
            _computer.WarrantyUntil = dpWarrantyUntil.SelectedDate;
            _computer.Comment = txtComment.Text;
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
