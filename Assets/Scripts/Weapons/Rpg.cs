using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rpg : Weapon
{
    public static string BULLET_TAG = "Bullet";
    public static float BULLET_SPEED = 5f;
    public static float RECOIL_TIME = 0.15f;
    public static float MAX_HORIZONTAL_REC_DISTANCE = 0.5f;
    public static float MAX_VERTICAL_REC_DISTANCE = 0.35f;

    // Stores the current recoil distance for the weapon. Resets to 0 when the weapon is not attacking
    private float _currRecoilDistance = 0f;

    // Stores the current max recoil distance for the weapon (changes w/ orientation)
    private float _currMaxRecoilDist = MAX_VERTICAL_REC_DISTANCE;
    // Stores the current recoil direction for the weapon (changes w/ orientation)
    private Vector2 _currRecoilDirection = Vector2.up;

    private bool recoilling = false;
    private bool firstShot = true;
    private Queue<Collectible> rpgAmmo = new Queue<Collectible>();

    [SerializeField] private TextMeshProUGUI ammoCountText;
    
    [SerializeField] private GameObject poisonPrefab;
    [SerializeField] private float finalPositionOffset = 1f;
    [SerializeField] private float panikeMoveDuration = 1f;
    
    /**
     * Attack method for the shovel.
     * The shovel will rotate back and forth between 0 and MAX_ANGLE degrees.
     * In the end of the animation, the shovel will return to its original position.
     */
    public override void Attack()
    {
        if (!this._isAttacking)
            return;
        
        // first shot, create a new bullet
        if (this.firstShot && rpgAmmo.Count > 0) {
            this.firstShot = false;
            Collectible ammo = rpgAmmo.Dequeue();
            GameObject bullet = ammo.gameObject;
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().velocity = Weapon.vecFromOrientation(this._orientation) * BULLET_SPEED;

            // Update Ammo Count in the UI
            this.ammoCountText.text = rpgAmmo.Count.ToString();
        }

        float recoilSpeed = this._currMaxRecoilDist / RECOIL_TIME;

        // weapon is attacking, recoil it
        if (!this.recoilling) {
            // if the weapon hasn't reached its max recoil distance, recoil it
            if (this._currRecoilDistance < this._currMaxRecoilDist)
            {
                Vector2 recoilVector = recoilSpeed * Time.deltaTime * this._currRecoilDirection;
                transform.localPosition += new Vector3(recoilVector.x, recoilVector.y, 0);
                this._currRecoilDistance += recoilSpeed * Time.deltaTime;
            }
            else
                this.recoilling = true; 
        } else {
            // if the weapon hasn't reached its original position, recoil it back
            if (this._currRecoilDistance > 0)
            {
                Vector2 recoilVector = recoilSpeed * Time.deltaTime * this._currRecoilDirection;
                transform.localPosition -= new Vector3(recoilVector.x, recoilVector.y, 0);
                this._currRecoilDistance -= recoilSpeed * Time.deltaTime;
            }
            else
            {
                this.stopAttacking();
            }
        }
    }

    public override void PowerUpAttack()
    { 
        if (!this._isAttacking)
            return;
        
        // first shot, create a new bullet
        if (this.firstShot)
        {
            // creates the panike and sets up its movement initial, final position and duration
            this.firstShot = false;
            
            Vector2 orientation = Weapon.vecFromOrientation(this._orientation);
            Vector3 initialPosition = transform.position;
            Vector3 finalPosition = initialPosition + new Vector3(orientation.x, orientation.y, 0) * finalPositionOffset;
            
            GameObject poison = Instantiate(poisonPrefab, initialPosition, Quaternion.identity);
            PoisonedPanike panike = poison.GetComponent<PoisonedPanike>();
            panike.ParabolDuration = panikeMoveDuration;
            panike.FinalPosition = finalPosition;
            StartCoroutine(PowerUpMovementReset());
        }
        
        
    }
    
    // Resets the weapon state after poisoned panike reaches its final position
    private IEnumerator PowerUpMovementReset()
    {
        yield return new WaitForSeconds(panikeMoveDuration);
        stopAttacking();
    }


    /**
     * Sets the weapon orientation according to the player orientation
     */
    public override void ChangeOrientation(WeaponOrientation newOrientation)
    {
        // If the orientation is the same, do nothing
        if (newOrientation == _orientation)
            return;
        
        // If the weapon is attacking, stop it
        if (this._isAttacking) this.stopAttacking();

        switch (newOrientation)
        {
            case WeaponOrientation.Left:
                sr.sortingOrder = -1;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localPosition = new Vector3(0f, 0f, 0f);

                this._currMaxRecoilDist = MAX_HORIZONTAL_REC_DISTANCE;
                this._currRecoilDirection = Vector2.right;
                break;
            case WeaponOrientation.Right:
                sr.sortingOrder = 1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.localPosition = new Vector3(1f, -0.3f, 0f);

                this._currMaxRecoilDist = MAX_HORIZONTAL_REC_DISTANCE;
                this._currRecoilDirection = Vector2.left;
                break;
            case WeaponOrientation.Up:
                sr.sortingOrder = 1;

                transform.rotation = Quaternion.Euler(0, 120, 0);
                transform.localPosition = new Vector3(1.8f, -0.1f, 0f);

                this._currMaxRecoilDist = MAX_VERTICAL_REC_DISTANCE;
                this._currRecoilDirection = Vector2.up;
                break;
            case WeaponOrientation.Down:
                // Move the weapon behind the player
                sr.sortingOrder = -1;

                // Adjust the weapon rotation and position
                transform.rotation = Quaternion.Euler(0, 230, 0);
                transform.localPosition = new Vector3(0.1f, -0.2f, 0f);

                // Adjust the max recoil distance
                this._currMaxRecoilDist = MAX_VERTICAL_REC_DISTANCE;
                this._currRecoilDirection = Vector2.up;
                break;
        }
        
        _orientation = newOrientation;
    }
    
    public override void AddAmmo(Collectible ammo) {
        ammo.tag = BULLET_TAG;
        rpgAmmo.Enqueue(ammo);

        // Update Ammo Count in the UI
        this.ammoCountText.text = rpgAmmo.Count.ToString();
    }

    /**
    * Resets the weapon to its original state
    */
    protected override void stopAttacking()
    {
        this._isAttacking = false;
        this.IsPowerUp = false;
        this.recoilling = false;
        this.firstShot = true;

        this._currRecoilDistance = 0f;
    }

    public int getAmmoCount() {
        return rpgAmmo.Count;
    }
}
