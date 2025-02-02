﻿using Radon.Common.Core.Extension;
using Radon.Common.Enums;
using Radon.Core.Data.Entity;
using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class NavigationRes : BaseApiRes<EntryNavigation[]>
{
    private NavigationRes(EntryNavigation[] data)
    {
        if (data.Length == 0)
        {
            Code = ResponseCodeType.NotFound.ToInt();
            Msg = "No navigation data found.";
        }
        else
        {
            Code = ResponseCodeType.Success.ToInt();
            Msg = $"Navigation data found with {data.Length}.";
        }

        Data = data;
    }

    public static NavigationRes Of(EntryNavigation[] data)
    {
        return new NavigationRes(data);
    }
}