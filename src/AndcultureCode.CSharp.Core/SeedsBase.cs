using System;
using AndcultureCode.CSharp.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AndcultureCode.CSharp.Core
{
    /// <summary>
    /// Shared functionality for configuring application data seeding
    /// </summary>
    public abstract class SeedsBase<TContext>
        where TContext : class, IContext
    {
        #region Properties

        /// <summary>
        /// Application's data context
        /// </summary>
        public TContext Context { get; private set; }

        /// <summary>
        /// Should development seeds be run?
        /// </summary>
        public bool IsDevelopment { get; private set; }

        /// <summary>
        /// Logger instance
        /// </summary>
        public ILogger<SeedsBase<TContext>> Logger { get; private set; }

        /// <summary>
        /// Application's dependency registry
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Shared functionality for configuring application data seeding
        /// </summary>
        /// <param name="serviceProvider">Application's dependency registry</param>
        /// <param name="isDevelopment">Should development seeds be run?</param>
        public SeedsBase(IServiceProvider serviceProvider, bool isDevelopment = false)
        {
            IsDevelopment = isDevelopment;
            ServiceProvider = serviceProvider;

            Context = GetDep<TContext>();
            Logger = GetDep<ILogger<SeedsBase<TContext>>>();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Primary entry point to initialize seed data
        /// </summary>
        public void Create()
        {
            CreateBase();

            if (IsDevelopment)
            {
                CreateDevelopment();
            }
        }

        /// <summary>
        /// Retrieve dependency from registry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetDep<T>() where T : class => ServiceProvider.GetService(typeof(T)) as T;

        /// <summary>
        /// Signify completion of seeding a particular type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void LogEnd<T>() where T : class => Logger.LogInformation($"Finished seeding `{typeof(T).Name}`");

        /// <summary>
        /// Signify start to seeding a particular type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void LogStart<T>() where T : class => Logger.LogInformation($"Seeding `{typeof(T).Name}`");

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Seed data that is required for the application to run, regardless of the environment
        /// </summary>
        protected abstract void CreateBase();

        /// <summary>
        /// Seed data that is only for development environments (ie. test accounts, use cases)
        /// </summary>
        protected abstract void CreateDevelopment();

        #endregion Protected Methods
    }
}
