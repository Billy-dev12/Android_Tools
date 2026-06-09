using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace gui_avalonia
{
    public partial class FlashDialog : Window
    {
        private DeviceModel _model;
        private bool _isFlashing = false;

        // Parameterless constructor required by XAML compiler
        public FlashDialog()
        {
            InitializeComponent();
        }

        public FlashDialog(DeviceModel model) : this()
        {
            _model = model;
            TxtHeader.Text = $"Flash Firmware — {model.Name} ({model.Code})";
        }

        private async void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (_isFlashing) return;

            var topLevel = GetTopLevel(this);
            if (topLevel == null) return;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Firmware Archive",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.All }
            });

            if (files.Count > 0)
            {
                TxtFirmwarePath.Text = files[0].Path.LocalPath;
            }
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_isFlashing) return;

            if (string.IsNullOrWhiteSpace(TxtFirmwarePath.Text))
            {
                TxtStatus.Text = "Status: Please select a firmware file first!";
                return;
            }

            _isFlashing = true;
            BtnStart.IsEnabled = false;
            BtnCancel.Content = "Stop";

            TxtStatus.Text = "Status: Initializing flash protocol...";
            PrgBar.Value = 0;

            try
            {
                // Simulate flash steps
                string[] steps = {
                    "Loading scatter/xml map...",
                    "Erasing target partitions...",
                    "Writing boot.img...",
                    "Writing system.img...",
                    "Writing vendor.img...",
                    "Verifying checksums...",
                    "Finalizing partition table..."
                };

                for (int i = 0; i < steps.Length; i++)
                {
                    if (!_isFlashing) break;
                    
                    TxtStatus.Text = $"Status: {steps[i]}";
                    
                    // Progress increments
                    double startVal = (i / (double)steps.Length) * 100;
                    double endVal = ((i + 1) / (double)steps.Length) * 100;

                    for (double val = startVal; val <= endVal; val += 2)
                    {
                        if (!_isFlashing) break;
                        PrgBar.Value = val;
                        await Task.Delay(40);
                    }
                }

                if (_isFlashing)
                {
                    PrgBar.Value = 100;
                    TxtStatus.Text = "Status: Flash Completed successfully!";
                    BtnCancel.Content = "Close";
                }
            }
            catch (Exception ex)
            {
                TxtStatus.Text = $"Status: Error: {ex.Message}";
            }
            finally
            {
                _isFlashing = false;
                BtnStart.IsEnabled = true;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_isFlashing)
            {
                // Stop flashing
                _isFlashing = false;
                TxtStatus.Text = "Status: Flash stopped by user.";
                BtnCancel.Content = "Close";
                BtnStart.IsEnabled = true;
            }
            else
            {
                Close();
            }
        }
    }
}
