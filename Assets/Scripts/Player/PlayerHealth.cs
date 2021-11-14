using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : AliveCreature
{
    [SerializeField] private float maxMonsterHp;
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float getBiggerValue;
    public float getSmallerValue;
    [SerializeField] private float timeToGetSmaller;

    [SerializeField] private Slider healhtSlider;
    [SerializeField] private Text healhtText;
    [SerializeField] private Slider sizeSlider;
    [SerializeField] private Text sizeText;

    private float prevTimeGotSmaller;

    private float maxRegularHp;

    [Header("Camera Shake")]
    private CameraShake cameraShakeComponent;
    [SerializeField] private float duration;
    [SerializeField] private float magnitude;

    protected override void Start()
    {
        base.Start();

        healhtSlider.minValue = 0;
        UpdateHealth();

        sizeSlider.minValue = 0;
        UpdateSize();

        maxRegularHp = maxHp;

        cameraShakeComponent = Camera.main.GetComponent<CameraShake>();
    }

    public override void ReceiveDamage(float damageAmount)
    {
        base.ReceiveDamage(damageAmount);

        UpdateHealth();

        StartCoroutine(cameraShakeComponent.Shake(duration, magnitude));
    }

    private void Update()
    {
        if (Time.time - prevTimeGotSmaller > timeToGetSmaller)
        {
            GetSmaller();
            prevTimeGotSmaller = Time.time;
        }
    }

    public void Heal(float toHeal)
    {
        GameManager.Instance.floatingTextManager.Show("+" + toHeal.ToString(), 20, Color.green, transform.position, new Vector3(50, 60, 0), 1f);
        
        if (currentHp == maxHp)
            return;

        currentHp += toHeal;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        UpdateHealth();
    }

    private void UpdateHealth()
    {
        healhtSlider.maxValue = maxHp;
        healhtSlider.value = currentHp;
        healhtText.text = currentHp.ToString();
    }

    private void UpdateSize()
    {
        sizeSlider.maxValue = maxScale - minScale;
        sizeSlider.value = transform.localScale.x - minScale;
        sizeText.text = ((int)(sizeSlider.value * 100)).ToString();
    }

    public void Mutate(float toHeal)
    {
        maxHp = maxMonsterHp;
        if (GameManager.Instance.state == GameManager.State.regular)
            currentHp = maxMonsterHp - (maxRegularHp - currentHp);

        Heal(toHeal);

        UpdateHealth();
        UpdateSize();
    }

    public void DeMutate()
    {
        maxHp = maxRegularHp;
        currentHp = Mathf.Min(currentHp, maxRegularHp);

        UpdateHealth();
        UpdateSize();
    }

    public void GetBigger()
    {
        if (transform.localScale.x + getBiggerValue < maxScale)
        {
            transform.localScale += new Vector3(getBiggerValue, getBiggerValue, 0);
        }
        else
        {
            transform.localScale = new Vector3(maxScale, maxScale, 0);
        }
        UpdateSize();
    }

    public void GetSmaller()
    {
        if (transform.localScale.x - getSmallerValue > minScale)
        {
            transform.localScale -= new Vector3(getSmallerValue, getSmallerValue, 0);
        }
        else if (GameManager.Instance.state == GameManager.State.mutated)
        {
            GameManager.Instance.playerAttack.DeMutate();
            DeMutate();
        }
        UpdateSize();
    }

    public override void Death()
    {
        if (GameManager.Instance.state == GameManager.State.regular)
        {
            currentHp = maxHp;
            transform.position = Vector3.zero;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            GameManager.Instance.playerAttack.DeMutate();
            DeMutate();
            currentHp = maxHp;
        }
    }
}
