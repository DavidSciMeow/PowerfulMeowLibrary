//using System;
//using System.Data;
//using System.Linq;
//using System.Reflection;

//namespace MeowCoreTest
//{
//    public static class AttributeSets
//    {
//        public static readonly Type methodbase = (from t in Assembly.GetExecutingAssembly().GetTypes()
//                                                  where t.IsClass && t.IsAbstract && t.IsSealed && t.GetCustomAttribute(typeof(TMBAttribute)) != null
//                                                  select t)?.FirstOrDefault(); // 方法集合静态类
//        public static readonly Type deletype = (from t in Assembly.GetExecutingAssembly().GetTypes()
//                                                where t.IsClass && t.IsAbstract && t.IsSealed && t.GetCustomAttribute(typeof(TDBAttribute)) != null
//                                                select t)?.FirstOrDefault(); // 委托集合静态类
//        public static readonly Type[] havin = (from t in Assembly.GetExecutingAssembly().GetTypes()
//                                               where t.IsClass && t.GetCustomAttribute(typeof(AddToAttribute)) != null
//                                               select t).ToArray(); // 已经注入的类
//        public static void Init()
//        {
//            if (havin != null)
//            {
//                foreach (var i in havin)
//                {
//                    _ = i.GetType().GetCustomAttributes(true);
//                }
//            }
//            else
//            {
//                Console.WriteLine("NOBODY USE ACCESS INIT");
//            }
//        }
//    }
//    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
//    public class TMBAttribute : Attribute { }
//    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
//    public class TDBAttribute : Attribute { }
//    [AttributeUsage(AttributeTargets.Delegate, AllowMultiple = true)]
//    public class AddToAttribute : Attribute
//    {
//        public bool AutoFindDelegateType = true;
//        public Type DelegateType = null;
//        public AddToAttribute(string delefunc, string target)
//        {
//            var method = AttributeSets.methodbase?.GetMethod(delefunc); // 注入方法
//            var delegateinfo = AttributeSets.deletype?.GetField(target); // 被注入的委托
//            if (AttributeSets.methodbase == null)
//            {
//                throw new("No Method Base Found");
//            }
//            if (AttributeSets.deletype == null)
//            {
//                throw new("No Delegate Base Found");
//            }
//            if (method == null)
//            {
//                throw new("No Method Found");
//            }
//            if (delegateinfo == null)
//            {
//                throw new("No delegate Info Found");
//            }
//            if (AutoFindDelegateType)
//            {
//                var info = AttributeSets.methodbase?.GetNestedType($"D{delefunc}");
//                delegateinfo.SetValue(null, method.CreateDelegate(info));
//            }
//            else
//            {
//                delegateinfo.SetValue(null, method.CreateDelegate(DelegateType));
//            }
//        }
//    }
//    internal class Program
//    {
//        public static void Main(string[] args)
//        {
//            AttributeSets.Init();
//            AllDelegate.ADelegate?.DynamicInvoke("Go Go Go");
//            AllDelegate.BDelegate?.DynamicInvoke("string", 2);
//        }
//    }
//    [TMB]
//    public static class AllFunc
//    {
//        [AddTo("StdFunc", "ADelegate")]
//        public delegate void DStdFunc(string s);
//        public static void StdFunc(string s) => Console.WriteLine(s);

//        [AddTo("StdFunc2", "BDelegate")]
//        public delegate void DStdFunc2(string s, int a);
//        public static void StdFunc2(string s, int a) => Console.WriteLine($"{s} -> {a}");
//    }
//    [TDB]
//    public static class AllDelegate
//    {
//        public static Delegate ADelegate;
//        public static Delegate BDelegate;
//    }
//}
