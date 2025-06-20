namespace VMTS.API.Helpers;

public class Pagination<T>
{
    public Pagination(int pageSize, int pageIndex, int count, IReadOnlyList<T> data)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
        Count = count;
        Data = data;
    }

    public int PageIndex { set; get; }
    public int PageSize { set; get; }
    public int Count { set; get; }
    public IReadOnlyList<T> Data { set; get; }
}
