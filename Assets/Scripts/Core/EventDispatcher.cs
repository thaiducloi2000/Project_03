using System.Collections.Generic;
namespace GameCore
{
    public interface EventData
    {
    }

    public static class EventDispatcher
    {
        public delegate void ActionCallback(EventData param = null);
        private static readonly Dictionary<string, ActionCallback> listeners = new();

        /// <summary>
        /// Register to listen for eventID
        /// </summary>
        /// <param name="eventID">EventID that object want to listen</param>
        /// <param name="callback">Callback will be invoked when this eventID be raised</param>
        public static void AddListener(string eventID, ActionCallback callback)
        {
            // check if listener exist in distionary
            if (listeners.ContainsKey(eventID))
            {
                // add callback to our collection
                listeners[eventID] += callback;
            }
            else
            {
                // add new key-value pair
                listeners.Add(eventID, null);
                listeners[eventID] += callback;
            }
        }

        /// <summary>
        /// Posts the event with param. This will notify all listener that register for this event
        /// </summary>
        /// <param name="eventID">EventID.</param>
        /// <param name="param">Parameter. Can be anything (struct, class ...), Data must be implement IEvenData to register</param>
        public static void PostEvent(string eventID, EventData param = null)
        {
            if (!listeners.ContainsKey(eventID))
                return;

            listeners[eventID]?.Invoke(param);
        }

        /// <summary>
        /// Removes the listener has param. Use to Unregister listener
        /// </summary>
        /// <param name="eventID">EventID.</param>
        /// <param name="callback">Callback.</param>
        public static void RemoveListener(string eventID, ActionCallback callback)
        {
            if (listeners.ContainsKey(eventID))
                listeners[eventID] -= callback;
        }

        /// <summary>
        /// Removes the listener has param. Use to Unregister listener
        /// </summary>
        /// <param name="eventID">EventID.</param>
        /// <param name="callback">Callback.</param>
        public static void RemoveAllListener(string eventID)
        {
            if (listeners.ContainsKey(eventID))
                listeners[eventID] = null;
        }

        /// <summary>
        /// Clears all the listener.
        /// </summary>
        public static void ClearAllListener()
        {
            foreach (string eventID in listeners.Keys)
            {
                RemoveAllListener(eventID);
            }
        }
    }
}

