using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : Character
{
    public int maxHealth = 100;

    // [SyncVar]
    public int currentHealth;
    public HealthBar healthBar;

    [SerializeField]
    private bool isHealthBarSmall = false;

    public bool IsHealthBarSmall
    {
        get { return isHealthBarSmall; }
        set
        {
            isHealthBarSmall = value;
            if (value)
            {
                // Run your custom function here
                MakeHealthBarSmaller();
            }
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        base.Start();
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    protected override void Update()
    {
        MakeHealthBarSmaller();
        if (!isLocalPlayer)
            return;
        base.Update();
    }

    // [Command]
    [ClientRpc]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void MakeHealthBarSmaller()
    {
        // get health gameobject
        GameObject Health = transform.Find("Health").gameObject;
        if (Health)
        {
            print("Health name" + Health.name);
            if (isHealthBarSmall)
            {
                //  make it smaller
                Canvas canvs = Health.GetComponent<Canvas>();
                canvs.renderMode = RenderMode.ScreenSpaceCamera;
                canvs.worldCamera = GameObject
                    .FindGameObjectWithTag("MainCamera")
                    .GetComponent<Camera>();

                // canvas scaller
                CanvasScaler canvasScaler = Health.GetComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                canvasScaler.scaleFactor = 1;
                canvasScaler.referencePixelsPerUnit = 100;

                //  adjust x y axis
                RectTransform healthBarRectTransform = healthBar?.GetComponent<RectTransform>();
                if (healthBarRectTransform)
                {
                    Vector3 posi = transform.position;
                    posi.y = posi.y + 1.3f;
                    healthBarRectTransform.position = posi;
                }
            }
        }
        else
        {
            print("not ehre");
        }
        // adjust settings
    }
}
