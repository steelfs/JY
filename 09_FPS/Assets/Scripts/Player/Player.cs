using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    bool isAlive = true;
    public bool IsAlive => isAlive;
    float hp;
    public float MaxHP = 100.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp<=0 && isAlive)
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHPChange?.Invoke(hp);
        }
    }
    public Action onDie;
    public Action<float> onHPChange;

    GameObject gunCamera;

    GunBase activeGun;
    GunBase defaultGun;
    GunBase[] powerGuns;    

    StarterAssets.FirstPersonController controller;

    CharacterController cc;

    private void Awake()
    {
        gunCamera = transform.GetChild(2).gameObject;

        Transform child = transform.GetChild(3);
        defaultGun = child.GetComponent<GunBase>();
        defaultGun.onFireRecoil += GunFireRecoil;

        child = transform.GetChild(4);
        powerGuns = child.GetComponentsInChildren<GunBase>(true);
        foreach (var gun in powerGuns)
        {
            gun.onFireRecoil += GunFireRecoil;
            gun.onBulletCountChange += OnAmmoDepleted;
        }

        activeGun = defaultGun;        

        controller = GetComponent<StarterAssets.FirstPersonController>();
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Crosshair crosshair = FindAnyObjectByType<Crosshair>();
        
        defaultGun.onFireRecoil += (expend) => crosshair.Expend(expend * 10);

        foreach(var gun in powerGuns)
        {
            gun.onFireRecoil += (expend) => crosshair.Expend(expend * 10);
        }

        HP = MaxHP;
        GunChange(GunType.Revoler);

        Spawn();
    }

    private void GunFireRecoil(float recoil)
    {
        controller.FireRecoil(recoil);
    }

    /// <summary>
    /// 총 용 카메라 활성화 설정
    /// </summary>
    /// <param name="show">true면 총이 보인다., flase면 총이 안보인다.</param>
    public void ShowGunCamera(bool show = true)
    {
        gunCamera.SetActive(show);
    }

    public void GunFire(bool isFireStart)
    {
        activeGun.Fire(isFireStart);
    }

    public void GunRevolverReload()
    {
        Revolver revolver = activeGun as Revolver;
        if(revolver != null)
        {
            revolver.Reload();
        }
    }

    public void GunChange(GunType type)
    {
        activeGun.gameObject.SetActive(false);

        GunBase newGun = null;
        switch (type)
        {
            case GunType.Revoler:
                newGun = defaultGun;
                break;
            case GunType.Shotgun:
                newGun = powerGuns[0];
                break;
            case GunType.AssaultRifle: 
                newGun = powerGuns[1];
                break;
        }

        activeGun.UnEquip();
        activeGun = newGun;
        activeGun.Equip();
        activeGun.gameObject.SetActive(true);
    }

    public void AddBulletCountChangeDelegate(Action<int> callback)
    {
        defaultGun.onBulletCountChange = callback + defaultGun.onBulletCountChange;
        foreach (var gun in powerGuns)
        {
            gun.onBulletCountChange = callback + gun.onBulletCountChange;
        }
    }

    private void OnAmmoDepleted(int ammo)
    {
        if (ammo <= 0)
        {
            GunChange(GunType.Revoler);
        }
    }

    public Action<float> onAttacked;
    public void Attacked(Enemy enemy)
    {
        Vector3 dir = enemy.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.forward, dir, transform.up);
        onAttacked?.Invoke(-angle);

        HP -= enemy.attackPower;
    }

    void Die()
    {
        isAlive = false;
        Debug.Log("사망");


        onDie?.Invoke();
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        cc.enabled = false;

        MazeVisualizer maze = FindAnyObjectByType<MazeVisualizer>();
        int width = (int)(maze.width * 0.2f);
        int height = (int)(maze.height * 0.2f);

        int widthMin = (int)((maze.width - width) * 0.5f);
        int widthMax = (int)((maze.width + width) * 0.5f);
        int heightMin = (int)((maze.height - height) * 0.5f);
        int heightMax = (int)((maze.height + height) * 0.5f);

        int x = UnityEngine.Random.Range(widthMin, widthMax);
        int y = UnityEngine.Random.Range(heightMin, heightMax);

        //Debug.Log($"{x}, {y}");

        Vector3 world = maze.GridToWorld(x, y);                
        transform.position = world;

        Ray ray = new(world + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10.0f))
        {
            CellVisualizer cell = hitInfo.collider.gameObject.GetComponentInParent<CellVisualizer>();
            Direction paths = cell.GetPaths();
            Debug.Log(paths);
            List<Vector3> dirList = new List<Vector3>(4);
            if ((paths & Direction.North) != 0)
            {
                dirList.Add(Vector3.forward);
            }
            if ((paths & Direction.East) != 0)
            {
                dirList.Add(Vector3.right);
            }
            if ((paths & Direction.South) != 0)
            {
                dirList.Add(Vector3.back);
            }
            if ((paths & Direction.West) != 0)
            {
                dirList.Add(Vector3.left);
            }

            Vector3 dir = dirList[UnityEngine.Random.Range(0, dirList.Count)];
            transform.LookAt(transform.position + dir);
        }

        cc.enabled = true;

    }
}
