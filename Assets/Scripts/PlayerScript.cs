using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerScript : MonoBehaviour
{
    public EnemyManager EnemyManager;
    public MinionManager minionManager;
    public float moveSpeed = 5f;
    public bool canMove = false;
    private Vector3 targetPos;
    public GameObject moveRadius;
    public bool inCombat = true;

    public bool canAttack = false;

    //здоровье
    public GameObject healthBar;
    public float hp = 10;

    public float maxHp = 10;

    //вращение меча
    public GameObject bowButton;
    public GameObject swordButton;
    public GameObject fireballButton;

    private Animator _animator;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    //ходы  
    public int turnsPassed = 0;
    private List<CooldownService> _cooldownServices = new List<CooldownService>();
    public CooldownService _bowCooldownService;
    public CooldownService _swordCooldownService;
    public CooldownService _fireballCooldownService;

    private void Start()
    {
        _bowCooldownService = new CooldownService(bowButton, "ЛКМ - лук", "ЛУК", "bow");
        _cooldownServices.Add(_bowCooldownService);
        _swordCooldownService = new CooldownService(swordButton, "Z - меч","МЕЧ", "sword");
        _cooldownServices.Add(_swordCooldownService);
        _fireballCooldownService = new CooldownService(fireballButton, "2 - прицел fireball, X - выстрелить им","FIREBALL", "fireball");
        _cooldownServices.Add(_fireballCooldownService);
        hp = maxHp;
        targetPos = transform.position;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<TextMeshPro>().text = hp.ToString(CultureInfo.InvariantCulture) + "/" +
                                                     maxHp.ToString(CultureInfo.InvariantCulture);


        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndTurn();
        }


        if (canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _animator.SetBool(IsWalking, true);

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("moveRadius"))
                {
                    //todo сделать так чтобы нельзя было выйти за границы комнаты
                    targetPos = hit.point;

                    if (EnemyManager.HasEnemies())
                    {
                        moveRadius.SetActive(false);
                        StartCoroutine(endMovement());
                    }
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (Mathf.Approximately((targetPos - transform.position).magnitude, 0))
            {
                _animator.SetBool(IsWalking, false);
            }
        }
    }


    public IEnumerator endMovement()
    {
        yield return new WaitForSeconds(3f);
        canMove = false;
    }

    public void myTurn()
    {
        canMove = true;
        canAttack = true;
        moveRadius.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void EndTurn()
    {
        turnsPassed++;
            // EnemyManager.SpawnEnemy();


        minionManager.NewTurn();
        canMove = false;
        canAttack = false;
        
        foreach (var x in _cooldownServices)
        {
            x.Handle();
        }
        

    
    }

    public void GetCooldown(string usedAbility)
    {
        foreach (var cooldownService in _cooldownServices)
        {
            if (cooldownService.usedAbility == usedAbility)
            {
                cooldownService.cooldown = 2;
                cooldownService.UpdateCooldownText();
                break;
            }
        }
       
    }
}