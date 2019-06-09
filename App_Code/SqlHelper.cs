using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml;

public class SysError
{
    //记录错误日志位置
    private const string FILE_NAME = "c:\\users\\administrator\\Desktop\\BLCU1\\errlog.txt";
    /// <summary>
    /// 记录日志至文本文件
    /// </summary>
    /// <param name="message">记录的内容</param>
    public static void Log(string message)
    {
        //errMsg = message;
        //if (File.Exists(FILE_NAME))
        //{
        //    StreamWriter sr = File.AppendText(FILE_NAME);
        //    sr.WriteLine("\n");
        //    sr.WriteLine(DateTime.Now.ToString() + message);
        //    sr.Close();
        //}
        //else
        //{
        //    StreamWriter sr = File.CreateText(FILE_NAME);
        //    sr.Close();
        //}
    }
    private static string errMsg = "";
    /// <summary>
    /// 错误信息
    /// </summary>
    public static string ErrMsg
    {
        get { return errMsg; }
    }
}

public sealed class SqlHelper
{
    private static string connectionString = "Data Source=(local);Initial Catalog=BookClass;Integrated Security=SSPI;Max Pool Size=15000";
    /// <summary>
    /// ConnectionString的属性
    /// </summary>
    public static string ConnectionString
    {
        set
        {
            connectionString = value;
        }
        get
        {
            return connectionString;
        }
    }

    #region private utility methods & constructors

    //Since this class provides only static methods, make the default constructor private to prevent 
    //instances from being created with "new SqlHelper()".
    private SqlHelper() { }



    /// <summary>
    /// This method is used to attach array of SqlParameters to a SqlCommand.
    /// 
    /// This method will assign a value of DbNull to any parameter with a direction of
    /// InputOutput and a value of null.  
    /// 
    /// This behavior will prevent default values from being used, but
    /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
    /// where the user provided no input value.
    /// </summary>
    /// <param name="command">The command to which the parameters will be added</param>
    /// <param name="commandParameters">an array of SqlParameters tho be added to command</param>
    private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
    {
        foreach (SqlParameter p in commandParameters)
        {
            //check for derived output value with no value assigned
            if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
            {
                p.Value = DBNull.Value;
            }

            command.Parameters.Add(p);
        }
    }

    /// <summary>
    /// This method assigns an array of values to an array of SqlParameters.
    /// </summary>
    /// <param name="commandParameters">array of SqlParameters to be assigned values</param>
    /// <param name="parameterValues">array of objects holding the values to be assigned</param>
    private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
    {
        if ((commandParameters == null) || (parameterValues == null))
        {
            //do nothing if we get no data
            return;
        }

        // we must have the same number of values as we pave parameters to put them in
        if (commandParameters.Length != parameterValues.Length)
        {
            throw new ArgumentException("Parameter count does not match Parameter Value count.");
        }

        //iterate through the SqlParameters, assigning the values from the corresponding position in the 
        //value array
        for (int i = 0, j = commandParameters.Length; i < j; i++)
        {
            commandParameters[i].Value = parameterValues[i];
        }
    }

    /// <summary>
    /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
    /// to the provided command.
    /// </summary>
    /// <param name="command">the SqlCommand to be prepared</param>
    /// <param name="connection">a valid SqlConnection, on which to execute this command</param>
    /// <param name="transaction">a valid SqlTransaction, or 'null'</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
    private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
    {
        //if the provided connection is not open, we will open it
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        //associate the connection with the command
        command.Connection = connection;

        //set the command text (stored procedure name or SQL statement)
        command.CommandText = commandText;

        //if we were provided a transaction, assign it.
        if (transaction != null)
        {
            command.Transaction = transaction;
        }

        //set the command type
        command.CommandType = commandType;

        //attach the command parameters if they are provided
        if (commandParameters != null)
        {
            AttachParameters(command, commandParameters);
        }

        return;
    }


    #endregion private utility methods & constructors

    #region ExecuteNonQuery

    /// <summary>
    /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
    /// the connection string. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteNonQuery(commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create & open a SqlConnection, and dispose of it after we are done.
        using (SqlConnection cn = new SqlConnection(connectionString))
        {
            cn.Open();

            //call the overload that takes a connection in place of the connection string
            return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
        }
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
    /// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  int result = ExecuteNonQuery("PublishOrders", 24, 36);
    /// </remarks>
    /// <param name="spName">the name of the stored prcedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteNonQuery(CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteNonQuery(CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

        //finally, execute the command.
        int retval = cmd.ExecuteNonQuery();

        // detach the SqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();
        return retval;
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
    /// using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) against the specified SqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

        //finally, execute the command.
        int retval = cmd.ExecuteNonQuery();

