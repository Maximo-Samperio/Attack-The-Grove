using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
   
    //private Camera _camera;

    private void Start()
    {
        //_camera = Camera.main;
    }
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _healthbarSprite.fillAmount = currentHealth / maxHealth;
    }

    //private void Update()
    //{
    //    transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
    //}
}
