using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : Character
{
    public int maxHealth = 100;
    public Sprite[] objects;

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
        TurnIntoObject();
    }

    // WHEN PLAYER TAKES DAMAGE
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

    // end

    // TO CHANGE HEALTH BAR SIZE ON HUNTER SCREEN
    private void MakeHealthBarSmaller()
    {
        // get health gameobject
        GameObject Health = transform.Find("Health").gameObject;
        if (Health)
        {
            // print("Health name" + Health.name);
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

    // end

    // TO TURN INTO OBJECT
    void TurnIntoObject()
    {
        BoxCollider2D boxy = gameObject.GetComponent<BoxCollider2D>();
        CapsuleCollider2D capy = gameObject.GetComponent<CapsuleCollider2D>();
        Spriter2UnityDX.EntityRenderer entity =
            gameObject.GetComponent<Spriter2UnityDX.EntityRenderer>();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        void OnKeyPress(Sprite sprite)
        {
            // set new sprite
            if (spriteRenderer == null)
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingLayerName = LayerMask.LayerToName(gameObject.layer);
            spriteRenderer.sortingOrder = 2;
            // new object size
            playerScale = 1f;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * playerScale;
            // disable old sprite
            entity.enabled = false;
            spriteRenderer.enabled = true;
            // collider size
            Vector2 size = sprite.bounds.size;
            capy.size = size;
            capy.offset = new Vector2(0, size.y / 2f);
            size.y = size.y / 2f;
            boxy.size = size;
            boxy.offset = new Vector2(0, size.y / 2f);
        }
        // assign object sprite
        if (Input.GetKeyDown(KeyCode.Alpha1) && objects[0])
            OnKeyPress(objects[0]);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && objects[1])
            OnKeyPress(objects[1]);
        else if (Input.GetKeyDown(KeyCode.R))
        {
            // new object size
            playerScale = 0.2f;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * playerScale;
            spriteRenderer.sprite = null;
            entity.enabled = true;
            boxy.size = new Vector2(2.4f, 0.748f);
            boxy.offset = new Vector2(-0.12f, 0.4f);
            capy.size = new Vector2(4.345342f, 5.739083f);
            capy.offset = new Vector2(-0.2070716f, 2.863083f);
        }
    }
}
