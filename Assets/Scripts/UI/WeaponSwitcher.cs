using UnityEngine;

public class WeaponSwitcher : MonoBehaviour {
    private Transform weaponSwitcherContainer;
    private Transform currentWeaponContainer;
    private Transform nextWeaponContainer;

    private Vector2 currWeaponPos;
    private Vector2 nextWeaponPos;

    private void Awake() {
        this.weaponSwitcherContainer = GameObject.Find("WeaponSwitcher").transform;
        this.currentWeaponContainer = this.weaponSwitcherContainer.Find("CurrentWeapon");
        this.nextWeaponContainer = this.weaponSwitcherContainer.Find("NextWeapon");

        // To calculate the Game Positions of each weapon, we need to convert the Screen Space to Canvas / RectTransform space
        // Calculate the screen position of the weapons
        Vector3 currWeaponScreenPos = Camera.main.WorldToScreenPoint(this.currentWeaponContainer.Find("Weapon").position);
        Vector3 nextWeaponScreenPos = Camera.main.WorldToScreenPoint(this.nextWeaponContainer.Find("Weapon").position);

        // Convert screen position to Canvas / RectTransform space and store the result
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.currentWeaponContainer.GetComponent<RectTransform>(), currWeaponScreenPos, 
            Camera.main, out this.currWeaponPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.nextWeaponContainer.GetComponent<RectTransform>(), nextWeaponScreenPos, 
            Camera.main, out this.nextWeaponPos);
    }

    public void cycleWeapon() {
        // Store the current weapon
        Transform currWeapon = this.currentWeaponContainer.Find("Weapon");
        Transform nextWeapon = this.nextWeaponContainer.Find("Weapon");

        // Set the current weapon as the next weapon 
        currWeapon.SetParent(this.nextWeaponContainer);
        currWeapon.localPosition = this.nextWeaponPos;
        // If the new next weapon has ammo, set the ammo container to inactive
        Transform ammoContainer = currWeapon.Find("Ammo");
        if (ammoContainer != null) {
            ammoContainer.gameObject.SetActive(false);
        }

        // Set the next weapon as the current weapon
        nextWeapon.SetParent(this.currentWeaponContainer);
        nextWeapon.localPosition = this.currWeaponPos;
         // If the new current weapon has ammo, set the ammo container to active
        ammoContainer = nextWeapon.Find("Ammo");
        if (ammoContainer != null) {
            ammoContainer.gameObject.SetActive(true);
        }
    }
}
