using System;
using Prism.Events;

namespace Jasmine.Core.Events
{
    public class NewEntitySavedEvent : PubSubEvent<string>
    {
    }

    //!Todo Migrate all classes using EntitySavedEvent to NewEntitySavedEvent
    public class EntitySavedEvent:PubSubEvent
    {
    }

    public class NewEntityDeletedEvent : PubSubEvent<string>
    {

    }

    //!Todo Migrate all classes using EntitySavedEvent to NewEntitySavedEvent
    public class EntityDeletedEvent : PubSubEvent
    {

    }

    //public class SearchEvent<T> : PubSubEvent<T>
    //{

    //}


    public class ReportStatusEvent : PubSubEvent<(string statusMessage,Guid reportId)>
    {

    }

    public class UpdateStatusEvent:PubSubEvent<string>{}

    public class UpdateProgressEvent : PubSubEvent<(string statusMessage,Guid reportId,int progress)>
    {

    }

    public class LookupItemSelectedEvent:PubSubEvent<LookupItem>
    {
        
    }
}
