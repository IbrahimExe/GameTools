using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using WeaponLib;

namespace Assignment_2_c
{
    public partial class EditWeaponWindow : Window
{
    public Weapon TempWeapon { get; set; }

    private bool _isEditMode = false;
    private Random _rng = new Random();

    public EditWeaponWindow()
    {
        InitializeComponent();

        // Populate Type combo
        TypeComboBox.ItemsSource = Enum.GetNames(typeof(Weapon.WeaponType)).ToList();

        // Populate Rarity (1..5)
        RarityComboBox.ItemsSource = Enumerable.Range(1, 5).ToList();
    }

    /// <summary>
    /// Call before ShowDialog to prepare the fields.
    /// </summary>
    public void Setup(bool isEdit)
    {
        _isEditMode = isEdit;
        if (TempWeapon == null)
            TempWeapon = new Weapon();

        // Fill fields
        NameTextBox.Text = TempWeapon.Name ?? "";
        TypeComboBox.SelectedItem = TempWeapon.Type.ToString();
        ImageUrlTextBox.Text = TempWeapon.Image ?? "";
        RarityComboBox.SelectedItem = TempWeapon.Rarity > 0 ? (object)TempWeapon.Rarity : 1;
        BaseAttackTextBox.Text = TempWeapon.BaseAttack.ToString();
        SecondaryStatTextBox.Text = TempWeapon.SecondaryStat ?? "";
        PassiveTextBox.Text = TempWeapon.Passive ?? "";

        // Update window title and submit button text based on mode
        this.Title = isEdit ? "Edit Weapon" : "Add Weapon";
        SubmitButton.Content = isEdit ? "Save" : "Add";

        UpdateImagePreview();
    }

    private void UpdateImagePreview()
    {
        var url = ImageUrlTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(url))
        {
            ImagePreview.Source = null;
            return;
        }

        try
        {
            var uri = new Uri(url);
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = uri;
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.EndInit();
            ImagePreview.Source = bmp;
        }
        catch
        {
            ImagePreview.Source = null;
        }
    }

    private void ImageUrlTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        UpdateImagePreview();
    }

    private void SubmitClicked(object sender, RoutedEventArgs e)
    {
        // minimal validation
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            MessageBox.Show("Please enter a name.");
            return;
        }

        if (!int.TryParse(BaseAttackTextBox.Text, out int baseAttack))
        {
            MessageBox.Show("Base Attack must be an integer.");
            return;
        }

        if (!int.TryParse(RarityComboBox.SelectedItem?.ToString() ?? "1", out int rarity))
        {
            rarity = 1;
        }

        // Write fields back into TempWeapon
        TempWeapon.Name = NameTextBox.Text.Trim();
        if (Enum.TryParse(TypeComboBox.SelectedItem?.ToString(), true, out Weapon.WeaponType t))
            TempWeapon.Type = t;
        TempWeapon.Image = ImageUrlTextBox.Text?.Trim();
        TempWeapon.Rarity = rarity;
        TempWeapon.BaseAttack = baseAttack;
        TempWeapon.SecondaryStat = SecondaryStatTextBox.Text?.Trim();
        TempWeapon.Passive = PassiveTextBox.Text?.Trim();

        this.DialogResult = true;
        this.Close();
    }

    private void CancelClicked(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }

    private void GenerateClicked(object sender, RoutedEventArgs e)
    {
        // Randomly generate base attack and rarity and type
        TempWeapon.BaseAttack = _rng.Next(20, 51); // inclusive lower, exclusive upper
        TempWeapon.Rarity = _rng.Next(1, 6);

        // random type
        var types = Enum.GetValues(typeof(Weapon.WeaponType)).Cast<Weapon.WeaponType>().ToArray();
        TempWeapon.Type = types[_rng.Next(types.Length)];

        // update UI
        BaseAttackTextBox.Text = TempWeapon.BaseAttack.ToString();
        RarityComboBox.SelectedItem = TempWeapon.Rarity;
        TypeComboBox.SelectedItem = TempWeapon.Type.ToString();
    }
}
}
