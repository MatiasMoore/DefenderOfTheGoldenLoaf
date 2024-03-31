using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    [SerializeField]
    private Text _textField;

    [SerializeField]
    private int _maxAmmo;

    [SerializeField]
    private int _currentAmmo;

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        _currentAmmo = currentAmmo;
        _maxAmmo = maxAmmo;
        UpdateAmmoText();
    }

    public void SetCurrentAmmo(int currentAmmo)
    {
        _currentAmmo = currentAmmo;
        UpdateAmmoText();
    }

    public void SetMaxAmmo(int maxAmmo)
    {
        _maxAmmo = maxAmmo;
        UpdateAmmoText();
    }

    public void UpdateAmmoText()
    {
        string ammoText = _currentAmmo + " / " + _maxAmmo;
        _textField.text = ammoText;
    }
}
