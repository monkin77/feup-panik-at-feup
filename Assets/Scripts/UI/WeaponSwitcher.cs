using UnityEngine;

public class WeaponSwitcher : MonoBehaviour {
    private Transform weaponSwitcherContainer;
    private Transform currentWeaponContainer;
    private Transform nextWeaponContainer;

    private Vector3 currWeaponBannerPos;
    private Vector3 nextWeaponBannerPos;

    private void Awake() {
        this.weaponSwitcherContainer = this.transform.Find("WeaponSwitcher");
        this.currentWeaponContainer = this.weaponSwitcherContainer.Find("CurrentWeapon");
        this.nextWeaponContainer = this.weaponSwitcherContainer.Find("NextWeapon");

        this.currWeaponBannerPos = this.currentWeaponContainer.Find("Banner").position;
        this.nextWeaponBannerPos = this.nextWeaponContainer.Find("Banner").position;
    }

    public void cycleWeapon() {
        // Store the current weapon banner
        Transform currBanner = this.currentWeaponContainer.Find("Banner");
        Transform nextBanner = this.nextWeaponContainer.Find("Banner");

        // Set the current weapon banner as the next weapon banner
        currBanner.SetParent(this.nextWeaponContainer);
        currBanner.position = this.nextWeaponBannerPos;

        // Set the next weapon banner as the current weapon banner
        nextBanner.SetParent(this.currentWeaponContainer);
        nextBanner.position = this.currWeaponBannerPos;
    }
}
