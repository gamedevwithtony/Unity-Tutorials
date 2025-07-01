using System;
using UnityEngine;

public interface ITransitionEffect
{
    // Sets the sprite for the transition effect
    void SetSprite(Sprite sprite);
    // Sets the duration of the transition effect
    void SetDuration(float duration);
    // Displays the given sprite instantly without a transition
    void DisplayImmediately(Sprite sprite);
    // Initiates the transition effect
    void Transition(Action onTransitionComplete = null);
}
