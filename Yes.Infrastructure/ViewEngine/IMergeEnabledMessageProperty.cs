namespace Yes.Infrastructure.ViewEngine
{
    internal interface IMergeEnabledMessageProperty
    {
        bool TryMergeWithProperty(object propertyToMerge);
    }
}
