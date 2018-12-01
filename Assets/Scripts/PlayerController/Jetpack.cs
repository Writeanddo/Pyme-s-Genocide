
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] float m_strength;
    public float Strength { get { return m_strength; } }

    [SerializeField] int m_AmmoPerSecond;
    public int AmmoPerSecond { get { return m_AmmoPerSecond; } }
}
