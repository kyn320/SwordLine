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
    public string name;
    /// <summary>
    /// 설명
    /// </summary>
    [TextArea]
    public string context;
    /// <summary>
    /// 속성 타입
    /// </summary>
    public PropType type;
    /// <summary>
    /// 속성 색상
    /// </summary>
    public Color color;
    /// <summary>
    /// 레벨에 따른 데미지 증가량
    /// </summary>
    public List<float> levelToMultipleDamage;
    /// <summary>
    /// 레벨에 따른 재사용 시간
    /// </summary>
    public List<float> levelToCoolTime;
    /// <summary>
    /// 레벨에 따른 확률
    /// </summary>
    public List<float> levelToPercent;
    /// <summary>
    /// 이펙트 이름
    /// </summary>
    public string effectName;
}

[System.Serializable]
public enum PropType
{
    OverDrive,
    Hacking,
    Hologram
}
