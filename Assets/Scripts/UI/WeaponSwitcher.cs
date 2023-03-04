using UnityEngine;

public class WeaponSwitcher : MonoBehaviour {
    private Transform weaponSwitcherContainer;
    private Transform currentWeaponContainer;
    private Transform nextWeaponContainer;

    private Vector2 currWeaponBannerPos;
    private Vector2 nextWeaponBannerPos;

    private void Awake() {
        this.weaponSwitcherContainer = GameObject.Find("WeaponSwitcher").transform;
        this.currentWeaponContainer = this.weaponSwitcherContainer.Find("CurrentWeapon");
        this.nextWeaponContainer = this.weaponSwitcherContainer.Find("NextWeapon");

        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();

        // Screen position
        Vector3 currWeaponBannerScreenPos = Camera.main.WorldToScreenPoint(this.currentWeaponContainer.Find("Banner").position);
        Vector3 nextWeaponBannerScreenPos = Camera.main.WorldToScreenPoint(this.nextWeaponContainer.Find("Banner").position);

        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.currentWeaponContainer.GetComponent<RectTransform>(), currWeaponBannerScreenPos, Camera.main, out this.currWeaponBannerPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.nextWeaponContainer.GetComponent<RectTransform>(), nextWeaponBannerScreenPos, Camera.main, out this.nextWeaponBannerPos);
    }

    public void cycleWeapon() {
        // Store the current weapon banner
        Transform currBanner = this.currentWeaponContainer.Find("Banner");
        Transform nextBanner = this.nextWeaponContainer.Find("Banner");

        Debug.Log("Current Weapon pos: " + this.currWeaponBannerPos);
        Debug.Log("Next Weapon pos: " + this.nextWeaponBannerPos);

        // Set the current weapon banner as the next weapon banner
        currBanner.SetParent(this.nextWeaponContainer);
        currBanner.localPosition = this.nextWeaponBannerPos;

        // Set the next weapon banner as the current weapon banner
        nextBanner.SetParent(this.currentWeaponContainer);
        nextBanner.localPosition = this.currWeaponBannerPos;
    }
}
