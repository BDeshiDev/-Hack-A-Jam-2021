using System;
using System.Collections;
using System.Collections.Generic;
using bdeshi.utility;
using BDeshi.Utility;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.Combat
{
    public class ScoreViewer: MonoBehaviour
    {
        public float animTimePerLine = .332f;

        public TextMeshProUGUI survivedText;
        public TextMeshProUGUI WaveText;
        public TextMeshProUGUI KilledText;
        public TextMeshProUGUI hypnotizedText;
        public TextMeshProUGUI CyclesText;

        public Tween createScoreLineAnim(TextMeshProUGUI tmp, String s)
        {
            tmp.text = s;
            tmp.alpha = 0;
            tmp.transform.localScale = Vector3.zero;

            Vector3 startPos = tmp.rectTransform.localPosition;
            startPos.x = -tmp.rectTransform.rect.width/2;
            tmp.rectTransform.localPosition = startPos;
            return DOTween.Sequence()
                .Append(tmp.DOFade(1, animTimePerLine))
                .Insert(0, tmp.rectTransform.DOMoveX( tmp.rectTransform.rect.width/2, animTimePerLine))
                .Insert(0, tmp.rectTransform.DOScale(Vector3.one, animTimePerLine * .5f))
                .Append(tmp.rectTransform.DOPunchScale(Vector3.one * 1.5f, animTimePerLine * .5f));
        }
//
        public Sequence appendTo(Sequence sequence)
        {
            Spawner spawner = FindObjectOfType<Spawner>();
            PowerupCycle powerupCycle = FindObjectOfType<PowerupCycle>();

            sequence
                .Append(createScoreLineAnim(survivedText, 
                    $"Time Survived: {spawner.spawnerRunningTime} secs"))
                .Append(createScoreLineAnim(WaveText, 
                    $"Reached Wave: {spawner.lastReachedWave}."))
                .Append(createScoreLineAnim(KilledText,
                    $"Enemies Killed: {spawner.totalEnemiesKilled}."))
                .Append(createScoreLineAnim(hypnotizedText,
                    $"Total Time enemies spent hypnotized : {spawner.totalHypnoTime} secs."))
                .Append(createScoreLineAnim(CyclesText,
                    $"Total Power up Cycles: {powerupCycle.NumPowerupCyclesCompleted}."));

            return sequence;
        }

    }
}