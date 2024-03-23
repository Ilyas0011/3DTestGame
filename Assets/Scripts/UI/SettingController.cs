using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField] private Animator canvasAnimation;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject buttonSettings;
    [SerializeField] private ShootingController shootingController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ZombieController zombieController;

    private int healthPointSettings = 100;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private TextMeshProUGUI healthBarText;

    private int damagePlayerSettings;
    [SerializeField] private Slider damagePlayerSlider;
    [SerializeField] private TextMeshProUGUI damagePlayerText;

    private int speedPlayerSettings;
    [SerializeField] private Slider speedPlayerSlider;
    [SerializeField] private TextMeshProUGUI speedPlayerText;

    private float delayShotSettings;
    [SerializeField] private Slider delayShotSlider;
    [SerializeField] private TextMeshProUGUI delayShotsText;

    private int healthZombieSettings = 100;
    [SerializeField] private Slider healthZombieSlider;
    [SerializeField] private TextMeshProUGUI healthZombieText;

    private int damageZombieSettings = 100;
    [SerializeField] private Slider damageZombieSlider;
    [SerializeField] private TextMeshProUGUI damageZombieText;

    private float speedZombieSettings = 100;
    [SerializeField] private Slider speedZombieSlider;
    [SerializeField] private TextMeshProUGUI speedZombieText;


    void Start()
    {
        canvasAnimation.updateMode = AnimatorUpdateMode.UnscaledTime; // Установка режима проигрывания анимации в нереальном времени
        UpdatePlayerSettings();
        UpdateZombieSettings();

    }

    public void ResetButtonClick()
    {
        PlayerPrefs.SetInt("DamagePowerPlayer", 20);
        PlayerPrefs.SetInt("HP", 100);
        PlayerPrefs.SetInt("moveSpeedPlayer", 9);
        PlayerPrefs.SetFloat("DelayShot", 0.6f);

        PlayerPrefs.SetInt("ZombieHealth", 100);
        PlayerPrefs.SetInt("DamageZombieAmount", 10);
        PlayerPrefs.SetFloat("SpeedZombie", 3f);

        UpdatePlayerSettings();
        UpdateZombieSettings();
    }

    public void UpdatePlayerSettings()
    {
        UpdateHealthSettings();
        UpdateDamagePlayer();
        UpdateSpeedPlayer();
        UpdateTimerShots();
    }

    public void UpdateZombieSettings()
    {
        UpdateHealthZombie();
        UpdateDamageZombie();
        UpdateSpeedZombie();
    }


    public void SettingsExit()
    {
        StartCoroutine(PauseOff(0.4f));
        canvasAnimation.Play("SettingsExit");
        Time.timeScale = 1;
    }

    public IEnumerator PauseOff(float timer)
    {
        yield return new WaitForSeconds(timer);
        settingsCanvas.SetActive(false); // Отключить текущий объект
        buttonSettings.SetActive(true);

    }

    private void UpdateDamagePlayer()
    {
        damagePlayerSettings = playerController.ReturnDamage();
        damagePlayerSlider.value = damagePlayerSettings;
        damagePlayerText.text = damagePlayerSettings.ToString();
    }

    public void ChangeDamageSettings()
    {
        damagePlayerSettings = Mathf.RoundToInt(damagePlayerSlider.value);
        if (damagePlayerSettings < 0)
            damagePlayerSettings = 1;

        damagePlayerText.text = damagePlayerSettings.ToString();
        PlayerPrefs.SetInt("DamagePowerPlayer", damagePlayerSettings);
        playerController.ApplyDamageSettings();
    }

    private void UpdateHealthSettings()
    {
        healthPointSettings = PlayerPrefs.GetInt("HP", 100);

        if (healthPointSettings < 0)
                healthPointSettings = 1;

        healthBarText.text = healthPointSettings.ToString();
        healthBarSlider.value = healthPointSettings;

        Time.timeScale = 0f;
    }

    public void ChangeHealthSettings() //Метод изменения здоровья
    {
        healthPointSettings = Mathf.RoundToInt(healthBarSlider.value);
        if (healthPointSettings < 0)
            healthPointSettings = 0;

        healthBarText.text = healthPointSettings.ToString();
        PlayerPrefs.SetInt("HP", healthPointSettings);
        playerController.ApplyHealthSettings();
    }

    private void UpdateSpeedPlayer()
    {
        speedPlayerSettings = PlayerPrefs.GetInt("moveSpeedPlayer", 9);

        speedPlayerSlider.value = speedPlayerSettings;
        speedPlayerText.text = speedPlayerSettings.ToString();
    }

    public void ChangeSpeedSettings()
    {
        speedPlayerSettings = Mathf.RoundToInt(speedPlayerSlider.value);
        if (speedPlayerSettings < 0)
            speedPlayerSettings = 1;

        speedPlayerText.text = speedPlayerSettings.ToString();
        PlayerPrefs.SetInt("moveSpeedPlayer", speedPlayerSettings);
        playerController.ApplyMoveSpeed();
    }

    private void UpdateTimerShots()
    {
        delayShotSettings = PlayerPrefs.GetFloat("DelayShot", 0.6f);

        delayShotSlider.value = delayShotSettings;
        delayShotsText.text = delayShotSettings.ToString("F2");
    }

    public void ChangeTimerShots()
    {
        delayShotSettings = delayShotSlider.value;
        if (delayShotSettings < 0)
            delayShotSettings = 0.1f;

        delayShotsText.text = delayShotSettings.ToString("F2");
        PlayerPrefs.SetFloat("DelayShot", delayShotSettings);
        shootingController.ApplyStartTimeBtwShots();
    }

    private void UpdateHealthZombie()
    {
        healthZombieSettings = zombieController.ReturnHealthPoint();
        healthZombieSlider.value = healthZombieSettings;
        healthZombieText.text = healthZombieSettings.ToString();
    }

    public void ChangeHealthZombie()
    {
        healthZombieSettings = Mathf.RoundToInt(healthZombieSlider.value);
        if (healthZombieSettings < 0)
            healthZombieSettings = 1;

        healthZombieText.text = healthZombieSettings.ToString();
        PlayerPrefs.SetInt("ZombieHealth", healthZombieSettings);
        zombieController.ApplyZombiesSettings();
    }

    private void UpdateDamageZombie()
    {
        damageZombieSettings = zombieController.ReturnDamageAmount();
        damageZombieSlider.value = damageZombieSettings;
        damageZombieText.text = damageZombieSettings.ToString();
    }

    public void ChangeDamageZombie()
    {
        damageZombieSettings = Mathf.RoundToInt(damageZombieSlider.value);
        if (damageZombieSettings < 0)
            damageZombieSettings = 1;

        damageZombieText.text = damageZombieSettings.ToString();
        PlayerPrefs.SetInt("DamageZombieAmount", damageZombieSettings);
        zombieController.ApplyZombiesSettings();
    }

    private void UpdateSpeedZombie()
    {
        speedZombieSettings = zombieController.ReturnSpeed();
        speedZombieSlider.value = speedZombieSettings;
        speedZombieText.text = speedZombieSettings.ToString();
    }

    public void ChangeSpeedZombie()
    {
        speedZombieSettings = Mathf.RoundToInt(speedZombieSlider.value);
        if (speedZombieSettings < 0)
            speedZombieSettings = 1;

        speedZombieText.text = speedZombieSettings.ToString();
        PlayerPrefs.SetFloat("SpeedZombie", speedZombieSettings);
        zombieController.ApplyZombiesSettings();
    }
}
