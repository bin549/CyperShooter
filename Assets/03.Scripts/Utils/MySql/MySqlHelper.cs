using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MySqlHelper
{
    //public static string linkUserDB = "server=127.0.0.1;port=3306;database=loluser;user=root;password=123456;";
    public static string linkGameDB = "server=127.0.0.1;port=3306;database=cybershooter;user=root;password=123456;";

    public static List<MySqlEntity> RecoveryPool = new List<MySqlEntity>();

    private static MySqlEntity Get(ConnectType connectType, string sql)
    {
        MySqlEntity mysqlEntity = Connect(connectType);
        mysqlEntity.CreateCMD(sql);
        return mysqlEntity;
    }

    private static MySqlEntity Connect(ConnectType connectType)
    {
        var poolCount = RecoveryPool.Count;
        Debug.Log("Pool Count:" + poolCount);

        for (int i = poolCount; i > 0; i--)
        {
            if (RecoveryPool[i - 1].state == 1)
            {
                if (RecoveryPool[i - 1].connectType == connectType)
                {
                    RecoveryPool[i - 1].Reset();
                    return RecoveryPool[i - 1];
                }
                else
                {
                    RecoveryPool[i - 1].mySqlConnection.Close();
                    RecoveryPool.RemoveAt(i - 1);
                }
            }
        }

        switch (connectType)
        {
            case ConnectType.USER:
                return new MySqlEntity(connectType);
            case ConnectType.GAME:
                return new MySqlEntity(connectType);
        }
        return null;
    }

    public static MySqlDataReader SelectCMD(ConnectType connectType, string sqlCMD)
    {
        MySqlEntity entity = Get(connectType, sqlCMD);
        entity.ExecuteReader();
        if (!RecoveryPool.Contains(entity))
        {
            RecoveryPool.Add(entity);
        }
        return entity.mySqlDataReader;
    }

    public static bool UpdateCMD(ConnectType connectType, string sqlCMD)
    {
        MySqlEntity entity = Get(connectType, sqlCMD);
        entity.ExecuteNonQuery();
        if (!RecoveryPool.Contains(entity))
        {
            RecoveryPool.Add(entity);
        }
        return entity.Result;
    }

    public static bool AddCMD(ConnectType connectType, string sqlCMD)
    {
        MySqlEntity entity = Get(connectType, sqlCMD);
        entity.ExecuteNonQuery();

        if (!RecoveryPool.Contains(entity))
        {
            RecoveryPool.Add(entity);
        }
        return entity.Result;
    }

    public static bool DeleteCMD(ConnectType connectType, string sqlCMD)
    {
        MySqlEntity entity = Get(connectType, sqlCMD);
        entity.ExecuteNonQuery();
        if (!RecoveryPool.Contains(entity))
        {
            RecoveryPool.Add(entity);
        }
        return entity.Result;
    }

}

public enum ConnectType
{
    USER,
    GAME,
}
