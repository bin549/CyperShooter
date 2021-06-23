using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MySqlEntity
{
    public int state = 0;
    public ConnectType connectType;
    public MySqlConnection mySqlConnection;
    public MySqlCommand mySqlCommand;
    public MySqlDataReader mySqlDataReader;
    public bool Result = false;

    public MySqlEntity(ConnectType connectType)
    {
        this.connectType = connectType;
        switch (connectType)
        {
            case ConnectType.USER:
                //mySqlConnection = new MySqlConnection(MySqlHelper.linkUserDB);
                break;
            case ConnectType.GAME:
                mySqlConnection = new MySqlConnection(MySqlHelper.linkGameDB);
                break;
        }
    }

    public void CreateCMD(string sql)
    {
        mySqlCommand = new MySqlCommand(sql, mySqlConnection);
        try
        {
            mySqlConnection.Open();
        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
        }
    }

    internal void Reset()
    {
        Result = false;
        mySqlCommand = null;
        mySqlDataReader = null;
        state = 0;
        mySqlConnection.Close();
    }

    internal void ExecuteNonQuery()
    {
        try
        {
            Result = mySqlCommand.ExecuteNonQuery() > 0 ? true : false;
        }
        catch (Exception e)
        {
            Result = false;
            Debug.Log(e.Message);
        }
        state = 1;
    }

    internal void ExecuteReader()
    {
        try
        {
            mySqlDataReader = mySqlCommand.ExecuteReader();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message); ;
        }
        state = 1;
    }
}
