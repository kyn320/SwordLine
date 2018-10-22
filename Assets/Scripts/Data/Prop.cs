using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 속성을 정의하는 클래스입니다.
/// </summary>
[System.Serializable]
public class Prop
{
    /// <summary>
    /// 이름
    /// </summary>
    [Header("이름")]
    public string name;
    /// <summary>
    /// 설명
    /// </summary>
    [Header("설명")]
    [TextArea]
    public string context;
    /// <summary>
    /// 속성 타입
    /// </summary>
    [Header("타입")]
    public PropType type;
    /// <summary>
    /// 속성 색상
    /// </summary>
    [Header("색상")]
    public Color color;
    /// <summary>
    /// 레벨에 따른 데미지 증가량
    /// </summary>
    [Header("레벨 당 데미지 배수")]
    public List<float> levelToMultipleDamage;
    /// <summary>
    /// 레벨에 따른 재사용 시간
    /// </summary>
    [Header("레벨 당 쿨타임")]
    public List<float> levelToCoolTime;
    /// <summary>
    /// 레벨에 따른 확률
    /// </summary>
    [Header("레벨 당 확률")]
    public List<float> levelToPercent;
    /// <summary>
    /// 이펙트 이름
    /// </summary>
    [Header("이펙트 오브젝트")]
    public string effectName;
}

[System.Serializable]
public enum PropType
{
    OverDrive,
    Hacking,
    Hologram
}
