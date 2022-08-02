
# Meow Interpreter
## 1. 引 言
1.1 总 
本包当前为Interpreter(解释器组件), 其目的是为了更加便捷的解释器简写使用.  
1.2 包和包更新  
您可以在 `Nuget` 搜索 `Electronicute.Meow.Interpreter`  
如果在Nuget更新您只需要关注VisualStudio的内部的Dep管理即可  
如果为单独下载或者Clone本库请注意版本和时间  

## 2. 包内含
| 解释器 | 命名空间 | 用途 |
|-------|---------|-----|
|解释器内核|Meow.Interpreter|基类解释器|
|命令解释器|Meow.Interpreter.Command|命令解释器内核|
|参数解释器|Meow.Interpreter.Args|命令行参数解释器|

## 3. 简易使用方法
### 3.1 参数解释器
```csharp
...main(...)...
...

var d = args.Interprete(new()
{
    ("--dav", 1), // ("需要读取的命令",参数个数)
    ("--req", 0),
});
Console.WriteLine(d["--dav"][0]);
...
```
|命令格式|输出|测试方案|
|-----|----|------|
|.exe --dav open --req|