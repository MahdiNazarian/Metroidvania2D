using System;
using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Refrences")]
    [HideInInspector] public Player player;

    private bool coolDownOver = true;

    [Header("Raycast Info")]
    [SerializeField] private LayerMask hitDetectionLayerMask;

    [Header("Line Renderer")]
    private LineRenderer lineRenderer;
    private bool isShootLineActive = false;
    private Vector3 lineStartPoint;
    private Vector3 lineEndPoint;

    


    private void Awake()
    {
        player = GetComponent<Player>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        player.gatherInput.SetShootActionPerformedInstructions(TryToShoot);
        player.gatherInput.SetShootActionCancelledInstructions(StopShooting);
        player.gatherInput.changeWeaponActionBlueprint.SetActionPerformedInstructions(TryToChangeWeapon);
        LoadWeapons();
        WeaponDisplayController.onUpdateAllDisplayInfo?.Invoke(player.currentWeaponScript.weaponIconSprite, player.currentWeaponScript.currentAmmo, player.currentWeaponScript.maxAmmo, player.currentWeaponScript.storageAmmo);
    }

    private void OnDisable()
    {
        player.gatherInput.RemoveShootActionPerformedInstructions(TryToShoot);
        player.gatherInput.RemoveShootActionCancelledInstructions(StopShooting);
        player.gatherInput.changeWeaponActionBlueprint.RemoveActionPerformedInstructions(TryToChangeWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.currentWeaponScript == null ||
            player.stateMachine.currentState == PlayerStates.State.WallSlide ||
            player.stateMachine.currentState == PlayerStates.State.Ladders ||
            player.stateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        if (player.gatherInput.tryToShoot && player.currentWeaponScript.isAutomatic && coolDownOver)
        {
            Shoot();
        }
        //if (isShootLineActive)
        //{
        //    lineRenderer.SetPosition(0, currentWeapon.shootingPoint.position);
        //    lineRenderer.SetPosition(1, lineEndPoint);
        //}
    }

    private void LoadWeapons()
    {
        foreach(Weapon weapon in player.allWeaponsList)
        {
            weapon.LoadWeaponData();
        }
    }
    public void AddAmmoToStorage(string ID, int ammo)
    {
        foreach(Weapon weapon in player.allWeaponsList)
        {
            if(weapon.ID == ID)
            {
                weapon.storageAmmo += ammo;
                WeaponDisplayController.onUpdateAmmoDisplayInfo?.Invoke(player.currentWeaponScript.currentAmmo, player.currentWeaponScript.maxAmmo, player.currentWeaponScript.storageAmmo);
                break;
            }
        }
    }

    private void TryToShoot()
    {
        if (player.currentWeaponScript == null ||
            player.stateMachine.currentState == PlayerStates.State.WallSlide ||
            player.stateMachine.currentState == PlayerStates.State.Ladders ||
            player.stateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        if (player.gatherInput.tryToShoot || !coolDownOver)
            return;
        if (player.currentWeaponScript.isAutomatic)
            return;
        
        Shoot();
    }
    private void StopShooting()
    {

    }

    private void TryToChangeWeapon()
    {
        if (player.currentWeaponScript == null ||
            player.stateMachine.currentState == PlayerStates.State.WallSlide ||
            player.stateMachine.currentState == PlayerStates.State.Ladders ||
            player.stateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        if (player.currentWeaponScript.isRealoading)
            return;
        if(player.currentWeaponType == ItemTypes.PrimaryWeapon)
        {
            if (player.secondaryWeaponPrefab == null)
                return;
            player.primaryWeaponPrefab.SetActive(false);
            player.secondaryWeaponPrefab.SetActive(true);
            player.currentWeapon = player.secondaryWeaponPrefab;
            player.currentWeaponType = ItemTypes.SecondaryWeapon;
            player.currentWeaponScript = player.currentWeapon.GetComponent<Weapon>();
            player.animator.SetLayerWeight(1, 1);
        }
        else
        {
            if (player.primaryWeaponPrefab == null)
                return;
            player.secondaryWeaponPrefab.SetActive(false);
            player.primaryWeaponPrefab.SetActive(true);
            player.currentWeapon = player.primaryWeaponPrefab;
            player.currentWeaponType = ItemTypes.PrimaryWeapon;
            player.currentWeaponScript = player.currentWeapon.GetComponent<Weapon>();
            player.animator.SetLayerWeight(1, 0);
        }
        player.SetWeaponPosition();
        WeaponDisplayController.onUpdateAllDisplayInfo?.Invoke(player.currentWeaponScript.weaponIconSprite, player.currentWeaponScript.currentAmmo, player.currentWeaponScript.maxAmmo, player.currentWeaponScript.storageAmmo);
    }

    private void Shoot()
    {
        if (player.currentWeaponScript.currentAmmo <= 0 && !player.currentWeaponScript.isRealoading)
        {
            if (player.currentWeaponScript is null)
                return;
            if (!player.physicsControl.grounded || player.stateMachine.currentState == PlayerStates.State.Dash ||
                player.stateMachine.currentState == PlayerStates.State.Ladders || player.stateMachine.currentState == PlayerStates.State.KnockBack)
                return;
            if (!player.currentWeaponScript.ReloadCheck() || player.currentWeaponScript.isRealoading)
                return;
            player.stateMachine.ChangeState(PlayerStates.State.Reload);
            return;
        }
        if (player.currentWeaponScript.isRealoading)
            return;

        Instantiate(player.currentWeaponScript.shellPrefab, player.currentWeaponScript.shellSpawnPoint.position, player.currentWeapon.transform.rotation);
        player.currentWeaponScript.shootingEffectPrefab.transform.position = player.currentWeaponScript.shootingPoint.transform.position;
        player.currentWeaponScript.shootingEffectPrefab.SetActive(true);
        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = player.currentWeaponScript.lineWidthMultiplier;
        if(player.stateMachine.currentState != PlayerStates.State.ShootUp)
        {
            player.currentWeapon.transform.localPosition = player.defaultWeaponPositionVector - Vector3.right * player.currentWeaponScript.recoilForce;
        }
        else
        {
            player.currentWeapon.transform.localPosition = player.defaultWeaponPositionVector - Vector3.up * player.currentWeaponScript.recoilForce;
        }
        Vector3 direction = player.currentWeaponScript.shootingPoint.right;
        RaycastHit2D hitInfo = Physics2D.Raycast(player.currentWeaponScript.shootingPoint.position, direction, Mathf.Infinity, hitDetectionLayerMask);
        if (hitInfo)
        {
            lineStartPoint = player.currentWeaponScript.shootingPoint.position;
            lineEndPoint = hitInfo.point;
            Vector2 hitParticleEffectNormal = hitInfo.normal;
            float hitParticleEffectAngle = Mathf.Atan2(hitParticleEffectNormal.y, hitParticleEffectNormal.x) * Mathf.Rad2Deg;
            Quaternion hitParticleEffectRotation = Quaternion.Euler(0, 0, hitParticleEffectAngle);
            Instantiate(player.currentWeaponScript.shootHitParticleEffect, lineEndPoint, hitParticleEffectRotation);
            lineRenderer.SetPosition(0, lineStartPoint);
            lineRenderer.SetPosition(1, lineEndPoint);
            EnemyStats enemyObject = hitInfo.collider.GetComponent<EnemyStats>();
            IDamagable damagedObject = hitInfo.collider.GetComponent<IDamagable>();
            if(enemyObject != null)
            {
                enemyObject.TakeDamage(player.currentWeaponScript.damage);
            }
            else if(damagedObject != null)
            {
                damagedObject.TakeDamage(player.currentWeaponScript.damage);
            }
            Debug.Log("Hit Something");
        }
        else
        {
            lineStartPoint = player.currentWeaponScript.shootingPoint.position;
            lineEndPoint = player.currentWeaponScript.shootingPoint.position + direction * 10;
            lineRenderer.SetPosition(0, lineStartPoint);
            lineRenderer.SetPosition(1, lineEndPoint);
            Debug.Log("Hit Nothing");
        }
        player.currentWeaponScript.currentAmmo--;
        WeaponDisplayController.onUpdateAmmoDisplayInfo?.Invoke(player.currentWeaponScript.currentAmmo, player.currentWeaponScript.maxAmmo, player.currentWeaponScript.storageAmmo);
        StartCoroutine(ShootCoolDown());
        StartCoroutine(ResetShootLine());
        StartCoroutine(ShootRecoil());
        if (player.currentWeaponScript.currentAmmo <= 0 && !player.currentWeaponScript.isRealoading)
        {
            if (player.currentWeaponScript is null)
                return;
            if (!player.physicsControl.grounded || player.stateMachine.currentState == PlayerStates.State.Dash ||
                player.stateMachine.currentState == PlayerStates.State.Ladders || player.stateMachine.currentState == PlayerStates.State.KnockBack)
                return;
            if (!player.currentWeaponScript.ReloadCheck() || player.currentWeaponScript.isRealoading)
                return;
            player.stateMachine.ChangeState(PlayerStates.State.Reload);
            return;
        }
    }

    private IEnumerator ShootCoolDown()
    {
        coolDownOver = false;
        yield return new WaitForSeconds(player.currentWeaponScript.shootCooldown);
        coolDownOver = true;
    }
    private IEnumerator ResetShootLine()
    {
        isShootLineActive = true;
        yield return new WaitForSeconds(player.currentWeaponScript.visibleLineTime);
        lineRenderer.positionCount = 0;
        isShootLineActive = false;
    }
    private IEnumerator ShootRecoil()
    {
        yield return new WaitForSeconds(player.currentWeaponScript.recoilTime);
        player.currentWeapon.transform.localPosition = player.defaultWeaponPositionVector;
    }
}
