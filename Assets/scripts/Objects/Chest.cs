using UnityEngine;

public class Chest : Interactable
{
    public Sprite CHEST_OPEN;
    public ItemType[] contents = { ItemType.POTION_RED };

    public ParticleSystem itemGlow;

    private bool animatingUp = false;
    private bool animatingDown = false;
    private float acceleration = 50f;
    private bool waiting = false;
    private float waitTime = 1.5f;

    private Vector3 targetPos = Vector3.zero;
    private float speed = 2f;
    private float downSpeed = 3.0f;
    private PickupItem item;
    private ParticleSystem glow;

    private SpriteMask mask;

    public Chest()
    {
        this.InfoText = "<Press E to open>";
    }

    public void Start(){
        mask = GetComponent<SpriteMask>();
    }

    public override void Interact()
    {
        this.interactable = false;
        this.GetComponent<SpriteRenderer>().sprite = CHEST_OPEN;
        SFXManager.PlaySFX(SFX_TYPE.STAIRS_DOWN);
        for (int i = 0; i < contents.Length; i++)
        {
            Vector3 pos = transform.position;
            targetPos = new Vector3(pos.x, pos.y + 1f, pos.z);
            GameObject obj = (GameObject)LootManager.InstantiateItem(contents[i], pos);
            item = obj.GetComponent<PickupItem>();
            item.Grabbable = false;
            item.Float();
            animatingUp = true;
            glow = Instantiate(itemGlow, PlacementUtil.PosAtZ(-1f, item.gameObject), Quaternion.Euler(new Vector3(90f, 0f, 0f)));
        }
    }

    public void Update()
    {
        if (animatingUp || animatingDown)
        {
            float step = speed * Time.deltaTime;
            if(animatingDown){
                downSpeed += Time.deltaTime * acceleration;
                step = downSpeed * Time.deltaTime;
            }
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPos, step);
            if(animatingUp){
                glow.transform.position = item.transform.position;
            }
            if (item.transform.position == targetPos)
            {
                if(animatingUp){
                    waiting = true;
                } 
                if(animatingDown){
                    item.Grabbable = true;
                    item.Ground();
                }
                animatingUp = false;
                animatingDown = false;
            }
        }
        if (waiting && waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else if (waiting && waitTime <= 0)
        {
            Destroy(glow);
            Destroy(mask);
            if (!item.Pickup())
            {
                targetPos = transform.position + new Vector3(0f, -1f, 0f);
                animatingDown = true;
                speed = downSpeed;
            }
            waiting = false;
        }
    }
}