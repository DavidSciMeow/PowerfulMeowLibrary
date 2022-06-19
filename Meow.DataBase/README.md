# Meow Database 
-----------

## 1. 引 言
1.1 总 
本包当前为Mysql.Data(Nuget)标准包的二次封装, 其目的是为了更加便捷的简写使用.  
1.2 包和包更新  
您可以在 `Nuget` 搜索 `Electronicute.Meow.DataBase`  
如果在Nuget更新您只需要关注VisualStudio的内部的Dep管理即可  
如果为单独下载或者Clone本库请注意版本和时间  
1.3 包的计划内容  
> 1. 更新关于SQLlite的使用方案
> 1. 更新关于DBO的使用方案
> 1. 优化SQL(mysql)的安全性质
> 1. 根据微软语法糖提供更多简写方案

----------

## 2. 包特性
### 2.1 命名空间   
本包的引用起始命名空间为 `Meow.` 而非 `Electronicute.`   
### 2.2 包含状况  
本包亦被 `Electronicute.Meow` 包含但其提供更多API,如果您无需使用太多API建议单独下载本包即可  
### 2.3 连写和连写处理  
您可以使用连写特性(语法糖)进行快速开发.详细信息请参见使用方案

----------

## 3. 使用方案
### 3.1 建立数据库链接(Mysql)/生成DBHelper  
#### 3.1.1 使用实例化链接方案  
```csharp
using MysqlDBH dblk = new("dbname", "ip", "port", "user", "pass");
```
#### 3.1.2 使用链接字符串  
```csharp
using MysqlDBH dblk = new($"Database={DataBase};DataSource={DataSource};Port={Port};UserId={UserId};Password={password};Charset={Charset};{otherParameter}");
```

注1: 使用 `using` 引起是因为 `MysqlDBH` 实现了接口 `IDisposeable`,  
从而在其生命周期内(定义域/作用域内)当其失去使用效果时`自动断开链接`,  
防止过多链接导致Mysql达到最大连接数;  

注2: 如果您希望一直`保持单一链接`链接数据库, 您需要实现如下功能:  
1.将实例化的DBH设置为全局静态成员   
2.在您的数据库链接保存时间内随便进行一次操作(一般是查表)  
否则Mysql主机将会`主动断开`超过`8小时`未进行任何操作的链接,导致程序出错.  

注3: `并不建议`提高Mysql对于超时链接的默认设定.

#### 3.2 准备数据库操作并返回[函数:PrepareDb]
```csharp
using var d = ReturnService(); //数据库链接已经准备完毕
```
#### 3.2.1 增删改查,修改权限等 (PlainText)
```
//简单写法
链接.PrepareDb(...).操作().
```
#### 3.2.1.1 无需参数和变量传入的操作
```csharp
//查表,并返回整个表
var kx = d.PrepareDb("SELECT * FROM table").GetTable();
//获取表内某行元素
var colname = "col";
var row = 0;
if (kx.Rows.Count > 0)
{
    return kx.Rows[row].Field<int>(colname); //有行(并且获取一个Int值)
}
else
{
    return 0; //空行
}
```
```csharp
//执行NonQuery类型
var kx = d.PrepareDb("INSERT INTO table (col1,col2,col3) VALUES (1,2,3)").ExecuteNonQuery();
//检测是否成功
var roweffect = kx;
return roweffect>0;

//简写方案(最佳实践)
return d.PrepareDb("INSERT INTO table (col1,col2,col3) VALUES (1,2,3)").ExecuteNonQuery()>0;
```
#### 3.2.1.2 需要传入变量的操作
```csharp
///无限添加参数 (param方案)
int c = 0;
var kx = d.PrepareDb("SELECT c FROM table WHERE c1=@c1 and c2=@c2 ORDER BY c DESC LIMIT 1",
		new MySql.Data.MySqlClient.MySqlParameter("@c1", (int)c),
		new MySql.Data.MySqlClient.MySqlParameter("@c2", (int)c) //.....
		).GetTable();
```
```csharp
///无限添加参数方案二 (数组方案)
int c = 0;
var kx2 = d.PrepareDb("SELECT c FROM table WHERE c1=@c1 and c2=@c2 ORDER BY c DESC LIMIT 1",
        new MySql.Data.MySqlClient.MySqlParameter[] {new("@c1",c),new("@c2",c)}
        ).GetDataSet().Tables[0];
```

## 4.附录
### 4.1 返回值
返回值均为Mysql.Data内部的合成值,包含了Mysql.Data的最佳实现,
### 4.2 GetDataSet/GetTable的区别
`GetDataSet`是一个`备用扩展`,`GetTable()` 和 `GetDataSet().Tables[0]` 完全一致
### 4.3 可不可以进行字符串内插来更改参数
可以,但不建议,因为你需要`手动过滤`并且`防范Sql注入攻击`,  
其实使用SqlPara也未必能`完全防御`但 无论是从`写法`还是`逻辑安全性`他都要比字符串内插高.
### 4.4 事务等其他的语句怎么提交
使用基础语句
```
.....
PrepareDb("statement", CommandType.StoredProcedure //更改此处
.....	
```