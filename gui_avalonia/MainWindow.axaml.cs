using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace gui_avalonia
{
    public enum LogLevel
    {
        Info,
        OK,
        Warn,
        Error
    }

    public partial class MainWindow : Window
    {
        private List<DeviceModel> _allModels = new List<DeviceModel>();
        private DeviceModel _selectedModel = null;
        private bool _isOperationRunning = false;

        public MainWindow()
        {
            InitializeComponent();
            LoadBrands();
            AppendLog("Android Tools Billy v1.0 initialized.", LogLevel.Info);
            AppendLog("Utilitarian service interface loaded successfully.", LogLevel.OK);
        }

        private void LoadBrands()
        {
            var brands = DeviceDatabase.Brands.Keys.ToList();
            CboBrand.ItemsSource = brands;
            if (brands.Count > 0)
            {
                CboBrand.SelectedIndex = 0;
            }
        }

        private void UpdateModelList()
        {
            if (CboBrand.SelectedItem is string selectedBrand)
            {
                if (DeviceDatabase.Brands.TryGetValue(selectedBrand, out var models))
                {
                    _allModels = models;
                    FilterModels();
                }
            }
        }

        private void FilterModels()
        {
            string filter = TxtSearch.Text ?? "";
            var filtered = _allModels
                .Where(m => m.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) || 
                            m.Code.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

            LstModels.ItemsSource = filtered;
        }

        private void CboBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateModelList();
            ResetOperationsView();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterModels();
        }

        private void LstModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstModels.SelectedItem is DeviceModel model)
            {
                _selectedModel = model;
                TxtSelectedDeviceHeader.Text = $"{CboBrand.SelectedItem.ToString().ToUpper()} {model.Name.ToUpper()} - {model.Code.ToUpper()}";
                TxtBreadcrumb.Text = $"Android Tools Billy > Operations > {model.Name}";
                PanelPlaceholder.IsVisible = false;
                PanelOperations.IsVisible = true;
                TxtStatus.Text = $"Selected: {model.Name}";
            }
            else
            {
                ResetOperationsView();
            }
        }

        private void ResetOperationsView()
        {
            _selectedModel = null;
            PanelOperations.IsVisible = false;
            PanelPlaceholder.IsVisible = true;
            TxtBreadcrumb.Text = "Android Tools Billy > Home";
            TxtStatus.Text = "Ready";
        }

        // ────────────────────────────────────────────────────────
        //  LOG UTILITY
        // ────────────────────────────────────────────────────────
        public void AppendLog(string message, LogLevel level)
        {
            Dispatcher.UIThread.Post(() =>
            {
                string prefix = level switch
                {
                    LogLevel.OK => "[OK]   ",
                    LogLevel.Warn => "[WARN] ",
                    LogLevel.Error => "[ERROR]",
                    _ => "[INFO] "
                };

                string timestamp = $"[{DateTime.Now:HH:mm:ss}]";
                string logLine = $"{timestamp} {prefix} {message}\n";

                TxtLog.Text += logLine;
                
                // Auto scroll to end
                TxtLog.CaretIndex = TxtLog.Text.Length;
            });
        }

        // ────────────────────────────────────────────────────────
        //  OPERATIONS
        // ────────────────────────────────────────────────────────
        private async void OpButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedModel == null || _isOperationRunning) return;

            if (sender is Button btn && btn.Tag is string opName)
            {
                if (opName == "Flash Firmware")
                {
                    OpenFlashDialog();
                    return;
                }

                await RunSimulatedOperation(opName);
            }
        }

        private async Task RunSimulatedOperation(string opName)
        {
            _isOperationRunning = true;
            TxtStatus.Text = $"Running: {opName}...";
            AppendLog($"Starting operation: {opName} on {_selectedModel.Name} ({_selectedModel.Code})", LogLevel.Info);

            try
            {
                await Task.Delay(500);
                AppendLog("Connecting to device...", LogLevel.Info);
                await Task.Delay(800);
                AppendLog("Device detected in COM4 (MediaTek USB VCOM)", LogLevel.OK);
                await Task.Delay(600);
                AppendLog("Handshake protocol successful. Sending boot loader...", LogLevel.OK);
                await Task.Delay(1000);
                
                if (opName.Contains("Reset") || opName.Contains("FRP"))
                {
                    AppendLog("Erasing user partitions...", LogLevel.Info);
                    await Task.Delay(800);
                    AppendLog("Operation completed successfully.", LogLevel.OK);
                }
                else if (opName.Contains("IMEI"))
                {
                    AppendLog("Writing NVRAM IMEI values...", LogLevel.Warn);
                    await Task.Delay(1200);
                    AppendLog("IMEI Repair finished. Please reboot device.", LogLevel.OK);
                }
                else if (opName.Contains("Reboot"))
                {
                    AppendLog("Sending reboot command signal...", LogLevel.Info);
                    await Task.Delay(300);
                    AppendLog("Device rebooted.", LogLevel.OK);
                }
                else
                {
                    AppendLog("Processing service operation parameters...", LogLevel.Info);
                    await Task.Delay(1000);
                    AppendLog("Task completed successfully.", LogLevel.OK);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error: {ex.Message}", LogLevel.Error);
            }
            finally
            {
                _isOperationRunning = false;
                TxtStatus.Text = "Ready";
                AppendLog("--- Operation Finished ---", LogLevel.Info);
            }
        }

        private void OpenFlashDialog()
        {
            if (_selectedModel == null) return;
            var dialog = new FlashDialog(_selectedModel);
            dialog.ShowDialog(this);
            AppendLog("Opened Flash Firmware dialog.", LogLevel.Info);
        }

        // ────────────────────────────────────────────────────────
        //  NAVIGATION BUTTONS
        // ────────────────────────────────────────────────────────
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ResetOperationsView();
            LstModels.SelectedItem = null;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedModel == null)
            {
                AppendLog("Please select a device first.", LogLevel.Warn);
            }
            else
            {
                AppendLog($"Proceeding with {_selectedModel.Name}...", LogLevel.Info);
            }
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            AppendLog("Settings panel opened (Mock).", LogLevel.Info);
        }

        // ────────────────────────────────────────────────────────
        //  MENU CLICKS
        // ────────────────────────────────────────────────────────
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuScanPorts_Click(object sender, RoutedEventArgs e)
        {
            AppendLog("Scanning COM ports... Found COM4 (MediaTek USB VCOM)", LogLevel.Info);
        }

        private void MenuFlash_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedModel != null)
            {
                OpenFlashDialog();
            }
            else
            {
                AppendLog("Please select a device from the list first to flash firmware.", LogLevel.Warn);
            }
        }

        private void MenuAdbTerminal_Click(object sender, RoutedEventArgs e)
        {
            AppendLog("Opening ADB Terminal emulator...", LogLevel.Info);
        }

        private void MenuToggleLog_Click(object sender, RoutedEventArgs e)
        {
            BorderLog.IsVisible = MenuShowLogItem.IsChecked;
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            AppendLog("Android Tools Billy v1.0 - Utilitarian Service Edition.", LogLevel.Info);
            AppendLog("Developed for kounter HP technicians.", LogLevel.Info);
        }
    }
}