namespace Nano3.Core.Tracking
{
    /// <summary>
    /// TrackableEntities constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Change-tracking property names.
        /// </summary>
        public static class TrackingProperties
        {
            /// <summary>TrackingState property name</summary>
            public const string TrackingState = "TrackingState";
            /// <summary>ModifiedProperties property name</summary>
            public const string ModifiedProperties = "ModifiedProperties";

            public const string IsDirty = "IsDirty";
            public const string HasErrors = "HasErrors";
            public const string Validators = "Validators";
        }

        /// <summary>
        /// Change-tracking member names.
        /// </summary>
        public static class ChangeTrackingMembers
        {
            /// <summary>Change-tracking property end</summary>
            public const string ChangeTrackingPropEnd = "ChangeTracker";
        }
    }
}
