using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : SavableSingleton<UserData>
{
    public override List<SaveSystem.IFriendWith_SaveSystem> Instances { get; protected set; } = instances;
    public static List<SaveSystem.IFriendWith_SaveSystem> instances = new();

    #region ÉfÅ[É^
    [SerializeField] public string playerName = "mao";
    #endregion


}