using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// ReSharper disable PossibleLossOfFraction


public class WiseTree : MonoBehaviour
{
    [SerializeField] private bool gameIsPaused;
    [SerializeField] private List<SkillButton> buttons;
    [SerializeField] private Button activeSkill;
    private GameObject _lowerCanvas;
    private Player _player;
    private List<Image> _icons;
    private List<Image> _prices;

    private void Awake()
    {
        _player = FindObjectOfType<Player>().gameObject.GetComponent<Player>();

        var branches = new List<List<Skill>>();

        var fireBranch = new List<Skill>
        {
            new(3, ExperienceTypes.Fire, "Увеличивает урон всего оружия на 5 единиц.", () =>
            {
                _player.ChangeMeleeAttack(5);
                _player.ChangeRangeAttack(5);
                buttons[0].interactable = false;
            }),
            new(6, ExperienceTypes.Fire, "Позволяет мгновенно убить любого врага с вероятностью 10%.", () =>
            {
                var enemies = FindObjectsOfType<Enemy>();
                foreach (var enemy in enemies)
                {
                    enemy.HealthSystem.onDamageTaken.AddListener(
                        _ =>
                        {
                            if (Random.Range(0, 100) > 10) return;
                            Debug.Log("Enemy got assassinated");
                            Destroy(enemy.gameObject);
                        }
                    );
                }

                buttons[1].interactable = false;
            }),
            new(9, ExperienceTypes.Fire,
                "Увеличивает урон всего оружия на 10 единиц в течение 15 секунд. Кулдаун 30 секунд.", () =>
                {
                    const int damageBonus = 10;
                    activeSkill.GetComponent<Image>().sprite = buttons[2].GetComponent<Image>().sprite;
                    activeSkill.gameObject.SetActive(true);
                    activeSkill.onClick.AddListener(() =>
                    {
                        _player.ChangeMeleeAttack(damageBonus);
                        _player.ChangeRangeAttack(damageBonus);
                        activeSkill.interactable = false;
                        StartCoroutine(Utils.DoActionAfterDelay(15, () =>
                        {
                            _player.ChangeMeleeAttack(-damageBonus);
                            _player.ChangeRangeAttack(-damageBonus);
                        }));
                        StartCoroutine(Utils.DoActionAfterDelay(30, () => activeSkill.interactable = true));
                    });
                    buttons[2].interactable = false;
                })
        };

        var earthBranch = new List<Skill>
        {
            new(3, ExperienceTypes.Earth,
                "Увеличивает здоровье на 20 единиц.",
                () =>
                {
                    _player.HealthSystem.IncreaseMaxHealth(20);
                    _player.HealthSystem.Heal(20);
                    buttons[6].interactable = false;
                }),
            new(6, ExperienceTypes.Earth,
                "Исцеляет игрока на 10% когда ему наносят урон, с вероятностью 15%.",
                () =>
                {
                    _player.HealthSystem.onDamageTaken.AddListener(_ =>
                    {
                        if (Random.Range(0, 100) <= 15)
                            _player.HealthSystem.Heal((int) (_player.HealthSystem.maxHealth * 0.1f));
                    });
                    buttons[7].interactable = false;
                }),
            new(9, ExperienceTypes.Earth,
                "Позволяет сделать мощный выстрел из лука, который проходит сквозь врагов и наносит 15 единиц урона. Кулдаун 60 секунд.",
                () =>
                {
                    activeSkill.GetComponent<Image>().sprite = buttons[8].GetComponent<Image>().sprite;
                    activeSkill.gameObject.SetActive(true);
                    activeSkill.onClick.AddListener(() =>
                    {
                        activeSkill.interactable = false;
                        _player.Shoot(true);
                        StartCoroutine(Utils.DoActionAfterDelay(10, () => activeSkill.interactable = true));
                    });
                    buttons[8].interactable = false;
                })
        };

        var airBranch = new List<Skill>
        {
            new(3, ExperienceTypes.Air, "Увеличивает скорость полета стрел в 1.5 раза.", () =>
            {
                _player.IncreaseRangeAttacksSpeed(5);
                buttons[3].interactable = false;
            }),
            new(6, ExperienceTypes.Air,
                "Увеличивает скорость передвижения игрока в 2 раза на 5 секунд. Кулдаун 30 секунд.",
                () =>
                {
                    activeSkill.GetComponent<Image>().sprite = buttons[4].GetComponent<Image>().sprite;
                    activeSkill.gameObject.SetActive(true);
                    activeSkill.onClick.AddListener(() =>
                    {
                        _player.ChangeSpeedValue(_player.Speed);
                        activeSkill.interactable = false;
                        StartCoroutine(Utils.DoActionAfterDelay(5,
                            () => _player.ChangeSpeedValue(-_player.Speed / 2)));
                        StartCoroutine(Utils.DoActionAfterDelay(30, () => activeSkill.interactable = true));
                    });
                    buttons[4].interactable = false;
                }),
            new(9, ExperienceTypes.Air, "Позволяет избежать любого входящего урона по игроку с шансом 25%.", () =>
            {
                _player.IsEvasionLearned = true;
                buttons[5].interactable = false;
            })
        };

        branches.Add(fireBranch);
        branches.Add(airBranch);
        branches.Add(earthBranch);
        var skills = branches.SelectMany(s => s).ToList();
        for (var i = 0; i < skills.Count; i++)
        {
            var i1 = i;
            buttons[i].Skill = skills[i];
            buttons[i1].onClick.AddListener(() => _player.ExperienceSystem.LearnSkill(skills[i1], i1 / 3, i1 % 3));
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
    }


    public void ShowSkillTree()
    {
        if (!gameIsPaused)
        {
            _lowerCanvas ??= GameObject.Find("LowerCanvas");
            _lowerCanvas.SetActive(false);
            foreach (var child in buttons)
            {
                child.gameObject.SetActive(true);
            }

            _icons ??= GameObject.Find("UpperCanvas/SkillIconsFrame").GetComponentsInChildren<Image>(true).ToList();

            foreach (var child in _icons)
            {
                child.gameObject.SetActive(true);
            }

            _prices ??= GameObject.Find("UpperCanvas/SkillCostFrame").GetComponentsInChildren<Image>(true).ToList();
            foreach (var child in _prices)
            {
                child.gameObject.SetActive(true);
            }


            Time.timeScale = 0f;
            gameIsPaused = true;
        }
        else
        {
            _lowerCanvas ??= GameObject.Find("LowerCanvas");
            _lowerCanvas.SetActive(true);
            foreach (var child in buttons)
            {
                child.gameObject.SetActive(false);
            }

            _icons ??= GameObject.Find("UpperCanvas/SkillIconsFrame").GetComponentsInChildren<Image>(true).ToList();
            foreach (var child in _icons)
            {
                child.gameObject.SetActive(false);
            }

            _prices ??= GameObject.Find("UpperCanvas/SkillCostFrame").GetComponentsInChildren<Image>(true).ToList();
            foreach (var child in _prices)
            {
                child.gameObject.SetActive(false);
            }

            Time.timeScale = 1f;
            gameIsPaused = false;
        }
    }
}