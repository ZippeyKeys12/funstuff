// using System.Collections.Generic;
// using System.Linq;
// using Unity.Collections;
// using Unity.Entities;

// namespace AI
// {
//     public struct AIContext : IComponentData
//     {
//         private NativeHashMap<NativeString512, NativeString512> pairs;

//         public Entity Owner { get; }

//         public AIContext(Entity owner)
//             : this(owner, new Dictionary<string, object>()) { }

//         public AIContext(Entity owner, Dictionary<NativeString512, NativeString512> pairs)
//         {
//             Owner = owner;
//             this.pairs = new NativeHashMap<NativeString512, NativeString512>(pairs.Count + 4, Allocator.Persistent);

//             foreach (var pair in pairs)
//             {
//                 this.pairs.TryAdd(pair.Key, pair.Value);
//             }
//         }

//         // public object this[params string[] keys]
//         // {
//         //     get => pairs[string.Join(".", keys)];
//         //     set => pairs[string.Join(".", keys)] = value;
//         // }

//         public object this[string key]
//         {
//             get => pairs[key];
//             set => pairs[key] = value;
//         }

//         public T Get<T>(string key, params string[] keys)
//             => (T)this[keys.Append(key).ToArray()];

//         public static AIContext operator +(AIContext a, AIContext b)
//             => new AIContext(b.Owner, new Dictionary<string, object>(
//                              a.pairs.Concat(b.pairs)
//                               .ToLookup(p => p.Key, p => p.Value)
//                               .ToDictionary(g => g.Key, g => g.Last())));

//     }
// }