        // detach the SqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();
        return retval;
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified 
    /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }
    }


    #endregion ExecuteNonQuery

    #region ExecuteDataSet

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
    /// the connection string. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteDataset(commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset( CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create & open a SqlConnection, and dispose of it after we are done.
        using (SqlConnection cn = new SqlConnection(connectionString))
        {
            cn.Open();

            //call the overload that takes a connection in place of the connection string
            return ExecuteDataset(cn, commandType, commandText, commandParameters);
        }
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
    /// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset("GetOrders", 24, 36);
    /// </remarks>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteDataset(CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteDataset(CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

        //create the DataAdapter & DataSet
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        //fill the DataSet using default values for DataTable names, etc.
        da.Fill(ds);

        // detach the SqlParameters from the command object, so they can be used again.			
        cmd.Parameters.Clear();

        //return the dataset
        return ds;
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    /// using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

        //create the DataAdapter & DataSet
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        //fill the DataSet using default values for DataTable names, etc.
        da.Fill(ds);

        // detach the SqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();

        //return the dataset
        return ds;
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
    /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
    /// the connection string. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(CommandType.StoredProcedure, "GetOrders", "Orders");
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="tableName">the DataSet's table name</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(CommandType commandType, string commandText, string tableName)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteDataset(commandType, commandText, tableName, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset( CommandType.StoredProcedure, "GetOrders", "Orders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="tableName">the DataSet's table name</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(CommandType commandType, string commandText, string tableName, params SqlParameter[] commandParameters)
    {
        //create & open a SqlConnection, and dispose of it after we are done.
        using (SqlConnection cn = new SqlConnection(connectionString))
        {
            cn.Open();

            //call the overload that takes a connection in place of the connection string
            return ExecuteDataset(cn, commandType, commandText, tableName, commandParameters);
        }
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
    /// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset("GetOrders", "Orders", 24, 36);!!!!!!!!!!!!!
    /// </remarks>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="tableName">the DataSet's table name</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDatasetTable(string spName, string tableName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteDataset(CommandType.StoredProcedure, spName, tableName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteDataset(CommandType.StoredProcedure, spName, tableName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", "Orders");
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="tableName">the DataSet's table name</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, string tableName)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteDataset(connection, commandType, commandText, tableName, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", "Orders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="tableName">the DataSet's table name</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, string tableName, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

        //create the DataAdapter & DataSet
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        //fill the DataSet using default values for DataTable names, etc.
        da.Fill(ds, tableName);

        // detach the SqlParameters from the command object, so they can be used again.			
        cmd.Parameters.Clear();

        //return the dataset
        return ds;
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    /// using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  DataSet ds = ExecuteDatasetTable(conn, "GetOrders", "Orders", 24, 36);!!!!!!!!!!!!
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="tableName">the DataSet's table name</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDatasetTable(SqlConnection connection, string spName, string tableName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName, tableName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName, tableName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", "Orders");
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="tableName">the DataSet's table name</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, string tableName)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteDataset(transaction, commandType, commandText, tableName, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", "Orders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="tableName">the DataSet's table name</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, string tableName, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

        //create the DataAdapter & DataSet
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        //fill the DataSet using default values for DataTable names, etc.
        da.Fill(ds, tableName);

        // detach the SqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();

        //return the dataset
        return ds;
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
    /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(trans, "GetOrders", "Orders", 24, 36);!!!!!!!!!!
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="tableName">the DataSet's table name</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDatasetTable(SqlTransaction transaction, string spName, string tableName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, tableName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, tableName);
        }
    }
    #endregion ExecuteDataSet

    #region ExecuteReader

    /// <summary>
    /// this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
    /// we can set the appropriate CommandBehavior when calling ExecuteReader()
    /// </summary>
    private enum SqlConnectionOwnership
    {
        /// <summary>Connection is owned and managed by SqlHelper</summary>
        Internal,
        /// <summary>Connection is owned and managed by the caller</summary>
        External
    }

    /// <summary>
    /// Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
    /// </summary>
    /// <remarks>
    /// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
    /// 
    /// If the caller provided the connection, we want to leave it to them to manage.
    /// </remarks>
    /// <param name="connection">a valid SqlConnection, on which to execute this command</param>
    /// <param name="transaction">a valid SqlTransaction, or 'null'</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
    /// <param name="connectionOwnership">indicates whether the connection parameter was provided by the caller, or created by SqlHelper</param>
    /// <returns>SqlDataReader containing the results of the command</returns>
    private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

        //create a reader
        SqlDataReader dr;

        // call ExecuteReader with the appropriate CommandBehavior
        if (connectionOwnership == SqlConnectionOwnership.External)
        {
            dr = cmd.ExecuteReader();
        }
        else
        {
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        // detach the SqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();

        return dr;
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
    /// the connection string. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  SqlDataReader dr = ExecuteReader( CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>a SqlDataReader containing the resultset generated by the command</returns>
    public static SqlDataReader ExecuteReader(CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteReader(commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  SqlDataReader dr = ExecuteReader(CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a SqlDataReader containing the resultset generated by the command</returns>
    public static SqlDataReader ExecuteReader(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create & open a SqlConnection
        SqlConnection cn = new SqlConnection(connectionString);
        cn.Open();

        try
        {
            //call the private overload that takes an internally owned connection in place of the connection string
            return ExecuteReader(cn, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
        }
        catch
        {
            //if we fail to return the SqlDatReader, we need to close the connection ourselves
            cn.Close();
            throw;
        }
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
    /// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  SqlDataReader dr = ExecuteReader("GetOrders", 24, 36);
    /// </remarks>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a SqlDataReader containing the resultset generated by the command</returns>
    public static SqlDataReader ExecuteReader(string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteReader(CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteReader(CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>a SqlDataReader containing the resultset generated by the command</returns>
    public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a SqlDataReader containing the resultset generated by the command</returns>
    public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //pass through the call to the private overload using a null transaction value and an externally owned connection
        return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    /// using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a SqlDataReader containing the resultset generated by the command</returns>
    public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

            AssignParameterValues(commandParameters, parameterValues);

            return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>a SqlDataReader containing the resultset generated by the command</returns>
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///   SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a SqlDataReader containing the resultset generated by the command</returns>
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //pass through to private overload, indicating that the connection is owned by the caller
        return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
    /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a SqlDataReader containing the resultset generated by the command</returns>
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

            AssignParameterValues(commandParameters, parameterValues);

            return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }
    }

    #endregion ExecuteReader

    #region ExecuteScalar

    /// <summary>
    /// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
    /// the connection string. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar( CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteScalar(commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar( CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create & open a SqlConnection, and dispose of it after we are done.
        using (SqlConnection cn = new SqlConnection(connectionString))
        {
            cn.Open();

            //call the overload that takes a connection in place of the connection string
            return ExecuteScalar(cn, commandType, commandText, commandParameters);
        }
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
    /// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar("GetOrderCount", 24, 36);
    /// </remarks>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteScalar(CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteScalar(CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

        //execute the command & return the results
        object retval = cmd.ExecuteScalar();

        // detach the SqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();
        return retval;

    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
    /// using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

        //execute the command & return the results
        object retval = cmd.ExecuteScalar();

        // detach the SqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();
        return retval;
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified
    /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }
    }

    #endregion ExecuteScalar

    #region ExecuteXmlReader

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
    /// <returns>an XmlReader containing the resultset generated by the command</returns>
    public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an XmlReader containing the resultset generated by the command</returns>
    public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

        //create the DataAdapter & DataSet
        XmlReader retval = cmd.ExecuteXmlReader();

        // detach the SqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();
        return retval;

    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    /// using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">a valid SqlConnection</param>
    /// <param name="spName">the name of the stored procedure using "FOR XML AUTO"</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>an XmlReader containing the resultset generated by the command</returns>
    public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
    /// <returns>an XmlReader containing the resultset generated by the command</returns>
    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an XmlReader containing the resultset generated by the command</returns>
    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

        //create the DataAdapter & DataSet
        XmlReader retval = cmd.ExecuteXmlReader();

        // detach the SqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();
        return retval;
    }

    /// <summary>
    /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
    /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
    /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    /// </summary>
    /// <remarks>
    /// This method provides no access to output parameters or the stored procedure's return value parameter.
    /// 
    /// e.g.:  
    ///  XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">a valid SqlTransaction</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        //if we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        //otherwise we can just call the SP without params
        else
        {
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }
    }


    #endregion ExecuteXmlReader

    #region====================存储过程、SQL语句支持函数集===================

    /// <summary>
    /// Create command object used to call stored procedure.
    /// </summary>
    /// <param name="procName">Name of stored procedure.</param>
    /// <param name="prams">Params to stored procedure.</param>
    /// <returns>Command object.</returns>

    /// <summary>
    /// Make input param.
    /// </summary>
    /// <param name="ParamName">Name of param.</param>
    /// <param name="DbType">Param type.</param>
    /// <param name="Size">Param size.</param>
    /// <param name="Value">Param value.</param>
    /// <returns>New parameter.</returns>
    public static SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
    {
        return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
    }

    /// <summary>
    /// Make input param.
    /// </summary>
    /// <param name="ParamName">Name of param.</param>
    /// <param name="DbType">Param type.</param>
    /// <param name="Size">Param size.</param>
    /// <returns>New parameter.</returns>
    public static SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
    {
        return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
    }

    /// <summary>
    /// Make stored procedure param.
    /// </summary>
    /// <param name="ParamName">Name of param.</param>
    /// <param name="DbType">Param type.</param>
    /// <param name="Size">Param size.</param>
    /// <param name="Direction">Parm direction.</param>
    /// <param name="Value">Param value.</param>
    /// <returns>New parameter.</returns>
    public static SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
    {
        SqlParameter param;

        if (Size > 0 && Size != 16)
            param = new SqlParameter(ParamName, DbType, Size);
        else
            param = new SqlParameter(ParamName, DbType);

        param.Direction = Direction;
        if (!(Direction == ParameterDirection.Output && Value == null))
            param.Value = Value;
        return param;
    }
    #endregion

    #region=====================Sql语句执行函数集============================
    public static int ExecuteSql(string strSQL)
    {
        int Result;
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand Cmd = new SqlCommand(strSQL);
        con.Open();
        try
        {
            Cmd.Connection = con;
            Cmd.CommandType = CommandType.Text;
            Result = Cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            SysError.Log(ex.Message);
            Result = 0;
        }
        finally
        {
            Cmd.Dispose();
            con.Close();
        }
        return Result;
    }
    public static DataSet ExecuteSqlOfDs(string strSQL)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand Cmd = new SqlCommand(strSQL);
        SqlDataAdapter Adp = new SqlDataAdapter();
        DataSet Ds = new DataSet();
        con.Open();
        try
        {
            Cmd.Connection = con;
            Cmd.CommandType = CommandType.Text;
            Adp.SelectCommand = Cmd;
            Ds.Tables.Clear();
            Adp.Fill(Ds);
        }
        catch (SqlException ex)
        {
            SysError.Log(ex.Message);
            Ds = null;
        }
        finally
        {
            Cmd.Dispose();
            Adp.Dispose();
            con.Close();
        }
        return Ds;
    }

    public static SqlDataReader ExecuteSqlOfReader(string strSQL)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand Cmd = new SqlCommand(strSQL);
        SqlDataReader sdr;
        //SqlDataAdapter Adp = new SqlDataAdapter();
        //DataSet Ds = new DataSet();
        con.Open();

        Cmd.Connection = con;
        Cmd.CommandType = CommandType.Text;
        con.Close();
        return sdr = Cmd.ExecuteReader();

    }
    #endregion
}

