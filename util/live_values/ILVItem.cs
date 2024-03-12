public interface ILVItem<TValue> where TValue : ILiveValue
{
    public void Connect(LiveValuesModel model, TValue val);
}