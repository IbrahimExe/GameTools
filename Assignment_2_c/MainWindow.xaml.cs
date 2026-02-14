using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WeaponLib;

namespace Assignment_2_c
{
    public partial class MainWindow : Window
    {
        // Member: the main Weapon collection
        public WeaponCollection mWeaponCollection { get; private set; }

        private List<Weapon> mCurrentView = new List<Weapon>();

        public MainWindow()
        {
            InitializeComponent();

            mWeaponCollection = new WeaponCollection();

            var names = Enum.GetNames(typeof(Weapon.WeaponType)).ToList();
            names.Insert(0, "All");
            TypeFilterComboBox.ItemsSource = names;
            TypeFilterComboBox.SelectedIndex = 0;

            // bind initially
            WeaponListBox.ItemsSource = mCurrentView;
            RefreshList();
        }


        // Event handlers
        private void LoadClicked(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "CSV Files (*.csv)|*.csv|JSON Files (*.json)|*.json|XML Files (*.xml)|*.xml|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                bool ok = mWeaponCollection.Load(dlg.FileName);
                if (!ok)
                {
                    MessageBox.Show("Failed to load file. Make sure the format matches the extension.");
                    return;
                }
                RefreshList();
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "CSV Files (*.csv)|*.csv|JSON Files (*.json)|*.json|XML Files (*.xml)|*.xml";
            if (dlg.ShowDialog() == true)
            {
                bool ok = mWeaponCollection.Save(dlg.FileName);
                if (!ok)
                    MessageBox.Show("Failed to save file.");
                else
                    MessageBox.Show("Saved successfully.");
            }
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            var win = new EditWeaponWindow();

            win.TempWeapon = new Weapon()
            {
                Name = "New Weapon",
                Type = Weapon.WeaponType.None,
                Image = "",
                Rarity = 1,
                BaseAttack = 20,
                SecondaryStat = "",
                Passive = ""
            };
            win.Setup(isEdit: false);
            var result = win.ShowDialog();
            if (result == true)
            {
                // Add the created weapon
                mWeaponCollection.Add(win.TempWeapon);
                RefreshList();
            }
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            var selected = WeaponListBox.SelectedItem as Weapon;
            if (selected == null)
            {
                MessageBox.Show("Please select a weapon to edit.");
                return;
            }

            var temp = new Weapon()
            {
                Name = selected.Name,
                Type = selected.Type,
                Image = selected.Image,
                Rarity = selected.Rarity,
                BaseAttack = selected.BaseAttack,
                SecondaryStat = selected.SecondaryStat,
                Passive = selected.Passive
            };

            var win = new EditWeaponWindow();
            win.TempWeapon = temp;
            win.Setup(isEdit: true);
            var result = win.ShowDialog();
            if (result == true)
            {
                // Apply edits back to the selected item
                selected.Name = win.TempWeapon.Name;
                selected.Type = win.TempWeapon.Type;
                selected.Image = win.TempWeapon.Image;
                selected.Rarity = win.TempWeapon.Rarity;
                selected.BaseAttack = win.TempWeapon.BaseAttack;
                selected.SecondaryStat = win.TempWeapon.SecondaryStat;
                selected.Passive = win.TempWeapon.Passive;

                RefreshList();
            }
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            var selected = WeaponListBox.SelectedItem as Weapon;
            if (selected == null)
            {
                MessageBox.Show("Please select a weapon to remove.");
                return;
            }

            if (MessageBox.Show($"Remove '{selected.Name}'?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                mWeaponCollection.Remove(selected);
                RefreshList();
            }
        }

        private void SortRadioSelected(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb == null) return;
            string col = rb.Tag as string;
            if (string.IsNullOrWhiteSpace(col)) return;

            try
            {
                mWeaponCollection.SortBy(col);
            }
            catch
            {

            }
            RefreshList();
        }

        private void FilterTypeOnlySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }

        private void FilterNameTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshList();
        }


        // Helper: refresh view and apply filters
        private void RefreshList()
        {
            IEnumerable<Weapon> view = mWeaponCollection;

            // Type filter
            string typeSel = TypeFilterComboBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(typeSel) && typeSel != "All")
            {
                if (Enum.TryParse<Weapon.WeaponType>(typeSel, out var t))
                {
                    view = view.Where(w => w.Type == t);
                }
            }

            // Name filter
            string nameFilter = FilterNameTextBox.Text ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                view = view.Where(w => w.Name != null && w.Name.StartsWith(nameFilter, StringComparison.OrdinalIgnoreCase));
            }


            mCurrentView = view.ToList();
            WeaponListBox.ItemsSource = mCurrentView;
            WeaponListBox.Items.Refresh();
        }
    }
}