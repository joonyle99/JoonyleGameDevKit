// namespace JoonyleGameDevKit
// {
//     public interface IEvent
//     {
        
//     }

//     /// <summary>
//     /// 
//     /// </summary>
//     public class EventBus
//     {
//         /// <summary>
//         /// private 생성자 (외부 생성 방지)
//         /// </summary>
//         private EventBus()
//         {
//             Debug.Log("EventBus initialized");
//         }

//         private static EventBus _instance;
//         public static EventBus Instance => _instance ??= new EventBus();

//         /// <summary>
//         /// 
//         /// </summary>
//         private Dictionary<Type, Delegate> eventHandlers = new();


//         public void Subscribe<T>(Action<T> handler) where T : IEvent
//         {
//             var evtType = typeof(T);
//             if (eventHandlers.TryGetValue(evtType, out Delegate temp))
//             {
//                 eventHandlers[evtType] = Delegate.Combine(temp);
//             }
//             else
//             {
//                 eventHandlers[evtType] = temp;
//             }
//         }
//         public void Publish<T>(T evt) where T : IEvent
//         {
//             var evtType = typeof(T);
//             if (eventHandlers.TryGetValue(evtType, out Delegate temp))
//             {
//                 temp?.Invoke();
//             }
//         }
//         public void Clear()
//         {
//             eventHandlers.Clear();
//         }
//     }
// }
