using System.Windows;
using AndASM_NMS.Windows;

namespace AndASM_NMS.EnableMSStoreMods
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public NmsUwpPackage NmsUwpPackage;

		public MainWindow()
		{
			NmsUwpPackage = new NmsUwpPackage();
			InitializeComponent();
			DataContext = NmsUwpPackage;
		}

		private void ToggleModsClick(object sender, RoutedEventArgs e)
		{
			NmsUwpPackage.IsModdingEnabled = !NmsUwpPackage.IsModdingEnabled;
		}

		private void OpenModsClick(object sender, RoutedEventArgs e)
		{
			NmsUwpPackage.ModsFolder.OpenExplorer();
		}

		private void ShortcutModsClick(object sender, RoutedEventArgs e)
		{
			NmsUwpPackage.ModsFolder.CreateDesktopShortcut("No Man's Sky Mods");
		}
	}
}