public sealed class SqlHelperParameterCache
{
    #region private methods, variables, and constructors

    //Since this class provides only static methods, make the default constructor private to prevent 
    //instances from being created with "new SqlHelperParameterCache()".
    private SqlHelperParameterCache() { }

    private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

    /// <summary>
    /// resolve at run time the appropriate set of SqlParameters for a stored procedure
    /// </summary>
    /// <param name="connectionString">a valid connection string for a SqlConnection</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
    /// <returns></returns>
    private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
    {
        using (SqlConnection cn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(spName, cn))
        {
            cn.Open();
            cmd.CommandType = CommandType.StoredProcedure;

            SqlCommandBuilder.DeriveParameters(cmd);

            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count]; ;

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            return discoveredParameters;
        }
    }

    //deep copy of cached SqlParameter array
    private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
    {
        SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

        for (int i = 0, j = originalParameters.Length; i < j; i++)
        {
            clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
        }

        return clonedParameters;
    }

    #endregion private methods, variables, and constructors

    #region caching functions

    /// <summary>
    /// add parameter array to the cache
    /// </summary>
    /// <param name="connectionString">a valid connection string for a SqlConnection</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters to be cached</param>
    public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
    {
        string hashKey = connectionString + ":" + commandText;

        paramCache[hashKey] = commandParameters;
    }

    /// <summary>
    /// retrieve a parameter array from the cache
    /// </summary>
    /// <param name="connectionString">a valid connection string for a SqlConnection</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <returns>an array of SqlParamters</returns>
    public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
    {
        string hashKey = connectionString + ":" + commandText;

        SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];

        if (cachedParameters == null)
        {
            return null;
        }
        else
        {
            return CloneParameters(cachedParameters);
        }
    }

    #endregion caching functions

    #region Parameter Discovery Functions

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connectionString">a valid connection string for a SqlConnection</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <returns>an array of SqlParameters</returns>
    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
    {
        return GetSpParameterSet(connectionString, spName, false);
    }

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connectionString">a valid connection string for a SqlConnection</param>
    /// <param name="spName">the name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
    /// <returns>an array of SqlParameters</returns>
    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
    {
        string hashKey = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

        SqlParameter[] cachedParameters;

        cachedParameters = (SqlParameter[])paramCache[hashKey];

        if (cachedParameters == null)
        {
            cachedParameters = (SqlParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
        }

        return CloneParameters(cachedParameters);
    }

    #endregion Parameter Discovery Functions

}

