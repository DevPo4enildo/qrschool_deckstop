using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using qrschool_deckstop.DataAccess;

namespace qrschool_deckstop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string connectionString = null;
            try
            {
                var configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                if (File.Exists(configPath))
                {
                    var doc = XDocument.Load(configPath);
                    var add = doc.Descendants("connectionStrings").Descendants("add")
                        .FirstOrDefault(x => (string)x.Attribute("name") == "DefaultConnection");
                    if (add != null)
                        connectionString = (string)add.Attribute("connectionString");
                }
            }
            catch
            {
                connectionString = null;
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Не найдена строка подключения к базе данных в App.config", "Ошибка конфигурации", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
                return;
            }

            DatabaseContext.Initialize(connectionString);
        }
    }
}
