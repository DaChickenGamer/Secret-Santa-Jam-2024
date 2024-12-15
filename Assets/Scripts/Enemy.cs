using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private float _speed = 10.0f;

    public GameObject gameManagerObject;
    private Vector3 _spawnPosition;

    private int _health = 100;
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (_player == null) return;
        
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
    }

    public void GetDamaged(int damageReceived)
    {
        _health -= damageReceived;

        if (_health > 0) return;
        
        Die();
    }

    private void Die()
    {
        GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
        gameManager.AddPotentialSpawnPoint(_spawnPosition);
        gameManager.DecreaseCurrentEnemiesAlive();
        Destroy(transform.parent.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Decrease player health here
            
            // For Testing On Death Features
            GetDamaged(100);
        }
    }

    public void SetSpawnPosition(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
    }
}
