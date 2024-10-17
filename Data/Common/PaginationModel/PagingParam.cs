
using System.ComponentModel;


namespace Data.Common.PaginationModel;
public class PagingParam
{
    private int _page = PagingConstants.DefaultPage;

    /// <summary>
    /// Gets or sets current page number.
    /// </summary>
    public int PageIndex
    {
        get => _page;
        set => _page = (value);
    }

    /// <summary>
    /// Gets or sets size of current page.
    /// </summary>
    [DefaultValue(PagingConstants.DefaultPageSize)]
    public int PageSize { get; set; } = PagingConstants.DefaultPageSize;
}