using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSoundFeedback : MonoBehaviour
{
    [SerializeField]
    AudioSource wrongAnswerAudioSource;
    [SerializeField]
    AudioSource correctAnswerAudioSource;

    public static PuzzleSoundFeedback instance = null;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void WrongAnswer() => wrongAnswerAudioSource.Play();

    public void CorrectAnswer() => correctAnswerAudioSource.Play();
}
