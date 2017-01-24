using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication.Utilities
{
    public class SessionVariable<T>
    {
        private readonly string key;
        private readonly Func<T> initializer;

        /// <summary>
        /// Initializes a new session variable.
        /// </summary>
        /// <param name=”key”>
        /// The key to use for storing the value in the session.
        /// </param>
        public SessionVariable(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            this.key = GetType() + key;
        }

        /// <summary>
        /// Initializes a new session variable with a initializer.
        /// </summary>
        /// <param name=”key”>
        /// The key to use for storing the value in the session.
        /// </param>
        /// <param name=”initializer”>
        /// A function that is called in order to create a
        /// default value per session.
        /// </param>
        public SessionVariable(string key, Func<T> initializer)
            : this(key)
        {
            if (initializer == null)
                throw new ArgumentNullException("initializer");

            this.initializer = initializer;
        }

        private object GetInternalValue(bool initializeIfNessesary)
        {
            HttpSessionState session = CurrentSession;

            var value = session[key];

            if (value == null && initializeIfNessesary
              && initializer != null)
                session[key] = value = initializer();

            return value;
        }

        private static HttpSessionState CurrentSession
        {
            get
            {
                var current = HttpContext.Current;

                if (current == null)
                    throw new InvalidOperationException(
                      "No HttpContext is not available.");

                var session = current.Session;
                if (session == null)
                    throw new InvalidOperationException(
                      "No Session available on current HttpContext.");
                return session;
            }
        }

        /// <summary>
        /// Indicates wether there is a value present or not.
        /// </summary>
        public bool HasValue
        {
            get { return GetInternalValue(false) != null; }
        }

        /// <summary>
        /// Sets or gets the value in the current session.
        /// </summary>
        /// <exception cref=”InvalidOperationException”>
        /// If you try to get a value while none is set.
        /// Use <see cref=”ValueOrDefault”/> for safe access.
        /// </exception>
        public T Value
        {
            get
            {
                object v = GetInternalValue(true);

                if (v == null)
                    throw new InvalidOperationException(
                      "The session does not contain any value for ‘"
                      + key + "‘.");

                return (T)v;
            }
            set { CurrentSession[key] = value; }
        }

        /// <summary>
        /// Gets the value in the current session or if
        /// none is available <c>default(T)</c>.
        /// </summary>
        public T ValueOrDefault
        {
            get
            {
                object v = GetInternalValue(true);

                if (v == null)
                    return default(T);

                return (T)v;
            }
        }

        /// <summary>
        /// Clears the value in the current session.
        /// </summary>
        public void Clear()
        {
            CurrentSession.Remove(key);
        }
    }

    public class MySessionVariables
    {
        // public static readonly SessionVariable<string> FavouriteColour = new SessionVariable<string>("FavouriteColour");

        public static readonly SessionVariable<List<int>> NurseSearchItems = new SessionVariable<List<int>>("NurseSearchItems", () => new List<int>());

        public static SessionVariable<int> CurrentIndex = new SessionVariable<int>("CurrentIndex");

        public static SessionVariable<SearchParameters> SearchParameters = new SessionVariable<SearchParameters>("SearchParameters");
    }

    public class SearchParameters
    {
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
        public string line1 { get; set; }
        public string phone { get; set; }
        public byte? designationId { get; set; }
        public byte? sectorId { get; set; }
        public int? facilityId { get; set; }
        public byte? districtId { get; set; }
        public byte? regionId { get; set; }
        public byte? localPositionId { get; set; }
        public byte? tableOfficerId { get; set; }
        public byte? committeeId { get; set; }
        public byte? positionId { get; set; }
        public int? communicationOptionId { get; set; }
        public byte? employerGroupId { get; set; }
        public string employmentStatusList { get; set; }
        public string localTableOfficerPositionList { get; set; }
    }
}